// Type: System.IO.Pipes.AnonymousPipeClientStream
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.IO.Pipes
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class AnonymousPipeClientStream : PipeStream
  {
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
    public AnonymousPipeClientStream(string pipeHandleAsString)
      : this(PipeDirection.In, pipeHandleAsString)
    {
    }

    [SecurityCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeClientStream(PipeDirection direction, string pipeHandleAsString)
      : base(direction, 0)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.GetString("NotSupported_AnonymousPipeUnidirectional"));
      if (pipeHandleAsString == null)
        throw new ArgumentNullException("pipeHandleAsString");
      long result = 0L;
      if (!long.TryParse(pipeHandleAsString, out result))
        throw new ArgumentException(SR.GetString("Argument_InvalidHandle"), "pipeHandleAsString");
      SafePipeHandle safePipeHandle = new SafePipeHandle((IntPtr) result, true);
      if (safePipeHandle.IsInvalid)
        throw new ArgumentException(SR.GetString("Argument_InvalidHandle"), "pipeHandleAsString");
      this.Init(direction, safePipeHandle);
    }

    [SecurityCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public AnonymousPipeClientStream(PipeDirection direction, SafePipeHandle safePipeHandle)
      : base(direction, 0)
    {
      if (direction == PipeDirection.InOut)
        throw new NotSupportedException(SR.GetString("NotSupported_AnonymousPipeUnidirectional"));
      if (safePipeHandle == null)
        throw new ArgumentNullException("safePipeHandle");
      if (safePipeHandle.IsInvalid)
        throw new ArgumentException(SR.GetString("Argument_InvalidHandle"), "safePipeHandle");
      this.Init(direction, safePipeHandle);
    }

    ~AnonymousPipeClientStream()
    {
      this.Dispose(false);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private void Init(PipeDirection direction, SafePipeHandle safePipeHandle)
    {
      if (Microsoft.Win32.UnsafeNativeMethods.GetFileType(safePipeHandle) != 3)
        throw new IOException(SR.GetString("IO_IO_InvalidPipeHandle"));
      this.InitializeHandle(safePipeHandle, true, false);
      this.State = PipeState.Connected;
    }
  }
}
