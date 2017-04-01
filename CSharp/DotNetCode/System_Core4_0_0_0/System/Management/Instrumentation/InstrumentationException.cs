// Type: System.Management.Instrumentation.InstrumentationException
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management.Instrumentation
{
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class InstrumentationException : InstrumentationBaseException
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public InstrumentationException()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public InstrumentationException(string message)
      : base(message)
    {
    }

    public InstrumentationException(Exception innerException)
      : base((string) null, innerException)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public InstrumentationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected InstrumentationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
