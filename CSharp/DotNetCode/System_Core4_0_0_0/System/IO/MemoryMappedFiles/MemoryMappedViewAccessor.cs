// Type: System.IO.MemoryMappedFiles.MemoryMappedViewAccessor
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.IO.MemoryMappedFiles
{
  public sealed class MemoryMappedViewAccessor : UnmanagedMemoryAccessor
  {
    private MemoryMappedView m_view;

    public SafeMemoryMappedViewHandle SafeMemoryMappedViewHandle
    {
      [SecurityCritical, SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)] get
      {
        if (this.m_view == null)
          return (SafeMemoryMappedViewHandle) null;
        else
          return this.m_view.ViewHandle;
      }
    }

    public long PointerOffset
    {
      get
      {
        if (this.m_view == null)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_ViewIsNull"));
        else
          return this.m_view.PointerOffset;
      }
    }

    [SecurityCritical]
    internal MemoryMappedViewAccessor(MemoryMappedView view)
    {
      this.m_view = view;
      this.Initialize((SafeBuffer) this.m_view.ViewHandle, this.m_view.PointerOffset, this.m_view.Size, MemoryMappedFile.GetFileAccess(this.m_view.Access));
    }

    [SecuritySafeCritical]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this.m_view == null || this.m_view.IsClosed)
          return;
        this.Flush();
      }
      finally
      {
        try
        {
          if (this.m_view != null)
            this.m_view.Dispose();
        }
        finally
        {
          base.Dispose(disposing);
        }
      }
    }

    [SecurityCritical]
    public void Flush()
    {
      if (!this.IsOpen)
        throw new ObjectDisposedException("MemoryMappedViewAccessor", SR.GetString("ObjectDisposed_ViewAccessorClosed"));
      if (this.m_view == null)
        return;
      this.m_view.Flush((IntPtr) this.Capacity);
    }
  }
}
