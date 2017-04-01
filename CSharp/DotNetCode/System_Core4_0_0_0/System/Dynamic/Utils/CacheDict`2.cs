// Type: System.Dynamic.Utils.CacheDict`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Dynamic.Utils
{
  internal class CacheDict<TKey, TValue>
  {
    protected readonly int mask;
    protected readonly CacheDict<TKey, TValue>.Entry[] entries;

    internal TValue this[TKey key]
    {
      get
      {
        TValue obj;
        if (this.TryGetValue(key, out obj))
          return obj;
        else
          throw new KeyNotFoundException();
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.Add(key, value);
      }
    }

    internal CacheDict(int size)
    {
      int length = CacheDict<TKey, TValue>.AlignSize(size);
      this.mask = length - 1;
      this.entries = new CacheDict<TKey, TValue>.Entry[length];
    }

    private static int AlignSize(int size)
    {
      --size;
      size |= size >> 1;
      size |= size >> 2;
      size |= size >> 4;
      size |= size >> 8;
      size |= size >> 16;
      return size + 1;
    }

    internal bool TryGetValue(TKey key, out TValue value)
    {
      int hashCode = key.GetHashCode();
      CacheDict<TKey, TValue>.Entry entry = Volatile.Read<CacheDict<TKey, TValue>.Entry>(ref this.entries[hashCode & this.mask]);
      if (entry != null && entry.hash == hashCode && entry.key.Equals((object) key))
      {
        value = entry.value;
        return true;
      }
      else
      {
        value = default (TValue);
        return false;
      }
    }

    internal void Add(TKey key, TValue value)
    {
      int hashCode = key.GetHashCode();
      int index = hashCode & this.mask;
      CacheDict<TKey, TValue>.Entry entry = Volatile.Read<CacheDict<TKey, TValue>.Entry>(ref this.entries[index]);
      if (entry != null && entry.hash == hashCode && entry.key.Equals((object) key))
        return;
      Volatile.Write<CacheDict<TKey, TValue>.Entry>(ref this.entries[index], new CacheDict<TKey, TValue>.Entry(hashCode, key, value));
    }

    internal class Entry
    {
      internal readonly int hash;
      internal readonly TKey key;
      internal readonly TValue value;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal Entry(int hash, TKey key, TValue value)
      {
        this.hash = hash;
        this.key = key;
        this.value = value;
      }
    }
  }
}
