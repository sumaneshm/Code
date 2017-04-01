// Type: Microsoft.Win32.SafeHandles.SafeCapiHashHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  internal sealed class SafeCapiHashHandle : SafeCapiHandleBase
  {
    public static SafeCapiHashHandle InvalidHandle
    {
      get
      {
        SafeCapiHashHandle safeCapiHashHandle = new SafeCapiHashHandle();
        safeCapiHashHandle.SetHandle(IntPtr.Zero);
        return safeCapiHashHandle;
      }
    }

    private SafeCapiHashHandle()
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("advapi32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool CryptDestroyHash(IntPtr hHash);

    protected override bool ReleaseCapiChildHandle()
    {
      return SafeCapiHashHandle.CryptDestroyHash(this.handle);
    }
  }
}
