// Type: System.Diagnostics.Eventing.Reader.EventLogException
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.ComponentModel;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogException : Exception, ISerializable
  {
    private int errorCode;

    public override string Message
    {
      [SecurityCritical] get
      {
        EventLogPermissionHolder.GetEventLogPermission().Demand();
        return new Win32Exception(this.errorCode).Message;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogException()
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogException(string message)
      : base(message)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EventLogException(SerializationInfo serializationInfo, StreamingContext streamingContext)
      : base(serializationInfo, streamingContext)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EventLogException(int errorCode)
    {
      this.errorCode = errorCode;
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      info.AddValue("errorCode", this.errorCode);
      base.GetObjectData(info, context);
    }

    internal static void Throw(int errorCode)
    {
      switch (errorCode)
      {
        case 15011:
        case 15012:
          throw new EventLogReadingException(errorCode);
        case 15027:
        case 15028:
        case 15002:
        case 15007:
        case 2:
        case 3:
          throw new EventLogNotFoundException(errorCode);
        case 15037:
          throw new EventLogProviderDisabledException(errorCode);
        case 15005:
        case 13:
          throw new EventLogInvalidDataException(errorCode);
        case 1223:
        case 1818:
          throw new OperationCanceledException();
        case 5:
          throw new UnauthorizedAccessException();
        default:
          throw new EventLogException(errorCode);
      }
    }
  }
}
