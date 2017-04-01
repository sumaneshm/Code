// Type: Microsoft.Win32.SafeHandles.SafeMemoryMappedFileHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  public sealed class SafeMemoryMappedFileHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal SafeMemoryMappedFileHandle()
      : base(true)
    {
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal SafeMemoryMappedFileHandle(IntPtr handle, bool ownsHandle)
      : base(ownsHandle)
    {
      this.SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
      return Microsoft.Win32.UnsafeNativeMethods.CloseHandle(this.handle);
    }
  }
}
