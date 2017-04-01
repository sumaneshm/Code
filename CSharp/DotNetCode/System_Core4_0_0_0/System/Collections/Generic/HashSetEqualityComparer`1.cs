// Type: System.Collections.Generic.HashSetEqualityComparer`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Collections.Generic
{
  [Serializable]
  internal class HashSetEqualityComparer<T> : IEqualityComparer<HashSet<T>>
  {
    private IEqualityComparer<T> m_comparer;

    public HashSetEqualityComparer()
    {
      this.m_comparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
    }

    public HashSetEqualityComparer(IEqualityComparer<T> comparer)
    {
      if (comparer == null)
        this.m_comparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
      else
        this.m_comparer = comparer;
    }

    public bool Equals(HashSet<T> x, HashSet<T> y)
    {
      return HashSet<T>.HashSetEquals(x, y, this.m_comparer);
    }

    public int GetHashCode(HashSet<T> obj)
    {
      int num = 0;
      if (obj != null)
      {
        foreach (T obj1 in obj)
          num ^= this.m_comparer.GetHashCode(obj1) & int.MaxValue;
      }
      return num;
    }

    public override bool Equals(object obj)
    {
      HashSetEqualityComparer<T> equalityComparer = obj as HashSetEqualityComparer<T>;
      if (equalityComparer == null)
        return false;
      else
        return this.m_comparer == equalityComparer.m_comparer;
    }

    public override int GetHashCode()
    {
      return ((object) this.m_comparer).GetHashCode();
    }
  }
}
