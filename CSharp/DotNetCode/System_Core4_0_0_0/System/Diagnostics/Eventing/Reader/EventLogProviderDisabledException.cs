// Type: System.Diagnostics.Eventing.Reader.EventLogProviderDisabledException
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogProviderDisabledException : EventLogException
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogProviderDisabledException()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogProviderDisabledException(string message)
      : base(message)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogProviderDisabledException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EventLogProviderDisabledException(SerializationInfo serializationInfo, StreamingContext streamingContext)
      : base(serializationInfo, streamingContext)
    {
    }

    internal EventLogProviderDisabledException(int errorCode)
      : base(errorCode)
    {
    }
  }
}
