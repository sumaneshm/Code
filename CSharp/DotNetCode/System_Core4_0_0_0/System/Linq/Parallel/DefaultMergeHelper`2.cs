// Type: System.Linq.Parallel.DefaultMergeHelper`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class DefaultMergeHelper<TInputOutput, TIgnoreKey> : IMergeHelper<TInputOutput>
  {
    private QueryTaskGroupState m_taskGroupState;
    private PartitionedStream<TInputOutput, TIgnoreKey> m_partitions;
    private AsynchronousChannel<TInputOutput>[] m_asyncChannels;
    private SynchronousChannel<TInputOutput>[] m_syncChannels;
    private IEnumerator<TInputOutput> m_channelEnumerator;
    private TaskScheduler m_taskScheduler;
    private bool m_ignoreOutput;

    internal DefaultMergeHelper(PartitionedStream<TInputOutput, TIgnoreKey> partitions, bool ignoreOutput, ParallelMergeOptions options, TaskScheduler taskScheduler, CancellationState cancellationState, int queryId)
    {
      this.m_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
      this.m_partitions = partitions;
      this.m_taskScheduler = taskScheduler;
      this.m_ignoreOutput = ignoreOutput;
      IntValueEvent consumerEvent = new IntValueEvent();
      if (ignoreOutput)
        return;
      if (options != ParallelMergeOptions.FullyBuffered)
      {
        if (partitions.PartitionCount > 1)
        {
          this.m_asyncChannels = MergeExecutor<TInputOutput>.MakeAsynchronousChannels(partitions.PartitionCount, options, consumerEvent, cancellationState.MergedCancellationToken);
          this.m_channelEnumerator = (IEnumerator<TInputOutput>) new AsynchronousChannelMergeEnumerator<TInputOutput>(this.m_taskGroupState, this.m_asyncChannels, consumerEvent);
        }
        else
          this.m_channelEnumerator = ExceptionAggregator.WrapQueryEnumerator<TInputOutput, TIgnoreKey>(partitions[0], this.m_taskGroupState.CancellationState).GetEnumerator();
      }
      else
      {
        this.m_syncChannels = MergeExecutor<TInputOutput>.MakeSynchronousChannels(partitions.PartitionCount);
        this.m_channelEnumerator = (IEnumerator<TInputOutput>) new SynchronousChannelMergeEnumerator<TInputOutput>(this.m_taskGroupState, this.m_syncChannels);
      }
    }

    void IMergeHelper<TInputOutput>.Execute()
    {
      if (this.m_asyncChannels != null)
        SpoolingTask.SpoolPipeline<TInputOutput, TIgnoreKey>(this.m_taskGroupState, this.m_partitions, this.m_asyncChannels, this.m_taskScheduler);
      else if (this.m_syncChannels != null)
      {
        SpoolingTask.SpoolStopAndGo<TInputOutput, TIgnoreKey>(this.m_taskGroupState, this.m_partitions, this.m_syncChannels, this.m_taskScheduler);
      }
      else
      {
        if (!this.m_ignoreOutput)
          return;
        SpoolingTask.SpoolForAll<TInputOutput, TIgnoreKey>(this.m_taskGroupState, this.m_partitions, this.m_taskScheduler);
      }
    }

    IEnumerator<TInputOutput> IMergeHelper<TInputOutput>.GetEnumerator()
    {
      return this.m_channelEnumerator;
    }

    public TInputOutput[] GetResultsAsArray()
    {
      if (this.m_syncChannels != null)
      {
        int length = 0;
        for (int index = 0; index < this.m_syncChannels.Length; ++index)
          length += this.m_syncChannels[index].Count;
        TInputOutput[] array = new TInputOutput[length];
        int arrayIndex = 0;
        for (int index = 0; index < this.m_syncChannels.Length; ++index)
        {
          this.m_syncChannels[index].CopyTo(array, arrayIndex);
          arrayIndex += this.m_syncChannels[index].Count;
        }
        return array;
      }
      else
      {
        List<TInputOutput> list = new List<TInputOutput>();
        foreach (TInputOutput inputOutput in (IMergeHelper<TInputOutput>) this)
          list.Add(inputOutput);
        return list.ToArray();
      }
    }
  }
}
