// Type: System.IO.MemoryMappedFiles.MemoryMappedView
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.IO.MemoryMappedFiles
{
  internal class MemoryMappedView : IDisposable
  {
    private SafeMemoryMappedViewHandle m_viewHandle;
    private long m_pointerOffset;
    private long m_size;
    private MemoryMappedFileAccess m_access;
    private const int MaxFlushWaits = 15;
    private const int MaxFlushRetriesPerWait = 20;

    internal SafeMemoryMappedViewHandle ViewHandle
    {
      [SecurityCritical] get
      {
        return this.m_viewHandle;
      }
    }

    internal long PointerOffset
    {
      get
      {
        return this.m_pointerOffset;
      }
    }

    internal long Size
    {
      get
      {
        return this.m_size;
      }
    }

    internal MemoryMappedFileAccess Access
    {
      get
      {
        return this.m_access;
      }
    }

    internal bool IsClosed
    {
      [SecuritySafeCritical] get
      {
        if (this.m_viewHandle != null)
          return this.m_viewHandle.IsClosed;
        else
          return true;
      }
    }

    [SecurityCritical]
    private MemoryMappedView(SafeMemoryMappedViewHandle viewHandle, long pointerOffset, long size, MemoryMappedFileAccess access)
    {
      this.m_viewHandle = viewHandle;
      this.m_pointerOffset = pointerOffset;
      this.m_size = size;
      this.m_access = access;
    }

    [SecurityCritical]
    public unsafe void Flush(IntPtr capacity)
    {
      if (this.m_viewHandle == null)
        return;
      byte* pointer = (byte*) null;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        this.m_viewHandle.AcquirePointer(ref pointer);
        bool flag1 = UnsafeNativeMethods.FlushViewOfFile(pointer, capacity);
        if (flag1)
          return;
        int lastWin32Error = Marshal.GetLastWin32Error();
        bool flag2 = !flag1 && lastWin32Error == 33;
        for (int index1 = 0; flag2 && index1 < 15; ++index1)
        {
          Thread.Sleep(1 << index1);
          for (int index2 = 0; flag2 && index2 < 20; ++index2)
          {
            if (UnsafeNativeMethods.FlushViewOfFile(pointer, capacity))
              return;
            Thread.Sleep(0);
            lastWin32Error = Marshal.GetLastWin32Error();
            flag2 = lastWin32Error == 33;
          }
        }
        __Error.WinIOError(lastWin32Error, string.Empty);
      }
      finally
      {
        if ((IntPtr) pointer != IntPtr.Zero)
          this.m_viewHandle.ReleasePointer();
      }
    }

    [SecurityCritical]
    protected virtual void Dispose(bool disposing)
    {
      if (this.m_viewHandle == null || this.m_viewHandle.IsClosed)
        return;
      this.m_viewHandle.Dispose();
    }

    [SecurityCritical]
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SecurityCritical]
    internal static MemoryMappedView CreateView(SafeMemoryMappedFileHandle memMappedFileHandle, MemoryMappedFileAccess access, long offset, long size)
    {
      ulong num1 = (ulong) offset % (ulong) MemoryMappedFile.GetSystemPageAllocationGranularity();
      ulong num2 = (ulong) offset - num1;
      ulong num3 = size == 0L ? 0UL : (ulong) size + num1;
      if (IntPtr.Size == 4 && num3 > (ulong) uint.MaxValue)
        throw new ArgumentOutOfRangeException("size", SR.GetString("ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed"));
      UnsafeNativeMethods.MEMORYSTATUSEX lpBuffer = new UnsafeNativeMethods.MEMORYSTATUSEX();
      UnsafeNativeMethods.GlobalMemoryStatusEx(lpBuffer);
      ulong num4 = lpBuffer.ullTotalVirtual;
      if (num3 >= num4)
        throw new IOException(SR.GetString("IO_NotEnoughMemory"));
      uint dwFileOffsetLow = (uint) (num2 & (ulong) uint.MaxValue);
      uint dwFileOffsetHigh = (uint) (num2 >> 32);
      SafeMemoryMappedViewHandle mappedViewHandle = UnsafeNativeMethods.MapViewOfFile(memMappedFileHandle, MemoryMappedFile.GetFileMapAccess(access), dwFileOffsetHigh, dwFileOffsetLow, new UIntPtr(num3));
      if (mappedViewHandle.IsInvalid)
        __Error.WinIOError(Marshal.GetLastWin32Error(), string.Empty);
      UnsafeNativeMethods.MEMORY_BASIC_INFORMATION buffer = new UnsafeNativeMethods.MEMORY_BASIC_INFORMATION();
      UnsafeNativeMethods.VirtualQuery(mappedViewHandle, ref buffer, (IntPtr) Marshal.SizeOf<UnsafeNativeMethods.MEMORY_BASIC_INFORMATION>(buffer));
      ulong num5 = (ulong) buffer.RegionSize;
      if (((int) buffer.State & 8192) != 0)
      {
        UnsafeNativeMethods.VirtualAlloc(mappedViewHandle, (UIntPtr) num5, 4096, MemoryMappedFile.GetPageAccess(access));
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (mappedViewHandle.IsInvalid)
          __Error.WinIOError(lastWin32Error, string.Empty);
      }
      if (size == 0L)
        size = (long) num5 - (long) num1;
      mappedViewHandle.Initialize((ulong) size + num1);
      return new MemoryMappedView(mappedViewHandle, (long) num1, size, access);
    }
  }
}
