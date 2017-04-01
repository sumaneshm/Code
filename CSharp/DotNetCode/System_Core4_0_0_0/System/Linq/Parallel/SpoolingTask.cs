// Type: System.Linq.Parallel.SpoolingTask
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal static class SpoolingTask
  {
    internal static void SpoolStopAndGo<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, SynchronousChannel<TInputOutput>[] channels, TaskScheduler taskScheduler)
    {
      Task rootTask = new Task((Action) (() =>
      {
        int local_0 = partitions.PartitionCount - 1;
        for (int local_1 = 0; local_1 < local_0; ++local_1)
          new StopAndGoSpoolingTask<TInputOutput, TIgnoreKey>(local_1, groupState, partitions[local_1], channels[local_1]).RunAsynchronously(taskScheduler);
        new StopAndGoSpoolingTask<TInputOutput, TIgnoreKey>(local_0, groupState, partitions[local_0], channels[local_0]).RunSynchronously(taskScheduler);
      }));
      groupState.QueryBegin(rootTask);
      rootTask.RunSynchronously(taskScheduler);
      groupState.QueryEnd(false);
    }

    internal static void SpoolPipeline<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, AsynchronousChannel<TInputOutput>[] channels, TaskScheduler taskScheduler)
    {
      Task rootTask = new Task((Action) (() =>
      {
        for (int local_0 = 0; local_0 < partitions.PartitionCount; ++local_0)
          new PipelineSpoolingTask<TInputOutput, TIgnoreKey>(local_0, groupState, partitions[local_0], channels[local_0]).RunAsynchronously(taskScheduler);
      }));
      groupState.QueryBegin(rootTask);
      rootTask.Start(taskScheduler);
    }

    internal static void SpoolForAll<TInputOutput, TIgnoreKey>(QueryTaskGroupState groupState, PartitionedStream<TInputOutput, TIgnoreKey> partitions, TaskScheduler taskScheduler)
    {
      Task rootTask = new Task((Action) (() =>
      {
        int local_0 = partitions.PartitionCount - 1;
        for (int local_1 = 0; local_1 < local_0; ++local_1)
          new ForAllSpoolingTask<TInputOutput, TIgnoreKey>(local_1, groupState, partitions[local_1]).RunAsynchronously(taskScheduler);
        new ForAllSpoolingTask<TInputOutput, TIgnoreKey>(local_0, groupState, partitions[local_0]).RunSynchronously(taskScheduler);
      }));
      groupState.QueryBegin(rootTask);
      rootTask.RunSynchronously(taskScheduler);
      groupState.QueryEnd(false);
    }
  }
}
