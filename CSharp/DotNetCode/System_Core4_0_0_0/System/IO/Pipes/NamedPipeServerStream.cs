// Type: System.IO.Pipes.NamedPipeServerStream
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.IO.Pipes
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class NamedPipeServerStream : PipeStream
  {
    [SecurityCritical]
    private static readonly IOCompletionCallback WaitForConnectionCallback = new IOCompletionCallback(NamedPipeServerStream.AsyncWaitForConnectionCallback);
    private static RuntimeHelpers.TryCode tryCode = new RuntimeHelpers.TryCode(NamedPipeServerStream.ImpersonateAndTryCode);
    private static RuntimeHelpers.CleanupCode cleanupCode = new RuntimeHelpers.CleanupCode(NamedPipeServerStream.RevertImpersonationOnBackout);
    public const int MaxAllowedServerInstances = -1;

    [SecurityCritical]
    static NamedPipeServerStream()
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName)
      : this(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, (PipeSecurity) null, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction)
      : this(pipeName, direction, 1, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, (PipeSecurity) null, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances)
      : this(pipeName, direction, maxNumberOfServerInstances, PipeTransmissionMode.Byte, PipeOptions.None, 0, 0, (PipeSecurity) null, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, PipeOptions.None, 0, 0, (PipeSecurity) null, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, 0, 0, (PipeSecurity) null, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, (PipeSecurity) null, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeSecurity pipeSecurity)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, pipeSecurity, HandleInheritability.None, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeSecurity pipeSecurity, HandleInheritability inheritability)
      : this(pipeName, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, pipeSecurity, inheritability, (PipeAccessRights) 0)
    {
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(string pipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeSecurity pipeSecurity, HandleInheritability inheritability, PipeAccessRights additionalAccessRights)
      : base(direction, transmissionMode, outBufferSize)
    {
      if (pipeName == null)
        throw new ArgumentNullException("pipeName");
      if (pipeName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_NeedNonemptyPipeName"));
      if ((options & ~(PipeOptions.WriteThrough | PipeOptions.Asynchronous)) != PipeOptions.None)
        throw new ArgumentOutOfRangeException("options", SR.GetString("ArgumentOutOfRange_OptionsInvalid"));
      if (inBufferSize < 0)
        throw new ArgumentOutOfRangeException("inBufferSize", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
      if ((maxNumberOfServerInstances < 1 || maxNumberOfServerInstances > 254) && maxNumberOfServerInstances != -1)
        throw new ArgumentOutOfRangeException("maxNumberOfServerInstances", SR.GetString("ArgumentOutOfRange_MaxNumServerInstances"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability", SR.GetString("ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable"));
      if ((additionalAccessRights & ~(PipeAccessRights.ChangePermissions | PipeAccessRights.TakeOwnership | PipeAccessRights.AccessSystemSecurity)) != (PipeAccessRights) 0)
        throw new ArgumentOutOfRangeException("additionalAccessRights", SR.GetString("ArgumentOutOfRange_AdditionalAccessLimited"));
      if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
        throw new PlatformNotSupportedException(SR.GetString("PlatformNotSupported_NamedPipeServers"));
      string fullPath = Path.GetFullPath("\\\\.\\pipe\\" + pipeName);
      if (string.Compare(fullPath, "\\\\.\\pipe\\anonymous", StringComparison.OrdinalIgnoreCase) == 0)
        throw new ArgumentOutOfRangeException("pipeName", SR.GetString("ArgumentOutOfRange_AnonymousReserved"));
      object pinningHandle = (object) null;
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(inheritability, pipeSecurity, out pinningHandle);
      try
      {
        this.Create(fullPath, direction, maxNumberOfServerInstances, transmissionMode, options, inBufferSize, outBufferSize, additionalAccessRights, secAttrs);
      }
      finally
      {
        if (pinningHandle != null)
          ((GCHandle) pinningHandle).Free();
      }
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeServerStream(PipeDirection direction, bool isAsync, bool isConnected, SafePipeHandle safePipeHandle)
      : base(direction, PipeTransmissionMode.Byte, 0)
    {
      if (safePipeHandle == null)
        throw new ArgumentNullException("safePipeHandle");
      if (safePipeHandle.IsInvalid)
        throw new ArgumentException(SR.GetString("Argument_InvalidHandle"), "safePipeHandle");
      if (Microsoft.Win32.UnsafeNativeMethods.GetFileType(safePipeHandle) != 3)
        throw new IOException(SR.GetString("IO_IO_InvalidPipeHandle"));
      this.InitializeHandle(safePipeHandle, true, isAsync);
      if (!isConnected)
        return;
      this.State = PipeState.Connected;
    }

    ~NamedPipeServerStream()
    {
      this.Dispose(false);
    }

    [SecurityCritical]
    public void WaitForConnection()
    {
      this.CheckConnectOperationsServer();
      if (this.IsAsync)
      {
        this.EndWaitForConnection(this.BeginWaitForConnection((AsyncCallback) null, (object) null));
      }
      else
      {
        if (!Microsoft.Win32.UnsafeNativeMethods.ConnectNamedPipe(this.InternalHandle, Microsoft.Win32.UnsafeNativeMethods.NULL))
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          if (lastWin32Error != 535)
            __Error.WinIOError(lastWin32Error, string.Empty);
          if (lastWin32Error == 535 && this.State == PipeState.Connected)
            throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeAlreadyConnected"));
        }
        this.State = PipeState.Connected;
      }
    }

    [SecurityCritical]
    [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
    public unsafe IAsyncResult BeginWaitForConnection(AsyncCallback callback, object state)
    {
      this.CheckConnectOperationsServer();
      if (!this.IsAsync)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotAsync"));
      PipeAsyncResult pipeAsyncResult = new PipeAsyncResult();
      pipeAsyncResult._handle = this.InternalHandle;
      pipeAsyncResult._userCallback = callback;
      pipeAsyncResult._userStateObject = state;
      ManualResetEvent manualResetEvent = new ManualResetEvent(false);
      pipeAsyncResult._waitHandle = manualResetEvent;
      NativeOverlapped* nativeOverlappedPtr = new Overlapped(0, 0, IntPtr.Zero, (IAsyncResult) pipeAsyncResult).Pack(NamedPipeServerStream.WaitForConnectionCallback, (object) null);
      pipeAsyncResult._overlapped = nativeOverlappedPtr;
      if (!Microsoft.Win32.UnsafeNativeMethods.ConnectNamedPipe(this.InternalHandle, nativeOverlappedPtr))
      {
        int lastWin32Error = Marshal.GetLastWin32Error();
        if (lastWin32Error == 997)
          return (IAsyncResult) pipeAsyncResult;
        Overlapped.Free(nativeOverlappedPtr);
        pipeAsyncResult._overlapped = (NativeOverlapped*) null;
        if (lastWin32Error == 535)
        {
          if (this.State == PipeState.Connected)
            throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeAlreadyConnected"));
          pipeAsyncResult.CallUserCallback();
          return (IAsyncResult) pipeAsyncResult;
        }
        else
          __Error.WinIOError(lastWin32Error, string.Empty);
      }
      return (IAsyncResult) pipeAsyncResult;
    }

    [SecurityCritical]
    public void EndWaitForConnection(IAsyncResult asyncResult)
    {
      this.CheckConnectOperationsServer();
      if (asyncResult == null)
        throw new ArgumentNullException("asyncResult");
      if (!this.IsAsync)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotAsync"));
      PipeAsyncResult pipeAsyncResult = asyncResult as PipeAsyncResult;
      if (pipeAsyncResult == null)
        __Error.WrongAsyncResult();
      if (1 == Interlocked.CompareExchange(ref pipeAsyncResult._EndXxxCalled, 1, 0))
        __Error.EndWaitForConnectionCalledTwice();
      WaitHandle waitHandle = (WaitHandle) pipeAsyncResult._waitHandle;
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
      if (pipeAsyncResult._errorCode != 0)
        __Error.WinIOError(pipeAsyncResult._errorCode, string.Empty);
      this.State = PipeState.Connected;
    }

    [SecurityCritical]
    public void Disconnect()
    {
      this.CheckDisconnectOperations();
      if (!Microsoft.Win32.UnsafeNativeMethods.DisconnectNamedPipe(this.InternalHandle))
        __Error.WinIOError(Marshal.GetLastWin32Error(), string.Empty);
      this.State = PipeState.Disconnected;
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
    public void RunAsClient(PipeStreamImpersonationWorker impersonationWorker)
    {
      this.CheckWriteOperations();
      NamedPipeServerStream.ExecuteHelper executeHelper = new NamedPipeServerStream.ExecuteHelper(impersonationWorker, this.InternalHandle);
      RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(NamedPipeServerStream.tryCode, NamedPipeServerStream.cleanupCode, (object) executeHelper);
      if (executeHelper.m_impersonateErrorCode != 0)
      {
        this.WinIOError(executeHelper.m_impersonateErrorCode);
      }
      else
      {
        if (executeHelper.m_revertImpersonateErrorCode == 0)
          return;
        this.WinIOError(executeHelper.m_revertImpersonateErrorCode);
      }
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
    public string GetImpersonationUserName()
    {
      this.CheckWriteOperations();
      StringBuilder lpUserName = new StringBuilder(514);
      if (!Microsoft.Win32.UnsafeNativeMethods.GetNamedPipeHandleState(this.InternalHandle, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, lpUserName, lpUserName.Capacity))
        this.WinIOError(Marshal.GetLastWin32Error());
      return ((object) lpUserName).ToString();
    }

    [SecurityCritical]
    private void Create(string fullPipeName, PipeDirection direction, int maxNumberOfServerInstances, PipeTransmissionMode transmissionMode, PipeOptions options, int inBufferSize, int outBufferSize, PipeAccessRights rights, Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs)
    {
      int openMode = (int) ((PipeOptions) (direction | (maxNumberOfServerInstances == 1 ? (PipeDirection) 524288 : (PipeDirection) 0)) | options | (PipeOptions) rights);
      int pipeMode = (int) transmissionMode << 2 | (int) transmissionMode << 1;
      if (maxNumberOfServerInstances == -1)
        maxNumberOfServerInstances = (int) byte.MaxValue;
      SafePipeHandle namedPipe = Microsoft.Win32.UnsafeNativeMethods.CreateNamedPipe(fullPipeName, openMode, pipeMode, maxNumberOfServerInstances, outBufferSize, inBufferSize, 0, secAttrs);
      if (namedPipe.IsInvalid)
        __Error.WinIOError(Marshal.GetLastWin32Error(), string.Empty);
      this.InitializeHandle(namedPipe, false, (options & PipeOptions.Asynchronous) != PipeOptions.None);
    }

    [SecurityCritical]
    private static void ImpersonateAndTryCode(object helper)
    {
      NamedPipeServerStream.ExecuteHelper executeHelper = (NamedPipeServerStream.ExecuteHelper) helper;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
      }
      finally
      {
        if (Microsoft.Win32.UnsafeNativeMethods.ImpersonateNamedPipeClient(executeHelper.m_handle))
          executeHelper.m_mustRevert = true;
        else
          executeHelper.m_impersonateErrorCode = Marshal.GetLastWin32Error();
      }
      if (!executeHelper.m_mustRevert)
        return;
      executeHelper.m_userCode();
    }

    [PrePrepareMethod]
    [SecurityCritical]
    private static void RevertImpersonationOnBackout(object helper, bool exceptionThrown)
    {
      NamedPipeServerStream.ExecuteHelper executeHelper = (NamedPipeServerStream.ExecuteHelper) helper;
      if (!executeHelper.m_mustRevert || Microsoft.Win32.UnsafeNativeMethods.RevertToSelf())
        return;
      executeHelper.m_revertImpersonateErrorCode = Marshal.GetLastWin32Error();
    }

    [SecurityCritical]
    private static unsafe void AsyncWaitForConnectionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
    {
      PipeAsyncResult pipeAsyncResult = (PipeAsyncResult) Overlapped.Unpack(pOverlapped).AsyncResult;
      Overlapped.Free(pOverlapped);
      pipeAsyncResult._overlapped = (NativeOverlapped*) null;
      if ((int) errorCode == 535)
        errorCode = 0U;
      pipeAsyncResult._errorCode = (int) errorCode;
      pipeAsyncResult._completedSynchronously = false;
      pipeAsyncResult._isComplete = true;
      ManualResetEvent manualResetEvent = pipeAsyncResult._waitHandle;
      if (manualResetEvent != null && !manualResetEvent.Set())
        __Error.WinIOError();
      AsyncCallback asyncCallback = pipeAsyncResult._userCallback;
      if (asyncCallback == null)
        return;
      asyncCallback((IAsyncResult) pipeAsyncResult);
    }

    [SecurityCritical]
    private void CheckConnectOperationsServer()
    {
      if (this.InternalHandle == null)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
      if (this.State == PipeState.Closed)
        __Error.PipeNotOpen();
      if (this.InternalHandle.IsClosed)
        __Error.PipeNotOpen();
      if (this.State == PipeState.Broken)
        throw new IOException(SR.GetString("IO_IO_PipeBroken"));
    }

    [SecurityCritical]
    private void CheckDisconnectOperations()
    {
      if (this.State == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotYetConnected"));
      if (this.State == PipeState.Disconnected)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeAlreadyDisconnected"));
      if (this.InternalHandle == null)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeHandleNotSet"));
      if (this.State == PipeState.Closed)
        __Error.PipeNotOpen();
      if (!this.InternalHandle.IsClosed)
        return;
      __Error.PipeNotOpen();
    }

    internal class ExecuteHelper
    {
      internal PipeStreamImpersonationWorker m_userCode;
      internal SafePipeHandle m_handle;
      internal bool m_mustRevert;
      internal int m_impersonateErrorCode;
      internal int m_revertImpersonateErrorCode;

      [SecurityCritical]
      internal ExecuteHelper(PipeStreamImpersonationWorker userCode, SafePipeHandle handle)
      {
        this.m_userCode = userCode;
        this.m_handle = handle;
      }
    }
  }
}
