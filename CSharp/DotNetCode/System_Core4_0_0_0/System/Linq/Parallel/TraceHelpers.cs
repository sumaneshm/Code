// Type: System.Linq.Parallel.TraceHelpers
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;

namespace System.Linq.Parallel
{
  internal static class TraceHelpers
  {
    [Conditional("PFXTRACE")]
    internal static void SetVerbose()
    {
    }

    [Conditional("PFXTRACE")]
    internal static void TraceInfo(string msg, params object[] args)
    {
    }

    [Conditional("PFXTRACE")]
    internal static void TraceWarning(string msg, params object[] args)
    {
    }

    [Conditional("PFXTRACE")]
    internal static void TraceError(string msg, params object[] args)
    {
    }

    internal static void NotYetImplemented()
    {
      TraceHelpers.NotYetImplemented(false, "NYI");
    }

    internal static void NotYetImplemented(string message)
    {
      TraceHelpers.NotYetImplemented(false, "NYI: " + message);
    }

    internal static void NotYetImplemented(bool assertCondition, string message)
    {
      if (!assertCondition)
        throw new NotImplementedException();
    }
  }
}
