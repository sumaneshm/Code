// Type: Microsoft.Win32.SafeHandles.SafeCspHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  internal sealed class SafeCspHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeCspHandle()
      : base(true)
    {
    }

    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("advapi32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool CryptContextAddRef(SafeCspHandle hProv, IntPtr pdwReserved, int dwFlags);

    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("advapi32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool CryptReleaseContext(IntPtr hProv, int dwFlags);

    public SafeCspHandle Duplicate()
    {
      bool success = false;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        this.DangerousAddRef(ref success);
        IntPtr handle = this.DangerousGetHandle();
        int hr = 0;
        SafeCspHandle safeCspHandle = new SafeCspHandle();
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
        }
        finally
        {
          if (!SafeCspHandle.CryptContextAddRef(this, IntPtr.Zero, 0))
            hr = Marshal.GetLastWin32Error();
          else
            safeCspHandle.SetHandle(handle);
        }
        if (hr == 0)
          return safeCspHandle;
        safeCspHandle.Dispose();
        throw new CryptographicException(hr);
      }
      finally
      {
        if (success)
          this.DangerousRelease();
      }
    }

    protected override bool ReleaseHandle()
    {
      return SafeCspHandle.CryptReleaseContext(this.handle, 0);
    }
  }
}
