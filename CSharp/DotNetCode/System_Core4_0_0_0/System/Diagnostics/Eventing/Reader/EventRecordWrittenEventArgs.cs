// Type: System.Diagnostics.Eventing.Reader.EventRecordWrittenEventArgs
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class EventRecordWrittenEventArgs : EventArgs
  {
    private EventRecord record;
    private Exception exception;

    public EventRecord EventRecord
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.record;
      }
    }

    public Exception EventException
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.exception;
      }
    }

    internal EventRecordWrittenEventArgs(EventLogRecord record)
    {
      this.record = (EventRecord) record;
    }

    internal EventRecordWrittenEventArgs(Exception exception)
    {
      this.exception = exception;
    }
  }
}
