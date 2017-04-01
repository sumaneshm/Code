// Type: System.Linq.ParallelQuery
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Linq.Parallel;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class ParallelQuery : IEnumerable
  {
    private QuerySettings m_specifiedSettings;

    internal QuerySettings SpecifiedQuerySettings
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_specifiedSettings;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ParallelQuery(QuerySettings specifiedSettings)
    {
      this.m_specifiedSettings = specifiedSettings;
    }

    internal virtual ParallelQuery<TCastTo> Cast<TCastTo>()
    {
      throw new NotSupportedException();
    }

    internal virtual ParallelQuery<TCastTo> OfType<TCastTo>()
    {
      throw new NotSupportedException();
    }

    internal virtual IEnumerator GetEnumeratorUntyped()
    {
      throw new NotSupportedException();
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumeratorUntyped();
    }
  }
}
