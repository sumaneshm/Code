// Type: System.Threading.Tasks.TaskExtensions
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.InteropServices;

namespace System.Threading.Tasks
{
  [__DynamicallyInvokable]
  public static class TaskExtensions
  {
    [__DynamicallyInvokable]
    public static Task Unwrap(this Task<Task> task)
    {
      if (task == null)
        throw new ArgumentNullException("task");
      else
        return (Task) Task.CreateUnwrapPromise<TaskExtensions.VoidResult>((Task) task, false);
    }

    [__DynamicallyInvokable]
    public static Task<TResult> Unwrap<TResult>(this Task<Task<TResult>> task)
    {
      if (task == null)
        throw new ArgumentNullException("task");
      else
        return Task.CreateUnwrapPromise<TResult>((Task) task, false);
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct VoidResult
    {
    }
  }
}
