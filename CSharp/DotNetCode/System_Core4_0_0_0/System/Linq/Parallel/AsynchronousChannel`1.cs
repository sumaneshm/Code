// Type: System.Linq.Parallel.AsynchronousChannel`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class AsynchronousChannel<T> : IDisposable
  {
    private T[][] m_buffer;
    private readonly int m_index;
    private volatile int m_producerBufferIndex;
    private volatile int m_consumerBufferIndex;
    private volatile bool m_done;
    private T[] m_producerChunk;
    private int m_producerChunkIndex;
    private T[] m_consumerChunk;
    private int m_consumerChunkIndex;
    private int m_chunkSize;
    private ManualResetEventSlim m_producerEvent;
    private IntValueEvent m_consumerEvent;
    private volatile int m_producerIsWaiting;
    private volatile int m_consumerIsWaiting;
    private CancellationToken m_cancellationToken;

    internal bool IsFull
    {
      get
      {
        int num1 = this.m_producerBufferIndex;
        int num2 = this.m_consumerBufferIndex;
        if (num1 == num2 - 1)
          return true;
        if (num2 == 0)
          return num1 == this.m_buffer.Length - 1;
        else
          return false;
      }
    }

    internal bool IsChunkBufferEmpty
    {
      get
      {
        return this.m_producerBufferIndex == this.m_consumerBufferIndex;
      }
    }

    internal bool IsDone
    {
      get
      {
        return this.m_done;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal AsynchronousChannel(int index, int chunkSize, CancellationToken cancellationToken, IntValueEvent consumerEvent)
      : this(index, 512, chunkSize, cancellationToken, consumerEvent)
    {
    }

    internal AsynchronousChannel(int index, int capacity, int chunkSize, CancellationToken cancellationToken, IntValueEvent consumerEvent)
    {
      if (chunkSize == 0)
        chunkSize = Scheduling.GetDefaultChunkSize<T>();
      this.m_index = index;
      this.m_buffer = new T[capacity + 1][];
      this.m_producerBufferIndex = 0;
      this.m_consumerBufferIndex = 0;
      this.m_producerEvent = new ManualResetEventSlim();
      this.m_consumerEvent = consumerEvent;
      this.m_chunkSize = chunkSize;
      this.m_producerChunk = new T[chunkSize];
      this.m_producerChunkIndex = 0;
      this.m_cancellationToken = cancellationToken;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal void FlushBuffers()
    {
      this.FlushCachedChunk();
    }

    internal void SetDone()
    {
      this.m_done = true;
      lock (this)
      {
        if (this.m_consumerEvent == null)
          return;
        this.m_consumerEvent.Set(this.m_index);
      }
    }

    internal void Enqueue(T item)
    {
      int index = this.m_producerChunkIndex;
      this.m_producerChunk[index] = item;
      if (index == this.m_chunkSize - 1)
      {
        this.EnqueueChunk(this.m_producerChunk);
        this.m_producerChunk = new T[this.m_chunkSize];
      }
      this.m_producerChunkIndex = (index + 1) % this.m_chunkSize;
    }

    private void EnqueueChunk(T[] chunk)
    {
      if (this.IsFull)
        this.WaitUntilNonFull();
      int index = this.m_producerBufferIndex;
      this.m_buffer[index] = chunk;
      Interlocked.Exchange(ref this.m_producerBufferIndex, (index + 1) % this.m_buffer.Length);
      if (this.m_consumerIsWaiting != 1 || this.IsChunkBufferEmpty)
        return;
      this.m_consumerIsWaiting = 0;
      this.m_consumerEvent.Set(this.m_index);
    }

    private void WaitUntilNonFull()
    {
      do
      {
        this.m_producerEvent.Reset();
        Interlocked.Exchange(ref this.m_producerIsWaiting, 1);
        if (this.IsFull)
          this.m_producerEvent.Wait(this.m_cancellationToken);
        else
          this.m_producerIsWaiting = 0;
      }
      while (this.IsFull);
    }

    private void FlushCachedChunk()
    {
      if (this.m_producerChunk == null || this.m_producerChunkIndex == 0)
        return;
      T[] chunk = new T[this.m_producerChunkIndex];
      Array.Copy((Array) this.m_producerChunk, (Array) chunk, this.m_producerChunkIndex);
      this.EnqueueChunk(chunk);
      this.m_producerChunk = (T[]) null;
    }

    internal bool TryDequeue(ref T item)
    {
      if (this.m_consumerChunk == null)
      {
        if (!this.TryDequeueChunk(ref this.m_consumerChunk))
          return false;
        this.m_consumerChunkIndex = 0;
      }
      item = this.m_consumerChunk[this.m_consumerChunkIndex];
      ++this.m_consumerChunkIndex;
      if (this.m_consumerChunkIndex == this.m_consumerChunk.Length)
        this.m_consumerChunk = (T[]) null;
      return true;
    }

    private bool TryDequeueChunk(ref T[] chunk)
    {
      if (this.IsChunkBufferEmpty)
        return false;
      chunk = this.InternalDequeueChunk();
      return true;
    }

    internal bool TryDequeue(ref T item, ref bool isDone)
    {
      isDone = false;
      if (this.m_consumerChunk == null)
      {
        if (!this.TryDequeueChunk(ref this.m_consumerChunk, ref isDone))
          return false;
        this.m_consumerChunkIndex = 0;
      }
      item = this.m_consumerChunk[this.m_consumerChunkIndex];
      ++this.m_consumerChunkIndex;
      if (this.m_consumerChunkIndex == this.m_consumerChunk.Length)
        this.m_consumerChunk = (T[]) null;
      return true;
    }

    private bool TryDequeueChunk(ref T[] chunk, ref bool isDone)
    {
      isDone = false;
      while (this.IsChunkBufferEmpty)
      {
        if (this.IsDone && this.IsChunkBufferEmpty)
        {
          isDone = true;
          return false;
        }
        else
        {
          Interlocked.Exchange(ref this.m_consumerIsWaiting, 1);
          if (this.IsChunkBufferEmpty && !this.IsDone)
            return false;
          this.m_consumerIsWaiting = 0;
        }
      }
      chunk = this.InternalDequeueChunk();
      return true;
    }

    private T[] InternalDequeueChunk()
    {
      int index = this.m_consumerBufferIndex;
      T[] objArray = this.m_buffer[index];
      this.m_buffer[index] = (T[]) null;
      Interlocked.Exchange(ref this.m_consumerBufferIndex, (index + 1) % this.m_buffer.Length);
      if (this.m_producerIsWaiting == 1 && !this.IsFull)
      {
        this.m_producerIsWaiting = 0;
        this.m_producerEvent.Set();
      }
      return objArray;
    }

    internal void DoneWithDequeueWait()
    {
      this.m_consumerIsWaiting = 0;
    }

    public void Dispose()
    {
      lock (this)
      {
        this.m_producerEvent.Dispose();
        this.m_producerEvent = (ManualResetEventSlim) null;
        this.m_consumerEvent = (IntValueEvent) null;
      }
    }
  }
}
