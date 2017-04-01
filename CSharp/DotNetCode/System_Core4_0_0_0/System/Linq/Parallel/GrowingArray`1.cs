// Type: System.Linq.Parallel.GrowingArray`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class GrowingArray<T>
  {
    private T[] m_array;
    private int m_count;
    private const int DEFAULT_ARRAY_SIZE = 1024;

    internal T[] InternalArray
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_array;
      }
    }

    internal int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_count;
      }
    }

    internal GrowingArray()
    {
      this.m_array = new T[1024];
      this.m_count = 0;
    }

    internal void Add(T element)
    {
      if (this.m_count >= this.m_array.Length)
        this.GrowArray(2 * this.m_array.Length);
      this.m_array[this.m_count++] = element;
    }

    private void GrowArray(int newSize)
    {
      T[] objArray = new T[newSize];
      this.m_array.CopyTo((Array) objArray, 0);
      this.m_array = objArray;
    }

    internal void CopyFrom(T[] otherArray, int otherCount)
    {
      if (this.m_count + otherCount > this.m_array.Length)
        this.GrowArray(this.m_count + otherCount);
      Array.Copy((Array) otherArray, 0, (Array) this.m_array, this.m_count, otherCount);
      this.m_count += otherCount;
    }
  }
}
