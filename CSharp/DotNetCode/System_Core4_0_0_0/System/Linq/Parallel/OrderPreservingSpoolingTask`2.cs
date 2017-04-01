// Type: System.Linq.Parallel.OrderPreservingSpoolingTask`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class OrderPreservingSpoolingTask<TInputOutput, TKey> : SpoolingTaskBase
  {
    private Shared<TInputOutput[]> m_results;
    private SortHelper<TInputOutput> m_sortHelper;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private OrderPreservingSpoolingTask(int taskIndex, QueryTaskGroupState groupState, Shared<TInputOutput[]> results, SortHelper<TInputOutput> sortHelper)
      : base(taskIndex, groupState)
    {
      this.m_results = results;
      this.m_sortHelper = sortHelper;
    }

    internal static void Spool(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TKey> partitions, Shared<TInputOutput[]> results, TaskScheduler taskScheduler)
    {
      int maxToRunInParallel = partitions.PartitionCount - 1;
      SortHelper<TInputOutput, TKey>[] sortHelpers = SortHelper<TInputOutput, TKey>.GenerateSortHelpers(partitions, groupState);
      Task rootTask = new Task((Action) (() =>
      {
        for (int local_0 = 0; local_0 < maxToRunInParallel; ++local_0)
          new OrderPreservingSpoolingTask<TInputOutput, TKey>(local_0, groupState, results, (SortHelper<TInputOutput>) sortHelpers[local_0]).RunAsynchronously(taskScheduler);
        new OrderPreservingSpoolingTask<TInputOutput, TKey>(maxToRunInParallel, groupState, results, (SortHelper<TInputOutput>) sortHelpers[maxToRunInParallel]).RunSynchronously(taskScheduler);
      }));
      groupState.QueryBegin(rootTask);
      rootTask.RunSynchronously(taskScheduler);
      for (int index = 0; index < sortHelpers.Length; ++index)
        sortHelpers[index].Dispose();
      groupState.QueryEnd(false);
    }

    protected override void SpoolingWork()
    {
      TInputOutput[] inputOutputArray = this.m_sortHelper.Sort();
      if (this.m_groupState.CancellationState.MergedCancellationToken.IsCancellationRequested || this.m_taskIndex != 0)
        return;
      this.m_results.Value = inputOutputArray;
    }
  }
}
