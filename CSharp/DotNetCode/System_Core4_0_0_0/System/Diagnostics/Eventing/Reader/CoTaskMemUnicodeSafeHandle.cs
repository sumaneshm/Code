// Type: System.Diagnostics.Eventing.Reader.CoTaskMemUnicodeSafeHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Diagnostics.Eventing.Reader
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  internal sealed class CoTaskMemUnicodeSafeHandle : SafeHandle
  {
    public override bool IsInvalid
    {
      get
      {
        if (!this.IsClosed)
          return this.handle == IntPtr.Zero;
        else
          return true;
      }
    }

    public static CoTaskMemUnicodeSafeHandle Zero
    {
      get
      {
        return new CoTaskMemUnicodeSafeHandle();
      }
    }

    internal CoTaskMemUnicodeSafeHandle()
      : base(IntPtr.Zero, true)
    {
    }

    internal CoTaskMemUnicodeSafeHandle(IntPtr handle, bool ownsHandle)
      : base(IntPtr.Zero, ownsHandle)
    {
      this.SetHandle(handle);
    }

    internal void SetMemory(IntPtr handle)
    {
      this.SetHandle(handle);
    }

    internal IntPtr GetMemory()
    {
      return this.handle;
    }

    protected override bool ReleaseHandle()
    {
      Marshal.ZeroFreeCoTaskMemUnicode(this.handle);
      this.handle = IntPtr.Zero;
      return true;
    }
  }
}
