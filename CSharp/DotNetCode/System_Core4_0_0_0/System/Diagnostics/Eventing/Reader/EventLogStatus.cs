// Type: System.Diagnostics.Eventing.Reader.EventLogStatus
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class EventLogStatus
  {
    private string channelName;
    private int win32ErrorCode;

    public string LogName
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.channelName;
      }
    }

    public int StatusCode
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.win32ErrorCode;
      }
    }

    internal EventLogStatus(string channelName, int win32ErrorCode)
    {
      this.channelName = channelName;
      this.win32ErrorCode = win32ErrorCode;
    }
  }
}
