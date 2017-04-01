// Type: System.Linq.Parallel.QueryExecutionOption`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal class QueryExecutionOption<TSource> : QueryOperator<TSource>
  {
    private QueryOperator<TSource> m_child;
    private OrdinalIndexState m_indexState;

    internal override OrdinalIndexState OrdinalIndexState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_indexState;
      }
    }

    internal override bool LimitsParallelism
    {
      get
      {
        return this.m_child.LimitsParallelism;
      }
    }

    internal QueryExecutionOption(QueryOperator<TSource> source, QuerySettings settings)
      : base(source.OutputOrdered, settings.Merge(source.SpecifiedQuerySettings))
    {
      this.m_child = source;
      this.m_indexState = this.m_child.OrdinalIndexState;
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return this.m_child.Open(settings, preferStriping);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      return this.m_child.AsSequentialQuery(token);
    }
  }
}
