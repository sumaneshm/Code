// Type: System.Diagnostics.Eventing.Reader.EventLogRecord
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogRecord : EventRecord
  {
    [SecuritySafeCritical]
    private EventLogHandle handle;
    private EventLogSession session;
    private NativeWrapper.SystemProperties systemProperties;
    private string containerChannel;
    private int[] matchedQueryIds;
    private object syncObject;
    private string levelName;
    private string taskName;
    private string opcodeName;
    private IEnumerable<string> keywordsNames;
    private bool levelNameReady;
    private bool taskNameReady;
    private bool opcodeNameReady;
    private ProviderMetadataCachedInformation cachedMetadataInformation;
    private const int SYSTEM_PROPERTY_COUNT = 18;

    internal EventLogHandle Handle
    {
      [SecuritySafeCritical] get
      {
        return this.handle;
      }
    }

    public override int Id
    {
      get
      {
        this.PrepareSystemData();
        ushort? nullable = this.systemProperties.Id;
        if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
          return 0;
        else
          return (int) this.systemProperties.Id.Value;
      }
    }

    public override byte? Version
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.Version;
      }
    }

    public override int? Qualifiers
    {
      get
      {
        this.PrepareSystemData();
        ushort? nullable1 = this.systemProperties.Qualifiers;
        uint? nullable2 = nullable1.HasValue ? new uint?((uint) nullable1.GetValueOrDefault()) : new uint?();
        if (!nullable2.HasValue)
          return new int?();
        else
          return new int?((int) nullable2.GetValueOrDefault());
      }
    }

    public override byte? Level
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.Level;
      }
    }

    public override int? Task
    {
      get
      {
        this.PrepareSystemData();
        ushort? nullable1 = this.systemProperties.Task;
        uint? nullable2 = nullable1.HasValue ? new uint?((uint) nullable1.GetValueOrDefault()) : new uint?();
        if (!nullable2.HasValue)
          return new int?();
        else
          return new int?((int) nullable2.GetValueOrDefault());
      }
    }

    public override short? Opcode
    {
      get
      {
        this.PrepareSystemData();
        byte? nullable1 = this.systemProperties.Opcode;
        ushort? nullable2 = nullable1.HasValue ? new ushort?((ushort) nullable1.GetValueOrDefault()) : new ushort?();
        if (!nullable2.HasValue)
          return new short?();
        else
          return new short?((short) nullable2.GetValueOrDefault());
      }
    }

    public override long? Keywords
    {
      get
      {
        this.PrepareSystemData();
        ulong? nullable = this.systemProperties.Keywords;
        if (!nullable.HasValue)
          return new long?();
        else
          return new long?((long) nullable.GetValueOrDefault());
      }
    }

    public override long? RecordId
    {
      get
      {
        this.PrepareSystemData();
        ulong? nullable = this.systemProperties.RecordId;
        if (!nullable.HasValue)
          return new long?();
        else
          return new long?((long) nullable.GetValueOrDefault());
      }
    }

    public override string ProviderName
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.ProviderName;
      }
    }

    public override Guid? ProviderId
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.ProviderId;
      }
    }

    public override string LogName
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.ChannelName;
      }
    }

    public override int? ProcessId
    {
      get
      {
        this.PrepareSystemData();
        uint? nullable = this.systemProperties.ProcessId;
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
    }

    public override int? ThreadId
    {
      get
      {
        this.PrepareSystemData();
        uint? nullable = this.systemProperties.ThreadId;
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
    }

    public override string MachineName
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.ComputerName;
      }
    }

    public override SecurityIdentifier UserId
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.UserId;
      }
    }

    public override DateTime? TimeCreated
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.TimeCreated;
      }
    }

    public override Guid? ActivityId
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.ActivityId;
      }
    }

    public override Guid? RelatedActivityId
    {
      get
      {
        this.PrepareSystemData();
        return this.systemProperties.RelatedActivityId;
      }
    }

    public string ContainerLog
    {
      get
      {
        if (this.containerChannel != null)
          return this.containerChannel;
        lock (this.syncObject)
        {
          if (this.containerChannel == null)
            this.containerChannel = (string) NativeWrapper.EvtGetEventInfo(this.Handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventPropertyId.EvtEventPath);
          return this.containerChannel;
        }
      }
    }

    public IEnumerable<int> MatchedQueryIds
    {
      get
      {
        if (this.matchedQueryIds != null)
          return (IEnumerable<int>) this.matchedQueryIds;
        lock (this.syncObject)
        {
          if (this.matchedQueryIds == null)
            this.matchedQueryIds = (int[]) NativeWrapper.EvtGetEventInfo(this.Handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventPropertyId.EvtEventQueryIDs);
          return (IEnumerable<int>) this.matchedQueryIds;
        }
      }
    }

    public override EventBookmark Bookmark
    {
      [SecuritySafeCritical] get
      {
        EventLogPermissionHolder.GetEventLogPermission().Demand();
        EventLogHandle bookmark = NativeWrapper.EvtCreateBookmark((string) null);
        NativeWrapper.EvtUpdateBookmark(bookmark, this.handle);
        return new EventBookmark(NativeWrapper.EvtRenderBookmark(bookmark));
      }
    }

    public override string LevelDisplayName
    {
      get
      {
        if (this.levelNameReady)
          return this.levelName;
        lock (this.syncObject)
        {
          if (!this.levelNameReady)
          {
            this.levelNameReady = true;
            this.levelName = this.cachedMetadataInformation.GetLevelDisplayName(this.ProviderName, this.handle);
          }
          return this.levelName;
        }
      }
    }

    public override string OpcodeDisplayName
    {
      get
      {
        lock (this.syncObject)
        {
          if (!this.opcodeNameReady)
          {
            this.opcodeNameReady = true;
            this.opcodeName = this.cachedMetadataInformation.GetOpcodeDisplayName(this.ProviderName, this.handle);
          }
          return this.opcodeName;
        }
      }
    }

    public override string TaskDisplayName
    {
      get
      {
        if (this.taskNameReady)
          return this.taskName;
        lock (this.syncObject)
        {
          if (!this.taskNameReady)
          {
            this.taskNameReady = true;
            this.taskName = this.cachedMetadataInformation.GetTaskDisplayName(this.ProviderName, this.handle);
          }
          return this.taskName;
        }
      }
    }

    public override IEnumerable<string> KeywordsDisplayNames
    {
      get
      {
        if (this.keywordsNames != null)
          return this.keywordsNames;
        lock (this.syncObject)
        {
          if (this.keywordsNames == null)
            this.keywordsNames = this.cachedMetadataInformation.GetKeywordDisplayNames(this.ProviderName, this.handle);
          return this.keywordsNames;
        }
      }
    }

    public override IList<EventProperty> Properties
    {
      get
      {
        this.session.SetupUserContext();
        IList<object> list1 = NativeWrapper.EvtRenderBufferWithContextUserOrValues(this.session.renderContextHandleUser, this.handle);
        List<EventProperty> list2 = new List<EventProperty>();
        foreach (object obj in (IEnumerable<object>) list1)
          list2.Add(new EventProperty(obj));
        return (IList<EventProperty>) list2;
      }
    }

    [SecuritySafeCritical]
    internal EventLogRecord(EventLogHandle handle, EventLogSession session, ProviderMetadataCachedInformation cachedMetadataInfo)
    {
      this.cachedMetadataInformation = cachedMetadataInfo;
      this.handle = handle;
      this.session = session;
      this.systemProperties = new NativeWrapper.SystemProperties();
      this.syncObject = new object();
    }

    public override string FormatDescription()
    {
      return this.cachedMetadataInformation.GetFormatDescription(this.ProviderName, this.handle);
    }

    public override string FormatDescription(IEnumerable<object> values)
    {
      if (values == null)
        return this.FormatDescription();
      string[] array = new string[0];
      int index = 0;
      foreach (object obj in values)
      {
        if (array.Length == index)
          Array.Resize<string>(ref array, index + 1);
        array[index] = obj.ToString();
        ++index;
      }
      return this.cachedMetadataInformation.GetFormatDescription(this.ProviderName, this.handle, array);
    }

    public IList<object> GetPropertyValues(EventLogPropertySelector propertySelector)
    {
      if (propertySelector == null)
        throw new ArgumentNullException("propertySelector");
      else
        return NativeWrapper.EvtRenderBufferWithContextUserOrValues(propertySelector.Handle, this.handle);
    }

    [SecuritySafeCritical]
    public override string ToXml()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      StringBuilder buffer = new StringBuilder(2000);
      NativeWrapper.EvtRender(EventLogHandle.Zero, this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags.EvtRenderEventXml, buffer);
      return ((object) buffer).ToString();
    }

    [SecuritySafeCritical]
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing)
          EventLogPermissionHolder.GetEventLogPermission().Demand();
        if (this.handle == null || this.handle.IsInvalid)
          return;
        this.handle.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    internal void PrepareSystemData()
    {
      if (this.systemProperties.filled)
        return;
      this.session.SetupSystemContext();
      lock (this.syncObject)
      {
        if (this.systemProperties.filled)
          return;
        NativeWrapper.EvtRenderBufferWithContextSystem(this.session.renderContextHandleSystem, this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtRenderFlags.EvtRenderEventValues, this.systemProperties, 18);
        this.systemProperties.filled = true;
      }
    }

    [SecurityCritical]
    internal static EventLogHandle GetBookmarkHandleFromBookmark(EventBookmark bookmark)
    {
      if (bookmark == null)
        return EventLogHandle.Zero;
      else
        return NativeWrapper.EvtCreateBookmark(bookmark.BookmarkText);
    }
  }
}
