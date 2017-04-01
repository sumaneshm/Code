// Type: System.Diagnostics.Eventing.Reader.EventLogConfiguration
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogConfiguration : IDisposable
  {
    private EventLogHandle handle = EventLogHandle.Zero;
    private EventLogSession session;
    private string channelName;

    public string LogName
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.channelName;
      }
    }

    public EventLogType LogType
    {
      get
      {
        return (EventLogType) (uint) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigType);
      }
    }

    public EventLogIsolation LogIsolation
    {
      get
      {
        return (EventLogIsolation) (uint) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigIsolation);
      }
    }

    public bool IsEnabled
    {
      get
      {
        return (bool) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigEnabled);
      }
      set
      {
        NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigEnabled, (object) (bool) (value ? 1 : 0));
      }
    }

    public bool IsClassicLog
    {
      get
      {
        return (bool) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigClassicEventlog);
      }
    }

    public string SecurityDescriptor
    {
      get
      {
        return (string) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigAccess);
      }
      set
      {
        NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigAccess, (object) value);
      }
    }

    public string LogFilePath
    {
      get
      {
        return (string) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigLogFilePath);
      }
      set
      {
        NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigLogFilePath, (object) value);
      }
    }

    public long MaximumSizeInBytes
    {
      get
      {
        return (long) (ulong) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigMaxSize);
      }
      set
      {
        NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigMaxSize, (object) value);
      }
    }

    public EventLogMode LogMode
    {
      get
      {
        object channelConfigProperty1 = NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigRetention);
        object channelConfigProperty2 = NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigAutoBackup);
        bool flag = channelConfigProperty1 != null && (bool) channelConfigProperty1;
        if (channelConfigProperty2 != null && (bool) channelConfigProperty2)
          return EventLogMode.AutoBackup;
        return flag ? EventLogMode.Retain : EventLogMode.Circular;
      }
      set
      {
        switch (value)
        {
          case EventLogMode.Circular:
            NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigAutoBackup, (object) false);
            NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigRetention, (object) false);
            break;
          case EventLogMode.AutoBackup:
            NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigAutoBackup, (object) true);
            NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigRetention, (object) true);
            break;
          case EventLogMode.Retain:
            NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigAutoBackup, (object) false);
            NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelLoggingConfigRetention, (object) true);
            break;
        }
      }
    }

    public string OwningProviderName
    {
      get
      {
        return (string) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelConfigOwningPublisher);
      }
    }

    public IEnumerable<string> ProviderNames
    {
      get
      {
        return (IEnumerable<string>) (string[]) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublisherList);
      }
    }

    public int? ProviderLevel
    {
      get
      {
        uint? nullable = (uint?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigLevel);
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
      set
      {
        NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigLevel, (object) value);
      }
    }

    public long? ProviderKeywords
    {
      get
      {
        ulong? nullable = (ulong?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigKeywords);
        if (!nullable.HasValue)
          return new long?();
        else
          return new long?((long) nullable.GetValueOrDefault());
      }
      set
      {
        NativeWrapper.EvtSetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigKeywords, (object) value);
      }
    }

    public int? ProviderBufferSize
    {
      get
      {
        uint? nullable = (uint?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigBufferSize);
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
    }

    public int? ProviderMinimumNumberOfBuffers
    {
      get
      {
        uint? nullable = (uint?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigMinBuffers);
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
    }

    public int? ProviderMaximumNumberOfBuffers
    {
      get
      {
        uint? nullable = (uint?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigMaxBuffers);
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
    }

    public int? ProviderLatency
    {
      get
      {
        uint? nullable = (uint?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigLatency);
        if (!nullable.HasValue)
          return new int?();
        else
          return new int?((int) nullable.GetValueOrDefault());
      }
    }

    public Guid? ProviderControlGuid
    {
      get
      {
        return (Guid?) NativeWrapper.EvtGetChannelConfigProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtChannelConfigPropertyId.EvtChannelPublishingConfigControlGuid);
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogConfiguration(string logName)
      : this(logName, (EventLogSession) null)
    {
    }

    [SecurityCritical]
    public EventLogConfiguration(string logName, EventLogSession session)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (session == null)
        session = EventLogSession.GlobalSession;
      this.session = session;
      this.channelName = logName;
      this.handle = NativeWrapper.EvtOpenChannelConfig(this.session.Handle, this.channelName, 0);
    }

    public void SaveChanges()
    {
      NativeWrapper.EvtSaveChannelConfig(this.handle, 0);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SecuritySafeCritical]
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
        EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (this.handle == null || this.handle.IsInvalid)
        return;
      this.handle.Dispose();
    }
  }
}
