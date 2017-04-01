// Type: System.IO.__Error
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.IO
{
  internal static class __Error
  {
    internal static void EndOfFile()
    {
      throw new EndOfStreamException(SR.GetString("IO_EOF_ReadBeyondEOF"));
    }

    internal static void FileNotOpen()
    {
      throw new ObjectDisposedException((string) null, SR.GetString("ObjectDisposed_FileClosed"));
    }

    internal static void PipeNotOpen()
    {
      throw new ObjectDisposedException((string) null, SR.GetString("ObjectDisposed_PipeClosed"));
    }

    internal static void StreamIsClosed()
    {
      throw new ObjectDisposedException((string) null, SR.GetString("ObjectDisposed_StreamIsClosed"));
    }

    internal static void ReadNotSupported()
    {
      throw new NotSupportedException(SR.GetString("NotSupported_UnreadableStream"));
    }

    internal static void SeekNotSupported()
    {
      throw new NotSupportedException(SR.GetString("NotSupported_UnseekableStream"));
    }

    internal static void WrongAsyncResult()
    {
      throw new ArgumentException(SR.GetString("Argument_WrongAsyncResult"));
    }

    internal static void EndReadCalledTwice()
    {
      throw new ArgumentException(SR.GetString("InvalidOperation_EndReadCalledMultiple"));
    }

    internal static void EndWriteCalledTwice()
    {
      throw new ArgumentException(SR.GetString("InvalidOperation_EndWriteCalledMultiple"));
    }

    internal static void EndWaitForConnectionCalledTwice()
    {
      throw new ArgumentException(SR.GetString("InvalidOperation_EndWaitForConnectionCalledMultiple"));
    }

    [SecuritySafeCritical]
    internal static string GetDisplayablePath(string path, bool isInvalidPath)
    {
      if (string.IsNullOrEmpty(path))
        return path;
      bool flag1 = false;
      if (path.Length < 2)
        return path;
      if ((int) path[0] == (int) Path.DirectorySeparatorChar && (int) path[1] == (int) Path.DirectorySeparatorChar)
        flag1 = true;
      else if ((int) path[1] == (int) Path.VolumeSeparatorChar)
        flag1 = true;
      if (!flag1 && !isInvalidPath)
        return path;
      bool flag2 = false;
      try
      {
        if (!isInvalidPath)
        {
          new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[1]
          {
            path
          }).Demand();
          flag2 = true;
        }
      }
      catch (SecurityException ex)
      {
      }
      catch (ArgumentException ex)
      {
      }
      catch (NotSupportedException ex)
      {
      }
      if (!flag2)
        path = (int) path[path.Length - 1] != (int) Path.DirectorySeparatorChar ? Path.GetFileName(path) : SR.GetString("IO_IO_NoPermissionToDirectoryName");
      return path;
    }

    [SecurityCritical]
    internal static void WinIOError()
    {
      __Error.WinIOError(Marshal.GetLastWin32Error(), string.Empty);
    }

    [SecurityCritical]
    internal static void WinIOError(int errorCode, string maybeFullPath)
    {
      bool isInvalidPath = errorCode == 123 || errorCode == 161;
      string displayablePath = __Error.GetDisplayablePath(maybeFullPath, isInvalidPath);
      switch (errorCode)
      {
        case 206:
          throw new PathTooLongException(SR.GetString("IO_PathTooLong"));
        case 995:
          throw new OperationCanceledException();
        case 87:
          throw new IOException(Microsoft.Win32.UnsafeNativeMethods.GetMessage(errorCode), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
        case 183:
          if (displayablePath.Length != 0)
            throw new IOException(SR.GetString("IO_IO_AlreadyExists_Name", new object[1]
            {
              (object) displayablePath
            }), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
          else
            break;
        case 32:
          if (displayablePath.Length == 0)
            throw new IOException(SR.GetString("IO_IO_SharingViolation_NoFileName"), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
          throw new IOException(SR.GetString("IO_IO_SharingViolation_File", new object[1]
          {
            (object) displayablePath
          }), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
        case 80:
          if (displayablePath.Length != 0)
            throw new IOException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, SR.GetString("IO_IO_FileExists_Name"), new object[1]
            {
              (object) displayablePath
            }), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
          else
            break;
        case 2:
          if (displayablePath.Length == 0)
            throw new FileNotFoundException(SR.GetString("IO_FileNotFound"));
          throw new FileNotFoundException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, SR.GetString("IO_FileNotFound_FileName"), new object[1]
          {
            (object) displayablePath
          }), displayablePath);
        case 3:
          if (displayablePath.Length == 0)
            throw new DirectoryNotFoundException(SR.GetString("IO_PathNotFound_NoPathName"));
          throw new DirectoryNotFoundException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, SR.GetString("IO_PathNotFound_Path"), new object[1]
          {
            (object) displayablePath
          }));
        case 5:
          if (displayablePath.Length == 0)
            throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_NoPathName"));
          throw new UnauthorizedAccessException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, SR.GetString("UnauthorizedAccess_IODenied_Path"), new object[1]
          {
            (object) displayablePath
          }));
        case 15:
          throw new DriveNotFoundException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, SR.GetString("IO_DriveNotFound_Drive"), new object[1]
          {
            (object) displayablePath
          }));
      }
      throw new IOException(Microsoft.Win32.UnsafeNativeMethods.GetMessage(errorCode), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
    }

    internal static void WriteNotSupported()
    {
      throw new NotSupportedException(SR.GetString("NotSupported_UnwritableStream"));
    }
  }
}
