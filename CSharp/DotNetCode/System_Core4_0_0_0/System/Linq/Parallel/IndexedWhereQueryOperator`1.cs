// Type: System.Linq.Parallel.IndexedWhereQueryOperator`1
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
  internal sealed class IndexedWhereQueryOperator<TInputOutput> : UnaryQueryOperator<TInputOutput, TInputOutput>
  {
    private Func<TInputOutput, int, bool> m_predicate;
    private bool m_prematureMerge;
    private bool m_limitsParallelism;

    internal override bool LimitsParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_limitsParallelism;
      }
    }

    internal IndexedWhereQueryOperator(IEnumerable<TInputOutput> child, Func<TInputOutput, int, bool> predicate)
      : base(child)
    {
      this.m_predicate = predicate;
      this.m_outputOrdered = true;
      this.InitOrdinalIndexState();
    }

    private void InitOrdinalIndexState()
    {
      OrdinalIndexState ordinalIndexState = this.Child.OrdinalIndexState;
      if (ExchangeUtilities.IsWorseThan(ordinalIndexState, OrdinalIndexState.Correct))
      {
        this.m_prematureMerge = true;
        this.m_limitsParallelism = ordinalIndexState != OrdinalIndexState.Shuffled;
      }
      this.SetOrdinalIndexState(OrdinalIndexState.Increasing);
    }

    internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInputOutput>) new UnaryQueryOperator<TInputOutput, TInputOutput>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TInputOutput, TInputOutput>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TInputOutput, int> partitionedStream1 = !this.m_prematureMerge ? (PartitionedStream<TInputOutput, int>) inputStream : QueryOperator<TInputOutput>.ExecuteAndCollectResults<TKey>(inputStream, partitionCount, this.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream();
      PartitionedStream<TInputOutput, int> partitionedStream2 = new PartitionedStream<TInputOutput, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream2[index] = (QueryOperatorEnumerator<TInputOutput, int>) new IndexedWhereQueryOperator<TInputOutput>.IndexedWhereQueryOperatorEnumerator(partitionedStream1[index], this.m_predicate, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream2);
    }

    internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Where<TInputOutput>(CancellableEnumerable.Wrap<TInputOutput>(this.Child.AsSequentialQuery(token), token), this.m_predicate);
    }

    private class IndexedWhereQueryOperatorEnumerator : QueryOperatorEnumerator<TInputOutput, int>
    {
      private readonly QueryOperatorEnumerator<TInputOutput, int> m_source;
      private readonly Func<TInputOutput, int, bool> m_predicate;
      private CancellationToken m_cancellationToken;
      private Shared<int> m_outputLoopCount;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal IndexedWhereQueryOperatorEnumerator(QueryOperatorEnumerator<TInputOutput, int> source, Func<TInputOutput, int, bool> predicate, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_predicate = predicate;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref int currentKey)
      {
        if (this.m_outputLoopCount == null)
          this.m_outputLoopCount = new Shared<int>(0);
        while (this.m_source.MoveNext(ref currentElement, ref currentKey))
        {
          if ((this.m_outputLoopCount.Value++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (this.m_predicate(currentElement, currentKey))
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
