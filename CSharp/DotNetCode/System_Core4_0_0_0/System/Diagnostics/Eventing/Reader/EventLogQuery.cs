// Type: System.Diagnostics.Eventing.Reader.EventLogQuery
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;

namespace System.Diagnostics.Eventing.Reader
{
  public class EventLogQuery
  {
    private string query;
    private string path;
    private EventLogSession session;
    private PathType pathType;
    private bool tolerateErrors;
    private bool reverseDirection;

    public EventLogSession Session
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.session;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.session = value;
      }
    }

    public bool TolerateQueryErrors
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.tolerateErrors;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.tolerateErrors = value;
      }
    }

    public bool ReverseDirection
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.reverseDirection;
      }
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
      {
        this.reverseDirection = value;
      }
    }

    internal string Path
    {
      get
      {
        return this.path;
      }
    }

    internal PathType ThePathType
    {
      get
      {
        return this.pathType;
      }
    }

    internal string Query
    {
      get
      {
        return this.query;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogQuery(string path, PathType pathType)
      : this(path, pathType, (string) null)
    {
    }

    public EventLogQuery(string path, PathType pathType, string query)
    {
      this.session = EventLogSession.GlobalSession;
      this.path = path;
      this.pathType = pathType;
      if (query == null)
      {
        if (path == null)
          throw new ArgumentNullException("path");
      }
      else
        this.query = query;
    }
  }
}
