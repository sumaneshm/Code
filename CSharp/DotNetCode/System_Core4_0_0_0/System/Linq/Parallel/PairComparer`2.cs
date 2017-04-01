// Type: System.Linq.Parallel.PairComparer`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class PairComparer<T, U> : IComparer<Pair<T, U>>
  {
    private IComparer<T> m_comparer1;
    private IComparer<U> m_comparer2;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public PairComparer(IComparer<T> comparer1, IComparer<U> comparer2)
    {
      this.m_comparer1 = comparer1;
      this.m_comparer2 = comparer2;
    }

    public int Compare(Pair<T, U> x, Pair<T, U> y)
    {
      int num = this.m_comparer1.Compare(x.First, y.First);
      if (num != 0)
        return num;
      else
        return this.m_comparer2.Compare(x.Second, y.Second);
    }
  }
}
