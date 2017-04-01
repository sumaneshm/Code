// Type: System.Linq.Parallel.SpoolingTaskBase
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal abstract class SpoolingTaskBase : QueryTask
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected SpoolingTaskBase(int taskIndex, QueryTaskGroupState groupState)
      : base(taskIndex, groupState)
    {
    }

    protected override void Work()
    {
      try
      {
        this.SpoolingWork();
      }
      catch (Exception ex)
      {
        OperationCanceledException canceledException = ex as OperationCanceledException;
        if (canceledException != null && canceledException.CancellationToken == this.m_groupState.CancellationState.MergedCancellationToken && this.m_groupState.CancellationState.MergedCancellationToken.IsCancellationRequested)
          return;
        this.m_groupState.CancellationState.InternalCancellationTokenSource.Cancel();
        throw;
      }
      finally
      {
        this.SpoolingFinally();
      }
    }

    protected abstract void SpoolingWork();

    protected virtual void SpoolingFinally()
    {
    }
  }
}
