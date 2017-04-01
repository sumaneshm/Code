// Type: System.Linq.Parallel.QuerySettings
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal struct QuerySettings
  {
    private TaskScheduler m_taskScheduler;
    private int? m_degreeOfParallelism;
    private CancellationState m_cancellationState;
    private ParallelExecutionMode? m_executionMode;
    private ParallelMergeOptions? m_mergeOptions;
    private int m_queryId;

    internal CancellationState CancellationState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_cancellationState;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_cancellationState = value;
      }
    }

    internal TaskScheduler TaskScheduler
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_taskScheduler;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_taskScheduler = value;
      }
    }

    internal int? DegreeOfParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_degreeOfParallelism;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_degreeOfParallelism = value;
      }
    }

    internal ParallelExecutionMode? ExecutionMode
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_executionMode;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_executionMode = value;
      }
    }

    internal ParallelMergeOptions? MergeOptions
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_mergeOptions;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_mergeOptions = value;
      }
    }

    internal int QueryId
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_queryId;
      }
    }

    internal static QuerySettings Empty
    {
      get
      {
        return new QuerySettings((TaskScheduler) null, new int?(), new CancellationToken(), new ParallelExecutionMode?(), new ParallelMergeOptions?());
      }
    }

    internal QuerySettings(TaskScheduler taskScheduler, int? degreeOfParallelism, CancellationToken externalCancellationToken, ParallelExecutionMode? executionMode, ParallelMergeOptions? mergeOptions)
    {
      this.m_taskScheduler = taskScheduler;
      this.m_degreeOfParallelism = degreeOfParallelism;
      this.m_cancellationState = new CancellationState(externalCancellationToken);
      this.m_executionMode = executionMode;
      this.m_mergeOptions = mergeOptions;
      this.m_queryId = -1;
    }

    internal QuerySettings Merge(QuerySettings settings2)
    {
      if (this.TaskScheduler != null && settings2.TaskScheduler != null)
        throw new InvalidOperationException(System.Linq.SR.GetString("ParallelQuery_DuplicateTaskScheduler"));
      if (this.DegreeOfParallelism.HasValue && settings2.DegreeOfParallelism.HasValue)
        throw new InvalidOperationException(System.Linq.SR.GetString("ParallelQuery_DuplicateDOP"));
      if (this.CancellationState.ExternalCancellationToken.CanBeCanceled && settings2.CancellationState.ExternalCancellationToken.CanBeCanceled)
        throw new InvalidOperationException(System.Linq.SR.GetString("ParallelQuery_DuplicateWithCancellation"));
      if (this.ExecutionMode.HasValue && settings2.ExecutionMode.HasValue)
        throw new InvalidOperationException(System.Linq.SR.GetString("ParallelQuery_DuplicateExecutionMode"));
      if (this.MergeOptions.HasValue && settings2.MergeOptions.HasValue)
        throw new InvalidOperationException(System.Linq.SR.GetString("ParallelQuery_DuplicateMergeOptions"));
      else
        return new QuerySettings(this.TaskScheduler == null ? settings2.TaskScheduler : this.TaskScheduler, this.DegreeOfParallelism.HasValue ? this.DegreeOfParallelism : settings2.DegreeOfParallelism, this.CancellationState.ExternalCancellationToken.CanBeCanceled ? this.CancellationState.ExternalCancellationToken : settings2.CancellationState.ExternalCancellationToken, this.ExecutionMode.HasValue ? this.ExecutionMode : settings2.ExecutionMode, this.MergeOptions.HasValue ? this.MergeOptions : settings2.MergeOptions);
    }

    internal QuerySettings WithPerExecutionSettings()
    {
      return this.WithPerExecutionSettings(new CancellationTokenSource(), new Shared<bool>(false));
    }

    internal QuerySettings WithPerExecutionSettings(CancellationTokenSource topLevelCancellationTokenSource, Shared<bool> topLevelDisposedFlag)
    {
      QuerySettings querySettings = new QuerySettings(this.TaskScheduler, this.DegreeOfParallelism, this.CancellationState.ExternalCancellationToken, this.ExecutionMode, this.MergeOptions);
      querySettings.CancellationState.InternalCancellationTokenSource = topLevelCancellationTokenSource;
      querySettings.CancellationState.MergedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(querySettings.CancellationState.InternalCancellationTokenSource.Token, querySettings.CancellationState.ExternalCancellationToken);
      querySettings.CancellationState.TopLevelDisposedFlag = topLevelDisposedFlag;
      querySettings.m_queryId = PlinqEtwProvider.NextQueryId();
      return querySettings;
    }

    internal QuerySettings WithDefaults()
    {
      QuerySettings querySettings = this;
      if (querySettings.TaskScheduler == null)
        querySettings.TaskScheduler = TaskScheduler.Default;
      if (!querySettings.DegreeOfParallelism.HasValue)
        querySettings.DegreeOfParallelism = new int?(Scheduling.GetDefaultDegreeOfParallelism());
      if (!querySettings.ExecutionMode.HasValue)
        querySettings.ExecutionMode = new ParallelExecutionMode?(ParallelExecutionMode.Default);
      if (!querySettings.MergeOptions.HasValue)
        querySettings.MergeOptions = new ParallelMergeOptions?(ParallelMergeOptions.Default);
      ParallelMergeOptions? mergeOptions = querySettings.MergeOptions;
      if ((mergeOptions.GetValueOrDefault() != ParallelMergeOptions.Default ? 0 : (mergeOptions.HasValue ? 1 : 0)) != 0)
        querySettings.MergeOptions = new ParallelMergeOptions?(ParallelMergeOptions.AutoBuffered);
      return querySettings;
    }

    public void CleanStateAtQueryEnd()
    {
      this.m_cancellationState.MergedCancellationTokenSource.Dispose();
    }
  }
}
