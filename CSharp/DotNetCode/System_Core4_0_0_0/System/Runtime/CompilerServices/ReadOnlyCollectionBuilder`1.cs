// Type: System.Runtime.CompilerServices.ReadOnlyCollectionBuilder`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime;
using System.Threading;

namespace System.Runtime.CompilerServices
{
  [Serializable]
  public sealed class ReadOnlyCollectionBuilder<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
  {
    private static readonly T[] _emptyArray = new T[0];
    private T[] _items;
    private int _size;
    private int _version;
    [NonSerialized]
    private object _syncRoot;
    private const int DefaultCapacity = 4;

    public int Capacity
    {
      get
      {
        return this._items.Length;
      }
      set
      {
        ContractUtils.Requires(value >= this._size, "value");
        if (value == this._items.Length)
          return;
        if (value > 0)
        {
          T[] objArray = new T[value];
          if (this._size > 0)
            Array.Copy((Array) this._items, 0, (Array) objArray, 0, this._size);
          this._items = objArray;
        }
        else
          this._items = ReadOnlyCollectionBuilder<T>._emptyArray;
      }
    }

    public int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._size;
      }
    }

    public T this[int index]
    {
      get
      {
        ContractUtils.Requires(index < this._size, "index");
        return this._items[index];
      }
      set
      {
        ContractUtils.Requires(index < this._size, "index");
        this._items[index] = value;
        ++this._version;
      }
    }

    bool ICollection<T>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    bool IList.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    bool IList.IsFixedSize
    {
      get
      {
        return false;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        return false;
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    static ReadOnlyCollectionBuilder()
    {
    }

    public ReadOnlyCollectionBuilder()
    {
      this._items = ReadOnlyCollectionBuilder<T>._emptyArray;
    }

    public ReadOnlyCollectionBuilder(int capacity)
    {
      ContractUtils.Requires(capacity >= 0, "capacity");
      this._items = new T[capacity];
    }

    public ReadOnlyCollectionBuilder(IEnumerable<T> collection)
    {
      ContractUtils.Requires(collection != null, "collection");
      ICollection<T> collection1 = collection as ICollection<T>;
      if (collection1 != null)
      {
        int count = collection1.Count;
        this._items = new T[count];
        collection1.CopyTo(this._items, 0);
        this._size = count;
      }
      else
      {
        this._size = 0;
        this._items = new T[4];
        foreach (T obj in collection)
          this.Add(obj);
      }
    }

    public int IndexOf(T item)
    {
      return Array.IndexOf<T>(this._items, item, 0, this._size);
    }

    public void Insert(int index, T item)
    {
      ContractUtils.Requires(index <= this._size, "index");
      if (this._size == this._items.Length)
        this.EnsureCapacity(this._size + 1);
      if (index < this._size)
        Array.Copy((Array) this._items, index, (Array) this._items, index + 1, this._size - index);
      this._items[index] = item;
      ++this._size;
      ++this._version;
    }

    public void RemoveAt(int index)
    {
      ContractUtils.Requires(index >= 0 && index < this._size, "index");
      --this._size;
      if (index < this._size)
        Array.Copy((Array) this._items, index + 1, (Array) this._items, index, this._size - index);
      this._items[this._size] = default (T);
      ++this._version;
    }

    public void Add(T item)
    {
      if (this._size == this._items.Length)
        this.EnsureCapacity(this._size + 1);
      this._items[this._size++] = item;
      ++this._version;
    }

    public void Clear()
    {
      if (this._size > 0)
      {
        Array.Clear((Array) this._items, 0, this._size);
        this._size = 0;
      }
      ++this._version;
    }

    public bool Contains(T item)
    {
      if ((object) item == null)
      {
        for (int index = 0; index < this._size; ++index)
        {
          if ((object) this._items[index] == null)
            return true;
        }
        return false;
      }
      else
      {
        EqualityComparer<T> @default = EqualityComparer<T>.Default;
        for (int index = 0; index < this._size; ++index)
        {
          if (@default.Equals(this._items[index], item))
            return true;
        }
        return false;
      }
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      Array.Copy((Array) this._items, 0, (Array) array, arrayIndex, this._size);
    }

    public bool Remove(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAt(index);
      return true;
    }

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) new ReadOnlyCollectionBuilder<T>.Enumerator(this);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    int IList.Add(object value)
    {
      ReadOnlyCollectionBuilder<T>.ValidateNullValue(value, "value");
      try
      {
        this.Add((T) value);
      }
      catch (InvalidCastException ex)
      {
        ReadOnlyCollectionBuilder<T>.ThrowInvalidTypeException(value, "value");
      }
      return this.Count - 1;
    }

    bool IList.Contains(object value)
    {
      if (ReadOnlyCollectionBuilder<T>.IsCompatibleObject(value))
        return this.Contains((T) value);
      else
        return false;
    }

    int IList.IndexOf(object value)
    {
      if (ReadOnlyCollectionBuilder<T>.IsCompatibleObject(value))
        return this.IndexOf((T) value);
      else
        return -1;
    }

    void IList.Insert(int index, object value)
    {
      ReadOnlyCollectionBuilder<T>.ValidateNullValue(value, "value");
      try
      {
        this.Insert(index, (T) value);
      }
      catch (InvalidCastException ex)
      {
        ReadOnlyCollectionBuilder<T>.ThrowInvalidTypeException(value, "value");
      }
    }

    void IList.Remove(object value)
    {
      if (!ReadOnlyCollectionBuilder<T>.IsCompatibleObject(value))
        return;
      this.Remove((T) value);
    }

    object IList.get_Item(int index)
    {
      return (object) this[index];
    }

    void IList.set_Item(int index, object value)
    {
      ReadOnlyCollectionBuilder<T>.ValidateNullValue(value, "value");
      try
      {
        this[index] = (T) value;
      }
      catch (InvalidCastException ex)
      {
        ReadOnlyCollectionBuilder<T>.ThrowInvalidTypeException(value, "value");
      }
    }

    void ICollection.CopyTo(Array array, int index)
    {
      ContractUtils.RequiresNotNull((object) array, "array");
      ContractUtils.Requires(array.Rank == 1, "array");
      Array.Copy((Array) this._items, 0, array, index, this._size);
    }

    public void Reverse()
    {
      this.Reverse(0, this.Count);
    }

    public void Reverse(int index, int count)
    {
      ContractUtils.Requires(index >= 0, "index");
      ContractUtils.Requires(count >= 0, "count");
      Array.Reverse((Array) this._items, index, count);
      ++this._version;
    }

    public T[] ToArray()
    {
      T[] objArray = new T[this._size];
      Array.Copy((Array) this._items, 0, (Array) objArray, 0, this._size);
      return objArray;
    }

    public ReadOnlyCollection<T> ToReadOnlyCollection()
    {
      T[] list = this._size != this._items.Length ? this.ToArray() : this._items;
      this._items = ReadOnlyCollectionBuilder<T>._emptyArray;
      this._size = 0;
      ++this._version;
      return (ReadOnlyCollection<T>) new TrueReadOnlyCollection<T>(list);
    }

    private void EnsureCapacity(int min)
    {
      if (this._items.Length >= min)
        return;
      int num = 4;
      if (this._items.Length > 0)
        num = this._items.Length * 2;
      if (num < min)
        num = min;
      this.Capacity = num;
    }

    private static bool IsCompatibleObject(object value)
    {
      if (value is T)
        return true;
      if (value == null)
        return (object) default (T) == null;
      else
        return false;
    }

    private static void ValidateNullValue(object value, string argument)
    {
      if (value == null && (object) default (T) != null)
        throw new ArgumentException(Strings.InvalidNullValue((object) typeof (T)), argument);
    }

    private static void ThrowInvalidTypeException(object value, string argument)
    {
      throw new ArgumentException(Strings.InvalidObjectType(value != null ? (object) value.GetType() : (object) "null", (object) typeof (T)), argument);
    }

    [Serializable]
    private class Enumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private readonly ReadOnlyCollectionBuilder<T> _builder;
      private readonly int _version;
      private int _index;
      private T _current;

      public T Current
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this._current;
        }
      }

      object IEnumerator.Current
      {
        get
        {
          if (this._index == 0 || this._index > this._builder._size)
            throw Error.EnumerationIsDone();
          else
            return (object) this._current;
        }
      }

      internal Enumerator(ReadOnlyCollectionBuilder<T> builder)
      {
        this._builder = builder;
        this._version = builder._version;
        this._index = 0;
        this._current = default (T);
      }

      public void Dispose()
      {
        GC.SuppressFinalize((object) this);
      }

      public bool MoveNext()
      {
        if (this._version != this._builder._version)
          throw Error.CollectionModifiedWhileEnumerating();
        if (this._index < this._builder._size)
        {
          this._current = this._builder._items[this._index++];
          return true;
        }
        else
        {
          this._index = this._builder._size + 1;
          this._current = default (T);
          return false;
        }
      }

      void IEnumerator.Reset()
      {
        if (this._version != this._builder._version)
          throw Error.CollectionModifiedWhileEnumerating();
        this._index = 0;
        this._current = default (T);
      }
    }
  }
}
