// Type: System.Security.Cryptography.CngUIPolicy
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CngUIPolicy
  {
    private string m_creationTitle;
    private string m_description;
    private string m_friendlyName;
    private CngUIProtectionLevels m_protectionLevel;
    private string m_useContext;

    public string CreationTitle
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_creationTitle;
      }
    }

    public string Description
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_description;
      }
    }

    public string FriendlyName
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_friendlyName;
      }
    }

    public CngUIProtectionLevels ProtectionLevel
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_protectionLevel;
      }
    }

    public string UseContext
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_useContext;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public CngUIPolicy(CngUIProtectionLevels protectionLevel)
      : this(protectionLevel, (string) null)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName)
      : this(protectionLevel, friendlyName, (string) null)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description)
      : this(protectionLevel, friendlyName, description, (string) null)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description, string useContext)
      : this(protectionLevel, friendlyName, description, useContext, (string) null)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public CngUIPolicy(CngUIProtectionLevels protectionLevel, string friendlyName, string description, string useContext, string creationTitle)
    {
      this.m_creationTitle = creationTitle;
      this.m_description = description;
      this.m_friendlyName = friendlyName;
      this.m_protectionLevel = protectionLevel;
      this.m_useContext = useContext;
    }
  }
}
