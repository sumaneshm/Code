// Type: System.Linq.Parallel.AnyAllSearchOperator`1
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
  internal sealed class AnyAllSearchOperator<TInput> : UnaryQueryOperator<TInput, bool>
  {
    private readonly Func<TInput, bool> m_predicate;
    private readonly bool m_qualification;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal AnyAllSearchOperator(IEnumerable<TInput> child, bool qualification, Func<TInput, bool> predicate)
      : base(child)
    {
      this.m_qualification = qualification;
      this.m_predicate = predicate;
    }

    internal bool Aggregate()
    {
      using (IEnumerator<bool> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current == this.m_qualification)
            return this.m_qualification;
        }
      }
      return !this.m_qualification;
    }

    internal override QueryResults<bool> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<bool>) new UnaryQueryOperator<TInput, bool>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TInput, bool>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<bool> recipient, bool preferStriping, QuerySettings settings)
    {
      Shared<bool> resultFoundFlag = new Shared<bool>(false);
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<bool, int> partitionedStream = new PartitionedStream<bool, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
      for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
        partitionedStream[partitionIndex] = (QueryOperatorEnumerator<bool, int>) new AnyAllSearchOperator<TInput>.AnyAllSearchOperatorEnumerator<TKey>(inputStream[partitionIndex], this.m_qualification, this.m_predicate, partitionIndex, resultFoundFlag, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream);
    }

    internal override IEnumerable<bool> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }

    private class AnyAllSearchOperatorEnumerator<TKey> : QueryOperatorEnumerator<bool, int>
    {
      private readonly QueryOperatorEnumerator<TInput, TKey> m_source;
      private readonly Func<TInput, bool> m_predicate;
      private readonly bool m_qualification;
      private readonly int m_partitionIndex;
      private readonly Shared<bool> m_resultFoundFlag;
      private readonly CancellationToken m_cancellationToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal AnyAllSearchOperatorEnumerator(QueryOperatorEnumerator<TInput, TKey> source, bool qualification, Func<TInput, bool> predicate, int partitionIndex, Shared<bool> resultFoundFlag, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_qualification = qualification;
        this.m_predicate = predicate;
        this.m_partitionIndex = partitionIndex;
        this.m_resultFoundFlag = resultFoundFlag;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref bool currentElement, ref int currentKey)
      {
        if (this.m_resultFoundFlag.Value)
          return false;
        TInput currentElement1 = default (TInput);
        TKey currentKey1 = default (TKey);
        if (!this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          return false;
        currentElement = !this.m_qualification;
        currentKey = this.m_partitionIndex;
        int num = 0;
        do
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (this.m_resultFoundFlag.Value)
            return false;
          if (this.m_predicate(currentElement1) == this.m_qualification)
          {
            this.m_resultFoundFlag.Value = true;
            currentElement = this.m_qualification;
            break;
          }
        }
        while (this.m_source.MoveNext(ref currentElement1, ref currentKey1));
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
