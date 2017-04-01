// Type: System.Linq.Parallel.QueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal abstract class QueryOperator<TOutput> : ParallelQuery<TOutput>
  {
    protected bool m_outputOrdered;

    internal bool OutputOrdered
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_outputOrdered;
      }
    }

    internal abstract bool LimitsParallelism { get; }

    internal abstract OrdinalIndexState OrdinalIndexState { get; }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal QueryOperator(QuerySettings settings)
      : this(false, settings)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal QueryOperator(bool isOrdered, QuerySettings settings)
      : base(settings)
    {
      this.m_outputOrdered = isOrdered;
    }

    internal abstract QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping);

    public override IEnumerator<TOutput> GetEnumerator()
    {
      return this.GetEnumerator(new ParallelMergeOptions?(), false);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public IEnumerator<TOutput> GetEnumerator(ParallelMergeOptions? mergeOptions)
    {
      return this.GetEnumerator(mergeOptions, false);
    }

    internal virtual IEnumerator<TOutput> GetEnumerator(ParallelMergeOptions? mergeOptions, bool suppressOrderPreservation)
    {
      return (IEnumerator<TOutput>) new QueryOpeningEnumerator<TOutput>(this, mergeOptions, suppressOrderPreservation);
    }

    internal IEnumerator<TOutput> GetOpenedEnumerator(ParallelMergeOptions? mergeOptions, bool suppressOrder, bool forEffect, QuerySettings querySettings)
    {
      if (querySettings.ExecutionMode.Value == ParallelExecutionMode.Default && this.LimitsParallelism)
        return ExceptionAggregator.WrapEnumerable<TOutput>(this.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState).GetEnumerator();
      QueryResults<TOutput> queryResults = this.GetQueryResults(querySettings);
      if (!mergeOptions.HasValue)
        mergeOptions = querySettings.MergeOptions;
      if (querySettings.CancellationState.MergedCancellationToken.IsCancellationRequested)
      {
        if (querySettings.CancellationState.ExternalCancellationToken.IsCancellationRequested)
          throw new OperationCanceledException(querySettings.CancellationState.ExternalCancellationToken);
        else
          throw new OperationCanceledException();
      }
      else
      {
        bool outputOrdered = this.OutputOrdered && !suppressOrder;
        PartitionedStreamMerger<TOutput> partitionedStreamMerger = new PartitionedStreamMerger<TOutput>(forEffect, mergeOptions.GetValueOrDefault(), querySettings.TaskScheduler, outputOrdered, querySettings.CancellationState, querySettings.QueryId);
        queryResults.GivePartitionedStream((IPartitionedStreamRecipient<TOutput>) partitionedStreamMerger);
        if (forEffect)
          return (IEnumerator<TOutput>) null;
        else
          return partitionedStreamMerger.MergeExecutor.GetEnumerator();
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private QueryResults<TOutput> GetQueryResults(QuerySettings querySettings)
    {
      return this.Open(querySettings, false);
    }

    internal TOutput[] ExecuteAndGetResultsAsArray()
    {
      QuerySettings querySettings = this.SpecifiedQuerySettings.WithPerExecutionSettings().WithDefaults();
      QueryLifecycle.LogicalQueryExecutionBegin(querySettings.QueryId);
      try
      {
        if (querySettings.ExecutionMode.Value == ParallelExecutionMode.Default && this.LimitsParallelism)
          return Enumerable.ToArray<TOutput>(ExceptionAggregator.WrapEnumerable<TOutput>(CancellableEnumerable.Wrap<TOutput>(this.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState));
        QueryResults<TOutput> queryResults = this.GetQueryResults(querySettings);
        if (queryResults.IsIndexible && this.OutputOrdered)
        {
          ArrayMergeHelper<TOutput> arrayMergeHelper = new ArrayMergeHelper<TOutput>(this.SpecifiedQuerySettings, queryResults);
          arrayMergeHelper.Execute();
          TOutput[] resultsAsArray = arrayMergeHelper.GetResultsAsArray();
          querySettings.CleanStateAtQueryEnd();
          return resultsAsArray;
        }
        else
        {
          PartitionedStreamMerger<TOutput> partitionedStreamMerger = new PartitionedStreamMerger<TOutput>(false, ParallelMergeOptions.FullyBuffered, querySettings.TaskScheduler, this.OutputOrdered, querySettings.CancellationState, querySettings.QueryId);
          queryResults.GivePartitionedStream((IPartitionedStreamRecipient<TOutput>) partitionedStreamMerger);
          TOutput[] resultsAsArray = partitionedStreamMerger.MergeExecutor.GetResultsAsArray();
          querySettings.CleanStateAtQueryEnd();
          return resultsAsArray;
        }
      }
      finally
      {
        QueryLifecycle.LogicalQueryExecutionEnd(querySettings.QueryId);
      }
    }

    internal abstract IEnumerable<TOutput> AsSequentialQuery(CancellationToken token);

    internal static ListQueryResults<TOutput> ExecuteAndCollectResults<TKey>(PartitionedStream<TOutput, TKey> openedChild, int partitionCount, bool outputOrdered, bool useStriping, QuerySettings settings)
    {
      TaskScheduler taskScheduler = settings.TaskScheduler;
      return new ListQueryResults<TOutput>((IList<TOutput>) MergeExecutor<TOutput>.Execute<TKey>(openedChild, false, ParallelMergeOptions.FullyBuffered, taskScheduler, outputOrdered, settings.CancellationState, settings.QueryId).GetResultsAsArray(), partitionCount, useStriping);
    }

    internal static QueryOperator<TOutput> AsQueryOperator(IEnumerable<TOutput> source)
    {
      QueryOperator<TOutput> queryOperator = source as QueryOperator<TOutput>;
      if (queryOperator == null)
      {
        OrderedParallelQuery<TOutput> orderedParallelQuery = source as OrderedParallelQuery<TOutput>;
        queryOperator = orderedParallelQuery == null ? (QueryOperator<TOutput>) new ScanQueryOperator<TOutput>(source) : orderedParallelQuery.SortOperator;
      }
      return queryOperator;
    }
  }
}
