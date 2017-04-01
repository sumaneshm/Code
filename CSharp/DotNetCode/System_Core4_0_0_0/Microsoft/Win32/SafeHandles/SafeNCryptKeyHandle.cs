// Type: Microsoft.Win32.SafeHandles.SafeNCryptKeyHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
  public sealed class SafeNCryptKeyHandle : SafeNCryptHandle
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public SafeNCryptKeyHandle()
    {
    }

    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("ncrypt.dll")]
    private static int NCryptFreeObject(IntPtr hObject);

    protected override bool ReleaseNativeHandle()
    {
      return SafeNCryptKeyHandle.NCryptFreeObject(this.handle) == 0;
    }

    internal SafeNCryptKeyHandle Duplicate()
    {
      return base.Duplicate<SafeNCryptKeyHandle>();
    }
  }
}
