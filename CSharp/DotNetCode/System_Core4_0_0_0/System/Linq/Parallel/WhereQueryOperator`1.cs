// Type: System.Linq.Parallel.WhereQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class WhereQueryOperator<TInputOutput> : UnaryQueryOperator<TInputOutput, TInputOutput>
  {
    private Func<TInputOutput, bool> m_predicate;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal WhereQueryOperator(IEnumerable<TInputOutput> child, Func<TInputOutput, bool> predicate)
      : base(child)
    {
      this.SetOrdinalIndexState(ExchangeUtilities.Worse(this.Child.OrdinalIndexState, OrdinalIndexState.Increasing));
      this.m_predicate = predicate;
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      PartitionedStream<TInputOutput, TKey> partitionedStream = new PartitionedStream<TInputOutput, TKey>(inputStream.PartitionCount, inputStream.KeyComparer, this.OrdinalIndexState);
      for (int index = 0; index < inputStream.PartitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TInputOutput, TKey>) new WhereQueryOperator<TInputOutput>.WhereQueryOperatorEnumerator<TKey>(inputStream[index], this.m_predicate, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInputOutput>) new UnaryQueryOperator<TInputOutput, TInputOutput>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TInputOutput, TInputOutput>) this, settings, preferStriping);
    }

    internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Where<TInputOutput>(CancellableEnumerable.Wrap<TInputOutput>(this.Child.AsSequentialQuery(token), token), this.m_predicate);
    }

    private class WhereQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TInputOutput, TKey>
    {
      private readonly QueryOperatorEnumerator<TInputOutput, TKey> m_source;
      private readonly Func<TInputOutput, bool> m_predicate;
      private CancellationToken m_cancellationToken;
      private Shared<int> m_outputLoopCount;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal WhereQueryOperatorEnumerator(QueryOperatorEnumerator<TInputOutput, TKey> source, Func<TInputOutput, bool> predicate, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_predicate = predicate;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref TKey currentKey)
      {
        if (this.m_outputLoopCount == null)
          this.m_outputLoopCount = new Shared<int>(0);
        while (this.m_source.MoveNext(ref currentElement, ref currentKey))
        {
          if ((this.m_outputLoopCount.Value++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (this.m_predicate(currentElement))
            return true;
        }
        return false;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
