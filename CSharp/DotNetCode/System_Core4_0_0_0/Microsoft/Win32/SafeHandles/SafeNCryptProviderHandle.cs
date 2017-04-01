// Type: Microsoft.Win32.SafeHandles.SafeNCryptProviderHandle
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
  public sealed class SafeNCryptProviderHandle : SafeNCryptHandle
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public SafeNCryptProviderHandle()
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("ncrypt.dll")]
    private static int NCryptFreeObject(IntPtr hObject);

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal void SetHandleValue(IntPtr newHandleValue)
    {
      this.SetHandle(newHandleValue);
    }

    protected override bool ReleaseNativeHandle()
    {
      return SafeNCryptProviderHandle.NCryptFreeObject(this.handle) == 0;
    }

    internal SafeNCryptProviderHandle Duplicate()
    {
      return base.Duplicate<SafeNCryptProviderHandle>();
    }
  }
}
