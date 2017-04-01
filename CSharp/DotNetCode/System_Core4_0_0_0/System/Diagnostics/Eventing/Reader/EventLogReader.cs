// Type: System.Diagnostics.Eventing.Reader.EventLogReader
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogReader : IDisposable
  {
    private EventLogQuery eventQuery;
    private int batchSize;
    private EventLogHandle handle;
    private IntPtr[] eventsBuffer;
    private int currentIndex;
    private int eventCount;
    private bool isEof;
    private ProviderMetadataCachedInformation cachedMetadataInformation;

    public int BatchSize
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.batchSize;
      }
      set
      {
        if (value < 1)
          throw new ArgumentOutOfRangeException("value");
        this.batchSize = value;
      }
    }

    public IList<EventLogStatus> LogStatus
    {
      [SecurityCritical] get
      {
        EventLogPermissionHolder.GetEventLogPermission().Demand();
        EventLogHandle handle = this.handle;
        if (handle.IsInvalid)
          throw new InvalidOperationException();
        string[] strArray = (string[]) NativeWrapper.EvtGetQueryInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtQueryPropertyId.EvtQueryNames);
        int[] numArray = (int[]) NativeWrapper.EvtGetQueryInfo(handle, Microsoft.Win32.UnsafeNativeMethods.EvtQueryPropertyId.EvtQueryStatuses);
        if (strArray.Length != numArray.Length)
          throw new InvalidOperationException();
        List<EventLogStatus> list = new List<EventLogStatus>(strArray.Length);
        for (int index = 0; index < strArray.Length; ++index)
        {
          EventLogStatus eventLogStatus = new EventLogStatus(strArray[index], numArray[index]);
          list.Add(eventLogStatus);
        }
        return (IList<EventLogStatus>) list.AsReadOnly();
      }
    }

    public EventLogReader(string path)
      : this(new EventLogQuery(path, PathType.LogName), (EventBookmark) null)
    {
    }

    public EventLogReader(string path, PathType pathType)
      : this(new EventLogQuery(path, pathType), (EventBookmark) null)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogReader(EventLogQuery eventQuery)
      : this(eventQuery, (EventBookmark) null)
    {
    }

    [SecurityCritical]
    public EventLogReader(EventLogQuery eventQuery, EventBookmark bookmark)
    {
      if (eventQuery == null)
        throw new ArgumentNullException("eventQuery");
      string logfile = (string) null;
      if (eventQuery.ThePathType == PathType.FilePath)
        logfile = eventQuery.Path;
      this.cachedMetadataInformation = new ProviderMetadataCachedInformation(eventQuery.Session, logfile, 50);
      this.eventQuery = eventQuery;
      this.batchSize = 64;
      this.eventsBuffer = new IntPtr[this.batchSize];
      int num = 0;
      int flags = this.eventQuery.ThePathType != PathType.LogName ? num | 2 : num | 1;
      if (this.eventQuery.ReverseDirection)
        flags |= 512;
      if (this.eventQuery.TolerateQueryErrors)
        flags |= 4096;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      this.handle = NativeWrapper.EvtQuery(this.eventQuery.Session.Handle, this.eventQuery.Path, this.eventQuery.Query, flags);
      EventLogHandle handleFromBookmark = EventLogRecord.GetBookmarkHandleFromBookmark(bookmark);
      if (handleFromBookmark.IsInvalid)
        return;
      using (handleFromBookmark)
        NativeWrapper.EvtSeek(this.handle, 1L, handleFromBookmark, 0, Microsoft.Win32.UnsafeNativeMethods.EvtSeekFlags.EvtSeekRelativeToBookmark);
    }

    public EventRecord ReadEvent()
    {
      return this.ReadEvent(TimeSpan.MaxValue);
    }

    [SecurityCritical]
    public EventRecord ReadEvent(TimeSpan timeout)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (this.isEof)
        throw new InvalidOperationException();
      if (this.currentIndex >= this.eventCount)
      {
        this.GetNextBatch(timeout);
        if (this.currentIndex >= this.eventCount)
        {
          this.isEof = true;
          return (EventRecord) null;
        }
      }
      EventLogRecord eventLogRecord = new EventLogRecord(new EventLogHandle(this.eventsBuffer[this.currentIndex], true), this.eventQuery.Session, this.cachedMetadataInformation);
      ++this.currentIndex;
      return (EventRecord) eventLogRecord;
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
      for (; this.currentIndex < this.eventCount; ++this.currentIndex)
        NativeWrapper.EvtClose(this.eventsBuffer[this.currentIndex]);
      if (this.handle == null || this.handle.IsInvalid)
        return;
      this.handle.Dispose();
    }

    public void Seek(EventBookmark bookmark)
    {
      this.Seek(bookmark, 0L);
    }

    [SecurityCritical]
    public void Seek(EventBookmark bookmark, long offset)
    {
      if (bookmark == null)
        throw new ArgumentNullException("bookmark");
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      this.SeekReset();
      using (EventLogHandle handleFromBookmark = EventLogRecord.GetBookmarkHandleFromBookmark(bookmark))
        NativeWrapper.EvtSeek(this.handle, offset, handleFromBookmark, 0, Microsoft.Win32.UnsafeNativeMethods.EvtSeekFlags.EvtSeekRelativeToBookmark);
    }

    [SecurityCritical]
    public void Seek(SeekOrigin origin, long offset)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      switch (origin)
      {
        case SeekOrigin.Begin:
          this.SeekReset();
          NativeWrapper.EvtSeek(this.handle, offset, EventLogHandle.Zero, 0, Microsoft.Win32.UnsafeNativeMethods.EvtSeekFlags.EvtSeekRelativeToFirst);
          break;
        case SeekOrigin.Current:
          if (offset >= 0L)
          {
            if ((long) this.currentIndex + offset < (long) this.eventCount)
            {
              for (int index = this.currentIndex; (long) index < (long) this.currentIndex + offset; ++index)
                NativeWrapper.EvtClose(this.eventsBuffer[index]);
              this.currentIndex = (int) ((long) this.currentIndex + offset);
              break;
            }
            else
            {
              this.SeekCommon(offset);
              break;
            }
          }
          else if ((long) this.currentIndex + offset >= 0L)
          {
            this.SeekCommon(offset);
            break;
          }
          else
          {
            this.SeekCommon(offset);
            break;
          }
        case SeekOrigin.End:
          this.SeekReset();
          NativeWrapper.EvtSeek(this.handle, offset, EventLogHandle.Zero, 0, Microsoft.Win32.UnsafeNativeMethods.EvtSeekFlags.EvtSeekRelativeToLast);
          break;
      }
    }

    public void CancelReading()
    {
      NativeWrapper.EvtCancel(this.handle);
    }

    [SecurityCritical]
    private bool GetNextBatch(TimeSpan ts)
    {
      int timeout = !(ts == TimeSpan.MaxValue) ? (int) ts.TotalMilliseconds : -1;
      if (this.batchSize != this.eventsBuffer.Length)
        this.eventsBuffer = new IntPtr[this.batchSize];
      int returned = 0;
      if (!NativeWrapper.EvtNext(this.handle, this.batchSize, this.eventsBuffer, timeout, 0, ref returned))
      {
        this.eventCount = 0;
        this.currentIndex = 0;
        return false;
      }
      else
      {
        this.currentIndex = 0;
        this.eventCount = returned;
        return true;
      }
    }

    [SecurityCritical]
    internal void SeekReset()
    {
      for (; this.currentIndex < this.eventCount; ++this.currentIndex)
        NativeWrapper.EvtClose(this.eventsBuffer[this.currentIndex]);
      this.currentIndex = 0;
      this.eventCount = 0;
      this.isEof = false;
    }

    [SecurityCritical]
    internal void SeekCommon(long offset)
    {
      offset -= (long) (this.eventCount - this.currentIndex);
      this.SeekReset();
      NativeWrapper.EvtSeek(this.handle, offset, EventLogHandle.Zero, 0, Microsoft.Win32.UnsafeNativeMethods.EvtSeekFlags.EvtSeekRelativeToCurrent);
    }
  }
}
