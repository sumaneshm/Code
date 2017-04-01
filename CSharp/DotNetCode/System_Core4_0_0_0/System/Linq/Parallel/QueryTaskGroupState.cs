// Type: System.Linq.Parallel.QueryTaskGroupState
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class QueryTaskGroupState
  {
    private Task m_rootTask;
    private int m_alreadyEnded;
    private CancellationState m_cancellationState;
    private int m_queryId;

    internal bool IsAlreadyEnded
    {
      get
      {
        return this.m_alreadyEnded == 1;
      }
    }

    internal CancellationState CancellationState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_cancellationState;
      }
    }

    internal int QueryId
    {
      get
      {
        return this.m_queryId;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal QueryTaskGroupState(CancellationState cancellationState, int queryId)
    {
      this.m_cancellationState = cancellationState;
      this.m_queryId = queryId;
    }

    internal void QueryBegin(Task rootTask)
    {
      this.m_rootTask = rootTask;
    }

    internal void QueryEnd(bool userInitiatedDispose)
    {
      if (Interlocked.Exchange(ref this.m_alreadyEnded, 1) != 0)
        return;
      try
      {
        this.m_rootTask.Wait();
      }
      catch (AggregateException ex)
      {
        AggregateException aggregateException = ex.Flatten();
        bool flag = true;
        for (int index = 0; index < aggregateException.InnerExceptions.Count; ++index)
        {
          OperationCanceledException canceledException = aggregateException.InnerExceptions[index] as OperationCanceledException;
          if (canceledException == null || (!canceledException.CancellationToken.IsCancellationRequested || canceledException.CancellationToken != this.m_cancellationState.ExternalCancellationToken))
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          throw aggregateException;
      }
      finally
      {
        this.m_rootTask.Dispose();
      }
      if (!this.m_cancellationState.MergedCancellationToken.IsCancellationRequested)
        return;
      if (!this.m_cancellationState.TopLevelDisposedFlag.Value)
        CancellationState.ThrowWithStandardMessageIfCanceled(this.m_cancellationState.ExternalCancellationToken);
      if (!userInitiatedDispose)
        throw new ObjectDisposedException("enumerator", System.Linq.SR.GetString("PLINQ_DisposeRequested"));
    }
  }
}
