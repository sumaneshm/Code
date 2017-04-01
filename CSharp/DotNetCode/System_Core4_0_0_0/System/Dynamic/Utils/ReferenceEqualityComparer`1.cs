// Type: System.Dynamic.Utils.ReferenceEqualityComparer`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Dynamic.Utils
{
  internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>
  {
    internal static readonly ReferenceEqualityComparer<T> Instance = new ReferenceEqualityComparer<T>();

    static ReferenceEqualityComparer()
    {
    }

    private ReferenceEqualityComparer()
    {
    }

    public bool Equals(T x, T y)
    {
      return object.ReferenceEquals((object) x, (object) y);
    }

    public int GetHashCode(T obj)
    {
      return RuntimeHelpers.GetHashCode((object) obj);
    }
  }
}
