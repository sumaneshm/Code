// Type: System.IO.Pipes.PipeStream
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

namespace System.IO.Pipes
{
  [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public abstract class PipeStream : Stream
  {
    private static readonly bool _canUseAsync = Environment.OSVersion.Platform == PlatformID.Win32NT;
    [SecurityCritical]
    private static readonly IOCompletionCallback IOCallback = new IOCompletionCallback(PipeStream.AsyncPSCallback);
    private SafePipeHandle m_handle;
    private bool m_canRead;
    private bool m_canWrite;
    private bool m_isAsync;
    private bool m_isMessageComplete;
    private bool m_isFromExistingHandle;
    private bool m_isHandleExposed;
    private PipeTransmissionMode m_readMode;
    private PipeTransmissionMode m_transmissionMode;
    private PipeDirection m_pipeDirection;
    private int m_outBufferSize;
    private PipeState m_state;

    public bool IsConnected
    {
      get
      {
        return this.State == PipeState.Connected;
      }
      protected set
      {
        this.m_state = value ? PipeState.Connected : PipeState.Disconnected;
      }
    }

    public bool IsAsync
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_isAsync;
      }
    }

    public bool IsMessageComplete
    {
      [SecurityCritical] get
      {
        if (this.m_state == PipeState.WaitingToConnect)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotYetConnected"));
        if (this.m_state == PipeState.Disconnected)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeDisconnected"));
        if (this.m_handle == null)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
        if (this.m_state == PipeState.Closed)
          __Error.PipeNotOpen();
        if (this.m_handle.IsClosed)
          __Error.PipeNotOpen();
        if (this.m_readMode != PipeTransmissionMode.Message)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeReadModeNotMessage"));
        else
          return this.m_isMessageComplete;
      }
    }

    public virtual PipeTransmissionMode TransmissionMode
    {
      [SecurityCritical] get
      {
        this.CheckPipePropertyOperations();
        if (!this.m_isFromExistingHandle)
          return this.m_transmissionMode;
        int lpFlags;
        if (!Microsoft.Win32.UnsafeNativeMethods.GetNamedPipeInfo(this.m_handle, out lpFlags, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL))
          this.WinIOError(Marshal.GetLastWin32Error());
        return (lpFlags & 4) != 0 ? PipeTransmissionMode.Message : PipeTransmissionMode.Byte;
      }
    }

    public virtual int InBufferSize
    {
      [SecurityCritical] get
      {
        this.CheckPipePropertyOperations();
        if (!this.CanRead)
          throw new NotSupportedException(SR.GetString("NotSupported_UnreadableStream"));
        int lpInBufferSize;
        if (!Microsoft.Win32.UnsafeNativeMethods.GetNamedPipeInfo(this.m_handle, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, out lpInBufferSize, Microsoft.Win32.UnsafeNativeMethods.NULL))
          this.WinIOError(Marshal.GetLastWin32Error());
        return lpInBufferSize;
      }
    }

    public virtual int OutBufferSize
    {
      [SecurityCritical] get
      {
        this.CheckPipePropertyOperations();
        if (!this.CanWrite)
          throw new NotSupportedException(SR.GetString("NotSupported_UnwritableStream"));
        int lpOutBufferSize;
        if (this.m_pipeDirection == PipeDirection.Out)
          lpOutBufferSize = this.m_outBufferSize;
        else if (!Microsoft.Win32.UnsafeNativeMethods.GetNamedPipeInfo(this.m_handle, Microsoft.Win32.UnsafeNativeMethods.NULL, out lpOutBufferSize, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL))
          this.WinIOError(Marshal.GetLastWin32Error());
        return lpOutBufferSize;
      }
    }

    public virtual unsafe PipeTransmissionMode ReadMode
    {
      [SecurityCritical] get
      {
        this.CheckPipePropertyOperations();
        if (this.m_isFromExistingHandle || this.IsHandleExposed)
          this.UpdateReadMode();
        return this.m_readMode;
      }
      [SecurityCritical] set
      {
        this.CheckPipePropertyOperations();
        if (value < PipeTransmissionMode.Byte || value > PipeTransmissionMode.Message)
          throw new ArgumentOutOfRangeException("value", SR.GetString("ArgumentOutOfRange_TransmissionModeByteOrMsg"));
        if (!Microsoft.Win32.UnsafeNativeMethods.SetNamedPipeHandleState(this.m_handle, &((int) value << 1), Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL))
          this.WinIOError(Marshal.GetLastWin32Error());
        else
          this.m_readMode = value;
      }
    }

    public SafePipeHandle SafePipeHandle
    {
      [SecurityCritical] get
      {
        if (this.m_handle == null)
          throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
        if (this.m_handle.IsClosed)
          __Error.PipeNotOpen();
        this.m_isHandleExposed = true;
        return this.m_handle;
      }
    }

    internal SafePipeHandle InternalHandle
    {
      [SecurityCritical] get
      {
        return this.m_handle;
      }
    }

    protected bool IsHandleExposed
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_isHandleExposed;
      }
    }

    public override bool CanRead
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_canRead;
      }
    }

    public override bool CanWrite
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_canWrite;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return false;
      }
    }

    public override long Length
    {
      get
      {
        __Error.SeekNotSupported();
        return 0L;
      }
    }

    public override long Position
    {
      get
      {
        __Error.SeekNotSupported();
        return 0L;
      }
      set
      {
        __Error.SeekNotSupported();
      }
    }

    internal PipeState State
    {
      get
      {
        return this.m_state;
      }
      set
      {
        this.m_state = value;
      }
    }

    [SecurityCritical]
    static PipeStream()
    {
    }

    protected PipeStream(PipeDirection direction, int bufferSize)
    {
      if (direction < PipeDirection.In || direction > PipeDirection.InOut)
        throw new ArgumentOutOfRangeException("direction", SR.GetString("ArgumentOutOfRange_DirectionModeInOutOrInOut"));
      if (bufferSize < 0)
        throw new ArgumentOutOfRangeException("bufferSize", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      this.Init(direction, PipeTransmissionMode.Byte, bufferSize);
    }

    protected PipeStream(PipeDirection direction, PipeTransmissionMode transmissionMode, int outBufferSize)
    {
      if (direction < PipeDirection.In || direction > PipeDirection.InOut)
        throw new ArgumentOutOfRangeException("direction", SR.GetString("ArgumentOutOfRange_DirectionModeInOutOrInOut"));
      if (transmissionMode < PipeTransmissionMode.Byte || transmissionMode > PipeTransmissionMode.Message)
        throw new ArgumentOutOfRangeException("transmissionMode", SR.GetString("ArgumentOutOfRange_TransmissionModeByteOrMsg"));
      if (outBufferSize < 0)
        throw new ArgumentOutOfRangeException("outBufferSize", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      this.Init(direction, transmissionMode, outBufferSize);
    }

    [SecurityCritical]
    protected void InitializeHandle(SafePipeHandle handle, bool isExposed, bool isAsync)
    {
      isAsync = isAsync & PipeStream._canUseAsync;
      if (isAsync)
      {
        bool flag = false;
        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
        try
        {
          flag = ThreadPool.BindHandle((SafeHandle) handle);
        }
        finally
        {
          CodeAccessPermission.RevertAssert();
        }
        if (!flag)
          throw new IOException(SR.GetString("IO_IO_BindHandleFailed"));
      }
      this.m_handle = handle;
      this.m_isAsync = isAsync;
      this.m_isHandleExposed = isExposed;
      this.m_isFromExistingHandle = isExposed;
    }

    [SecurityCritical]
    public override int Read([In, Out] byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (buffer.Length - offset < count)
        throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
      if (!this.CanRead)
        __Error.ReadNotSupported();
      this.CheckReadOperations();
      return this.ReadCore(buffer, offset, count);
    }

    [SecurityCritical]
    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (buffer.Length - offset < count)
        throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
      if (!this.CanRead)
        __Error.ReadNotSupported();
      this.CheckReadOperations();
      if (this.m_isAsync)
        return (IAsyncResult) this.BeginReadCore(buffer, offset, count, callback, state);
      if (this.m_state != PipeState.Broken)
        return base.BeginRead(buffer, offset, count, callback, state);
      PipeStreamAsyncResult streamAsyncResult = new PipeStreamAsyncResult();
      streamAsyncResult._handle = this.m_handle;
      streamAsyncResult._userCallback = callback;
      streamAsyncResult._userStateObject = state;
      streamAsyncResult._isWrite = false;
      streamAsyncResult.CallUserCallback();
      return (IAsyncResult) streamAsyncResult;
    }

    [SecurityCritical]
    public override unsafe int EndRead(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      if (!this.m_isAsync)
        return base.EndRead(asyncResult);
      PipeStreamAsyncResult streamAsyncResult = asyncResult as PipeStreamAsyncResult;
      if (streamAsyncResult == null || streamAsyncResult._isWrite)
        __Error.WrongAsyncResult();
      if (1 == Interlocked.CompareExchange(ref streamAsyncResult._EndXxxCalled, 1, 0))
        __Error.EndReadCalledTwice();
      WaitHandle waitHandle = (WaitHandle) streamAsyncResult._waitHandle;
      if (waitHandle != null)
      {
        try
        {
          waitHandle.WaitOne();
        }
        finally
        {
          waitHandle.Close();
        }
      }
      NativeOverlapped* nativeOverlappedPtr = streamAsyncResult._overlapped;
      if ((IntPtr) nativeOverlappedPtr != IntPtr.Zero)
        Overlapped.Free(nativeOverlappedPtr);
      if (streamAsyncResult._errorCode != 0)
        this.WinIOError(streamAsyncResult._errorCode);
      this.m_isMessageComplete = this.m_state == PipeState.Broken || streamAsyncResult._isMessageComplete;
      return streamAsyncResult._numBytes;
    }

    [SecurityCritical]
    public override void Write(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (buffer.Length - offset < count)
        throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
      if (!this.CanWrite)
        __Error.WriteNotSupported();
      this.CheckWriteOperations();
      this.WriteCore(buffer, offset, count);
    }

    [SecurityCritical]
    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
      if (offset < 0)
        throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (count < 0)
        throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if (buffer.Length - offset < count)
        throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
      if (!this.CanWrite)
        __Error.WriteNotSupported();
      this.CheckWriteOperations();
      if (!this.m_isAsync)
        return base.BeginWrite(buffer, offset, count, callback, state);
      else
        return (IAsyncResult) this.BeginWriteCore(buffer, offset, count, callback, state);
    }

    [SecurityCritical]
    public override unsafe void EndWrite(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      if (!this.m_isAsync)
      {
        base.EndWrite(asyncResult);
      }
      else
      {
        PipeStreamAsyncResult streamAsyncResult = asyncResult as PipeStreamAsyncResult;
        if (streamAsyncResult == null || !streamAsyncResult._isWrite)
          __Error.WrongAsyncResult();
        if (1 == Interlocked.CompareExchange(ref streamAsyncResult._EndXxxCalled, 1, 0))
          __Error.EndWriteCalledTwice();
        WaitHandle waitHandle = (WaitHandle) streamAsyncResult._waitHandle;
        if (waitHandle != null)
        {
          try
          {
            waitHandle.WaitOne();
          }
          finally
          {
            waitHandle.Close();
          }
        }
        NativeOverlapped* nativeOverlappedPtr = streamAsyncResult._overlapped;
        if ((IntPtr) nativeOverlappedPtr != IntPtr.Zero)
          Overlapped.Free(nativeOverlappedPtr);
        if (streamAsyncResult._errorCode == 0)
          return;
        this.WinIOError(streamAsyncResult._errorCode);
      }
    }

    [SecurityCritical]
    public override int ReadByte()
    {
      this.CheckReadOperations();
      if (!this.CanRead)
        __Error.ReadNotSupported();
      byte[] buffer = new byte[1];
      if (this.ReadCore(buffer, 0, 1) == 0)
        return -1;
      else
        return (int) buffer[0];
    }

    [SecurityCritical]
    public override void WriteByte(byte value)
    {
      this.CheckWriteOperations();
      if (!this.CanWrite)
        __Error.WriteNotSupported();
      this.WriteCore(new byte[1]
      {
        value
      }, 0, 1);
    }

    [SecurityCritical]
    public override void Flush()
    {
      this.CheckWriteOperations();
      if (this.CanWrite)
        return;
      __Error.WriteNotSupported();
    }

    [SecurityCritical]
    public void WaitForPipeDrain()
    {
      this.CheckWriteOperations();
      if (!this.CanWrite)
        __Error.WriteNotSupported();
      if (Microsoft.Win32.UnsafeNativeMethods.FlushFileBuffers(this.m_handle))
        return;
      this.WinIOError(Marshal.GetLastWin32Error());
    }

    [SecurityCritical]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.m_handle != null)
        {
          if (!this.m_handle.IsClosed)
            this.m_handle.Dispose();
        }
      }
      finally
      {
        base.Dispose(disposing);
      }
      this.m_state = PipeState.Closed;
    }

    [SecurityCritical]
    public PipeSecurity GetAccessControl()
    {
      if (this.m_state == PipeState.Closed)
        __Error.PipeNotOpen();
      if (this.m_handle == null)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
      if (this.m_handle.IsClosed)
        __Error.PipeNotOpen();
      return new PipeSecurity(this.m_handle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
    }

    [SecurityCritical]
    public void SetAccessControl(PipeSecurity pipeSecurity)
    {
      if (pipeSecurity == null)
        throw new ArgumentNullException("pipeSecurity");
      this.CheckPipePropertyOperations();
      pipeSecurity.Persist((SafeHandle) this.m_handle);
    }

    public override void SetLength(long value)
    {
      __Error.SeekNotSupported();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      __Error.SeekNotSupported();
      return 0L;
    }

    [SecurityCritical]
    protected internal virtual void CheckPipePropertyOperations()
    {
      if (this.m_handle == null)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
      if (this.m_state == PipeState.Closed)
        __Error.PipeNotOpen();
      if (!this.m_handle.IsClosed)
        return;
      __Error.PipeNotOpen();
    }

    [SecurityCritical]
    protected internal void CheckReadOperations()
    {
      if (this.m_state == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotYetConnected"));
      if (this.m_state == PipeState.Disconnected)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeDisconnected"));
      if (this.m_handle == null)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
      if (this.m_state == PipeState.Closed)
        __Error.PipeNotOpen();
      if (!this.m_handle.IsClosed)
        return;
      __Error.PipeNotOpen();
    }

    [SecurityCritical]
    protected internal void CheckWriteOperations()
    {
      if (this.m_state == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotYetConnected"));
      if (this.m_state == PipeState.Disconnected)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeDisconnected"));
      if (this.m_handle == null)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
      if (this.m_state == PipeState.Broken)
        throw new IOException(SR.GetString("IO_IO_PipeBroken"));
      if (this.m_state == PipeState.Closed)
        __Error.PipeNotOpen();
      if (!this.m_handle.IsClosed)
        return;
      __Error.PipeNotOpen();
    }

    private void Init(PipeDirection direction, PipeTransmissionMode transmissionMode, int outBufferSize)
    {
      this.m_readMode = transmissionMode;
      this.m_transmissionMode = transmissionMode;
      this.m_pipeDirection = direction;
      if ((this.m_pipeDirection & PipeDirection.In) != (PipeDirection) 0)
        this.m_canRead = true;
      if ((this.m_pipeDirection & PipeDirection.Out) != (PipeDirection) 0)
        this.m_canWrite = true;
      this.m_outBufferSize = outBufferSize;
      this.m_isMessageComplete = true;
      this.m_state = PipeState.WaitingToConnect;
    }

    [SecurityCritical]
    private unsafe int ReadCore(byte[] buffer, int offset, int count)
    {
      if (this.m_isAsync)
        return this.EndRead((IAsyncResult) this.BeginReadCore(buffer, offset, count, (AsyncCallback) null, (object) null));
      int hr = 0;
      int num = this.ReadFileNative(this.m_handle, buffer, offset, count, (NativeOverlapped*) null, out hr);
      if (num == -1)
      {
        if (hr == 109 || hr == 233)
        {
          this.State = PipeState.Broken;
          num = 0;
        }
        else
          __Error.WinIOError(hr, string.Empty);
      }
      this.m_isMessageComplete = hr != 234;
      return num;
    }

    [SecurityCritical]
    private unsafe PipeStreamAsyncResult BeginReadCore(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
      PipeStreamAsyncResult streamAsyncResult = new PipeStreamAsyncResult();
      streamAsyncResult._handle = this.m_handle;
      streamAsyncResult._userCallback = callback;
      streamAsyncResult._userStateObject = state;
      streamAsyncResult._isWrite = false;
      if (buffer.Length == 0)
      {
        streamAsyncResult.CallUserCallback();
      }
      else
      {
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        streamAsyncResult._waitHandle = manualResetEvent;
        NativeOverlapped* overlapped = new Overlapped(0, 0, IntPtr.Zero, (IAsyncResult) streamAsyncResult).Pack(PipeStream.IOCallback, (object) buffer);
        streamAsyncResult._overlapped = overlapped;
        int hr = 0;
        if (this.ReadFileNative(this.m_handle, buffer, offset, count, overlapped, out hr) == -1)
        {
          if (hr == 109 || hr == 233)
          {
            this.State = PipeState.Broken;
            overlapped->InternalLow = IntPtr.Zero;
            streamAsyncResult.CallUserCallback();
          }
          else if (hr != 997)
            __Error.WinIOError(hr, string.Empty);
        }
      }
      return streamAsyncResult;
    }

    [SecurityCritical]
    private unsafe void WriteCore(byte[] buffer, int offset, int count)
    {
      if (this.m_isAsync)
      {
        this.EndWrite((IAsyncResult) this.BeginWriteCore(buffer, offset, count, (AsyncCallback) null, (object) null));
      }
      else
      {
        int hr = 0;
        if (this.WriteFileNative(this.m_handle, buffer, offset, count, (NativeOverlapped*) null, out hr) != -1)
          return;
        this.WinIOError(hr);
      }
    }

    [SecurityCritical]
    private unsafe PipeStreamAsyncResult BeginWriteCore(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
    {
      PipeStreamAsyncResult streamAsyncResult = new PipeStreamAsyncResult();
      streamAsyncResult._userCallback = callback;
      streamAsyncResult._userStateObject = state;
      streamAsyncResult._isWrite = true;
      streamAsyncResult._handle = this.m_handle;
      if (buffer.Length == 0)
      {
        streamAsyncResult.CallUserCallback();
      }
      else
      {
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        streamAsyncResult._waitHandle = manualResetEvent;
        NativeOverlapped* nativeOverlappedPtr = new Overlapped(0, 0, IntPtr.Zero, (IAsyncResult) streamAsyncResult).Pack(PipeStream.IOCallback, (object) buffer);
        streamAsyncResult._overlapped = nativeOverlappedPtr;
        int hr = 0;
        if (this.WriteFileNative(this.m_handle, buffer, offset, count, nativeOverlappedPtr, out hr) == -1 && hr != 997)
        {
          if ((IntPtr) nativeOverlappedPtr != IntPtr.Zero)
            Overlapped.Free(nativeOverlappedPtr);
          this.WinIOError(hr);
        }
      }
      return streamAsyncResult;
    }

    [SecurityCritical]
    private unsafe int ReadFileNative(SafePipeHandle handle, byte[] buffer, int offset, int count, NativeOverlapped* overlapped, out int hr)
    {
      if (buffer.Length == 0)
      {
        hr = 0;
        return 0;
      }
      else
      {
        int numBytesRead = 0;
        int num;
        fixed (byte* numPtr = buffer)
          num = !this.m_isAsync ? Microsoft.Win32.UnsafeNativeMethods.ReadFile(handle, numPtr + offset, count, out numBytesRead, IntPtr.Zero) : Microsoft.Win32.UnsafeNativeMethods.ReadFile(handle, numPtr + offset, count, IntPtr.Zero, overlapped);
        if (num == 0)
        {
          hr = Marshal.GetLastWin32Error();
          if (hr == 234)
            return numBytesRead;
          else
            return -1;
        }
        else
        {
          hr = 0;
          return numBytesRead;
        }
      }
    }

    [SecurityCritical]
    private unsafe int WriteFileNative(SafePipeHandle handle, byte[] buffer, int offset, int count, NativeOverlapped* overlapped, out int hr)
    {
      if (buffer.Length == 0)
      {
        hr = 0;
        return 0;
      }
      else
      {
        int numBytesWritten = 0;
        int num;
        fixed (byte* numPtr = buffer)
          num = !this.m_isAsync ? Microsoft.Win32.UnsafeNativeMethods.WriteFile(handle, numPtr + offset, count, out numBytesWritten, IntPtr.Zero) : Microsoft.Win32.UnsafeNativeMethods.WriteFile(handle, numPtr + offset, count, IntPtr.Zero, overlapped);
        if (num == 0)
        {
          hr = Marshal.GetLastWin32Error();
          return -1;
        }
        else
        {
          hr = 0;
          return numBytesWritten;
        }
      }
    }

    [SecurityCritical]
    private void UpdateReadMode()
    {
      int lpState;
      if (!Microsoft.Win32.UnsafeNativeMethods.GetNamedPipeHandleState(this.SafePipeHandle, out lpState, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, 0))
        this.WinIOError(Marshal.GetLastWin32Error());
      if ((lpState & 2) != 0)
        this.m_readMode = PipeTransmissionMode.Message;
      else
        this.m_readMode = PipeTransmissionMode.Byte;
    }

    [SecurityCritical]
    internal void WinIOError(int errorCode)
    {
      if (errorCode == 109 || errorCode == 233 || errorCode == 232)
      {
        this.m_state = PipeState.Broken;
        throw new IOException(SR.GetString("IO_IO_PipeBroken"), Microsoft.Win32.UnsafeNativeMethods.MakeHRFromErrorCode(errorCode));
      }
      else if (errorCode == 38)
      {
        __Error.EndOfFile();
      }
      else
      {
        if (errorCode == 6)
        {
          this.m_handle.SetHandleAsInvalid();
          this.m_state = PipeState.Broken;
        }
        __Error.WinIOError(errorCode, string.Empty);
      }
    }

    [SecurityCritical]
    internal static unsafe Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES GetSecAttrs(HandleInheritability inheritability, PipeSecurity pipeSecurity, out object pinningHandle)
    {
      pinningHandle = (object) null;
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES structure = (Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES) null;
      if ((inheritability & HandleInheritability.Inheritable) != HandleInheritability.None || pipeSecurity != null)
      {
        structure = new Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES();
        structure.nLength = Marshal.SizeOf<Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES>(structure);
        if ((inheritability & HandleInheritability.Inheritable) != HandleInheritability.None)
          structure.bInheritHandle = 1;
        if (pipeSecurity != null)
        {
          byte[] descriptorBinaryForm = pipeSecurity.GetSecurityDescriptorBinaryForm();
          pinningHandle = (object) GCHandle.Alloc((object) descriptorBinaryForm, GCHandleType.Pinned);
          fixed (byte* numPtr = descriptorBinaryForm)
            structure.pSecurityDescriptor = numPtr;
        }
      }
      return structure;
    }

    [SecurityCritical]
    internal static Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES GetSecAttrs(HandleInheritability inheritability)
    {
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES structure = (Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES) null;
      if ((inheritability & HandleInheritability.Inheritable) != HandleInheritability.None)
      {
        structure = new Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES();
        structure.nLength = Marshal.SizeOf<Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES>(structure);
        structure.bInheritHandle = 1;
      }
      return structure;
    }

    [SecurityCritical]
    private static unsafe void AsyncPSCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
    {
      PipeStreamAsyncResult streamAsyncResult = (PipeStreamAsyncResult) Overlapped.Unpack(pOverlapped).AsyncResult;
      streamAsyncResult._numBytes = (int) numBytes;
      if (!streamAsyncResult._isWrite && ((int) errorCode == 109 || (int) errorCode == 233 || (int) errorCode == 232))
      {
        errorCode = 0U;
        numBytes = 0U;
      }
      if ((int) errorCode == 234)
      {
        errorCode = 0U;
        streamAsyncResult._isMessageComplete = false;
      }
      else
        streamAsyncResult._isMessageComplete = true;
      streamAsyncResult._errorCode = (int) errorCode;
      streamAsyncResult._completedSynchronously = false;
      streamAsyncResult._isComplete = true;
      ManualResetEvent manualResetEvent = streamAsyncResult._waitHandle;
      if (manualResetEvent != null && !manualResetEvent.Set())
        __Error.WinIOError();
      AsyncCallback asyncCallback = streamAsyncResult._userCallback;
      if (asyncCallback == null)
        return;
      asyncCallback((IAsyncResult) streamAsyncResult);
    }
  }
}
