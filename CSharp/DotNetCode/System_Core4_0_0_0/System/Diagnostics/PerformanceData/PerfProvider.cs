// Type: System.Diagnostics.PerformanceData.PerfProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Security;

namespace System.Diagnostics.PerformanceData
{
  internal sealed class PerfProvider
  {
    internal Guid m_providerGuid;
    internal int m_counterSet;
    [SecurityCritical]
    internal SafePerfProviderHandle m_hProvider;

    [SecurityCritical]
    internal PerfProvider(Guid providerGuid)
    {
      this.m_providerGuid = providerGuid;
      uint num = UnsafeNativeMethods.PerfStartProvider(ref this.m_providerGuid, (UnsafeNativeMethods.PERFLIBREQUEST) null, out this.m_hProvider);
      if ((int) num != 0)
        throw new Win32Exception((int) num);
    }
  }
}
