// Type: System.Diagnostics.Eventing.Reader.ProviderMetadata
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class ProviderMetadata : IDisposable
  {
    private EventLogHandle handle = EventLogHandle.Zero;
    private EventLogHandle defaultProviderHandle = EventLogHandle.Zero;
    private EventLogSession session;
    private string providerName;
    private CultureInfo cultureInfo;
    private string logFilePath;
    private IList<EventLevel> levels;
    private IList<EventOpcode> opcodes;
    private IList<EventTask> tasks;
    private IList<EventKeyword> keywords;
    private IList<EventLevel> standardLevels;
    private IList<EventOpcode> standardOpcodes;
    private IList<EventTask> standardTasks;
    private IList<EventKeyword> standardKeywords;
    private IList<EventLogLink> channelReferences;
    private object syncObject;

    internal EventLogHandle Handle
    {
      get
      {
        return this.handle;
      }
    }

    public string Name
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.providerName;
      }
    }

    public Guid Id
    {
      get
      {
        return (Guid) NativeWrapper.EvtGetPublisherMetadataProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataPublisherGuid);
      }
    }

    public string MessageFilePath
    {
      get
      {
        return (string) NativeWrapper.EvtGetPublisherMetadataProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataMessageFilePath);
      }
    }

    public string ResourceFilePath
    {
      get
      {
        return (string) NativeWrapper.EvtGetPublisherMetadataProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataResourceFilePath);
      }
    }

    public string ParameterFilePath
    {
      get
      {
        return (string) NativeWrapper.EvtGetPublisherMetadataProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataParameterFilePath);
      }
    }

    public Uri HelpLink
    {
      get
      {
        string uriString = (string) NativeWrapper.EvtGetPublisherMetadataProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataHelpLink);
        if (uriString == null || uriString.Length == 0)
          return (Uri) null;
        else
          return new Uri(uriString);
      }
    }

    private uint ProviderMessageID
    {
      get
      {
        return (uint) NativeWrapper.EvtGetPublisherMetadataProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataPublisherMessageID);
      }
    }

    public string DisplayName
    {
      [SecurityCritical] get
      {
        uint providerMessageId = this.ProviderMessageID;
        if ((int) providerMessageId == -1)
          return (string) null;
        EventLogPermissionHolder.GetEventLogPermission().Demand();
        return NativeWrapper.EvtFormatMessage(this.handle, providerMessageId);
      }
    }

    public IList<EventLogLink> LogLinks
    {
      [SecurityCritical] get
      {
        EventLogHandle eventLogHandle = EventLogHandle.Zero;
        try
        {
          lock (this.syncObject)
          {
            if (this.channelReferences != null)
              return this.channelReferences;
            EventLogPermissionHolder.GetEventLogPermission().Demand();
            eventLogHandle = NativeWrapper.EvtGetPublisherMetadataPropertyHandle(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataChannelReferences);
            int local_1 = NativeWrapper.EvtGetObjectArraySize(eventLogHandle);
            List<EventLogLink> local_2 = new List<EventLogLink>(local_1);
            for (int local_3 = 0; local_3 < local_1; ++local_3)
            {
              string local_4 = (string) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, local_3, 7);
              uint local_5 = (uint) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, local_3, 9);
              bool local_7 = (int) (uint) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, local_3, 10) == 1;
              int local_8 = (int) (uint) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, local_3, 11);
              string local_9 = local_8 != -1 ? NativeWrapper.EvtFormatMessage(this.handle, (uint) local_8) : (string) null;
              if (local_9 == null && local_7)
              {
                int local_8_1 = string.Compare(local_4, "Application", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(local_4, "System", StringComparison.OrdinalIgnoreCase) != 0 ? (string.Compare(local_4, "Security", StringComparison.OrdinalIgnoreCase) != 0 ? -1 : 257) : 258) : 256;
                if (local_8_1 != -1)
                {
                  if (this.defaultProviderHandle.IsInvalid)
                    this.defaultProviderHandle = NativeWrapper.EvtOpenProviderMetadata(this.session.Handle, (string) null, (string) null, this.cultureInfo.LCID, 0);
                  local_9 = NativeWrapper.EvtFormatMessage(this.defaultProviderHandle, (uint) local_8_1);
                }
              }
              local_2.Add(new EventLogLink(local_4, local_7, local_9, local_5));
            }
            this.channelReferences = (IList<EventLogLink>) local_2.AsReadOnly();
          }
          return this.channelReferences;
        }
        finally
        {
          eventLogHandle.Close();
        }
      }
    }

    public IList<EventLevel> Levels
    {
      get
      {
        lock (this.syncObject)
        {
          if (this.levels != null)
            return this.levels;
          this.levels = (IList<EventLevel>) ((List<EventLevel>) this.GetProviderListProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataLevels)).AsReadOnly();
        }
        return this.levels;
      }
    }

    public IList<EventOpcode> Opcodes
    {
      get
      {
        lock (this.syncObject)
        {
          if (this.opcodes != null)
            return this.opcodes;
          this.opcodes = (IList<EventOpcode>) ((List<EventOpcode>) this.GetProviderListProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataOpcodes)).AsReadOnly();
        }
        return this.opcodes;
      }
    }

    public IList<EventKeyword> Keywords
    {
      get
      {
        lock (this.syncObject)
        {
          if (this.keywords != null)
            return this.keywords;
          this.keywords = (IList<EventKeyword>) ((List<EventKeyword>) this.GetProviderListProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataKeywords)).AsReadOnly();
        }
        return this.keywords;
      }
    }

    public IList<EventTask> Tasks
    {
      get
      {
        lock (this.syncObject)
        {
          if (this.tasks != null)
            return this.tasks;
          this.tasks = (IList<EventTask>) ((List<EventTask>) this.GetProviderListProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTasks)).AsReadOnly();
        }
        return this.tasks;
      }
    }

    public IEnumerable<EventMetadata> Events
    {
      [SecurityCritical] get
      {
        EventLogPermissionHolder.GetEventLogPermission().Demand();
        List<EventMetadata> list = new List<EventMetadata>();
        EventLogHandle eventMetadataEnum = NativeWrapper.EvtOpenEventMetadataEnum(this.handle, 0);
        using (eventMetadataEnum)
        {
          while (true)
          {
            EventLogHandle handle = NativeWrapper.EvtNextEventMetadata(eventMetadataEnum, 0);
            if (handle != null)
            {
              using (handle)
              {
                uint id = (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventID);
                byte version = (byte) (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventVersion);
                byte channelId = (byte) (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventChannel);
                byte level = (byte) (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventLevel);
                byte opcode = (byte) (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventOpcode);
                short task = (short) (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventTask);
                long keywords = (long) (ulong) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventKeyword);
                string template = (string) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventTemplate);
                int num = (int) (uint) NativeWrapper.EvtGetEventMetadataProperty(handle, Microsoft.Win32.UnsafeNativeMethods.EvtEventMetadataPropertyId.EventMetadataEventMessageID);
                string description = num != -1 ? NativeWrapper.EvtFormatMessage(this.handle, (uint) num) : (string) null;
                EventMetadata eventMetadata = new EventMetadata(id, version, channelId, level, opcode, task, keywords, template, description, this);
                list.Add(eventMetadata);
              }
            }
            else
              break;
          }
          return (IEnumerable<EventMetadata>) list.AsReadOnly();
        }
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ProviderMetadata(string providerName)
      : this(providerName, (EventLogSession) null, (CultureInfo) null, (string) null)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ProviderMetadata(string providerName, EventLogSession session, CultureInfo targetCultureInfo)
      : this(providerName, session, targetCultureInfo, (string) null)
    {
    }

    [SecuritySafeCritical]
    internal ProviderMetadata(string providerName, EventLogSession session, CultureInfo targetCultureInfo, string logFilePath)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (targetCultureInfo == null)
        targetCultureInfo = CultureInfo.CurrentCulture;
      if (session == null)
        session = EventLogSession.GlobalSession;
      this.session = session;
      this.providerName = providerName;
      this.cultureInfo = targetCultureInfo;
      this.logFilePath = logFilePath;
      this.handle = NativeWrapper.EvtOpenProviderMetadata(this.session.Handle, this.providerName, this.logFilePath, this.cultureInfo.LCID, 0);
      this.syncObject = new object();
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

    internal string FindStandardLevelDisplayName(string name, uint value)
    {
      if (this.standardLevels == null)
        this.standardLevels = (IList<EventLevel>) this.GetProviderListProperty(this.defaultProviderHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataLevels);
      foreach (EventLevel eventLevel in (IEnumerable<EventLevel>) this.standardLevels)
      {
        if (eventLevel.Name == name && (long) eventLevel.Value == (long) value)
          return eventLevel.DisplayName;
      }
      return (string) null;
    }

    internal string FindStandardOpcodeDisplayName(string name, uint value)
    {
      if (this.standardOpcodes == null)
        this.standardOpcodes = (IList<EventOpcode>) this.GetProviderListProperty(this.defaultProviderHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataOpcodes);
      foreach (EventOpcode eventOpcode in (IEnumerable<EventOpcode>) this.standardOpcodes)
      {
        if (eventOpcode.Name == name && (long) eventOpcode.Value == (long) value)
          return eventOpcode.DisplayName;
      }
      return (string) null;
    }

    internal string FindStandardKeywordDisplayName(string name, long value)
    {
      if (this.standardKeywords == null)
        this.standardKeywords = (IList<EventKeyword>) this.GetProviderListProperty(this.defaultProviderHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataKeywords);
      foreach (EventKeyword eventKeyword in (IEnumerable<EventKeyword>) this.standardKeywords)
      {
        if (eventKeyword.Name == name && eventKeyword.Value == value)
          return eventKeyword.DisplayName;
      }
      return (string) null;
    }

    internal string FindStandardTaskDisplayName(string name, uint value)
    {
      if (this.standardTasks == null)
        this.standardTasks = (IList<EventTask>) this.GetProviderListProperty(this.defaultProviderHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTasks);
      foreach (EventTask eventTask in (IEnumerable<EventTask>) this.standardTasks)
      {
        if (eventTask.Name == name && (long) eventTask.Value == (long) value)
          return eventTask.DisplayName;
      }
      return (string) null;
    }

    [SecuritySafeCritical]
    internal object GetProviderListProperty(EventLogHandle providerHandle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId metadataProperty)
    {
      EventLogHandle eventLogHandle = EventLogHandle.Zero;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      try
      {
        List<EventLevel> list1 = (List<EventLevel>) null;
        List<EventOpcode> list2 = (List<EventOpcode>) null;
        List<EventKeyword> list3 = (List<EventKeyword>) null;
        List<EventTask> list4 = (List<EventTask>) null;
        eventLogHandle = NativeWrapper.EvtGetPublisherMetadataPropertyHandle(providerHandle, metadataProperty);
        int objectArraySize = NativeWrapper.EvtGetObjectArraySize(eventLogHandle);
        Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId metadataPropertyId1;
        Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId metadataPropertyId2;
        Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId metadataPropertyId3;
        ProviderMetadata.ObjectTypeName objectTypeName;
        switch (metadataProperty)
        {
          case Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataOpcodes:
            metadataPropertyId1 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataOpcodeName;
            metadataPropertyId2 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataOpcodeValue;
            metadataPropertyId3 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataOpcodeMessageID;
            objectTypeName = ProviderMetadata.ObjectTypeName.Opcode;
            list2 = new List<EventOpcode>(objectArraySize);
            break;
          case Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataKeywords:
            metadataPropertyId1 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataKeywordName;
            metadataPropertyId2 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataKeywordValue;
            metadataPropertyId3 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataKeywordMessageID;
            objectTypeName = ProviderMetadata.ObjectTypeName.Keyword;
            list3 = new List<EventKeyword>(objectArraySize);
            break;
          case Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataLevels:
            metadataPropertyId1 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataLevelName;
            metadataPropertyId2 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataLevelValue;
            metadataPropertyId3 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataLevelMessageID;
            objectTypeName = ProviderMetadata.ObjectTypeName.Level;
            list1 = new List<EventLevel>(objectArraySize);
            break;
          case Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTasks:
            metadataPropertyId1 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTaskName;
            metadataPropertyId2 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTaskValue;
            metadataPropertyId3 = Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTaskMessageID;
            objectTypeName = ProviderMetadata.ObjectTypeName.Task;
            list4 = new List<EventTask>(objectArraySize);
            break;
          default:
            return (object) null;
        }
        for (int index = 0; index < objectArraySize; ++index)
        {
          string name = (string) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, index, (int) metadataPropertyId1);
          uint num1 = 0U;
          long num2 = 0L;
          if (objectTypeName != ProviderMetadata.ObjectTypeName.Keyword)
            num1 = (uint) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, index, (int) metadataPropertyId2);
          else
            num2 = (long) (ulong) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, index, (int) metadataPropertyId2);
          int num3 = (int) (uint) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, index, (int) metadataPropertyId3);
          string displayName = (string) null;
          if (num3 == -1)
          {
            if (providerHandle != this.defaultProviderHandle)
            {
              if (this.defaultProviderHandle.IsInvalid)
                this.defaultProviderHandle = NativeWrapper.EvtOpenProviderMetadata(this.session.Handle, (string) null, (string) null, this.cultureInfo.LCID, 0);
              switch (objectTypeName)
              {
                case ProviderMetadata.ObjectTypeName.Level:
                  displayName = this.FindStandardLevelDisplayName(name, num1);
                  break;
                case ProviderMetadata.ObjectTypeName.Opcode:
                  displayName = this.FindStandardOpcodeDisplayName(name, num1 >> 16);
                  break;
                case ProviderMetadata.ObjectTypeName.Task:
                  displayName = this.FindStandardTaskDisplayName(name, num1);
                  break;
                case ProviderMetadata.ObjectTypeName.Keyword:
                  displayName = this.FindStandardKeywordDisplayName(name, num2);
                  break;
                default:
                  displayName = (string) null;
                  break;
              }
            }
          }
          else
            displayName = NativeWrapper.EvtFormatMessage(providerHandle, (uint) num3);
          switch (objectTypeName)
          {
            case ProviderMetadata.ObjectTypeName.Level:
              list1.Add(new EventLevel(name, (int) num1, displayName));
              break;
            case ProviderMetadata.ObjectTypeName.Opcode:
              list2.Add(new EventOpcode(name, (int) (num1 >> 16), displayName));
              break;
            case ProviderMetadata.ObjectTypeName.Task:
              Guid guid = (Guid) NativeWrapper.EvtGetObjectArrayProperty(eventLogHandle, index, 18);
              list4.Add(new EventTask(name, (int) num1, displayName, guid));
              break;
            case ProviderMetadata.ObjectTypeName.Keyword:
              list3.Add(new EventKeyword(name, num2, displayName));
              break;
            default:
              return (object) null;
          }
        }
        switch (objectTypeName)
        {
          case ProviderMetadata.ObjectTypeName.Level:
            return (object) list1;
          case ProviderMetadata.ObjectTypeName.Opcode:
            return (object) list2;
          case ProviderMetadata.ObjectTypeName.Task:
            return (object) list4;
          case ProviderMetadata.ObjectTypeName.Keyword:
            return (object) list3;
          default:
            return (object) null;
        }
      }
      finally
      {
        eventLogHandle.Close();
      }
    }

    internal void CheckReleased()
    {
      lock (this.syncObject)
        this.GetProviderListProperty(this.handle, Microsoft.Win32.UnsafeNativeMethods.EvtPublisherMetadataPropertyId.EvtPublisherMetadataTasks);
    }

    internal enum ObjectTypeName
    {
      Level,
      Opcode,
      Task,
      Keyword,
    }
  }
}
