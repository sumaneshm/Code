﻿// Type: System.Diagnostics.Eventing.Reader.EventRecord
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public abstract class EventRecord : IDisposable
  {
    public abstract int Id { get; }

    public abstract byte? Version { get; }

    public abstract byte? Level { get; }

    public abstract int? Task { get; }

    public abstract short? Opcode { get; }

    public abstract long? Keywords { get; }

    public abstract long? RecordId { get; }

    public abstract string ProviderName { get; }

    public abstract Guid? ProviderId { get; }

    public abstract string LogName { get; }

    public abstract int? ProcessId { get; }

    public abstract int? ThreadId { get; }

    public abstract string MachineName { get; }

    public abstract SecurityIdentifier UserId { get; }

    public abstract DateTime? TimeCreated { get; }

    public abstract Guid? ActivityId { get; }

    public abstract Guid? RelatedActivityId { get; }

    public abstract int? Qualifiers { get; }

    public abstract string LevelDisplayName { get; }

    public abstract string OpcodeDisplayName { get; }

    public abstract string TaskDisplayName { get; }

    public abstract IEnumerable<string> KeywordsDisplayNames { get; }

    public abstract EventBookmark Bookmark { get; }

    public abstract IList<EventProperty> Properties { get; }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EventRecord()
    {
    }

    public abstract string FormatDescription();

    public abstract string FormatDescription(IEnumerable<object> values);

    public abstract string ToXml();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
  }
}
