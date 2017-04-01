// Type: System.IO.Pipes.AnonymousPipeServerStream
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.IO.Pipes
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class AnonymousPipeServerStream : PipeStream
  {
    private SafePipeHandle m_clientHandle;
    private bool m_clientHandleExposed;

    public SafePipeHandle ClientSafePipeHandle
    {
      [SecurityCritical] get
      {
        this.m_clientHandleExposed = true;
        return this.m_clientHandle;
      }
    }

    public override PipeTransmissionMode TransmissionMode
    {
      [SecurityCritical] get
      {
        return PipeTransmissionMode.Byte;
      }
    }

    public override PipeTransmissionMode ReadMode
    {
      [SecurityCritical] set
      {
        this.CheckPipePropertyOperations();
        if (value < PipeTransmissionMode.Byte || value > PipeTransmissionMode.Message)
          throw new ArgumentOutOfRangeException("value", SR.GetString("ArgumentOutOfRange_TransmissionModeByteOrMsg"));
        if (value == PipeTransmissionMode.Message)
          throw new NotSupportedException(SR.GetString("NotSupported_AnonymousPipeMessagesNotSupported"));
      }
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeServerStream()
      : this(PipeDirection.Out, HandleInheritability.None, 0, (PipeSecurity) null)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeServerStream(PipeDirection direction)
      : this(direction, HandleInheritability.None, 0)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability)
      : this(direction, inheritability, 0)
    {
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability, int bufferSize)
      : base(direction, bufferSize)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.GetString("NotSupported_AnonymousPipeUnidirectional"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability", SR.GetString("ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable"));
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(inheritability);
      this.Create(direction, secAttrs, bufferSize);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeServerStream(PipeDirection direction, HandleInheritability inheritability, int bufferSize, PipeSecurity pipeSecurity)
      : base(direction, bufferSize)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.GetString("NotSupported_AnonymousPipeUnidirectional"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability", SR.GetString("ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable"));
      object pinningHandle;
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(inheritability, pipeSecurity, out pinningHandle);
      try
      {
        this.Create(direction, secAttrs, bufferSize);
      }
      finally
      {
        if (pinningHandle != null)
          ((GCHandle) pinningHandle).Free();
      }
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeServerStream(PipeDirection direction, SafePipeHandle serverSafePipeHandle, SafePipeHandle clientSafePipeHandle)
      : base(direction, 0)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.GetString("NotSupported_AnonymousPipeUnidirectional"));
      if (serverSafePipeHandle == null)
        throw new ArgumentNullException("serverSafePipeHandle");
      if (clientSafePipeHandle == null)
        throw new ArgumentNullException("clientSafePipeHandle");
      if (serverSafePipeHandle.IsInvalid)
        throw new ArgumentException(SR.GetString("Argument_InvalidHandle"), "serverSafePipeHandle");
      if (clientSafePipeHandle.IsInvalid)
        throw new ArgumentException(SR.GetString("Argument_InvalidHandle"), "clientSafePipeHandle");
      if (Microsoft.Win32.UnsafeNativeMethods.GetFileType(serverSafePipeHandle) != 3)
        throw new IOException(SR.GetString("IO_IO_InvalidPipeHandle"));
      if (Microsoft.Win32.UnsafeNativeMethods.GetFileType(clientSafePipeHandle) != 3)
        throw new IOException(SR.GetString("IO_IO_InvalidPipeHandle"));
      this.InitializeHandle(serverSafePipeHandle, true, false);
      this.m_clientHandle = clientSafePipeHandle;
      this.m_clientHandleExposed = true;
      this.State = PipeState.Connected;
    }

    ~AnonymousPipeServerStream()
    {
      this.Dispose(false);
    }

    [SecurityCritical]
    public string GetClientHandleAsString()
    {
      this.m_clientHandleExposed = true;
      return this.m_clientHandle.DangerousGetHandle().ToString();
    }

    [SecurityCritical]
    public void DisposeLocalCopyOfClientHandle()
    {
      if (this.m_clientHandle == null || this.m_clientHandle.IsClosed)
        return;
      this.m_clientHandle.Dispose();
    }

    [SecurityCritical]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.m_clientHandleExposed || this.m_clientHandle == null || this.m_clientHandle.IsClosed)
          return;
        this.m_clientHandle.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    [SecurityCritical]
    private void Create(PipeDirection direction, Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs, int bufferSize)
    {
      SafePipeHandle hSourceHandle;
      if (!(direction != PipeDirection.In ? Microsoft.Win32.UnsafeNativeMethods.CreatePipe(out this.m_clientHandle, out hSourceHandle, secAttrs, bufferSize) : Microsoft.Win32.UnsafeNativeMethods.CreatePipe(out hSourceHandle, out this.m_clientHandle, secAttrs, bufferSize)))
        __Error.WinIOError(Marshal.GetLastWin32Error(), string.Empty);
      SafePipeHandle lpTargetHandle;
      if (!Microsoft.Win32.UnsafeNativeMethods.DuplicateHandle(Microsoft.Win32.UnsafeNativeMethods.GetCurrentProcess(), hSourceHandle, Microsoft.Win32.UnsafeNativeMethods.GetCurrentProcess(), out lpTargetHandle, 0U, false, 2U))
        __Error.WinIOError(Marshal.GetLastWin32Error(), string.Empty);
      hSourceHandle.Dispose();
      this.InitializeHandle(lpTargetHandle, false, false);
      this.State = PipeState.Connected;
    }
  }
}
