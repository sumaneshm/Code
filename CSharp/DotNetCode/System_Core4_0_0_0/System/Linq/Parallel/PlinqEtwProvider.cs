// Type: System.Linq.Parallel.PlinqEtwProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  [EventSource(Guid = "159eeeec-4a14-4418-a8fe-faabcd987887", LocalizationResources = "System.Linq", Name = "System.Linq.Parallel.PlinqEventSource")]
  internal sealed class PlinqEtwProvider : EventSource
  {
    internal static PlinqEtwProvider Log = new PlinqEtwProvider();
    private static readonly int s_defaultSchedulerId = TaskScheduler.Default.Id;
    private static int s_queryId = 0;
    private const EventKeywords ALL_KEYWORDS = ~EventKeywords.None;
    private const int PARALLELQUERYBEGIN_EVENTID = 1;
    private const int PARALLELQUERYEND_EVENTID = 2;
    private const int PARALLELQUERYFORK_EVENTID = 3;
    private const int PARALLELQUERYJOIN_EVENTID = 4;

    static PlinqEtwProvider()
    {
    }

    private PlinqEtwProvider()
    {
    }

    [NonEvent]
    internal static int NextQueryId()
    {
      return Interlocked.Increment(ref PlinqEtwProvider.s_queryId);
    }

    [NonEvent]
    internal void ParallelQueryBegin(int queryId)
    {
      if (!this.IsEnabled(EventLevel.Informational, ~EventKeywords.None))
        return;
      int taskId = Task.CurrentId ?? 0;
      this.ParallelQueryBegin(PlinqEtwProvider.s_defaultSchedulerId, taskId, queryId);
    }

    [Event(1, Level = EventLevel.Informational, Opcode = EventOpcode.Start, Task = (EventTask) 1)]
    private void ParallelQueryBegin(int taskSchedulerId, int taskId, int queryId)
    {
      this.WriteEvent(1, taskSchedulerId, taskId, queryId);
    }

    [NonEvent]
    internal void ParallelQueryEnd(int queryId)
    {
      if (!this.IsEnabled(EventLevel.Informational, ~EventKeywords.None))
        return;
      int taskId = Task.CurrentId ?? 0;
      this.ParallelQueryEnd(PlinqEtwProvider.s_defaultSchedulerId, taskId, queryId);
    }

    [Event(2, Level = EventLevel.Informational, Opcode = EventOpcode.Stop, Task = (EventTask) 1)]
    private void ParallelQueryEnd(int taskSchedulerId, int taskId, int queryId)
    {
      this.WriteEvent(2, taskSchedulerId, taskId, queryId);
    }

    [NonEvent]
    internal void ParallelQueryFork(int queryId)
    {
      if (!this.IsEnabled(EventLevel.Verbose, ~EventKeywords.None))
        return;
      int taskId = Task.CurrentId ?? 0;
      this.ParallelQueryFork(PlinqEtwProvider.s_defaultSchedulerId, taskId, queryId);
    }

    [Event(3, Level = EventLevel.Verbose, Opcode = EventOpcode.Start, Task = (EventTask) 2)]
    private void ParallelQueryFork(int taskSchedulerId, int taskId, int queryId)
    {
      this.WriteEvent(3, taskSchedulerId, taskId, queryId);
    }

    [NonEvent]
    internal void ParallelQueryJoin(int queryId)
    {
      if (!this.IsEnabled(EventLevel.Verbose, ~EventKeywords.None))
        return;
      int taskId = Task.CurrentId ?? 0;
      this.ParallelQueryJoin(PlinqEtwProvider.s_defaultSchedulerId, taskId, queryId);
    }

    [Event(4, Level = EventLevel.Verbose, Opcode = EventOpcode.Stop, Task = (EventTask) 2)]
    private void ParallelQueryJoin(int taskSchedulerId, int taskId, int queryId)
    {
      this.WriteEvent(4, taskSchedulerId, taskId, queryId);
    }

    public class Tasks
    {
      public const EventTask Query = (EventTask) 1;
      public const EventTask ForkJoin = (EventTask) 2;
    }
  }
}
