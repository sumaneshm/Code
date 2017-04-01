// Type: System.Linq.Parallel.QueryResults`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal abstract class QueryResults<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
  {
    internal virtual bool IsIndexible
    {
      get
      {
        return false;
      }
    }

    internal virtual int ElementsCount
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public T this[int index]
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetElement(index);
      }
      set
      {
        throw new NotSupportedException();
      }
    }

    public int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.ElementsCount;
      }
    }

    bool ICollection<T>.IsReadOnly
    {
      get
      {
        return true;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected QueryResults()
    {
    }

    internal abstract void GivePartitionedStream(IPartitionedStreamRecipient<T> recipient);

    internal virtual T GetElement(int index)
    {
      throw new NotSupportedException();
    }

    int IList<T>.IndexOf(T item)
    {
      throw new NotSupportedException();
    }

    void IList<T>.Insert(int index, T item)
    {
      throw new NotSupportedException();
    }

    void IList<T>.RemoveAt(int index)
    {
      throw new NotSupportedException();
    }

    void ICollection<T>.Add(T item)
    {
      throw new NotSupportedException();
    }

    void ICollection<T>.Clear()
    {
      throw new NotSupportedException();
    }

    bool ICollection<T>.Contains(T item)
    {
      throw new NotSupportedException();
    }

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
      throw new NotSupportedException();
    }

    bool ICollection<T>.Remove(T item)
    {
      throw new NotSupportedException();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      for (int index = 0; index < this.Count; ++index)
        yield return this[index];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
