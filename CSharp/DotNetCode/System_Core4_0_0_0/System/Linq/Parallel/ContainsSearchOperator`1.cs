// Type: System.Linq.Parallel.ContainsSearchOperator`1
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
  internal sealed class ContainsSearchOperator<TInput> : UnaryQueryOperator<TInput, bool>
  {
    private readonly TInput m_searchValue;
    private readonly IEqualityComparer<TInput> m_comparer;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal ContainsSearchOperator(IEnumerable<TInput> child, TInput searchValue, IEqualityComparer<TInput> comparer)
      : base(child)
    {
      this.m_searchValue = searchValue;
      if (comparer == null)
        this.m_comparer = (IEqualityComparer<TInput>) EqualityComparer<TInput>.Default;
      else
        this.m_comparer = comparer;
    }

    internal bool Aggregate()
    {
      using (IEnumerator<bool> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current)
            return true;
        }
      }
      return false;
    }

    internal override QueryResults<bool> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<bool>) new UnaryQueryOperator<TInput, bool>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TInput, bool>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<bool> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<bool, int> partitionedStream = new PartitionedStream<bool, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
      Shared<bool> resultFoundFlag = new Shared<bool>(false);
      for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
        partitionedStream[partitionIndex] = (QueryOperatorEnumerator<bool, int>) new ContainsSearchOperator<TInput>.ContainsSearchOperatorEnumerator<TKey>(inputStream[partitionIndex], this.m_searchValue, this.m_comparer, partitionIndex, resultFoundFlag, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream);
    }

    internal override IEnumerable<bool> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }

    private class ContainsSearchOperatorEnumerator<TKey> : QueryOperatorEnumerator<bool, int>
    {
      private readonly QueryOperatorEnumerator<TInput, TKey> m_source;
      private readonly TInput m_searchValue;
      private readonly IEqualityComparer<TInput> m_comparer;
      private readonly int m_partitionIndex;
      private readonly Shared<bool> m_resultFoundFlag;
      private CancellationToken m_cancellationToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ContainsSearchOperatorEnumerator(QueryOperatorEnumerator<TInput, TKey> source, TInput searchValue, IEqualityComparer<TInput> comparer, int partitionIndex, Shared<bool> resultFoundFlag, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_searchValue = searchValue;
        this.m_comparer = comparer;
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
        currentElement = false;
        currentKey = this.m_partitionIndex;
        int num = 0;
        do
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (this.m_resultFoundFlag.Value)
            return false;
          if (this.m_comparer.Equals(currentElement1, this.m_searchValue))
          {
            this.m_resultFoundFlag.Value = true;
            currentElement = true;
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
