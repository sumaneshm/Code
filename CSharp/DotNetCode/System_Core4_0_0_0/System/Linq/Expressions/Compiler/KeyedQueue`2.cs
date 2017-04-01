// Type: System.Linq.Expressions.Compiler.KeyedQueue`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class KeyedQueue<K, V>
  {
    private readonly Dictionary<K, Queue<V>> _data;

    internal KeyedQueue()
    {
      this._data = new Dictionary<K, Queue<V>>();
    }

    internal void Enqueue(K key, V value)
    {
      Queue<V> queue;
      if (!this._data.TryGetValue(key, out queue))
        this._data.Add(key, queue = new Queue<V>());
      queue.Enqueue(value);
    }

    internal V Dequeue(K key)
    {
      Queue<V> queue;
      if (!this._data.TryGetValue(key, out queue))
        throw Error.QueueEmpty();
      V v = queue.Dequeue();
      if (queue.Count == 0)
        this._data.Remove(key);
      return v;
    }

    internal bool TryDequeue(K key, out V value)
    {
      Queue<V> queue;
      if (this._data.TryGetValue(key, out queue) && queue.Count > 0)
      {
        value = queue.Dequeue();
        if (queue.Count == 0)
          this._data.Remove(key);
        return true;
      }
      else
      {
        value = default (V);
        return false;
      }
    }

    internal V Peek(K key)
    {
      Queue<V> queue;
      if (!this._data.TryGetValue(key, out queue))
        throw Error.QueueEmpty();
      else
        return queue.Peek();
    }

    internal int GetCount(K key)
    {
      Queue<V> queue;
      if (!this._data.TryGetValue(key, out queue))
        return 0;
      else
        return queue.Count;
    }

    internal void Clear()
    {
      this._data.Clear();
    }
  }
}
