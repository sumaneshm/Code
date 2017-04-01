// Type: Microsoft.Win32.SafeHandles.SafeMemoryMappedViewHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  public sealed class SafeMemoryMappedViewHandle : SafeBuffer
  {
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal SafeMemoryMappedViewHandle()
      : base(true)
    {
    }

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal SafeMemoryMappedViewHandle(IntPtr handle, bool ownsHandle)
      : base(ownsHandle)
    {
      this.SetHandle(handle);
    }

    protected override bool ReleaseHandle()
    {
      if (!Microsoft.Win32.UnsafeNativeMethods.UnmapViewOfFile(this.handle))
        return false;
      this.handle = IntPtr.Zero;
      return true;
    }
  }
}
