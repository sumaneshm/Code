// Type: System.Linq.Parallel.WrapperEqualityComparer`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;

namespace System.Linq.Parallel
{
  internal struct WrapperEqualityComparer<T> : IEqualityComparer<Wrapper<T>>
  {
    private IEqualityComparer<T> m_comparer;

    internal WrapperEqualityComparer(IEqualityComparer<T> comparer)
    {
      if (comparer == null)
        this.m_comparer = (IEqualityComparer<T>) EqualityComparer<T>.Default;
      else
        this.m_comparer = comparer;
    }

    public bool Equals(Wrapper<T> x, Wrapper<T> y)
    {
      return this.m_comparer.Equals(x.Value, y.Value);
    }

    public int GetHashCode(Wrapper<T> x)
    {
      return this.m_comparer.GetHashCode(x.Value);
    }
  }
}
