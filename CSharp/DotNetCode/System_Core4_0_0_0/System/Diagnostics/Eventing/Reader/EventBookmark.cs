// Type: System.Diagnostics.Eventing.Reader.EventBookmark
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [Serializable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventBookmark : ISerializable
  {
    private string bookmark;

    internal string BookmarkText
    {
      get
      {
        return this.bookmark;
      }
    }

    protected EventBookmark(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      this.bookmark = info.GetString("BookmarkText");
    }

    internal EventBookmark(string bookmarkText)
    {
      if (bookmarkText == null)
        throw new ArgumentNullException("bookmarkText");
      this.bookmark = bookmarkText;
    }

    [SecurityCritical]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      this.GetObjectData(info, context);
    }

    [SecurityCritical]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException("info");
      info.AddValue("BookmarkText", (object) this.bookmark);
    }
  }
}
