// Type: System.Linq.Parallel.ZipQueryOperator`3
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
  internal sealed class ZipQueryOperator<TLeftInput, TRightInput, TOutput> : QueryOperator<TOutput>
  {
    private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;
    private readonly QueryOperator<TLeftInput> m_leftChild;
    private readonly QueryOperator<TRightInput> m_rightChild;
    private readonly bool m_prematureMergeLeft;
    private readonly bool m_prematureMergeRight;
    private readonly bool m_limitsParallelism;

    internal override OrdinalIndexState OrdinalIndexState
    {
      get
      {
        return OrdinalIndexState.Indexible;
      }
    }

    internal override bool LimitsParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_limitsParallelism;
      }
    }

    internal ZipQueryOperator(ParallelQuery<TLeftInput> leftChildSource, IEnumerable<TRightInput> rightChildSource, Func<TLeftInput, TRightInput, TOutput> resultSelector)
      : this(QueryOperator<TLeftInput>.AsQueryOperator((IEnumerable<TLeftInput>) leftChildSource), QueryOperator<TRightInput>.AsQueryOperator(rightChildSource), resultSelector)
    {
    }

    private ZipQueryOperator(QueryOperator<TLeftInput> left, QueryOperator<TRightInput> right, Func<TLeftInput, TRightInput, TOutput> resultSelector)
      : base(left.SpecifiedQuerySettings.Merge(right.SpecifiedQuerySettings))
    {
      this.m_leftChild = left;
      this.m_rightChild = right;
      this.m_resultSelector = resultSelector;
      this.m_outputOrdered = this.m_leftChild.OutputOrdered || this.m_rightChild.OutputOrdered;
      OrdinalIndexState ordinalIndexState1 = this.m_leftChild.OrdinalIndexState;
      OrdinalIndexState ordinalIndexState2 = this.m_rightChild.OrdinalIndexState;
      this.m_prematureMergeLeft = ordinalIndexState1 != OrdinalIndexState.Indexible;
      this.m_prematureMergeRight = ordinalIndexState2 != OrdinalIndexState.Indexible;
      this.m_limitsParallelism = this.m_prematureMergeLeft && ordinalIndexState1 != OrdinalIndexState.Shuffled || this.m_prematureMergeRight && ordinalIndexState2 != OrdinalIndexState.Shuffled;
    }

    internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
    {
      QueryResults<TLeftInput> leftChildResults = this.m_leftChild.Open(settings, preferStriping);
      QueryResults<TRightInput> rightChildResults = this.m_rightChild.Open(settings, preferStriping);
      int partitionCount = settings.DegreeOfParallelism.Value;
      if (this.m_prematureMergeLeft)
      {
        PartitionedStreamMerger<TLeftInput> partitionedStreamMerger = new PartitionedStreamMerger<TLeftInput>(false, ParallelMergeOptions.FullyBuffered, settings.TaskScheduler, this.m_leftChild.OutputOrdered, settings.CancellationState, settings.QueryId);
        leftChildResults.GivePartitionedStream((IPartitionedStreamRecipient<TLeftInput>) partitionedStreamMerger);
        leftChildResults = (QueryResults<TLeftInput>) new ListQueryResults<TLeftInput>((IList<TLeftInput>) partitionedStreamMerger.MergeExecutor.GetResultsAsArray(), partitionCount, preferStriping);
      }
      if (this.m_prematureMergeRight)
      {
        PartitionedStreamMerger<TRightInput> partitionedStreamMerger = new PartitionedStreamMerger<TRightInput>(false, ParallelMergeOptions.FullyBuffered, settings.TaskScheduler, this.m_rightChild.OutputOrdered, settings.CancellationState, settings.QueryId);
        rightChildResults.GivePartitionedStream((IPartitionedStreamRecipient<TRightInput>) partitionedStreamMerger);
        rightChildResults = (QueryResults<TRightInput>) new ListQueryResults<TRightInput>((IList<TRightInput>) partitionedStreamMerger.MergeExecutor.GetResultsAsArray(), partitionCount, preferStriping);
      }
      return (QueryResults<TOutput>) new ZipQueryOperator<TLeftInput, TRightInput, TOutput>.ZipQueryOperatorResults(leftChildResults, rightChildResults, this.m_resultSelector, partitionCount, preferStriping);
    }

    internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
    {
      using (IEnumerator<TLeftInput> enumerator1 = this.m_leftChild.AsSequentialQuery(token).GetEnumerator())
      {
        using (IEnumerator<TRightInput> enumerator2 = this.m_rightChild.AsSequentialQuery(token).GetEnumerator())
        {
          while (enumerator1.MoveNext() && enumerator2.MoveNext())
            yield return this.m_resultSelector(enumerator1.Current, enumerator2.Current);
        }
      }
    }

    internal class ZipQueryOperatorResults : QueryResults<TOutput>
    {
      private readonly QueryResults<TLeftInput> m_leftChildResults;
      private readonly QueryResults<TRightInput> m_rightChildResults;
      private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;
      private readonly int m_count;
      private readonly int m_partitionCount;
      private readonly bool m_preferStriping;

      internal override int ElementsCount
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.m_count;
        }
      }

      internal override bool IsIndexible
      {
        get
        {
          return true;
        }
      }

      internal ZipQueryOperatorResults(QueryResults<TLeftInput> leftChildResults, QueryResults<TRightInput> rightChildResults, Func<TLeftInput, TRightInput, TOutput> resultSelector, int partitionCount, bool preferStriping)
      {
        this.m_leftChildResults = leftChildResults;
        this.m_rightChildResults = rightChildResults;
        this.m_resultSelector = resultSelector;
        this.m_partitionCount = partitionCount;
        this.m_preferStriping = preferStriping;
        this.m_count = Math.Min(this.m_leftChildResults.Count, this.m_rightChildResults.Count);
      }

      internal override TOutput GetElement(int index)
      {
        return this.m_resultSelector(this.m_leftChildResults.GetElement(index), this.m_rightChildResults.GetElement(index));
      }

      internal override void GivePartitionedStream(IPartitionedStreamRecipient<TOutput> recipient)
      {
        PartitionedStream<TOutput, int> partitionedStream = ExchangeUtilities.PartitionDataSource<TOutput>((IEnumerable<TOutput>) this, this.m_partitionCount, this.m_preferStriping);
        recipient.Receive<int>(partitionedStream);
      }
    }
  }
}
