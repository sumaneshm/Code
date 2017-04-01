// Type: System.Linq.Parallel.QueryTask
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal abstract class QueryTask
  {
    private static Action<object> s_runTaskSynchronouslyDelegate = new Action<object>(QueryTask.RunTaskSynchronously);
    private static Action<object> s_baseWorkDelegate = (Action<object>) (o => ((QueryTask) o).BaseWork((object) null));
    protected int m_taskIndex;
    protected QueryTaskGroupState m_groupState;

    static QueryTask()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected QueryTask(int taskIndex, QueryTaskGroupState groupState)
    {
      this.m_taskIndex = taskIndex;
      this.m_groupState = groupState;
    }

    internal Task RunSynchronously(TaskScheduler taskScheduler)
    {
      Task task = new Task(QueryTask.s_runTaskSynchronouslyDelegate, (object) this, TaskCreationOptions.AttachedToParent);
      task.RunSynchronously(taskScheduler);
      return task;
    }

    internal Task RunAsynchronously(TaskScheduler taskScheduler)
    {
      return Task.Factory.StartNew(QueryTask.s_baseWorkDelegate, (object) this, new CancellationToken(), TaskCreationOptions.PreferFairness | TaskCreationOptions.AttachedToParent, taskScheduler);
    }

    protected abstract void Work();

    private static void RunTaskSynchronously(object o)
    {
      ((QueryTask) o).BaseWork((object) null);
    }

    private void BaseWork(object unused)
    {
      PlinqEtwProvider.Log.ParallelQueryFork(this.m_groupState.QueryId);
      try
      {
        this.Work();
      }
      finally
      {
        PlinqEtwProvider.Log.ParallelQueryJoin(this.m_groupState.QueryId);
      }
    }
  }
}
