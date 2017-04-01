// Type: System.Linq.OrderedParallelQuery`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq.Parallel;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class OrderedParallelQuery<TSource> : ParallelQuery<TSource>
  {
    private QueryOperator<TSource> m_sortOp;

    internal QueryOperator<TSource> SortOperator
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_sortOp;
      }
    }

    internal IOrderedEnumerable<TSource> OrderedEnumerable
    {
      get
      {
        return (IOrderedEnumerable<TSource>) this.m_sortOp;
      }
    }

    internal OrderedParallelQuery(QueryOperator<TSource> sortOp)
      : base(sortOp.SpecifiedQuerySettings)
    {
      this.m_sortOp = sortOp;
    }

    [__DynamicallyInvokable]
    public override IEnumerator<TSource> GetEnumerator()
    {
      return ((ParallelQuery<TSource>) this.m_sortOp).GetEnumerator();
    }
  }
}
