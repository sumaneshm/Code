// Type: Microsoft.Win32.SafeHandles.SafeNCryptHandle
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
  [SecurityCritical(SecurityCriticalScope.Everything)]
  [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
  public abstract class SafeNCryptHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    private SafeNCryptHandle.OwnershipState m_ownershipState;
    private SafeNCryptHandle m_holder;

    private SafeNCryptHandle Holder
    {
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_holder;
      }
      [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] set
      {
        this.m_holder = value;
        this.m_ownershipState = SafeNCryptHandle.OwnershipState.Duplicate;
      }
    }

    protected SafeNCryptHandle()
      : base(true)
    {
    }

    internal T Duplicate<T>() where T : SafeNCryptHandle, new()
    {
      if (this.m_ownershipState == SafeNCryptHandle.OwnershipState.Owner)
        return this.DuplicateOwnerHandle<T>();
      else
        return this.DuplicateDuplicatedHandle<T>();
    }

    private T DuplicateDuplicatedHandle<T>() where T : SafeNCryptHandle, new()
    {
      bool success = false;
      T instance = Activator.CreateInstance<T>();
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
      }
      finally
      {
        this.Holder.DangerousAddRef(ref success);
        instance.SetHandle(this.Holder.DangerousGetHandle());
        instance.Holder = this.Holder;
      }
      return instance;
    }

    private T DuplicateOwnerHandle<T>() where T : SafeNCryptHandle, new()
    {
      bool success = false;
      T instance1 = Activator.CreateInstance<T>();
      T instance2 = Activator.CreateInstance<T>();
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
      }
      finally
      {
        instance1.m_ownershipState = SafeNCryptHandle.OwnershipState.Holder;
        instance1.SetHandle(this.DangerousGetHandle());
        GC.SuppressFinalize((object) instance1);
        this.Holder = (SafeNCryptHandle) instance1;
        instance1.DangerousAddRef(ref success);
        instance2.SetHandle(instance1.DangerousGetHandle());
        instance2.Holder = (SafeNCryptHandle) instance1;
      }
      return instance2;
    }

    protected override bool ReleaseHandle()
    {
      if (this.m_ownershipState != SafeNCryptHandle.OwnershipState.Duplicate)
        return this.ReleaseNativeHandle();
      this.Holder.DangerousRelease();
      return true;
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected abstract bool ReleaseNativeHandle();

    private enum OwnershipState
    {
      Owner,
      Duplicate,
      Holder,
    }
  }
}
