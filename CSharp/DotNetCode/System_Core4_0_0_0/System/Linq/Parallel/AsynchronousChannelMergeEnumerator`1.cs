// Type: System.Linq.Parallel.AsynchronousChannelMergeEnumerator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Parallel
{
  internal sealed class AsynchronousChannelMergeEnumerator<T> : MergeEnumerator<T>
  {
    private AsynchronousChannel<T>[] m_channels;
    private IntValueEvent m_consumerEvent;
    private bool[] m_done;
    private int m_channelIndex;
    private T m_currentElement;

    public override T Current
    {
      get
      {
        if (this.m_channelIndex == -1 || this.m_channelIndex == this.m_channels.Length)
          throw new InvalidOperationException(System.Linq.SR.GetString("PLINQ_CommonEnumerator_Current_NotStarted"));
        else
          return this.m_currentElement;
      }
    }

    internal AsynchronousChannelMergeEnumerator(QueryTaskGroupState taskGroupState, AsynchronousChannel<T>[] channels, IntValueEvent consumerEvent)
      : base(taskGroupState)
    {
      this.m_channels = channels;
      this.m_channelIndex = -1;
      this.m_done = new bool[this.m_channels.Length];
      this.m_consumerEvent = consumerEvent;
    }

    public override bool MoveNext()
    {
      int index = this.m_channelIndex;
      if (index == -1)
        this.m_channelIndex = index = 0;
      if (index == this.m_channels.Length)
        return false;
      if (this.m_done[index] || !this.m_channels[index].TryDequeue(ref this.m_currentElement))
        return this.MoveNextSlowPath();
      this.m_channelIndex = (index + 1) % this.m_channels.Length;
      return true;
    }

    private bool MoveNextSlowPath()
    {
      int num1 = 0;
      int num2 = this.m_channelIndex;
      int index1;
      while ((index1 = this.m_channelIndex) != this.m_channels.Length)
      {
        AsynchronousChannel<T> asynchronousChannel = this.m_channels[index1];
        bool flag = this.m_done[index1];
        if (!flag && asynchronousChannel.TryDequeue(ref this.m_currentElement))
        {
          this.m_channelIndex = (index1 + 1) % this.m_channels.Length;
          return true;
        }
        else
        {
          if (!flag && asynchronousChannel.IsDone)
          {
            if (!asynchronousChannel.IsChunkBufferEmpty)
            {
              asynchronousChannel.TryDequeue(ref this.m_currentElement);
              return true;
            }
            else
            {
              this.m_done[index1] = true;
              flag = true;
              asynchronousChannel.Dispose();
            }
          }
          if (flag && ++num1 == this.m_channels.Length)
          {
            int length;
            this.m_channelIndex = length = this.m_channels.Length;
            break;
          }
          else
          {
            int num3;
            this.m_channelIndex = num3 = (index1 + 1) % this.m_channels.Length;
            if (num3 == num2)
            {
              try
              {
                num1 = 0;
                for (int index2 = 0; index2 < this.m_channels.Length; ++index2)
                {
                  bool isDone = false;
                  if (!this.m_done[index2] && this.m_channels[index2].TryDequeue(ref this.m_currentElement, ref isDone))
                    return true;
                  if (isDone)
                  {
                    if (!this.m_done[index2])
                      this.m_done[index2] = true;
                    if (++num1 == this.m_channels.Length)
                    {
                      this.m_channelIndex = num3 = this.m_channels.Length;
                      break;
                    }
                  }
                }
                if (num3 != this.m_channels.Length)
                {
                  this.m_consumerEvent.Wait();
                  int num4;
                  this.m_channelIndex = num4 = this.m_consumerEvent.Value;
                  this.m_consumerEvent.Reset();
                  num2 = num4;
                  num1 = 0;
                }
                else
                  break;
              }
              finally
              {
                for (int index2 = 0; index2 < this.m_channels.Length; ++index2)
                {
                  if (!this.m_done[index2])
                    this.m_channels[index2].DoneWithDequeueWait();
                }
              }
            }
          }
        }
      }
      this.m_taskGroupState.QueryEnd(false);
      return false;
    }

    public override void Dispose()
    {
      if (this.m_consumerEvent == null)
        return;
      base.Dispose();
      this.m_consumerEvent.Dispose();
      this.m_consumerEvent = (IntValueEvent) null;
    }
  }
}
