// Type: System.Management.Instrumentation.ManagementMemberAttribute
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;

namespace System.Management.Instrumentation
{
  [AttributeUsage(AttributeTargets.All)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public abstract class ManagementMemberAttribute : Attribute
  {
    private string _Name;

    public string Name
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._Name;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this._Name = value;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected ManagementMemberAttribute()
    {
    }
  }
}
