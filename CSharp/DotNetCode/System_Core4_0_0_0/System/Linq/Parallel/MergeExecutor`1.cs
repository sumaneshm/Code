// Type: System.Linq.Parallel.MergeExecutor`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class MergeExecutor<TInputOutput> : IEnumerable<TInputOutput>, IEnumerable
  {
    private IMergeHelper<TInputOutput> m_mergeHelper;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private MergeExecutor()
    {
    }

    internal static MergeExecutor<TInputOutput> Execute<TKey>(PartitionedStream<TInputOutput, TKey> partitions, bool ignoreOutput, ParallelMergeOptions options, TaskScheduler taskScheduler, bool isOrdered, CancellationState cancellationState, int queryId)
    {
      MergeExecutor<TInputOutput> mergeExecutor = new MergeExecutor<TInputOutput>();
      if (isOrdered && !ignoreOutput)
      {
        if (options != ParallelMergeOptions.FullyBuffered && !ExchangeUtilities.IsWorseThan(partitions.OrdinalIndexState, OrdinalIndexState.Increasing))
        {
          bool autoBuffered = options == ParallelMergeOptions.AutoBuffered;
          mergeExecutor.m_mergeHelper = partitions.PartitionCount <= 1 ? (IMergeHelper<TInputOutput>) new DefaultMergeHelper<TInputOutput, TKey>(partitions, false, options, taskScheduler, cancellationState, queryId) : (IMergeHelper<TInputOutput>) new OrderPreservingPipeliningMergeHelper<TInputOutput, TKey>(partitions, taskScheduler, cancellationState, autoBuffered, queryId, partitions.KeyComparer);
        }
        else
          mergeExecutor.m_mergeHelper = (IMergeHelper<TInputOutput>) new OrderPreservingMergeHelper<TInputOutput, TKey>(partitions, taskScheduler, cancellationState, queryId);
      }
      else
        mergeExecutor.m_mergeHelper = (IMergeHelper<TInputOutput>) new DefaultMergeHelper<TInputOutput, TKey>(partitions, ignoreOutput, options, taskScheduler, cancellationState, queryId);
      mergeExecutor.Execute();
      return mergeExecutor;
    }

    private void Execute()
    {
      this.m_mergeHelper.Execute();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public IEnumerator<TInputOutput> GetEnumerator()
    {
      return this.m_mergeHelper.GetEnumerator();
    }

    internal TInputOutput[] GetResultsAsArray()
    {
      return this.m_mergeHelper.GetResultsAsArray();
    }

    internal static AsynchronousChannel<TInputOutput>[] MakeAsynchronousChannels(int partitionCount, ParallelMergeOptions options, IntValueEvent consumerEvent, CancellationToken cancellationToken)
    {
      AsynchronousChannel<TInputOutput>[] asynchronousChannelArray = new AsynchronousChannel<TInputOutput>[partitionCount];
      int chunkSize = 0;
      if (options == ParallelMergeOptions.NotBuffered)
        chunkSize = 1;
      for (int index = 0; index < asynchronousChannelArray.Length; ++index)
        asynchronousChannelArray[index] = new AsynchronousChannel<TInputOutput>(index, chunkSize, cancellationToken, consumerEvent);
      return asynchronousChannelArray;
    }

    internal static SynchronousChannel<TInputOutput>[] MakeSynchronousChannels(int partitionCount)
    {
      SynchronousChannel<TInputOutput>[] synchronousChannelArray = new SynchronousChannel<TInputOutput>[partitionCount];
      for (int index = 0; index < synchronousChannelArray.Length; ++index)
        synchronousChannelArray[index] = new SynchronousChannel<TInputOutput>();
      return synchronousChannelArray;
    }
  }
}
