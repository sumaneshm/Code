// Type: System.Linq.SystemCore_EnumerableDebugView`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
  internal sealed class SystemCore_EnumerableDebugView<T>
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IEnumerable<T> enumerable;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private T[] cachedCollection;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int count;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items
    {
      get
      {
        List<T> list = new List<T>();
        IEnumerator<T> enumerator = this.enumerable.GetEnumerator();
        if (enumerator != null)
        {
          this.count = 0;
          while (enumerator.MoveNext())
          {
            list.Add(enumerator.Current);
            ++this.count;
          }
        }
        if (this.count == 0)
          throw new SystemCore_EnumerableDebugViewEmptyException();
        this.cachedCollection = new T[this.count];
        list.CopyTo(this.cachedCollection, 0);
        return this.cachedCollection;
      }
    }

    public SystemCore_EnumerableDebugView(IEnumerable<T> enumerable)
    {
      if (enumerable == null)
        throw new ArgumentNullException("enumerable");
      this.enumerable = enumerable;
    }
  }
}
