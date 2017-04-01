// Type: System.Linq.Expressions.Set`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Expressions
{
  internal sealed class Set<T> : ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private readonly Dictionary<T, object> _data;

    public int Count
    {
      get
      {
        return this._data.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    internal Set()
    {
      this._data = new Dictionary<T, object>();
    }

    internal Set(IEqualityComparer<T> comparer)
    {
      this._data = new Dictionary<T, object>(comparer);
    }

    internal Set(IList<T> list)
    {
      this._data = new Dictionary<T, object>(list.Count);
      foreach (T obj in (IEnumerable<T>) list)
        this.Add(obj);
    }

    internal Set(IEnumerable<T> list)
    {
      this._data = new Dictionary<T, object>();
      foreach (T obj in list)
        this.Add(obj);
    }

    internal Set(int capacity)
    {
      this._data = new Dictionary<T, object>(capacity);
    }

    public void Add(T item)
    {
      this._data[item] = (object) null;
    }

    public void Clear()
    {
      this._data.Clear();
    }

    public bool Contains(T item)
    {
      return this._data.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      this._data.Keys.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
      return this._data.Remove(item);
    }

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) this._data.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this._data.Keys.GetEnumerator();
    }
  }
}
