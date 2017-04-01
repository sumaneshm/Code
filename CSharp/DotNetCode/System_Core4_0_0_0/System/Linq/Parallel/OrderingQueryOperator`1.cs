// Type: System.Linq.Parallel.OrderingQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class OrderingQueryOperator<TSource> : QueryOperator<TSource>
  {
    private bool m_orderOn;
    private QueryOperator<TSource> m_child;
    private OrdinalIndexState m_ordinalIndexState;

    internal override bool LimitsParallelism
    {
      get
      {
        return this.m_child.LimitsParallelism;
      }
    }

    internal override OrdinalIndexState OrdinalIndexState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_ordinalIndexState;
      }
    }

    public OrderingQueryOperator(QueryOperator<TSource> child, bool orderOn)
      : base(orderOn, child.SpecifiedQuerySettings)
    {
      this.m_child = child;
      this.m_ordinalIndexState = this.m_child.OrdinalIndexState;
      this.m_orderOn = orderOn;
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return this.m_child.Open(settings, preferStriping);
    }

    internal override IEnumerator<TSource> GetEnumerator(ParallelMergeOptions? mergeOptions, bool suppressOrderPreservation)
    {
      ScanQueryOperator<TSource> scanQueryOperator = this.m_child as ScanQueryOperator<TSource>;
      if (scanQueryOperator != null)
        return scanQueryOperator.Data.GetEnumerator();
      else
        return base.GetEnumerator(mergeOptions, suppressOrderPreservation);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      return this.m_child.AsSequentialQuery(token);
    }
  }
}
