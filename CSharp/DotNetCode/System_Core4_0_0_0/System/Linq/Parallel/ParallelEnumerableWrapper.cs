// Type: System.Linq.Parallel.ParallelEnumerableWrapper
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Parallel
{
  internal class ParallelEnumerableWrapper : ParallelQuery<object>
  {
    private readonly IEnumerable m_source;

    internal ParallelEnumerableWrapper(IEnumerable source)
      : base(QuerySettings.Empty)
    {
      this.m_source = source;
    }

    internal override IEnumerator GetEnumeratorUntyped()
    {
      return this.m_source.GetEnumerator();
    }

    public override IEnumerator<object> GetEnumerator()
    {
      return new EnumerableWrapperWeakToStrong(this.m_source).GetEnumerator();
    }
  }
}
