// Type: System.Diagnostics.Eventing.Reader.EventLogWatcher
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogWatcher : IDisposable
  {
    private EventLogQuery eventQuery;
    private EventBookmark bookmark;
    private bool readExistingEvents;
    private EventLogHandle handle;
    private IntPtr[] eventsBuffer;
    private int numEventsInBuffer;
    private bool isSubscribing;
    private int callbackThreadId;
    private AutoResetEvent subscriptionWaitHandle;
    private AutoResetEvent unregisterDoneHandle;
    private RegisteredWaitHandle registeredWaitHandle;
    private ProviderMetadataCachedInformation cachedMetadataInformation;
    private EventLogException asyncException;

    public bool Enabled
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.isSubscribing;
      }
      set
      {
        if (value && !this.isSubscribing)
        {
          this.StartSubscribing();
        }
        else
        {
          if (value || !this.isSubscribing)
            return;
          this.StopSubscribing();
        }
      }
    }

    public event EventHandler<EventRecordWrittenEventArgs> EventRecordWritten;

    public EventLogWatcher(string path)
      : this(new EventLogQuery(path, PathType.LogName), (EventBookmark) null, false)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogWatcher(EventLogQuery eventQuery)
      : this(eventQuery, (EventBookmark) null, false)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogWatcher(EventLogQuery eventQuery, EventBookmark bookmark)
      : this(eventQuery, bookmark, false)
    {
    }

    public EventLogWatcher(EventLogQuery eventQuery, EventBookmark bookmark, bool readExistingEvents)
    {
      if (eventQuery == null)
        throw new ArgumentNullException("eventQuery");
      if (bookmark != null)
        readExistingEvents = false;
      this.eventQuery = eventQuery;
      this.readExistingEvents = readExistingEvents;
      if (this.eventQuery.ReverseDirection)
        throw new InvalidOperationException();
      this.eventsBuffer = new IntPtr[64];
      this.cachedMetadataInformation = new ProviderMetadataCachedInformation(eventQuery.Session, (string) null, 50);
      this.bookmark = bookmark;
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
      {
        this.StopSubscribing();
      }
      else
      {
        for (int index = 0; index < this.numEventsInBuffer; ++index)
        {
          if (this.eventsBuffer[index] != IntPtr.Zero)
          {
            NativeWrapper.EvtClose(this.eventsBuffer[index]);
            this.eventsBuffer[index] = IntPtr.Zero;
          }
        }
        this.numEventsInBuffer = 0;
      }
    }

    [SecuritySafeCritical]
    internal void StopSubscribing()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      this.isSubscribing = false;
      if (this.registeredWaitHandle != null)
      {
        this.registeredWaitHandle.Unregister((WaitHandle) this.unregisterDoneHandle);
        if (this.callbackThreadId != Thread.CurrentThread.ManagedThreadId && this.unregisterDoneHandle != null)
          this.unregisterDoneHandle.WaitOne();
        this.registeredWaitHandle = (RegisteredWaitHandle) null;
      }
      if (this.unregisterDoneHandle != null)
      {
        this.unregisterDoneHandle.Close();
        this.unregisterDoneHandle = (AutoResetEvent) null;
      }
      if (this.subscriptionWaitHandle != null)
      {
        this.subscriptionWaitHandle.Close();
        this.subscriptionWaitHandle = (AutoResetEvent) null;
      }
      for (int index = 0; index < this.numEventsInBuffer; ++index)
      {
        if (this.eventsBuffer[index] != IntPtr.Zero)
        {
          NativeWrapper.EvtClose(this.eventsBuffer[index]);
          this.eventsBuffer[index] = IntPtr.Zero;
        }
      }
      this.numEventsInBuffer = 0;
      if (this.handle == null || this.handle.IsInvalid)
        return;
      this.handle.Dispose();
    }

    [SecuritySafeCritical]
    internal void StartSubscribing()
    {
      if (this.isSubscribing)
        throw new InvalidOperationException();
      int num = 0;
      int flags = this.bookmark == null ? (!this.readExistingEvents ? num | 1 : num | 2) : num | 3;
      if (this.eventQuery.TolerateQueryErrors)
        flags |= 4096;
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      this.callbackThreadId = -1;
      this.unregisterDoneHandle = new AutoResetEvent(false);
      this.subscriptionWaitHandle = new AutoResetEvent(false);
      EventLogHandle handleFromBookmark = EventLogRecord.GetBookmarkHandleFromBookmark(this.bookmark);
      using (handleFromBookmark)
        this.handle = NativeWrapper.EvtSubscribe(this.eventQuery.Session.Handle, this.subscriptionWaitHandle.SafeWaitHandle, this.eventQuery.Path, this.eventQuery.Query, handleFromBookmark, IntPtr.Zero, IntPtr.Zero, flags);
      this.isSubscribing = true;
      this.RequestEvents();
      this.registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject((WaitHandle) this.subscriptionWaitHandle, new WaitOrTimerCallback(this.SubscribedEventsAvailableCallback), (object) null, -1, false);
    }

    internal void SubscribedEventsAvailableCallback(object state, bool timedOut)
    {
      this.callbackThreadId = Thread.CurrentThread.ManagedThreadId;
      try
      {
        this.RequestEvents();
      }
      finally
      {
        this.callbackThreadId = -1;
      }
    }

    [SecuritySafeCritical]
    private void RequestEvents()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      this.asyncException = (EventLogException) null;
      bool flag = false;
      while (this.isSubscribing)
      {
        try
        {
          flag = NativeWrapper.EvtNext(this.handle, this.eventsBuffer.Length, this.eventsBuffer, 0, 0, ref this.numEventsInBuffer);
          if (!flag)
            break;
        }
        catch (Exception ex)
        {
          this.asyncException = new EventLogException();
          this.asyncException.Data.Add((object) "RealException", (object) ex);
        }
        this.HandleEventsRequestCompletion();
        if (!flag)
          break;
      }
    }

    private void IssueCallback(EventRecordWrittenEventArgs eventArgs)
    {
      if (this.EventRecordWritten == null)
        return;
      this.EventRecordWritten((object) this, eventArgs);
    }

    [SecurityCritical]
    private void HandleEventsRequestCompletion()
    {
      if (this.asyncException != null)
        this.IssueCallback(new EventRecordWrittenEventArgs(this.asyncException.Data[(object) "RealException"] as Exception));
      for (int index = 0; index < this.numEventsInBuffer && this.isSubscribing; ++index)
      {
        EventRecordWrittenEventArgs eventArgs = new EventRecordWrittenEventArgs(new EventLogRecord(new EventLogHandle(this.eventsBuffer[index], true), this.eventQuery.Session, this.cachedMetadataInformation));
        this.eventsBuffer[index] = IntPtr.Zero;
        this.IssueCallback(eventArgs);
      }
    }
  }
}
