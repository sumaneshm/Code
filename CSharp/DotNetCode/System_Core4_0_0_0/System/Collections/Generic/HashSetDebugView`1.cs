// Type: System.Collections.Generic.HashSetDebugView`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
  internal class HashSetDebugView<T>
  {
    private HashSet<T> set;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items
    {
      get
      {
        return this.set.ToArray();
      }
    }

    public HashSetDebugView(HashSet<T> set)
    {
      if (set == null)
        throw new ArgumentNullException("set");
      this.set = set;
    }
  }
}
