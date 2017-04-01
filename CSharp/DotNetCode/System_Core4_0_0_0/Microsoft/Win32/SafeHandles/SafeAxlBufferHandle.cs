// Type: Microsoft.Win32.SafeHandles.SafeAxlBufferHandle
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
  internal sealed class SafeAxlBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeAxlBufferHandle()
      : base(true)
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32")]
    private static IntPtr GetProcessHeap();

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [SuppressUnmanagedCodeSecurity]
    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static bool HeapFree(IntPtr hHeap, int dwFlags, IntPtr lpMem);

    protected override bool ReleaseHandle()
    {
      SafeAxlBufferHandle.HeapFree(SafeAxlBufferHandle.GetProcessHeap(), 0, this.handle);
      return true;
    }
  }
}
