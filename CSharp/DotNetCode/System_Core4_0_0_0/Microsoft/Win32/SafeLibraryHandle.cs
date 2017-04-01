// Type: Microsoft.Win32.SafeLibraryHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    internal SafeLibraryHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle()
    {
      return UnsafeNativeMethods.FreeLibrary(this.handle);
    }
  }
}
