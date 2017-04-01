// Type: System.IO.Pipes.NamedPipeClientStream
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
using System.Security.Principal;

namespace System.IO.Pipes
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class NamedPipeClientStream : PipeStream
  {
    private string m_normalizedPipePath;
    private TokenImpersonationLevel m_impersonationLevel;
    private PipeOptions m_pipeOptions;
    private HandleInheritability m_inheritability;
    private int m_access;

    public int NumberOfServerInstances
    {
      [SecurityCritical] get
      {
        this.CheckPipePropertyOperations();
        int lpCurInstances;
        if (!Microsoft.Win32.UnsafeNativeMethods.GetNamedPipeHandleState(this.InternalHandle, Microsoft.Win32.UnsafeNativeMethods.NULL, out lpCurInstances, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, Microsoft.Win32.UnsafeNativeMethods.NULL, 0))
          this.WinIOError(Marshal.GetLastWin32Error());
        return lpCurInstances;
      }
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string pipeName)
      : this(".", pipeName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string serverName, string pipeName)
      : this(serverName, pipeName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction)
      : this(serverName, pipeName, direction, PipeOptions.None, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction, PipeOptions options)
      : this(serverName, pipeName, direction, options, TokenImpersonationLevel.None, HandleInheritability.None)
    {
    }

    [SecuritySafeCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction, PipeOptions options, TokenImpersonationLevel impersonationLevel)
      : this(serverName, pipeName, direction, options, impersonationLevel, HandleInheritability.None)
    {
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string serverName, string pipeName, PipeDirection direction, PipeOptions options, TokenImpersonationLevel impersonationLevel, HandleInheritability inheritability)
      : base(direction, 0)
    {
      if (pipeName == null)
        throw new ArgumentNullException("pipeName");
      if (serverName == null)
        throw new ArgumentNullException("serverName", SR.GetString("ArgumentNull_ServerName"));
      if (pipeName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_NeedNonemptyPipeName"));
      if (serverName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_EmptyServerName"));
      if ((options & ~(PipeOptions.WriteThrough | PipeOptions.Asynchronous)) != PipeOptions.None)
        throw new ArgumentOutOfRangeException("options", SR.GetString("ArgumentOutOfRange_OptionsInvalid"));
      if (impersonationLevel < TokenImpersonationLevel.None || impersonationLevel > TokenImpersonationLevel.Delegation)
        throw new ArgumentOutOfRangeException("impersonationLevel", SR.GetString("ArgumentOutOfRange_ImpersonationInvalid"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability", SR.GetString("ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable"));
      this.m_normalizedPipePath = Path.GetFullPath("\\\\" + serverName + "\\pipe\\" + pipeName);
      if (string.Compare(this.m_normalizedPipePath, "\\\\.\\pipe\\anonymous", StringComparison.OrdinalIgnoreCase) == 0)
        throw new ArgumentOutOfRangeException("pipeName", SR.GetString("ArgumentOutOfRange_AnonymousReserved"));
      this.m_inheritability = inheritability;
      this.m_impersonationLevel = impersonationLevel;
      this.m_pipeOptions = options;
      if ((PipeDirection.In & direction) != (PipeDirection) 0)
        this.m_access |= int.MinValue;
      if ((PipeDirection.Out & direction) == (PipeDirection) 0)
        return;
      this.m_access |= 1073741824;
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(string serverName, string pipeName, PipeAccessRights desiredAccessRights, PipeOptions options, TokenImpersonationLevel impersonationLevel, HandleInheritability inheritability)
      : base(NamedPipeClientStream.DirectionFromRights(desiredAccessRights), 0)
    {
      if (pipeName == null)
        throw new ArgumentNullException("pipeName");
      if (serverName == null)
        throw new ArgumentNullException("serverName", SR.GetString("ArgumentNull_ServerName"));
      if (pipeName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_NeedNonemptyPipeName"));
      if (serverName.Length == 0)
        throw new ArgumentException(SR.GetString("Argument_EmptyServerName"));
      if ((options & ~(PipeOptions.WriteThrough | PipeOptions.Asynchronous)) != PipeOptions.None)
        throw new ArgumentOutOfRangeException("options", SR.GetString("ArgumentOutOfRange_OptionsInvalid"));
      if (impersonationLevel < TokenImpersonationLevel.None || impersonationLevel > TokenImpersonationLevel.Delegation)
        throw new ArgumentOutOfRangeException("impersonationLevel", SR.GetString("ArgumentOutOfRange_ImpersonationInvalid"));
      if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
        throw new ArgumentOutOfRangeException("inheritability", SR.GetString("ArgumentOutOfRange_HandleInheritabilityNoneOrInheritable"));
      if ((desiredAccessRights & ~(PipeAccessRights.FullControl | PipeAccessRights.AccessSystemSecurity)) != (PipeAccessRights) 0)
        throw new ArgumentOutOfRangeException("desiredAccessRights", SR.GetString("ArgumentOutOfRange_InvalidPipeAccessRights"));
      this.m_normalizedPipePath = Path.GetFullPath("\\\\" + serverName + "\\pipe\\" + pipeName);
      if (string.Compare(this.m_normalizedPipePath, "\\\\.\\pipe\\anonymous", StringComparison.OrdinalIgnoreCase) == 0)
        throw new ArgumentOutOfRangeException("pipeName", SR.GetString("ArgumentOutOfRange_AnonymousReserved"));
      this.m_inheritability = inheritability;
      this.m_impersonationLevel = impersonationLevel;
      this.m_pipeOptions = options;
      this.m_access = (int) desiredAccessRights;
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public NamedPipeClientStream(PipeDirection direction, bool isAsync, bool isConnected, SafePipeHandle safePipeHandle)
      : base(direction, 0)
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

    ~NamedPipeClientStream()
    {
      this.Dispose(false);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Connect()
    {
      this.Connect(-1);
    }

    [SecurityCritical]
    public void Connect(int timeout)
    {
      this.CheckConnectOperationsClient();
      if (timeout < 0 && timeout != -1)
        throw new ArgumentOutOfRangeException("timeout", SR.GetString("ArgumentOutOfRange_InvalidTimeout"));
      Microsoft.Win32.UnsafeNativeMethods.SECURITY_ATTRIBUTES secAttrs = PipeStream.GetSecAttrs(this.m_inheritability);
      int dwFlagsAndAttributes = (int) this.m_pipeOptions;
      if (this.m_impersonationLevel != TokenImpersonationLevel.None)
        dwFlagsAndAttributes = dwFlagsAndAttributes | 1048576 | (int) (this.m_impersonationLevel - 1) << 16;
      int tickCount = Environment.TickCount;
      int num = 0;
      do
      {
        if (!Microsoft.Win32.UnsafeNativeMethods.WaitNamedPipe(this.m_normalizedPipePath, timeout - num))
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          switch (lastWin32Error)
          {
            case 2:
              goto label_12;
            case 0:
              goto label_13;
            default:
              __Error.WinIOError(lastWin32Error, string.Empty);
              break;
          }
        }
        SafePipeHandle namedPipeClient = Microsoft.Win32.UnsafeNativeMethods.CreateNamedPipeClient(this.m_normalizedPipePath, this.m_access, FileShare.None, secAttrs, FileMode.Open, dwFlagsAndAttributes, Microsoft.Win32.UnsafeNativeMethods.NULL);
        if (namedPipeClient.IsInvalid)
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          if (lastWin32Error != 231)
            __Error.WinIOError(lastWin32Error, string.Empty);
          else
            goto label_12;
        }
        this.InitializeHandle(namedPipeClient, false, (this.m_pipeOptions & PipeOptions.Asynchronous) != PipeOptions.None);
        this.State = PipeState.Connected;
        return;
label_12:;
      }
      while (timeout == -1 || (num = Environment.TickCount - tickCount) < timeout);
label_13:
      throw new TimeoutException();
    }

    [SecurityCritical]
    protected internal override void CheckPipePropertyOperations()
    {
      base.CheckPipePropertyOperations();
      if (this.State == PipeState.WaitingToConnect)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeNotYetConnected"));
      if (this.State == PipeState.Broken)
        throw new IOException(SR.GetString("IO_IO_PipeBroken"));
    }

    private static PipeDirection DirectionFromRights(PipeAccessRights rights)
    {
      PipeDirection pipeDirection = (PipeDirection) 0;
      if ((rights & PipeAccessRights.ReadData) != (PipeAccessRights) 0)
        pipeDirection |= PipeDirection.In;
      if ((rights & PipeAccessRights.WriteData) != (PipeAccessRights) 0)
        pipeDirection |= PipeDirection.Out;
      return pipeDirection;
    }

    private void CheckConnectOperationsClient()
    {
      if (this.State == PipeState.Connected)
        throw new InvalidOperationException(SR.GetString("InvalidOperation_PipeAlreadyConnected"));
      if (this.State != PipeState.Closed)
        return;
      __Error.PipeNotOpen();
    }
  }
}
