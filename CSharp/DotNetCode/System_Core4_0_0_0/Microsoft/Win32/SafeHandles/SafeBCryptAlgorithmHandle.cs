// Type: Microsoft.Win32.SafeHandles.SafeBCryptAlgorithmHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  internal sealed class SafeBCryptAlgorithmHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeBCryptAlgorithmHandle()
      : base(true)
    {
    }

    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("bcrypt")]
    private static BCryptNative.ErrorCode BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, int flags);

    protected override bool ReleaseHandle()
    {
      return SafeBCryptAlgorithmHandle.BCryptCloseAlgorithmProvider(this.handle, 0) == BCryptNative.ErrorCode.Success;
    }
  }
}
