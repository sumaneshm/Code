// Type: System.Linq.Parallel.SynchronousChannelMergeEnumerator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Parallel
{
  internal sealed class SynchronousChannelMergeEnumerator<T> : MergeEnumerator<T>
  {
    private SynchronousChannel<T>[] m_channels;
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

    internal SynchronousChannelMergeEnumerator(QueryTaskGroupState taskGroupState, SynchronousChannel<T>[] channels)
      : base(taskGroupState)
    {
      this.m_channels = channels;
      this.m_channelIndex = -1;
    }

    public override bool MoveNext()
    {
      if (this.m_channelIndex == -1)
        this.m_channelIndex = 0;
      for (; this.m_channelIndex != this.m_channels.Length; ++this.m_channelIndex)
      {
        SynchronousChannel<T> synchronousChannel = this.m_channels[this.m_channelIndex];
        if (synchronousChannel.Count == 0)
          continue;
        this.m_currentElement = synchronousChannel.Dequeue();
        return true;
      }
      return false;
    }
  }
}
