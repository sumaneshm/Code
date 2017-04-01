// Type: System.Linq.Parallel.OrderPreservingPipeliningMergeHelper`2
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
  internal class OrderPreservingPipeliningMergeHelper<TOutput, TKey> : IMergeHelper<TOutput>
  {
    private readonly QueryTaskGroupState m_taskGroupState;
    private readonly PartitionedStream<TOutput, TKey> m_partitions;
    private readonly TaskScheduler m_taskScheduler;
    private readonly bool m_autoBuffered;
    private readonly Queue<Pair<TKey, TOutput>>[] m_buffers;
    private readonly bool[] m_producerDone;
    private readonly bool[] m_producerWaiting;
    private readonly bool[] m_consumerWaiting;
    private readonly object[] m_bufferLocks;
    private IComparer<Producer<TKey>> m_producerComparer;
    internal const int INITIAL_BUFFER_SIZE = 128;
    internal const int STEAL_BUFFER_SIZE = 1024;
    internal const int MAX_BUFFER_SIZE = 8192;

    internal OrderPreservingPipeliningMergeHelper(PartitionedStream<TOutput, TKey> partitions, TaskScheduler taskScheduler, CancellationState cancellationState, bool autoBuffered, int queryId, IComparer<TKey> keyComparer)
    {
      this.m_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
      this.m_partitions = partitions;
      this.m_taskScheduler = taskScheduler;
      this.m_autoBuffered = autoBuffered;
      int partitionCount = this.m_partitions.PartitionCount;
      this.m_buffers = new Queue<Pair<TKey, TOutput>>[partitionCount];
      this.m_producerDone = new bool[partitionCount];
      this.m_consumerWaiting = new bool[partitionCount];
      this.m_producerWaiting = new bool[partitionCount];
      this.m_bufferLocks = new object[partitionCount];
      if (keyComparer == Util.GetDefaultComparer<int>())
        this.m_producerComparer = (IComparer<Producer<TKey>>) new ProducerComparerInt();
      else
        this.m_producerComparer = (IComparer<Producer<TKey>>) new OrderPreservingPipeliningMergeHelper<TOutput, TKey>.ProducerComparer(keyComparer);
    }

    void IMergeHelper<TOutput>.Execute()
    {
      OrderPreservingPipeliningSpoolingTask<TOutput, TKey>.Spool(this.m_taskGroupState, this.m_partitions, this.m_consumerWaiting, this.m_producerWaiting, this.m_producerDone, this.m_buffers, this.m_bufferLocks, this.m_taskScheduler, this.m_autoBuffered);
    }

    IEnumerator<TOutput> IMergeHelper<TOutput>.GetEnumerator()
    {
      return (IEnumerator<TOutput>) new OrderPreservingPipeliningMergeHelper<TOutput, TKey>.OrderedPipeliningMergeEnumerator(this, this.m_producerComparer);
    }

    public TOutput[] GetResultsAsArray()
    {
      throw new InvalidOperationException();
    }

    private class ProducerComparer : IComparer<Producer<TKey>>
    {
      private IComparer<TKey> _keyComparer;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ProducerComparer(IComparer<TKey> keyComparer)
      {
        this._keyComparer = keyComparer;
      }

      public int Compare(Producer<TKey> x, Producer<TKey> y)
      {
        return this._keyComparer.Compare(y.MaxKey, x.MaxKey);
      }
    }

    private class OrderedPipeliningMergeEnumerator : MergeEnumerator<TOutput>
    {
      private OrderPreservingPipeliningMergeHelper<TOutput, TKey> m_mergeHelper;
      private readonly FixedMaxHeap<Producer<TKey>> m_producerHeap;
      private readonly TOutput[] m_producerNextElement;
      private readonly Queue<Pair<TKey, TOutput>>[] m_privateBuffer;
      private bool m_initialized;

      public override TOutput Current
      {
        get
        {
          return this.m_producerNextElement[this.m_producerHeap.MaxValue.ProducerIndex];
        }
      }

      internal OrderedPipeliningMergeEnumerator(OrderPreservingPipeliningMergeHelper<TOutput, TKey> mergeHelper, IComparer<Producer<TKey>> producerComparer)
        : base(mergeHelper.m_taskGroupState)
      {
        int partitionCount = mergeHelper.m_partitions.PartitionCount;
        this.m_mergeHelper = mergeHelper;
        this.m_producerHeap = new FixedMaxHeap<Producer<TKey>>(partitionCount, producerComparer);
        this.m_privateBuffer = new Queue<Pair<TKey, TOutput>>[partitionCount];
        this.m_producerNextElement = new TOutput[partitionCount];
      }

      public override bool MoveNext()
      {
        if (!this.m_initialized)
        {
          this.m_initialized = true;
          for (int index = 0; index < this.m_mergeHelper.m_partitions.PartitionCount; ++index)
          {
            Pair<TKey, TOutput> element = new Pair<TKey, TOutput>();
            if (this.TryWaitForElement(index, ref element))
            {
              this.m_producerHeap.Insert(new Producer<TKey>(element.First, index));
              this.m_producerNextElement[index] = element.Second;
            }
            else
              this.ThrowIfInTearDown();
          }
        }
        else
        {
          if (this.m_producerHeap.Count == 0)
            return false;
          int index = this.m_producerHeap.MaxValue.ProducerIndex;
          Pair<TKey, TOutput> element = new Pair<TKey, TOutput>();
          if (this.TryGetPrivateElement(index, ref element) || this.TryWaitForElement(index, ref element))
          {
            this.m_producerHeap.ReplaceMax(new Producer<TKey>(element.First, index));
            this.m_producerNextElement[index] = element.Second;
          }
          else
          {
            this.ThrowIfInTearDown();
            this.m_producerHeap.RemoveMax();
          }
        }
        return this.m_producerHeap.Count > 0;
      }

      private void ThrowIfInTearDown()
      {
        if (!this.m_mergeHelper.m_taskGroupState.CancellationState.MergedCancellationToken.IsCancellationRequested)
          return;
        try
        {
          object[] objArray = this.m_mergeHelper.m_bufferLocks;
          for (int index = 0; index < objArray.Length; ++index)
          {
            lock (objArray[index])
              Monitor.Pulse(objArray[index]);
          }
          this.m_taskGroupState.QueryEnd(false);
        }
        finally
        {
          this.m_producerHeap.Clear();
        }
      }

      private bool TryWaitForElement(int producer, ref Pair<TKey, TOutput> element)
      {
        Queue<Pair<TKey, TOutput>> queue = this.m_mergeHelper.m_buffers[producer];
        object obj = this.m_mergeHelper.m_bufferLocks[producer];
        lock (obj)
        {
          if (queue.Count == 0)
          {
            if (this.m_mergeHelper.m_producerDone[producer])
            {
              element = new Pair<TKey, TOutput>();
              return false;
            }
            else
            {
              this.m_mergeHelper.m_consumerWaiting[producer] = true;
              Monitor.Wait(obj);
              if (queue.Count == 0)
              {
                element = new Pair<TKey, TOutput>();
                return false;
              }
            }
          }
          if (this.m_mergeHelper.m_producerWaiting[producer])
          {
            Monitor.Pulse(obj);
            this.m_mergeHelper.m_producerWaiting[producer] = false;
          }
          if (queue.Count < 1024)
          {
            element = queue.Dequeue();
            return true;
          }
          else
          {
            this.m_privateBuffer[producer] = this.m_mergeHelper.m_buffers[producer];
            this.m_mergeHelper.m_buffers[producer] = new Queue<Pair<TKey, TOutput>>(128);
          }
        }
        this.TryGetPrivateElement(producer, ref element);
        return true;
      }

      private bool TryGetPrivateElement(int producer, ref Pair<TKey, TOutput> element)
      {
        Queue<Pair<TKey, TOutput>> queue = this.m_privateBuffer[producer];
        if (queue != null)
        {
          if (queue.Count > 0)
          {
            element = queue.Dequeue();
            return true;
          }
          else
            this.m_privateBuffer[producer] = (Queue<Pair<TKey, TOutput>>) null;
        }
        return false;
      }

      public override void Dispose()
      {
        int length = this.m_mergeHelper.m_buffers.Length;
        for (int index = 0; index < length; ++index)
        {
          object obj = this.m_mergeHelper.m_bufferLocks[index];
          lock (obj)
          {
            if (this.m_mergeHelper.m_producerWaiting[index])
              Monitor.Pulse(obj);
          }
        }
        base.Dispose();
      }
    }
  }
}
