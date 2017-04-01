// Type: System.Linq.Parallel.HashLookup`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class HashLookup<TKey, TValue>
  {
    private int[] buckets;
    private HashLookup<TKey, TValue>.Slot[] slots;
    private int count;
    private int freeList;
    private IEqualityComparer<TKey> comparer;

    internal TValue this[TKey key]
    {
      set
      {
        TValue obj = value;
        this.Find(key, false, true, ref obj);
      }
    }

    internal int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.count;
      }
    }

    internal KeyValuePair<TKey, TValue> this[int index]
    {
      get
      {
        return new KeyValuePair<TKey, TValue>(this.slots[index].key, this.slots[index].value);
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal HashLookup()
      : this((IEqualityComparer<TKey>) null)
    {
    }

    internal HashLookup(IEqualityComparer<TKey> comparer)
    {
      this.comparer = comparer;
      this.buckets = new int[7];
      this.slots = new HashLookup<TKey, TValue>.Slot[7];
      this.freeList = -1;
    }

    internal bool Add(TKey key, TValue value)
    {
      return !this.Find(key, true, false, ref value);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal bool TryGetValue(TKey key, ref TValue value)
    {
      return this.Find(key, false, false, ref value);
    }

    private int GetKeyHashCode(TKey key)
    {
      return int.MaxValue & (this.comparer == null ? ((object) key == null ? 0 : key.GetHashCode()) : this.comparer.GetHashCode(key));
    }

    private bool AreKeysEqual(TKey key1, TKey key2)
    {
      if (this.comparer != null)
        return this.comparer.Equals(key1, key2);
      if ((object) key1 == null && (object) key2 == null)
        return true;
      if ((object) key1 != null)
        return key1.Equals((object) key2);
      else
        return false;
    }

    internal bool Remove(TKey key)
    {
      int keyHashCode = this.GetKeyHashCode(key);
      int index1 = keyHashCode % this.buckets.Length;
      int index2 = -1;
      for (int index3 = this.buckets[index1] - 1; index3 >= 0; index3 = this.slots[index3].next)
      {
        if (this.slots[index3].hashCode == keyHashCode && this.AreKeysEqual(this.slots[index3].key, key))
        {
          if (index2 < 0)
            this.buckets[index1] = this.slots[index3].next + 1;
          else
            this.slots[index2].next = this.slots[index3].next;
          this.slots[index3].hashCode = -1;
          this.slots[index3].key = default (TKey);
          this.slots[index3].value = default (TValue);
          this.slots[index3].next = this.freeList;
          this.freeList = index3;
          return true;
        }
        else
          index2 = index3;
      }
      return false;
    }

    private bool Find(TKey key, bool add, bool set, ref TValue value)
    {
      int keyHashCode = this.GetKeyHashCode(key);
      for (int index = this.buckets[keyHashCode % this.buckets.Length] - 1; index >= 0; index = this.slots[index].next)
      {
        if (this.slots[index].hashCode == keyHashCode && this.AreKeysEqual(this.slots[index].key, key))
        {
          if (set)
          {
            this.slots[index].value = value;
            return true;
          }
          else
          {
            value = this.slots[index].value;
            return true;
          }
        }
      }
      if (add)
      {
        int index1;
        if (this.freeList >= 0)
        {
          index1 = this.freeList;
          this.freeList = this.slots[index1].next;
        }
        else
        {
          if (this.count == this.slots.Length)
            this.Resize();
          index1 = this.count;
          ++this.count;
        }
        int index2 = keyHashCode % this.buckets.Length;
        this.slots[index1].hashCode = keyHashCode;
        this.slots[index1].key = key;
        this.slots[index1].value = value;
        this.slots[index1].next = this.buckets[index2] - 1;
        this.buckets[index2] = index1 + 1;
      }
      return false;
    }

    private void Resize()
    {
      int length = checked (this.count * 2 + 1);
      int[] numArray = new int[length];
      HashLookup<TKey, TValue>.Slot[] slotArray = new HashLookup<TKey, TValue>.Slot[length];
      Array.Copy((Array) this.slots, 0, (Array) slotArray, 0, this.count);
      for (int index1 = 0; index1 < this.count; ++index1)
      {
        int index2 = slotArray[index1].hashCode % length;
        slotArray[index1].next = numArray[index2] - 1;
        numArray[index2] = index1 + 1;
      }
      this.buckets = numArray;
      this.slots = slotArray;
    }

    internal struct Slot
    {
      internal int hashCode;
      internal TKey key;
      internal TValue value;
      internal int next;
    }
  }
}
