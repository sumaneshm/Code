// Type: System.Linq.Parallel.OrderPreservingPipeliningSpoolingTask`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class OrderPreservingPipeliningSpoolingTask<TOutput, TKey> : SpoolingTaskBase
  {
    private readonly QueryTaskGroupState m_taskGroupState;
    private readonly TaskScheduler m_taskScheduler;
    private readonly QueryOperatorEnumerator<TOutput, TKey> m_partition;
    private readonly bool[] m_consumerWaiting;
    private readonly bool[] m_producerWaiting;
    private readonly bool[] m_producerDone;
    private readonly int m_partitionIndex;
    private readonly Queue<Pair<TKey, TOutput>>[] m_buffers;
    private readonly object m_bufferLock;
    private readonly bool m_autoBuffered;
    private const int PRODUCER_BUFFER_AUTO_SIZE = 16;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal OrderPreservingPipeliningSpoolingTask(QueryOperatorEnumerator<TOutput, TKey> partition, QueryTaskGroupState taskGroupState, bool[] consumerWaiting, bool[] producerWaiting, bool[] producerDone, int partitionIndex, Queue<Pair<TKey, TOutput>>[] buffers, object bufferLock, TaskScheduler taskScheduler, bool autoBuffered)
      : base(partitionIndex, taskGroupState)
    {
      this.m_partition = partition;
      this.m_taskGroupState = taskGroupState;
      this.m_producerDone = producerDone;
      this.m_consumerWaiting = consumerWaiting;
      this.m_producerWaiting = producerWaiting;
      this.m_partitionIndex = partitionIndex;
      this.m_buffers = buffers;
      this.m_bufferLock = bufferLock;
      this.m_taskScheduler = taskScheduler;
      this.m_autoBuffered = autoBuffered;
    }

    protected override void SpoolingWork()
    {
      TOutput currentElement = default (TOutput);
      TKey currentKey = default (TKey);
      int length = this.m_autoBuffered ? 16 : 1;
      Pair<TKey, TOutput>[] pairArray = new Pair<TKey, TOutput>[length];
      QueryOperatorEnumerator<TOutput, TKey> operatorEnumerator = this.m_partition;
      CancellationToken cancellationToken = this.m_taskGroupState.CancellationState.MergedCancellationToken;
      int index;
      do
      {
        for (index = 0; index < length && operatorEnumerator.MoveNext(ref currentElement, ref currentKey); ++index)
          pairArray[index] = new Pair<TKey, TOutput>(currentKey, currentElement);
        if (index == 0)
          break;
        lock (this.m_bufferLock)
        {
          if (cancellationToken.IsCancellationRequested)
            break;
          for (int local_7 = 0; local_7 < index; ++local_7)
            this.m_buffers[this.m_partitionIndex].Enqueue(pairArray[local_7]);
          if (this.m_consumerWaiting[this.m_partitionIndex])
          {
            Monitor.Pulse(this.m_bufferLock);
            this.m_consumerWaiting[this.m_partitionIndex] = false;
          }
          if (this.m_buffers[this.m_partitionIndex].Count >= 8192)
          {
            this.m_producerWaiting[this.m_partitionIndex] = true;
            Monitor.Wait(this.m_bufferLock);
          }
        }
      }
      while (index == length);
    }

    public static void Spool(QueryTaskGroupState groupState, PartitionedStream<TOutput, TKey> partitions, bool[] consumerWaiting, bool[] producerWaiting, bool[] producerDone, Queue<Pair<TKey, TOutput>>[] buffers, object[] bufferLocks, TaskScheduler taskScheduler, bool autoBuffered)
    {
      int degreeOfParallelism = partitions.PartitionCount;
      for (int index = 0; index < degreeOfParallelism; ++index)
      {
        buffers[index] = new Queue<Pair<TKey, TOutput>>(128);
        bufferLocks[index] = new object();
      }
      Task rootTask = new Task((Action) (() =>
      {
        for (int local_0 = 0; local_0 < degreeOfParallelism; ++local_0)
          new OrderPreservingPipeliningSpoolingTask<TOutput, TKey>(partitions[local_0], groupState, consumerWaiting, producerWaiting, producerDone, local_0, buffers, bufferLocks[local_0], taskScheduler, autoBuffered).RunAsynchronously(taskScheduler);
      }));
      groupState.QueryBegin(rootTask);
      rootTask.Start(taskScheduler);
    }

    protected override void SpoolingFinally()
    {
      lock (this.m_bufferLock)
      {
        this.m_producerDone[this.m_partitionIndex] = true;
        if (this.m_consumerWaiting[this.m_partitionIndex])
        {
          Monitor.Pulse(this.m_bufferLock);
          this.m_consumerWaiting[this.m_partitionIndex] = false;
        }
      }
      base.SpoolingFinally();
      this.m_partition.Dispose();
    }
  }
}
