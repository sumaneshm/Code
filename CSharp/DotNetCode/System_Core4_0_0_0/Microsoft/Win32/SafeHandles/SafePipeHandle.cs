// Type: Microsoft.Win32.SafeHandles.SafePipeHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
  public sealed class SafePipeHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafePipeHandle()
      : base(true)
    {
    }

    public SafePipeHandle(IntPtr preexistingHandle, bool ownsHandle)
      : base(ownsHandle)
    {
      this.SetHandle(preexistingHandle);
    }

    protected override bool ReleaseHandle()
    {
      return Microsoft.Win32.UnsafeNativeMethods.CloseHandle(this.handle);
    }
  }
}
