// Type: System.Security.Cryptography.CngKeyCreationParameters
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CngKeyCreationParameters
  {
    private CngPropertyCollection m_parameters = new CngPropertyCollection();
    private CngProvider m_provider = CngProvider.MicrosoftSoftwareKeyStorageProvider;
    private CngExportPolicies? m_exportPolicy;
    private CngKeyCreationOptions m_keyCreationOptions;
    private CngKeyUsages? m_keyUsage;
    private IntPtr m_parentWindowHandle;
    private CngUIPolicy m_uiPolicy;

    public CngExportPolicies? ExportPolicy
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_exportPolicy;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_exportPolicy = value;
      }
    }

    public CngKeyCreationOptions KeyCreationOptions
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_keyCreationOptions;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_keyCreationOptions = value;
      }
    }

    public CngKeyUsages? KeyUsage
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_keyUsage;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.m_keyUsage = value;
      }
    }

    public IntPtr ParentWindowHandle
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_parentWindowHandle;
      }
      [SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)] set
      {
        this.m_parentWindowHandle = value;
      }
    }

    public CngPropertyCollection Parameters
    {
      [SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)] get
      {
        return this.m_parameters;
      }
    }

    internal CngPropertyCollection ParametersNoDemand
    {
      get
      {
        return this.m_parameters;
      }
    }

    public CngProvider Provider
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_provider;
      }
      set
      {
        if (value == (CngProvider) null)
          throw new ArgumentNullException("value");
        this.m_provider = value;
      }
    }

    public CngUIPolicy UIPolicy
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_uiPolicy;
      }
      [SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries"), HostProtection(SecurityAction.LinkDemand, UI = true), UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.SafeSubWindows)] set
      {
        this.m_uiPolicy = value;
      }
    }
  }
}
