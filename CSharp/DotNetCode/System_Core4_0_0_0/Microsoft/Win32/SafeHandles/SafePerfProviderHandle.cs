// Type: Microsoft.Win32.SafeHandles.SafePerfProviderHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32;
using System;
using System.Security;
using System.Threading;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  internal sealed class SafePerfProviderHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafePerfProviderHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
      IntPtr hProvider = this.handle;
      if (Interlocked.Exchange(ref this.handle, IntPtr.Zero) != IntPtr.Zero)
      {
        int num = (int) UnsafeNativeMethods.PerfStopProvider(hProvider);
      }
      return true;
    }
  }
}
