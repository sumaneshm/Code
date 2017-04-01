// Type: System.Linq.Parallel.EmptyEnumerable`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Parallel
{
  internal class EmptyEnumerable<T> : ParallelQuery<T>
  {
    private static volatile EmptyEnumerable<T> s_instance;
    private static volatile EmptyEnumerator<T> s_enumeratorInstance;

    internal static EmptyEnumerable<T> Instance
    {
      get
      {
        if (EmptyEnumerable<T>.s_instance == null)
          EmptyEnumerable<T>.s_instance = new EmptyEnumerable<T>();
        return EmptyEnumerable<T>.s_instance;
      }
    }

    private EmptyEnumerable()
      : base(QuerySettings.Empty)
    {
    }

    public override IEnumerator<T> GetEnumerator()
    {
      if (EmptyEnumerable<T>.s_enumeratorInstance == null)
        EmptyEnumerable<T>.s_enumeratorInstance = new EmptyEnumerator<T>();
      return (IEnumerator<T>) EmptyEnumerable<T>.s_enumeratorInstance;
    }
  }
}
