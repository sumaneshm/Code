// Type: System.IO.LogStream
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
  internal class LogStream : BufferedStream2
  {
    private long _maxFileSize = 10240000L;
    private int _maxNumberOfFiles = 2;
    private int _currentFileNum = 1;
    private readonly object m_lockObject = new object();
    internal const long DefaultFileSize = 10240000L;
    internal const int DefaultNumberOfFiles = 2;
    internal const LogRetentionOption DefaultRetention = LogRetentionOption.SingleFileUnboundedSize;
    private const int _retentionRetryThreshold = 2;
    private LogRetentionOption _retention;
    private bool _disableLogging;
    private int _retentionRetryCount;
    private bool _canRead;
    private bool _canWrite;
    private bool _canSeek;
    [SecurityCritical]
    private SafeFileHandle _handle;
    private string _fileName;
    private string _fileNameWithoutExt;
    private string _fileExt;
    private string _pathSav;
    private int _fAccessSav;
    private FileShare _shareSav;
    private Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES _secAttrsSav;
    private FileIOPermissionAccess _secAccessSav;
    private FileMode _modeSav;
    private int _flagsAndAttributesSav;
    private bool _seekToEndSav;

    public override bool CanRead
    {
      get
      {
        return this._canRead;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return this._canWrite;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return this._canSeek;
      }
    }

    public override long Length
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public override long Position
    {
      get
      {
        throw new NotSupportedException();
      }
      set
      {
        throw new NotSupportedException();
      }
    }

    [SecurityCritical]
    internal LogStream(string path, int bufferSize, LogRetentionOption retention, long maxFileSize, int maxNumOfFiles)
    {
      string fullPath = Path.GetFullPath(path);
      this._fileName = fullPath;
      if (fullPath.StartsWith("\\\\.\\", StringComparison.Ordinal))
        throw new NotSupportedException(SR.GetString("NotSupported_IONonFileDevices"));
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = LogStream.GetSecAttrs(FileShare.Read);
      int num = 1048576;
      this._canWrite = true;
      this._pathSav = fullPath;
      this._fAccessSav = 1073741824;
      this._shareSav = FileShare.Read;
      this._secAttrsSav = secAttrs;
      this._secAccessSav = FileIOPermissionAccess.Write;
      this._modeSav = retention != LogRetentionOption.SingleFileUnboundedSize ? FileMode.Create : FileMode.OpenOrCreate;
      this._flagsAndAttributesSav = num;
      this._seekToEndSav = retention == LogRetentionOption.SingleFileUnboundedSize;
      this.bufferSize = bufferSize;
      this._retention = retention;
      this._maxFileSize = maxFileSize;
      this._maxNumberOfFiles = maxNumOfFiles;
      this._Init(fullPath, this._fAccessSav, this._shareSav, this._secAttrsSav, this._secAccessSav, this._modeSav, this._flagsAndAttributesSav, this._seekToEndSav);
    }

    [SecurityCritical]
    ~LogStream()
    {
      if (this._handle == null)
        return;
      this.Dispose(false);
    }

    [SecurityCritical]
    internal void _Init(string path, int fAccess, FileShare share, Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs, FileIOPermissionAccess secAccess, FileMode mode, int flagsAndAttributes, bool seekToEnd)
    {
      string fullPath = Path.GetFullPath(path);
      this._fileName = fullPath;
      new FileIOPermission(secAccess, new string[1]
      {
        fullPath
      }).Demand();
      int newMode = Microsoft.Win32.UnsafeNativeMethods.SetErrorMode(1);
      try
      {
        this._handle = Microsoft.Win32.UnsafeNativeMethods.SafeCreateFile(fullPath, fAccess, share, secAttrs, mode, flagsAndAttributes, Microsoft.Win32.UnsafeNativeMethods.NULL);
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (this._handle.IsInvalid)
        {
          bool flag = false;
          try
          {
            new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[1]
            {
              this._fileName
            }).Demand();
            flag = true;
          }
          catch (SecurityException ex)
          {
          }
          if (flag)
            __Error.WinIOError(lastWin32Error, this._fileName);
          else
            __Error.WinIOError(lastWin32Error, Path.GetFileName(this._fileName));
        }
      }
      finally
      {
        Microsoft.Win32.UnsafeNativeMethods.SetErrorMode(newMode);
      }
      this.pos = 0L;
      if (!seekToEnd)
        return;
      this.SeekCore(0L, SeekOrigin.End);
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      throw new NotSupportedException();
    }

    public override int Read(byte[] array, int offset, int count)
    {
      throw new NotSupportedException();
    }

    [SecurityCritical]
    protected override unsafe void WriteCore(byte[] buffer, int offset, int count, bool blockForWrite, out long streamPos)
    {
      int hr = 0;
      int num = this.WriteFileNative(buffer, offset, count, (NativeOverlapped*) null, out hr);
      if (num == -1)
      {
        if (hr == 232)
        {
          num = 0;
        }
        else
        {
          if (hr == 87)
            throw new IOException(SR.GetString("IO_FileTooLongOrHandleNotSync"));
          __Error.WinIOError(hr, string.Empty);
        }
      }
      streamPos = this.AddUnderlyingStreamPosition((long) num);
      this.EnforceRetentionPolicy(this._handle, streamPos);
      streamPos = this.pos;
    }

    [SecurityCritical]
    private unsafe int WriteFileNative(byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
    {
      if (this._handle.IsClosed)
        __Error.FileNotOpen();
      if (this._disableLogging)
      {
        hr = 0;
        return 0;
      }
      else
      {
        if (bytes.Length - offset < count)
          throw new IndexOutOfRangeException(SR.GetString("IndexOutOfRange_IORaceCondition"));
        if (bytes.Length == 0)
        {
          hr = 0;
          return 0;
        }
        else
        {
          int numBytesWritten = 0;
          int num;
          fixed (byte* numPtr = bytes)
            num = Microsoft.Win32.UnsafeNativeMethods.WriteFile(this._handle, numPtr + offset, count, out numBytesWritten, overlapped);
          if (num == 0)
          {
            hr = Marshal.GetLastWin32Error();
            if (hr == 6)
              this._handle.SetHandleAsInvalid();
            return -1;
          }
          else
          {
            hr = 0;
            return numBytesWritten;
          }
        }
      }
    }

    [SecurityCritical]
    private long SeekCore(long offset, SeekOrigin origin)
    {
      int hr = 0;
      long num = Microsoft.Win32.UnsafeNativeMethods.SetFilePointer(this._handle, offset, origin, out hr);
      if (num == -1L)
      {
        if (hr == 6)
          this._handle.SetHandleAsInvalid();
        __Error.WinIOError(hr, string.Empty);
      }
      this.UnderlyingStreamPosition = num;
      return num;
    }

    [SecurityCritical]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this._handle != null && !this._handle.IsClosed)
          return;
        this.DiscardBuffer();
      }
      finally
      {
        try
        {
          base.Dispose(disposing);
        }
        finally
        {
          if (this._handle != null && !this._handle.IsClosed)
            this._handle.Dispose();
          this._handle = (SafeFileHandle) null;
          this._canRead = false;
          this._canWrite = false;
          this._canSeek = false;
        }
      }
    }

    [SecurityCritical]
    private void EnforceRetentionPolicy(SafeFileHandle handle, long lastPos)
    {
      switch (this._retention)
      {
        case LogRetentionOption.UnlimitedSequentialFiles:
        case LogRetentionOption.LimitedCircularFiles:
        case LogRetentionOption.LimitedSequentialFiles:
          if (lastPos < this._maxFileSize || handle != this._handle)
            break;
          lock (this.m_lockObject)
          {
            if (handle != this._handle || lastPos < this._maxFileSize)
              break;
            ++this._currentFileNum;
            if (this._retention == LogRetentionOption.LimitedCircularFiles && this._currentFileNum > this._maxNumberOfFiles)
              this._currentFileNum = 1;
            else if (this._retention == LogRetentionOption.LimitedSequentialFiles && this._currentFileNum > this._maxNumberOfFiles)
            {
              this._DisableLogging();
              break;
            }
            if (this._fileNameWithoutExt == null)
            {
              this._fileNameWithoutExt = Path.Combine(Path.GetDirectoryName(this._pathSav), Path.GetFileNameWithoutExtension(this._pathSav));
              this._fileExt = Path.GetExtension(this._pathSav);
            }
            string local_0 = this._currentFileNum == 1 ? this._pathSav : this._fileNameWithoutExt + this._currentFileNum.ToString((IFormatProvider) CultureInfo.InvariantCulture) + this._fileExt;
            try
            {
              this._Init(local_0, this._fAccessSav, this._shareSav, this._secAttrsSav, this._secAccessSav, this._modeSav, this._flagsAndAttributesSav, this._seekToEndSav);
              if (handle == null || handle.IsClosed)
                break;
              handle.Dispose();
              break;
            }
            catch (IOException exception_0)
            {
              this._handle = handle;
              ++this._retentionRetryCount;
              if (this._retentionRetryCount < 2)
                break;
              this._DisableLogging();
              break;
            }
            catch (UnauthorizedAccessException exception_1)
            {
              this._DisableLogging();
              break;
            }
            catch (Exception exception_2)
            {
              this._DisableLogging();
              break;
            }
          }
        case LogRetentionOption.SingleFileBoundedSize:
          if (lastPos < this._maxFileSize)
            break;
          this._DisableLogging();
          break;
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private void _DisableLogging()
    {
      this._disableLogging = true;
    }

    [SecurityCritical]
    private static Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES GetSecAttrs(FileShare share)
    {
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES structure = (Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES) null;
      if ((share & FileShare.Inheritable) != FileShare.None)
      {
        structure = new Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES();
        structure.nLength = Marshal.SizeOf<Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES>(structure);
        structure.bInheritHandle = 1;
      }
      return structure;
    }
  }
}
