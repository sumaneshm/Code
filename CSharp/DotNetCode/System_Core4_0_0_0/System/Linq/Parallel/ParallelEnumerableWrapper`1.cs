// Type: System.Linq.Parallel.ParallelEnumerableWrapper`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class ParallelEnumerableWrapper<T> : ParallelQuery<T>
  {
    private readonly IEnumerable<T> m_wrappedEnumerable;

    internal IEnumerable<T> WrappedEnumerable
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_wrappedEnumerable;
      }
    }

    internal ParallelEnumerableWrapper(IEnumerable<T> wrappedEnumerable)
      : base(QuerySettings.Empty)
    {
      this.m_wrappedEnumerable = wrappedEnumerable;
    }

    public override IEnumerator<T> GetEnumerator()
    {
      return this.m_wrappedEnumerable.GetEnumerator();
    }
  }
}
