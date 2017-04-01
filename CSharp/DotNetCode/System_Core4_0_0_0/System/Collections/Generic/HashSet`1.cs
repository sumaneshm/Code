// Type: System.Collections.Generic.HashSet`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Collections.Generic
{
  [DebuggerTypeProxy(typeof (HashSetDebugView<>))]
  [DebuggerDisplay("Count = {Count}")]
  [__DynamicallyInvokable]
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class HashSet<T> : ISerializable, IDeserializationCallback, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private int[] m_buckets;
    private HashSet<T>.Slot[] m_slots;
    private int m_count;
    private int m_lastIndex;
    private int m_freeList;
    private IEqualityComparer<T> m_comparer;
    private int m_version;
    private SerializationInfo m_siInfo;
    private const int Lower31BitMask = 2147483647;
    private const int StackAllocThreshold = 100;
    private const int ShrinkThreshold = 3;
    private const string CapacityName = "Capacity";
    private const string ElementsName = "Elements";
    private const string ComparerName = "Comparer";
    private const string VersionName = "Version";

    [__DynamicallyInvokable]
    public int Count
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_count;
      }
    }

    [__DynamicallyInvokable]
    bool ICollection<T>.IsReadOnly
    {
      [__DynamicallyInvokable] get
      {
        return false;
      }
    }

    [__DynamicallyInvokable]
    public IEqualityComparer<T> Comparer
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_comparer;
      }
    }

    [__DynamicallyInvokable]
    public HashSet()
      : this((IEqualityComparer<T>) EqualityComparer<T>.Default)
    {
    }

    [__DynamicallyInvokable]
    public HashSet(IEqualityComparer<T> comparer)
    {
      if (comparer == null)
        comparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
      this.m_comparer = comparer;
      this.m_lastIndex = 0;
      this.m_count = 0;
      this.m_freeList = -1;
      this.m_version = 0;
    }

    [__DynamicallyInvokable]
    public HashSet(IEnumerable<T> collection)
      : this(collection, (IEqualityComparer<T>) EqualityComparer<T>.Default)
    {
    }

    [__DynamicallyInvokable]
    public HashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
      : this(comparer)
    {
      if (collection == null)
        throw new ArgumentNullException("collection");
      int capacity = 0;
      ICollection<T> collection1 = collection as ICollection<T>;
      if (collection1 != null)
        capacity = collection1.Count;
      this.Initialize(capacity);
      this.UnionWith(collection);
      if ((this.m_count != 0 || this.m_slots.Length <= HashHelpers.GetMinPrime()) && (this.m_count <= 0 || this.m_slots.Length / this.m_count <= 3))
        return;
      this.TrimExcess();
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected HashSet(SerializationInfo info, StreamingContext context)
    {
      this.m_siInfo = info;
    }

    [__DynamicallyInvokable]
    void ICollection<T>.Add(T item)
    {
      this.AddIfNotPresent(item);
    }

    [__DynamicallyInvokable]
    public void Clear()
    {
      if (this.m_lastIndex > 0)
      {
        Array.Clear((Array) this.m_slots, 0, this.m_lastIndex);
        Array.Clear((Array) this.m_buckets, 0, this.m_buckets.Length);
        this.m_lastIndex = 0;
        this.m_count = 0;
        this.m_freeList = -1;
      }
      ++this.m_version;
    }

    [__DynamicallyInvokable]
    public bool Contains(T item)
    {
      if (this.m_buckets != null)
      {
        int hashCode = this.InternalGetHashCode(item);
        for (int index = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index >= 0; index = this.m_slots[index].next)
        {
          if (this.m_slots[index].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index].value, item))
            return true;
        }
      }
      return false;
    }

    [__DynamicallyInvokable]
    public void CopyTo(T[] array, int arrayIndex)
    {
      this.CopyTo(array, arrayIndex, this.m_count);
    }

    [__DynamicallyInvokable]
    public bool Remove(T item)
    {
      if (this.m_buckets != null)
      {
        int hashCode = this.InternalGetHashCode(item);
        int index1 = hashCode % this.m_buckets.Length;
        int index2 = -1;
        for (int index3 = this.m_buckets[index1] - 1; index3 >= 0; index3 = this.m_slots[index3].next)
        {
          if (this.m_slots[index3].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index3].value, item))
          {
            if (index2 < 0)
              this.m_buckets[index1] = this.m_slots[index3].next + 1;
            else
              this.m_slots[index2].next = this.m_slots[index3].next;
            this.m_slots[index3].hashCode = -1;
            this.m_slots[index3].value = default (T);
            this.m_slots[index3].next = this.m_freeList;
            --this.m_count;
            ++this.m_version;
            if (this.m_count == 0)
            {
              this.m_lastIndex = 0;
              this.m_freeList = -1;
            }
            else
              this.m_freeList = index3;
            return true;
          }
          else
            index2 = index3;
        }
      }
      return false;
    }

    [__DynamicallyInvokable]
    public HashSet<T>.Enumerator GetEnumerator()
    {
      return new HashSet<T>.Enumerator(this);
    }

    [__DynamicallyInvokable]
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return (IEnumerator<T>) new HashSet<T>.Enumerator(this);
    }

    [__DynamicallyInvokable]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) new HashSet<T>.Enumerator(this);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      info.AddValue("Version", this.m_version);
      info.AddValue("Comparer", HashHelpers.GetEqualityComparerForSerialization((object) this.m_comparer), typeof (IEqualityComparer<T>));
      info.AddValue("Capacity", this.m_buckets == null ? 0 : this.m_buckets.Length);
      if (this.m_buckets == null)
        return;
      T[] array = new T[this.m_count];
      this.CopyTo(array);
      info.AddValue("Elements", (object) array, typeof (T[]));
    }

    public virtual void OnDeserialization(object sender)
    {
      if (this.m_siInfo == null)
        return;
      int int32 = this.m_siInfo.GetInt32("Capacity");
      this.m_comparer = (IEqualityComparer<T>) this.m_siInfo.GetValue("Comparer", typeof (IEqualityComparer<T>));
      this.m_freeList = -1;
      if (int32 != 0)
      {
        this.m_buckets = new int[int32];
        this.m_slots = new HashSet<T>.Slot[int32];
        T[] objArray = (T[]) this.m_siInfo.GetValue("Elements", typeof (T[]));
        if (objArray == null)
          throw new SerializationException(SR.GetString("Serialization_MissingKeys"));
        for (int index = 0; index < objArray.Length; ++index)
          this.AddIfNotPresent(objArray[index]);
      }
      else
        this.m_buckets = (int[]) null;
      this.m_version = this.m_siInfo.GetInt32("Version");
      this.m_siInfo = (SerializationInfo) null;
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public bool Add(T item)
    {
      return this.AddIfNotPresent(item);
    }

    [__DynamicallyInvokable]
    public void UnionWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      foreach (T obj in other)
        this.AddIfNotPresent(obj);
    }

    [__DynamicallyInvokable]
    public void IntersectWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      if (this.m_count == 0)
        return;
      ICollection<T> collection = other as ICollection<T>;
      if (collection != null)
      {
        if (collection.Count == 0)
        {
          this.Clear();
          return;
        }
        else
        {
          HashSet<T> hashSet = other as HashSet<T>;
          if (hashSet != null && HashSet<T>.AreEqualityComparersEqual(this, hashSet))
          {
            this.IntersectWithHashSetWithSameEC(hashSet);
            return;
          }
        }
      }
      this.IntersectWithEnumerable(other);
    }

    [__DynamicallyInvokable]
    public void ExceptWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      if (this.m_count == 0)
        return;
      if (other == this)
      {
        this.Clear();
      }
      else
      {
        foreach (T obj in other)
          this.Remove(obj);
      }
    }

    [__DynamicallyInvokable]
    public void SymmetricExceptWith(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      if (this.m_count == 0)
        this.UnionWith(other);
      else if (other == this)
      {
        this.Clear();
      }
      else
      {
        HashSet<T> hashSet = other as HashSet<T>;
        if (hashSet != null && HashSet<T>.AreEqualityComparersEqual(this, hashSet))
          this.SymmetricExceptWithUniqueHashSet(hashSet);
        else
          this.SymmetricExceptWithEnumerable(other);
      }
    }

    [__DynamicallyInvokable]
    public bool IsSubsetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      if (this.m_count == 0)
        return true;
      HashSet<T> hashSet = other as HashSet<T>;
      if (hashSet != null && HashSet<T>.AreEqualityComparersEqual(this, hashSet))
      {
        if (this.m_count > hashSet.Count)
          return false;
        else
          return this.IsSubsetOfHashSetWithSameEC(hashSet);
      }
      else
      {
        HashSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, false);
        if (elementCount.uniqueCount == this.m_count)
          return elementCount.unfoundCount >= 0;
        else
          return false;
      }
    }

    [__DynamicallyInvokable]
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      ICollection<T> collection = other as ICollection<T>;
      if (collection != null)
      {
        if (this.m_count == 0)
          return collection.Count > 0;
        HashSet<T> hashSet = other as HashSet<T>;
        if (hashSet != null && HashSet<T>.AreEqualityComparersEqual(this, hashSet))
        {
          if (this.m_count >= hashSet.Count)
            return false;
          else
            return this.IsSubsetOfHashSetWithSameEC(hashSet);
        }
      }
      HashSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, false);
      if (elementCount.uniqueCount == this.m_count)
        return elementCount.unfoundCount > 0;
      else
        return false;
    }

    [__DynamicallyInvokable]
    public bool IsSupersetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      ICollection<T> collection = other as ICollection<T>;
      if (collection != null)
      {
        if (collection.Count == 0)
          return true;
        HashSet<T> set2 = other as HashSet<T>;
        if (set2 != null && HashSet<T>.AreEqualityComparersEqual(this, set2) && set2.Count > this.m_count)
          return false;
      }
      return this.ContainsAllElements(other);
    }

    [__DynamicallyInvokable]
    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      if (this.m_count == 0)
        return false;
      ICollection<T> collection = other as ICollection<T>;
      if (collection != null)
      {
        if (collection.Count == 0)
          return true;
        HashSet<T> set2 = other as HashSet<T>;
        if (set2 != null && HashSet<T>.AreEqualityComparersEqual(this, set2))
        {
          if (set2.Count >= this.m_count)
            return false;
          else
            return this.ContainsAllElements((IEnumerable<T>) set2);
        }
      }
      HashSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, true);
      if (elementCount.uniqueCount < this.m_count)
        return elementCount.unfoundCount == 0;
      else
        return false;
    }

    [__DynamicallyInvokable]
    public bool Overlaps(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      if (this.m_count == 0)
        return false;
      foreach (T obj in other)
      {
        if (this.Contains(obj))
          return true;
      }
      return false;
    }

    [__DynamicallyInvokable]
    public bool SetEquals(IEnumerable<T> other)
    {
      if (other == null)
        throw new ArgumentNullException("other");
      HashSet<T> set2 = other as HashSet<T>;
      if (set2 != null && HashSet<T>.AreEqualityComparersEqual(this, set2))
      {
        if (this.m_count != set2.Count)
          return false;
        else
          return this.ContainsAllElements((IEnumerable<T>) set2);
      }
      else
      {
        ICollection<T> collection = other as ICollection<T>;
        if (collection != null && this.m_count == 0 && collection.Count > 0)
          return false;
        HashSet<T>.ElementCount elementCount = this.CheckUniqueAndUnfoundElements(other, true);
        if (elementCount.uniqueCount == this.m_count)
          return elementCount.unfoundCount == 0;
        else
          return false;
      }
    }

    [__DynamicallyInvokable]
    public void CopyTo(T[] array)
    {
      this.CopyTo(array, 0, this.m_count);
    }

    [__DynamicallyInvokable]
    public void CopyTo(T[] array, int arrayIndex, int count)
    {
      if (array == null)
        throw new ArgumentNullException("array");
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException("arrayIndex", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (arrayIndex > array.Length || count > array.Length - arrayIndex)
        throw new ArgumentException(SR.GetString("Arg_ArrayPlusOffTooSmall"));
      int num = 0;
      for (int index = 0; index < this.m_lastIndex && num < count; ++index)
      {
        if (this.m_slots[index].hashCode >= 0)
        {
          array[arrayIndex + num] = this.m_slots[index].value;
          ++num;
        }
      }
    }

    [__DynamicallyInvokable]
    public int RemoveWhere(Predicate<T> match)
    {
      if (match == null)
        throw new ArgumentNullException("match");
      int num = 0;
      for (int index = 0; index < this.m_lastIndex; ++index)
      {
        if (this.m_slots[index].hashCode >= 0)
        {
          T obj = this.m_slots[index].value;
          if (match(obj) && this.Remove(obj))
            ++num;
        }
      }
      return num;
    }

    [__DynamicallyInvokable]
    public void TrimExcess()
    {
      if (this.m_count == 0)
      {
        this.m_buckets = (int[]) null;
        this.m_slots = (HashSet<T>.Slot[]) null;
        ++this.m_version;
      }
      else
      {
        int prime = HashHelpers.GetPrime(this.m_count);
        HashSet<T>.Slot[] slotArray = new HashSet<T>.Slot[prime];
        int[] numArray = new int[prime];
        int index1 = 0;
        for (int index2 = 0; index2 < this.m_lastIndex; ++index2)
        {
          if (this.m_slots[index2].hashCode >= 0)
          {
            slotArray[index1] = this.m_slots[index2];
            int index3 = slotArray[index1].hashCode % prime;
            slotArray[index1].next = numArray[index3] - 1;
            numArray[index3] = index1 + 1;
            ++index1;
          }
        }
        this.m_lastIndex = index1;
        this.m_slots = slotArray;
        this.m_buckets = numArray;
        this.m_freeList = -1;
      }
    }

    public static IEqualityComparer<HashSet<T>> CreateSetComparer()
    {
      return (IEqualityComparer<HashSet<T>>) new HashSetEqualityComparer<T>();
    }

    private void Initialize(int capacity)
    {
      int prime = HashHelpers.GetPrime(capacity);
      this.m_buckets = new int[prime];
      this.m_slots = new HashSet<T>.Slot[prime];
    }

    private void IncreaseCapacity()
    {
      int newSize = HashHelpers.ExpandPrime(this.m_count);
      if (newSize <= this.m_count)
        throw new ArgumentException(SR.GetString("Arg_HSCapacityOverflow"));
      this.SetCapacity(newSize, false);
    }

    private void SetCapacity(int newSize, bool forceNewHashCodes)
    {
      HashSet<T>.Slot[] slotArray = new HashSet<T>.Slot[newSize];
      if (this.m_slots != null)
        Array.Copy((Array) this.m_slots, 0, (Array) slotArray, 0, this.m_lastIndex);
      if (forceNewHashCodes)
      {
        for (int index = 0; index < this.m_lastIndex; ++index)
        {
          if (slotArray[index].hashCode != -1)
            slotArray[index].hashCode = this.InternalGetHashCode(slotArray[index].value);
        }
      }
      int[] numArray = new int[newSize];
      for (int index1 = 0; index1 < this.m_lastIndex; ++index1)
      {
        int index2 = slotArray[index1].hashCode % newSize;
        slotArray[index1].next = numArray[index2] - 1;
        numArray[index2] = index1 + 1;
      }
      this.m_slots = slotArray;
      this.m_buckets = numArray;
    }

    private bool AddIfNotPresent(T value)
    {
      if (this.m_buckets == null)
        this.Initialize(0);
      int hashCode = this.InternalGetHashCode(value);
      int index1 = hashCode % this.m_buckets.Length;
      int num = 0;
      for (int index2 = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index2 >= 0; index2 = this.m_slots[index2].next)
      {
        if (this.m_slots[index2].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index2].value, value))
          return false;
        ++num;
      }
      int index3;
      if (this.m_freeList >= 0)
      {
        index3 = this.m_freeList;
        this.m_freeList = this.m_slots[index3].next;
      }
      else
      {
        if (this.m_lastIndex == this.m_slots.Length)
        {
          this.IncreaseCapacity();
          index1 = hashCode % this.m_buckets.Length;
        }
        index3 = this.m_lastIndex;
        ++this.m_lastIndex;
      }
      this.m_slots[index3].hashCode = hashCode;
      this.m_slots[index3].value = value;
      this.m_slots[index3].next = this.m_buckets[index1] - 1;
      this.m_buckets[index1] = index3 + 1;
      ++this.m_count;
      ++this.m_version;
      if (num > 100 && HashHelpers.IsWellKnownEqualityComparer((object) this.m_comparer))
      {
        this.m_comparer = (IEqualityComparer<T>) HashHelpers.GetRandomizedEqualityComparer((object) this.m_comparer);
        this.SetCapacity(this.m_buckets.Length, true);
      }
      return true;
    }

    private bool ContainsAllElements(IEnumerable<T> other)
    {
      foreach (T obj in other)
      {
        if (!this.Contains(obj))
          return false;
      }
      return true;
    }

    private bool IsSubsetOfHashSetWithSameEC(HashSet<T> other)
    {
      foreach (T obj in this)
      {
        if (!other.Contains(obj))
          return false;
      }
      return true;
    }

    private void IntersectWithHashSetWithSameEC(HashSet<T> other)
    {
      for (int index = 0; index < this.m_lastIndex; ++index)
      {
        if (this.m_slots[index].hashCode >= 0)
        {
          T obj = this.m_slots[index].value;
          if (!other.Contains(obj))
            this.Remove(obj);
        }
      }
    }

    [SecuritySafeCritical]
    private unsafe void IntersectWithEnumerable(IEnumerable<T> other)
    {
      int n = this.m_lastIndex;
      int length = BitHelper.ToIntArrayLength(n);
      BitHelper bitHelper;
      if (length <= 100)
      {
        int* bitArrayPtr = stackalloc int[length];
        bitHelper = new BitHelper(bitArrayPtr, length);
      }
      else
        bitHelper = new BitHelper(new int[length], length);
      foreach (T obj in other)
      {
        int bitPosition = this.InternalIndexOf(obj);
        if (bitPosition >= 0)
          bitHelper.MarkBit(bitPosition);
      }
      for (int bitPosition = 0; bitPosition < n; ++bitPosition)
      {
        if (this.m_slots[bitPosition].hashCode >= 0 && !bitHelper.IsMarked(bitPosition))
          this.Remove(this.m_slots[bitPosition].value);
      }
    }

    private int InternalIndexOf(T item)
    {
      int hashCode = this.InternalGetHashCode(item);
      for (int index = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index >= 0; index = this.m_slots[index].next)
      {
        if (this.m_slots[index].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index].value, item))
          return index;
      }
      return -1;
    }

    private void SymmetricExceptWithUniqueHashSet(HashSet<T> other)
    {
      foreach (T obj in other)
      {
        if (!this.Remove(obj))
          this.AddIfNotPresent(obj);
      }
    }

    [SecuritySafeCritical]
    private unsafe void SymmetricExceptWithEnumerable(IEnumerable<T> other)
    {
      int n = this.m_lastIndex;
      int length = BitHelper.ToIntArrayLength(n);
      BitHelper bitHelper1;
      BitHelper bitHelper2;
      if (length <= 50)
      {
        int* bitArrayPtr1 = stackalloc int[length];
        bitHelper1 = new BitHelper(bitArrayPtr1, length);
        int* bitArrayPtr2 = stackalloc int[length];
        bitHelper2 = new BitHelper(bitArrayPtr2, length);
      }
      else
      {
        bitHelper1 = new BitHelper(new int[length], length);
        bitHelper2 = new BitHelper(new int[length], length);
      }
      foreach (T obj in other)
      {
        int location = 0;
        if (this.AddOrGetLocation(obj, out location))
          bitHelper2.MarkBit(location);
        else if (location < n && !bitHelper2.IsMarked(location))
          bitHelper1.MarkBit(location);
      }
      for (int bitPosition = 0; bitPosition < n; ++bitPosition)
      {
        if (bitHelper1.IsMarked(bitPosition))
          this.Remove(this.m_slots[bitPosition].value);
      }
    }

    private bool AddOrGetLocation(T value, out int location)
    {
      int hashCode = this.InternalGetHashCode(value);
      int index1 = hashCode % this.m_buckets.Length;
      for (int index2 = this.m_buckets[hashCode % this.m_buckets.Length] - 1; index2 >= 0; index2 = this.m_slots[index2].next)
      {
        if (this.m_slots[index2].hashCode == hashCode && this.m_comparer.Equals(this.m_slots[index2].value, value))
        {
          location = index2;
          return false;
        }
      }
      int index3;
      if (this.m_freeList >= 0)
      {
        index3 = this.m_freeList;
        this.m_freeList = this.m_slots[index3].next;
      }
      else
      {
        if (this.m_lastIndex == this.m_slots.Length)
        {
          this.IncreaseCapacity();
          index1 = hashCode % this.m_buckets.Length;
        }
        index3 = this.m_lastIndex;
        ++this.m_lastIndex;
      }
      this.m_slots[index3].hashCode = hashCode;
      this.m_slots[index3].value = value;
      this.m_slots[index3].next = this.m_buckets[index1] - 1;
      this.m_buckets[index1] = index3 + 1;
      ++this.m_count;
      ++this.m_version;
      location = index3;
      return true;
    }

    [SecuritySafeCritical]
    private unsafe HashSet<T>.ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
    {
      if (this.m_count == 0)
      {
        int num = 0;
        using (IEnumerator<T> enumerator = other.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            T current = enumerator.Current;
            ++num;
          }
        }
        HashSet<T>.ElementCount elementCount;
        elementCount.uniqueCount = 0;
        elementCount.unfoundCount = num;
        return elementCount;
      }
      else
      {
        int length = BitHelper.ToIntArrayLength(this.m_lastIndex);
        BitHelper bitHelper;
        if (length <= 100)
        {
          int* bitArrayPtr = stackalloc int[length];
          bitHelper = new BitHelper(bitArrayPtr, length);
        }
        else
          bitHelper = new BitHelper(new int[length], length);
        int num1 = 0;
        int num2 = 0;
        foreach (T obj in other)
        {
          int bitPosition = this.InternalIndexOf(obj);
          if (bitPosition >= 0)
          {
            if (!bitHelper.IsMarked(bitPosition))
            {
              bitHelper.MarkBit(bitPosition);
              ++num2;
            }
          }
          else
          {
            ++num1;
            if (returnIfUnfound)
              break;
          }
        }
        HashSet<T>.ElementCount elementCount;
        elementCount.uniqueCount = num2;
        elementCount.unfoundCount = num1;
        return elementCount;
      }
    }

    internal T[] ToArray()
    {
      T[] array = new T[this.Count];
      this.CopyTo(array);
      return array;
    }

    internal static bool HashSetEquals(HashSet<T> set1, HashSet<T> set2, IEqualityComparer<T> comparer)
    {
      if (set1 == null)
        return set2 == null;
      if (set2 == null)
        return false;
      if (HashSet<T>.AreEqualityComparersEqual(set1, set2))
      {
        if (set1.Count != set2.Count)
          return false;
        foreach (T obj in set2)
        {
          if (!set1.Contains(obj))
            return false;
        }
        return true;
      }
      else
      {
        foreach (T x in set2)
        {
          bool flag = false;
          foreach (T y in set1)
          {
            if (comparer.Equals(x, y))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return false;
        }
        return true;
      }
    }

    private static bool AreEqualityComparersEqual(HashSet<T> set1, HashSet<T> set2)
    {
      return ((object) set1.Comparer).Equals((object) set2.Comparer);
    }

    private int InternalGetHashCode(T item)
    {
      if ((object) item == null)
        return 0;
      else
        return this.m_comparer.GetHashCode(item) & int.MaxValue;
    }

    internal struct ElementCount
    {
      internal int uniqueCount;
      internal int unfoundCount;
    }

    internal struct Slot
    {
      internal int hashCode;
      internal T value;
      internal int next;
    }

    [__DynamicallyInvokable]
    [Serializable]
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private HashSet<T> set;
      private int index;
      private int version;
      private T current;

      [__DynamicallyInvokable]
      public T Current
      {
        [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.current;
        }
      }

      [__DynamicallyInvokable]
      object IEnumerator.Current
      {
        [__DynamicallyInvokable] get
        {
          if (this.index == 0 || this.index == this.set.m_lastIndex + 1)
            throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
          else
            return (object) this.Current;
        }
      }

      internal Enumerator(HashSet<T> set)
      {
        this.set = set;
        this.index = 0;
        this.version = set.m_version;
        this.current = default (T);
      }

      [__DynamicallyInvokable]
      public void Dispose()
      {
      }

      [__DynamicallyInvokable]
      public bool MoveNext()
      {
        if (this.version != this.set.m_version)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
        for (; this.index < this.set.m_lastIndex; ++this.index)
        {
          if (this.set.m_slots[this.index].hashCode >= 0)
          {
            this.current = this.set.m_slots[this.index].value;
            ++this.index;
            return true;
          }
        }
        this.index = this.set.m_lastIndex + 1;
        this.current = default (T);
        return false;
      }

      [__DynamicallyInvokable]
      void IEnumerator.Reset()
      {
        if (this.version != this.set.m_version)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
        this.index = 0;
        this.current = default (T);
      }
    }
  }
}
