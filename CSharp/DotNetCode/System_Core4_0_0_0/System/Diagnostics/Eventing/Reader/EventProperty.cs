// Type: System.Diagnostics.Eventing.Reader.EventProperty
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class EventProperty
  {
    private object value;

    public object Value
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.value;
      }
    }

    internal EventProperty(object value)
    {
      this.value = value;
    }
  }
}
