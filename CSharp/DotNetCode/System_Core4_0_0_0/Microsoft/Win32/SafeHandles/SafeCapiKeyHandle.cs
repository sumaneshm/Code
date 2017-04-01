// Type: Microsoft.Win32.SafeHandles.SafeCapiKeyHandle
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
  internal sealed class SafeCapiKeyHandle : SafeCapiHandleBase
  {
    internal static SafeCapiKeyHandle InvalidHandle
    {
      get
      {
        SafeCapiKeyHandle safeCapiKeyHandle = new SafeCapiKeyHandle();
        safeCapiKeyHandle.SetHandle(IntPtr.Zero);
        return safeCapiKeyHandle;
      }
    }

    private SafeCapiKeyHandle()
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("advapi32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool CryptDestroyKey(IntPtr hKey);

    protected override bool ReleaseCapiChildHandle()
    {
      return SafeCapiKeyHandle.CryptDestroyKey(this.handle);
    }

    internal SafeCapiKeyHandle Duplicate()
    {
      SafeCapiKeyHandle phKey = (SafeCapiKeyHandle) null;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        if (!CapiNative.UnsafeNativeMethods.CryptDuplicateKey(this, IntPtr.Zero, 0, out phKey))
          throw new CryptographicException(Marshal.GetLastWin32Error());
      }
      finally
      {
        if (phKey != null && !phKey.IsInvalid && this.ParentCsp != IntPtr.Zero)
          phKey.ParentCsp = this.ParentCsp;
      }
      return phKey;
    }
  }
}
