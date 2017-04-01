// Type: System.Linq.ParallelQuery`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Parallel;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class ParallelQuery<TSource> : ParallelQuery, IEnumerable<TSource>, IEnumerable
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ParallelQuery(QuerySettings settings)
      : base(settings)
    {
    }

    internal override sealed ParallelQuery<TCastTo> Cast<TCastTo>()
    {
      return ParallelEnumerable.Select<TSource, TCastTo>(this, (Func<TSource, TCastTo>) (elem => (TCastTo) (object) elem));
    }

    internal override sealed ParallelQuery<TCastTo> OfType<TCastTo>()
    {
      return ParallelEnumerable.Select<TSource, TCastTo>(ParallelEnumerable.Where<TSource>(this, (Func<TSource, bool>) (elem => (object) elem is TCastTo)), (Func<TSource, TCastTo>) (elem => (TCastTo) (object) elem));
    }

    internal override IEnumerator GetEnumeratorUntyped()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    [__DynamicallyInvokable]
    public virtual IEnumerator<TSource> GetEnumerator()
    {
      throw new NotSupportedException();
    }
  }
}
