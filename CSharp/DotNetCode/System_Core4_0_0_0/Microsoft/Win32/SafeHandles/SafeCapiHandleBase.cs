// Type: Microsoft.Win32.SafeHandles.SafeCapiHandleBase
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
  internal abstract class SafeCapiHandleBase : SafeHandleZeroOrMinusOneIsInvalid
  {
    private IntPtr m_csp;

    protected IntPtr ParentCsp
    {
      get
      {
        return this.m_csp;
      }
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)] set
      {
        int hr = 0;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
        }
        finally
        {
          if (SafeCapiHandleBase.CryptContextAddRef(value, IntPtr.Zero, 0))
            this.m_csp = value;
          else
            hr = Marshal.GetLastWin32Error();
        }
        if (hr != 0)
          throw new CryptographicException(hr);
      }
    }

    internal SafeCapiHandleBase()
      : base(true)
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("advapi32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool CryptContextAddRef(IntPtr hProv, IntPtr pdwReserved, int dwFlags);

    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("advapi32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool CryptReleaseContext(IntPtr hProv, int dwFlags);

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    internal void SetParentCsp(SafeCspHandle parentCsp)
    {
      bool success = false;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        parentCsp.DangerousAddRef(ref success);
        this.ParentCsp = parentCsp.DangerousGetHandle();
      }
      finally
      {
        if (success)
          parentCsp.DangerousRelease();
      }
    }

    protected abstract bool ReleaseCapiChildHandle();

    protected override sealed bool ReleaseHandle()
    {
      bool flag1 = this.ReleaseCapiChildHandle();
      bool flag2 = true;
      if (this.m_csp != IntPtr.Zero)
        flag2 = SafeCapiHandleBase.CryptReleaseContext(this.m_csp, 0);
      if (flag1)
        return flag2;
      else
        return false;
    }
  }
}
