// Type: Microsoft.Win32.SafeHandles.SafeBCryptHashHandle
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
  internal sealed class SafeBCryptHashHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private IntPtr m_hashObject;

    internal IntPtr HashObject
    {
      get
      {
        return this.m_hashObject;
      }
      set
      {
        this.m_hashObject = value;
      }
    }

    private SafeBCryptHashHandle()
      : base(true)
    {
    }

    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [DllImport("bcrypt")]
    private static BCryptNative.ErrorCode BCryptDestroyHash(IntPtr hHash);

    protected override bool ReleaseHandle()
    {
      bool flag = SafeBCryptHashHandle.BCryptDestroyHash(this.handle) == BCryptNative.ErrorCode.Success;
      if (this.m_hashObject != IntPtr.Zero)
        Marshal.FreeCoTaskMem(this.m_hashObject);
      return flag;
    }
  }
}
