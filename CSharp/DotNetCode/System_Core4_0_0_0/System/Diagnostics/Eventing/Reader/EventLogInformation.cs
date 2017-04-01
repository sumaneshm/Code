// Type: System.Diagnostics.Eventing.Reader.EventLogInformation
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class EventLogInformation
  {
    private DateTime? creationTime;
    private DateTime? lastAccessTime;
    private DateTime? lastWriteTime;
    private long? fileSize;
    private int? fileAttributes;
    private long? recordCount;
    private long? oldestRecordNumber;
    private bool? isLogFull;

    public DateTime? CreationTime
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.creationTime;
      }
    }

    public DateTime? LastAccessTime
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.lastAccessTime;
      }
    }

    public DateTime? LastWriteTime
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.lastWriteTime;
      }
    }

    public long? FileSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.fileSize;
      }
    }

    public int? Attributes
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.fileAttributes;
      }
    }

    public long? RecordCount
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.recordCount;
      }
    }

    public long? OldestRecordNumber
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.oldestRecordNumber;
      }
    }

    public bool? IsLogFull
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.isLogFull;
      }
    }

    [SecuritySafeCritical]
    internal EventLogInformation(EventLogSession session, string channelName, PathType pathType)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      EventLogHandle handle = NativeWrapper.EvtOpenLog(session.Handle, channelName, pathType);
      using (handle)
      {
        this.creationTime = (DateTime?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogCreationTime);
        this.lastAccessTime = (DateTime?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogLastAccessTime);
        this.lastWriteTime = (DateTime?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogLastWriteTime);
        EventLogInformation eventLogInformation1 = this;
        ulong? nullable1 = (ulong?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogFileSize);
        long? nullable2 = nullable1.HasValue ? new long?((long) nullable1.GetValueOrDefault()) : new long?();
        eventLogInformation1.fileSize = nullable2;
        EventLogInformation eventLogInformation2 = this;
        uint? nullable3 = (uint?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogAttributes);
        int? nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
        eventLogInformation2.fileAttributes = nullable4;
        EventLogInformation eventLogInformation3 = this;
        ulong? nullable5 = (ulong?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogNumberOfLogRecords);
        long? nullable6 = nullable5.HasValue ? new long?((long) nullable5.GetValueOrDefault()) : new long?();
        eventLogInformation3.recordCount = nullable6;
        EventLogInformation eventLogInformation4 = this;
        ulong? nullable7 = (ulong?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogOldestRecordNumber);
        long? nullable8 = nullable7.HasValue ? new long?((long) nullable7.GetValueOrDefault()) : new long?();
        eventLogInformation4.oldestRecordNumber = nullable8;
        this.isLogFull = (bool?) NativeWrapper.EvtGetLogInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtLogPropertyId.EvtLogFull);
      }
    }
  }
}
