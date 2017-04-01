// Type: System.Linq.Parallel.MergeEnumerator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal abstract class MergeEnumerator<TInputOutput> : IEnumerator<TInputOutput>, IDisposable, IEnumerator
  {
    protected QueryTaskGroupState m_taskGroupState;

    public abstract TInputOutput Current { get; }

    object IEnumerator.Current
    {
      get
      {
        return (object) this.Current;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected MergeEnumerator(QueryTaskGroupState taskGroupState)
    {
      this.m_taskGroupState = taskGroupState;
    }

    public abstract bool MoveNext();

    public virtual void Reset()
    {
    }

    public virtual void Dispose()
    {
      if (this.m_taskGroupState.IsAlreadyEnded)
        return;
      this.m_taskGroupState.QueryEnd(true);
    }
  }
}
