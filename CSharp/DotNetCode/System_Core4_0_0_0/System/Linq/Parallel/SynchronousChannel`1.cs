// Type: System.Linq.Parallel.SynchronousChannel`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal sealed class SynchronousChannel<T>
  {
    private Queue<T> m_queue;

    internal int Count
    {
      get
      {
        return this.m_queue.Count;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal SynchronousChannel()
    {
    }

    internal void Init()
    {
      this.m_queue = new Queue<T>();
    }

    internal void Enqueue(T item)
    {
      this.m_queue.Enqueue(item);
    }

    internal T Dequeue()
    {
      return this.m_queue.Dequeue();
    }

    internal void SetDone()
    {
    }

    internal void CopyTo(T[] array, int arrayIndex)
    {
      this.m_queue.CopyTo(array, arrayIndex);
    }
  }
}
