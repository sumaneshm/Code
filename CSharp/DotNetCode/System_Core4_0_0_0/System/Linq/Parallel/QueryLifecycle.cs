// Type: System.Linq.Parallel.QueryLifecycle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Diagnostics;

namespace System.Linq.Parallel
{
  internal static class QueryLifecycle
  {
    internal static void LogicalQueryExecutionBegin(int queryID)
    {
      Debugger.NotifyOfCrossThreadDependency();
      PlinqEtwProvider.Log.ParallelQueryBegin(queryID);
    }

    internal static void LogicalQueryExecutionEnd(int queryID)
    {
      PlinqEtwProvider.Log.ParallelQueryEnd(queryID);
    }
  }
}
