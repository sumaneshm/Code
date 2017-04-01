// Type: System.Management.Instrumentation.WmiConfigurationAttribute
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Management.Instrumentation
{
  [AttributeUsage(AttributeTargets.Assembly)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class WmiConfigurationAttribute : Attribute
  {
    private bool _IdentifyLevel = true;
    private string _Scope;
    private string _SecurityRestriction;
    private string _NamespaceSecurity;
    private ManagementHostingModel _HostingModel;
    private string _HostingGroup;

    public string SecurityRestriction
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._SecurityRestriction;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._SecurityRestriction = value;
      }
    }

    public string NamespaceSecurity
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._NamespaceSecurity;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._NamespaceSecurity = value;
      }
    }

    public bool IdentifyLevel
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._IdentifyLevel;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._IdentifyLevel = value;
      }
    }

    public ManagementHostingModel HostingModel
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._HostingModel;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._HostingModel = value;
      }
    }

    public string HostingGroup
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._HostingGroup;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._HostingGroup = value;
      }
    }

    public string Scope
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._Scope;
      }
    }

    public WmiConfigurationAttribute(string scope)
    {
      string str1 = scope;
      if (str1 != null)
        str1 = str1.Replace('/', '\\');
      if (str1 == null || str1.Length == 0)
        str1 = "root\\default";
      bool flag = true;
      string str2 = str1;
      char[] chArray = new char[1]
      {
        '\\'
      };
      foreach (string str3 in str2.Split(chArray))
      {
        if (str3.Length != 0 && (!flag || string.Compare(str3, "root", StringComparison.OrdinalIgnoreCase) == 0) && (Regex.Match(str3, "^[a-z,A-Z]").Success && !Regex.Match(str3, "_$").Success))
        {
          int num = Regex.Match(str3, "[^a-z,A-Z,0-9,_,\\u0080-\\uFFFF]").Success ? 1 : 0;
        }
        flag = false;
      }
      this._Scope = str1;
    }
  }
}
