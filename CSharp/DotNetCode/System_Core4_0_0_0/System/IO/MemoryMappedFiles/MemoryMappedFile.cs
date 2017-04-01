// Type: System.IO.MemoryMappedFiles.MemoryMappedFile
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Threading;

namespace System.IO.MemoryMappedFiles
{
  public class MemoryMappedFile : IDisposable
  {
    private SafeMemoryMappedFileHandle _handle;
    private bool _leaveOpen;
    private FileStream _fileStream;
    internal const int DefaultSize = 0;

    public SafeMemoryMappedFileHandle SafeMemoryMappedFileHandle
    {
      [SecurityCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)] get
      {
        return this._handle;
      }
    }

    [SecurityCritical]
    private MemoryMappedFile(SafeMemoryMappedFileHandle handle)
    {
      this._handle = handle;
      this._leaveOpen = true;
    }

    [SecurityCritical]
    private MemoryMappedFile(SafeMemoryMappedFileHandle handle, FileStream fileStream, bool leaveOpen)
    {
      this._handle = handle;
      this._fileStream = fileStream;
      this._leaveOpen = leaveOpen;
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile OpenExisting(string mapName)
    {
      return MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.ReadWrite, HandleInheritability.None);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile OpenExisting(string mapName, MemoryMappedFileRights desiredAccessRights)
    {
      return MemoryMappedFile.OpenExisting(mapName, desiredAccessRights, HandleInheritability.None);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static MemoryMappedFile OpenExisting(string mapName, MemoryMappedFileRights desiredAccessRights, HandleInheritability inheritability)
    {
      if (mapName == null)
        throw new ArgumentNullException("mapName", SR.GetString("ArgumentNull_MapName"));
      if (mapName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_MapNameEmptyString"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability");
      if ((desiredAccessRights & ~(MemoryMappedFileRights.FullControl | MemoryMappedFileRights.AccessSystemSecurity)) != (MemoryMappedFileRights) 0)
        throw new ArgumentOutOfRangeException("desiredAccessRights");
      else
        return new MemoryMappedFile(MemoryMappedFile.OpenCore(mapName, inheritability, (int) desiredAccessRights, false));
    }

    public static MemoryMappedFile CreateFromFile(string path)
    {
      return MemoryMappedFile.CreateFromFile(path, FileMode.Open, (string) null, 0L, MemoryMappedFileAccess.ReadWrite);
    }

    public static MemoryMappedFile CreateFromFile(string path, FileMode mode)
    {
      return MemoryMappedFile.CreateFromFile(path, mode, (string) null, 0L, MemoryMappedFileAccess.ReadWrite);
    }

    public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string mapName)
    {
      return MemoryMappedFile.CreateFromFile(path, mode, mapName, 0L, MemoryMappedFileAccess.ReadWrite);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string mapName, long capacity)
    {
      return MemoryMappedFile.CreateFromFile(path, mode, mapName, capacity, MemoryMappedFileAccess.ReadWrite);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static MemoryMappedFile CreateFromFile(string path, FileMode mode, string mapName, long capacity, MemoryMappedFileAccess access)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (mapName != null && mapName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_MapNameEmptyString"));
      if (capacity < 0L)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_PositiveOrDefaultCapacityRequired"));
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException("access");
      if (mode == FileMode.Append)
        throw new ArgumentException(SR.GetString("Argument_NewMMFAppendModeNotAllowed"), "mode");
      if (access == MemoryMappedFileAccess.Write)
        throw new ArgumentException(SR.GetString("Argument_NewMMFWriteAccessNotAllowed"), "access");
      bool existed = File.Exists(path);
      FileStream fileStream = new FileStream(path, mode, MemoryMappedFile.GetFileStreamFileSystemRights(access), FileShare.None, 4096, FileOptions.None);
      if (capacity == 0L && fileStream.Length == 0L)
      {
        MemoryMappedFile.CleanupFile(fileStream, existed, path);
        throw new ArgumentException(SR.GetString("Argument_EmptyFile"));
      }
      else if (access == MemoryMappedFileAccess.Read && capacity > fileStream.Length)
      {
        MemoryMappedFile.CleanupFile(fileStream, existed, path);
        throw new ArgumentException(SR.GetString("Argument_ReadAccessWithLargeCapacity"));
      }
      else
      {
        if (capacity == 0L)
          capacity = fileStream.Length;
        if (fileStream.Length > capacity)
        {
          MemoryMappedFile.CleanupFile(fileStream, existed, path);
          throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_CapacityGEFileSizeRequired"));
        }
        else
        {
          SafeMemoryMappedFileHandle core;
          try
          {
            core = MemoryMappedFile.CreateCore(fileStream.SafeFileHandle, mapName, HandleInheritability.None, (MemoryMappedFileSecurity) null, access, MemoryMappedFileOptions.None, capacity);
          }
          catch
          {
            MemoryMappedFile.CleanupFile(fileStream, existed, path);
            throw;
          }
          return new MemoryMappedFile(core, fileStream, false);
        }
      }
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static MemoryMappedFile CreateFromFile(FileStream fileStream, string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability, bool leaveOpen)
    {
      if (fileStream == null)
        throw new ArgumentNullException("fileStream", SR.GetString("ArgumentNull_FileStream"));
      if (mapName != null && mapName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_MapNameEmptyString"));
      if (capacity < 0L)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_PositiveOrDefaultCapacityRequired"));
      if (capacity == 0L && fileStream.Length == 0L)
        throw new ArgumentException(SR.GetString("Argument_EmptyFile"));
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException("access");
      if (access == MemoryMappedFileAccess.Write)
        throw new ArgumentException(SR.GetString("Argument_NewMMFWriteAccessNotAllowed"), "access");
      if (access == MemoryMappedFileAccess.Read && capacity > fileStream.Length)
        throw new ArgumentException(SR.GetString("Argument_ReadAccessWithLargeCapacity"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability");
      ((Stream) fileStream).Flush();
      if (capacity == 0L)
        capacity = fileStream.Length;
      if (fileStream.Length > capacity)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_CapacityGEFileSizeRequired"));
      else
        return new MemoryMappedFile(MemoryMappedFile.CreateCore(fileStream.SafeFileHandle, mapName, inheritability, memoryMappedFileSecurity, access, MemoryMappedFileOptions.None, capacity), fileStream, leaveOpen);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile CreateNew(string mapName, long capacity)
    {
      return MemoryMappedFile.CreateNew(mapName, capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, (MemoryMappedFileSecurity) null, HandleInheritability.None);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile CreateNew(string mapName, long capacity, MemoryMappedFileAccess access)
    {
      return MemoryMappedFile.CreateNew(mapName, capacity, access, MemoryMappedFileOptions.None, (MemoryMappedFileSecurity) null, HandleInheritability.None);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static MemoryMappedFile CreateNew(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability)
    {
      if (mapName != null && mapName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_MapNameEmptyString"));
      if (capacity <= 0L)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_NeedPositiveNumber"));
      if (IntPtr.Size == 4 && capacity > (long) uint.MaxValue)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed"));
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException("access");
      if (access == MemoryMappedFileAccess.Write)
        throw new ArgumentException(SR.GetString("Argument_NewMMFWriteAccessNotAllowed"), "access");
      if ((options & ~MemoryMappedFileOptions.DelayAllocatePages) != MemoryMappedFileOptions.None)
        throw new ArgumentOutOfRangeException("options");
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability");
      else
        return new MemoryMappedFile(MemoryMappedFile.CreateCore(new SafeFileHandle(new IntPtr(-1), true), mapName, inheritability, memoryMappedFileSecurity, access, options, capacity));
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile CreateOrOpen(string mapName, long capacity)
    {
      return MemoryMappedFile.CreateOrOpen(mapName, capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, (MemoryMappedFileSecurity) null, HandleInheritability.None);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access)
    {
      return MemoryMappedFile.CreateOrOpen(mapName, capacity, access, MemoryMappedFileOptions.None, (MemoryMappedFileSecurity) null, HandleInheritability.None);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static MemoryMappedFile CreateOrOpen(string mapName, long capacity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, MemoryMappedFileSecurity memoryMappedFileSecurity, HandleInheritability inheritability)
    {
      if (mapName == null)
        throw new ArgumentNullException("mapName", SR.GetString("ArgumentNull_MapName"));
      if (mapName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_MapNameEmptyString"));
      if (capacity <= 0L)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_NeedPositiveNumber"));
      if (IntPtr.Size == 4 && capacity > (long) uint.MaxValue)
        throw new ArgumentOutOfRangeException("capacity", SR.GetString("ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed"));
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException("access");
      if ((options & ~MemoryMappedFileOptions.DelayAllocatePages) != MemoryMappedFileOptions.None)
        throw new ArgumentOutOfRangeException("options");
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability");
      else
        return new MemoryMappedFile(access != MemoryMappedFileAccess.Write ? MemoryMappedFile.CreateOrOpenCore(new SafeFileHandle(new IntPtr(-1), true), mapName, inheritability, memoryMappedFileSecurity, access, options, capacity) : MemoryMappedFile.OpenCore(mapName, inheritability, MemoryMappedFile.GetFileMapAccess(access), true));
    }

    public MemoryMappedViewStream CreateViewStream()
    {
      return this.CreateViewStream(0L, 0L, MemoryMappedFileAccess.ReadWrite);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public MemoryMappedViewStream CreateViewStream(long offset, long size)
    {
      return this.CreateViewStream(offset, size, MemoryMappedFileAccess.ReadWrite);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public MemoryMappedViewStream CreateViewStream(long offset, long size, MemoryMappedFileAccess access)
    {
      if (offset < 0L)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (size < 0L)
        throw new ArgumentOutOfRangeException("size", SR.GetString("ArgumentOutOfRange_PositiveOrDefaultSizeRequired"));
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException("access");
      if (IntPtr.Size == 4 && size > (long) uint.MaxValue)
        throw new ArgumentOutOfRangeException("size", SR.GetString("ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed"));
      else
        return new MemoryMappedViewStream(MemoryMappedView.CreateView(this._handle, access, offset, size));
    }

    public MemoryMappedViewAccessor CreateViewAccessor()
    {
      return this.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.ReadWrite);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public MemoryMappedViewAccessor CreateViewAccessor(long offset, long size)
    {
      return this.CreateViewAccessor(offset, size, MemoryMappedFileAccess.ReadWrite);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public MemoryMappedViewAccessor CreateViewAccessor(long offset, long size, MemoryMappedFileAccess access)
    {
      if (offset < 0L)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (size < 0L)
        throw new ArgumentOutOfRangeException("size", SR.GetString("ArgumentOutOfRange_PositiveOrDefaultSizeRequired"));
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException("access");
      if (IntPtr.Size == 4 && size > (long) uint.MaxValue)
        throw new ArgumentOutOfRangeException("size", SR.GetString("ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed"));
      else
        return new MemoryMappedViewAccessor(MemoryMappedView.CreateView(this._handle, access, offset, size));
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SecuritySafeCritical]
    protected virtual void Dispose(bool disposing)
    {
      try
      {
        if (this._handle == null || this._handle.IsClosed)
          return;
        this._handle.Dispose();
      }
      finally
      {
        if (this._fileStream != null && !this._leaveOpen)
          this._fileStream.Dispose();
      }
    }

    [SecurityCritical]
    public MemoryMappedFileSecurity GetAccessControl()
    {
      if (this._handle.IsClosed)
        __Error.FileNotOpen();
      return new MemoryMappedFileSecurity(this._handle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
    }

    [SecurityCritical]
    public void SetAccessControl(MemoryMappedFileSecurity memoryMappedFileSecurity)
    {
      if (memoryMappedFileSecurity == null)
        throw new ArgumentNullException("memoryMappedFileSecurity");
      if (this._handle.IsClosed)
        __Error.FileNotOpen();
      memoryMappedFileSecurity.PersistHandle((SafeHandle) this._handle);
    }

    [SecurityCritical]
    private static SafeMemoryMappedFileHandle CreateCore(SafeFileHandle fileHandle, string mapName, HandleInheritability inheritability, MemoryMappedFileSecurity memoryMappedFileSecurity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, long capacity)
    {
      SafeMemoryMappedFileHandle mappedFileHandle = (SafeMemoryMappedFileHandle) null;
      object pinningHandle;
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = MemoryMappedFile.GetSecAttrs(inheritability, memoryMappedFileSecurity, out pinningHandle);
      int dwMaximumSizeLow = (int) (capacity & (long) uint.MaxValue);
      int dwMaximumSizeHigh = (int) (capacity >> 32);
      try
      {
        mappedFileHandle = Microsoft.Win32.UnsafeNativeMethods.CreateFileMapping(fileHandle, secAttrs, (int) ((MemoryMappedFileOptions) MemoryMappedFile.GetPageAccess(access) | options), dwMaximumSizeHigh, dwMaximumSizeLow, mapName);
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (!mappedFileHandle.IsInvalid && lastWin32Error == 183)
        {
          mappedFileHandle.Dispose();
          __Error.WinIOError(lastWin32Error, string.Empty);
        }
        else if (mappedFileHandle.IsInvalid)
          __Error.WinIOError(lastWin32Error, string.Empty);
      }
      finally
      {
        if (pinningHandle != null)
          ((GCHandle) pinningHandle).Free();
      }
      return mappedFileHandle;
    }

    [SecurityCritical]
    private static SafeMemoryMappedFileHandle OpenCore(string mapName, HandleInheritability inheritability, int desiredAccessRights, bool createOrOpen)
    {
      SafeMemoryMappedFileHandle mappedFileHandle = Microsoft.Win32.UnsafeNativeMethods.OpenFileMapping(desiredAccessRights, (inheritability & HandleInheritability.Inheritable) != HandleInheritability.None, mapName);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (mappedFileHandle.IsInvalid)
      {
        if (createOrOpen && lastWin32Error == 2)
          throw new ArgumentException(SR.GetString("Argument_NewMMFWriteAccessNotAllowed"), "access");
        __Error.WinIOError(lastWin32Error, string.Empty);
      }
      return mappedFileHandle;
    }

    [SecurityCritical]
    private static SafeMemoryMappedFileHandle CreateOrOpenCore(SafeFileHandle fileHandle, string mapName, HandleInheritability inheritability, MemoryMappedFileSecurity memoryMappedFileSecurity, MemoryMappedFileAccess access, MemoryMappedFileOptions options, long capacity)
    {
      SafeMemoryMappedFileHandle mappedFileHandle = (SafeMemoryMappedFileHandle) null;
      object pinningHandle;
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = MemoryMappedFile.GetSecAttrs(inheritability, memoryMappedFileSecurity, out pinningHandle);
      int dwMaximumSizeLow = (int) (capacity & (long) uint.MaxValue);
      int dwMaximumSizeHigh = (int) (capacity >> 32);
      try
      {
        int num = 14;
        int millisecondsTimeout = 0;
        while (num > 0)
        {
          mappedFileHandle = Microsoft.Win32.UnsafeNativeMethods.CreateFileMapping(fileHandle, secAttrs, (int) ((MemoryMappedFileOptions) MemoryMappedFile.GetPageAccess(access) | options), dwMaximumSizeHigh, dwMaximumSizeLow, mapName);
          int lastWin32Error1 = Marshal.GetLastWin32Error();
          if (mappedFileHandle.IsInvalid)
          {
            if (lastWin32Error1 != 5)
              __Error.WinIOError(lastWin32Error1, string.Empty);
            mappedFileHandle.SetHandleAsInvalid();
            mappedFileHandle = Microsoft.Win32.UnsafeNativeMethods.OpenFileMapping(MemoryMappedFile.GetFileMapAccess(access), (inheritability & HandleInheritability.Inheritable) != HandleInheritability.None, mapName);
            int lastWin32Error2 = Marshal.GetLastWin32Error();
            if (mappedFileHandle.IsInvalid)
            {
              if (lastWin32Error2 != 2)
                __Error.WinIOError(lastWin32Error2, string.Empty);
              --num;
              if (millisecondsTimeout == 0)
              {
                millisecondsTimeout = 10;
              }
              else
              {
                Thread.Sleep(millisecondsTimeout);
                millisecondsTimeout *= 2;
              }
            }
            else
              break;
          }
          else
            break;
        }
        if (mappedFileHandle != null)
        {
          if (!mappedFileHandle.IsInvalid)
            goto label_18;
        }
        throw new InvalidOperationException(SR.GetString("InvalidOperation_CantCreateFileMapping"));
      }
      finally
      {
        if (pinningHandle != null)
          ((GCHandle) pinningHandle).Free();
      }
label_18:
      return mappedFileHandle;
    }

    [SecurityCritical]
    internal static int GetSystemPageAllocationGranularity()
    {
      Microsoft.Win32.UnsafeNativeMethods.SYSTEM_INFO lpSystemInfo = new Microsoft.Win32.UnsafeNativeMethods.SYSTEM_INFO();
      Microsoft.Win32.UnsafeNativeMethods.GetSystemInfo(ref lpSystemInfo);
      return lpSystemInfo.dwAllocationGranularity;
    }

    internal static int GetPageAccess(MemoryMappedFileAccess access)
    {
      if (access == MemoryMappedFileAccess.Read)
        return 2;
      if (access == MemoryMappedFileAccess.ReadWrite)
        return 4;
      if (access == MemoryMappedFileAccess.CopyOnWrite)
        return 8;
      if (access == MemoryMappedFileAccess.ReadExecute)
        return 32;
      if (access == MemoryMappedFileAccess.ReadWriteExecute)
        return 64;
      else
        throw new ArgumentOutOfRangeException("access");
    }

    internal static int GetFileMapAccess(MemoryMappedFileAccess access)
    {
      if (access == MemoryMappedFileAccess.Read)
        return 4;
      if (access == MemoryMappedFileAccess.Write)
        return 2;
      if (access == MemoryMappedFileAccess.ReadWrite)
        return 6;
      if (access == MemoryMappedFileAccess.CopyOnWrite)
        return 1;
      if (access == MemoryMappedFileAccess.ReadExecute)
        return 36;
      if (access == MemoryMappedFileAccess.ReadWriteExecute)
        return 38;
      else
        throw new ArgumentOutOfRangeException("access");
    }

    private static FileSystemRights GetFileStreamFileSystemRights(MemoryMappedFileAccess access)
    {
      switch (access)
      {
        case MemoryMappedFileAccess.ReadWrite:
          return FileSystemRights.ReadData | FileSystemRights.WriteData;
        case MemoryMappedFileAccess.Read:
        case MemoryMappedFileAccess.CopyOnWrite:
          return FileSystemRights.ReadData;
        case MemoryMappedFileAccess.Write:
          return FileSystemRights.WriteData;
        case MemoryMappedFileAccess.ReadExecute:
          return FileSystemRights.ReadData | FileSystemRights.ExecuteFile;
        case MemoryMappedFileAccess.ReadWriteExecute:
          return FileSystemRights.ReadData | FileSystemRights.WriteData | FileSystemRights.ExecuteFile;
        default:
          throw new ArgumentOutOfRangeException("access");
      }
    }

    internal static FileAccess GetFileAccess(MemoryMappedFileAccess access)
    {
      if (access == MemoryMappedFileAccess.Read)
        return FileAccess.Read;
      if (access == MemoryMappedFileAccess.Write)
        return FileAccess.Write;
      if (access == MemoryMappedFileAccess.ReadWrite || access == MemoryMappedFileAccess.CopyOnWrite)
        return FileAccess.ReadWrite;
      if (access == MemoryMappedFileAccess.ReadExecute)
        return FileAccess.Read;
      if (access == MemoryMappedFileAccess.ReadWriteExecute)
        return FileAccess.ReadWrite;
      else
        throw new ArgumentOutOfRangeException("access");
    }

    [SecurityCritical]
    private static unsafe Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES GetSecAttrs(HandleInheritability inheritability, MemoryMappedFileSecurity memoryMappedFileSecurity, out object pinningHandle)
    {
      pinningHandle = (object) null;
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES structure = (Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES) null;
      if ((inheritability & HandleInheritability.Inheritable) != HandleInheritability.None || memoryMappedFileSecurity != null)
      {
        structure = new Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES();
        structure.nLength = Marshal.SizeOf<Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES>(structure);
        if ((inheritability & HandleInheritability.Inheritable) != HandleInheritability.None)
          structure.bInheritHandle = 1;
        if (memoryMappedFileSecurity != null)
        {
          byte[] descriptorBinaryForm = memoryMappedFileSecurity.GetSecurityDescriptorBinaryForm();
          pinningHandle = (object) GCHandle.Alloc((object) descriptorBinaryForm, GCHandleType.Pinned);
          fixed (byte* numPtr = descriptorBinaryForm)
            structure.pSecurityDescriptor = numPtr;
        }
      }
      return structure;
    }

    private static void CleanupFile(FileStream fileStream, bool existed, string path)
    {
      fileStream.Close();
      if (existed)
        return;
      File.Delete(path);
    }
  }
}
