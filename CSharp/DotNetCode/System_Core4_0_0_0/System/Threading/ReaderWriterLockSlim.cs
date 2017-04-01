// Type: System.Threading.ReaderWriterLockSlim
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace System.Threading
{
  [__DynamicallyInvokable]
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
  public class ReaderWriterLockSlim : IDisposable
  {
    private bool fIsReentrant;
    private int myLock;
    private uint numWriteWaiters;
    private uint numReadWaiters;
    private uint numWriteUpgradeWaiters;
    private uint numUpgradeWaiters;
    private bool fNoWaiters;
    private int upgradeLockOwnerId;
    private int writeLockOwnerId;
    private EventWaitHandle writeEvent;
    private EventWaitHandle readEvent;
    private EventWaitHandle upgradeEvent;
    private EventWaitHandle waitUpgradeEvent;
    private static long s_nextLockID;
    private long lockID;
    [ThreadStatic]
    private static ReaderWriterCount t_rwc;
    private bool fUpgradeThreadHoldingRead;
    private uint owners;
    private bool fDisposed;
    private const int LockSpinCycles = 20;
    private const int LockSpinCount = 10;
    private const int LockSleep0Count = 5;
    private const int MaxSpinCount = 20;
    private const uint WRITER_HELD = 2147483648U;
    private const uint WAITING_WRITERS = 1073741824U;
    private const uint WAITING_UPGRADER = 536870912U;
    private const uint MAX_READER = 268435454U;
    private const uint READER_MASK = 268435455U;

    [__DynamicallyInvokable]
    public bool IsReadLockHeld
    {
      [__DynamicallyInvokable] get
      {
        return this.RecursiveReadCount > 0;
      }
    }

    [__DynamicallyInvokable]
    public bool IsUpgradeableReadLockHeld
    {
      [__DynamicallyInvokable] get
      {
        return this.RecursiveUpgradeCount > 0;
      }
    }

    [__DynamicallyInvokable]
    public bool IsWriteLockHeld
    {
      [__DynamicallyInvokable] get
      {
        return this.RecursiveWriteCount > 0;
      }
    }

    [__DynamicallyInvokable]
    public LockRecursionPolicy RecursionPolicy
    {
      [__DynamicallyInvokable] get
      {
        return this.fIsReentrant ? LockRecursionPolicy.SupportsRecursion : LockRecursionPolicy.NoRecursion;
      }
    }

    [__DynamicallyInvokable]
    public int CurrentReadCount
    {
      [__DynamicallyInvokable] get
      {
        int num = (int) this.GetNumReaders();
        if (this.upgradeLockOwnerId != -1)
          return num - 1;
        else
          return num;
      }
    }

    [__DynamicallyInvokable]
    public int RecursiveReadCount
    {
      [__DynamicallyInvokable] get
      {
        int num = 0;
        ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount != null)
          num = threadRwCount.readercount;
        return num;
      }
    }

    [__DynamicallyInvokable]
    public int RecursiveUpgradeCount
    {
      [__DynamicallyInvokable] get
      {
        if (this.fIsReentrant)
        {
          int num = 0;
          ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
          if (threadRwCount != null)
            num = threadRwCount.upgradecount;
          return num;
        }
        else
          return Thread.CurrentThread.ManagedThreadId == this.upgradeLockOwnerId ? 1 : 0;
      }
    }

    [__DynamicallyInvokable]
    public int RecursiveWriteCount
    {
      [__DynamicallyInvokable] get
      {
        if (this.fIsReentrant)
        {
          int num = 0;
          ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
          if (threadRwCount != null)
            num = threadRwCount.writercount;
          return num;
        }
        else
          return Thread.CurrentThread.ManagedThreadId == this.writeLockOwnerId ? 1 : 0;
      }
    }

    [__DynamicallyInvokable]
    public int WaitingReadCount
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return (int) this.numReadWaiters;
      }
    }

    [__DynamicallyInvokable]
    public int WaitingUpgradeCount
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return (int) this.numUpgradeWaiters;
      }
    }

    [__DynamicallyInvokable]
    public int WaitingWriteCount
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return (int) this.numWriteWaiters;
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ReaderWriterLockSlim()
      : this(LockRecursionPolicy.NoRecursion)
    {
    }

    [__DynamicallyInvokable]
    public ReaderWriterLockSlim(LockRecursionPolicy recursionPolicy)
    {
      if (recursionPolicy == LockRecursionPolicy.SupportsRecursion)
        this.fIsReentrant = true;
      this.InitializeThreadCounts();
      this.fNoWaiters = true;
      this.lockID = Interlocked.Increment(ref ReaderWriterLockSlim.s_nextLockID);
    }

    [__DynamicallyInvokable]
    public void EnterReadLock()
    {
      this.TryEnterReadLock(-1);
    }

    [__DynamicallyInvokable]
    public bool TryEnterReadLock(TimeSpan timeout)
    {
      return this.TryEnterReadLock(new ReaderWriterLockSlim.TimeoutTracker(timeout));
    }

    [__DynamicallyInvokable]
    public bool TryEnterReadLock(int millisecondsTimeout)
    {
      return this.TryEnterReadLock(new ReaderWriterLockSlim.TimeoutTracker(millisecondsTimeout));
    }

    [__DynamicallyInvokable]
    public void EnterWriteLock()
    {
      this.TryEnterWriteLock(-1);
    }

    [__DynamicallyInvokable]
    public bool TryEnterWriteLock(TimeSpan timeout)
    {
      return this.TryEnterWriteLock(new ReaderWriterLockSlim.TimeoutTracker(timeout));
    }

    [__DynamicallyInvokable]
    public bool TryEnterWriteLock(int millisecondsTimeout)
    {
      return this.TryEnterWriteLock(new ReaderWriterLockSlim.TimeoutTracker(millisecondsTimeout));
    }

    [__DynamicallyInvokable]
    public void EnterUpgradeableReadLock()
    {
      this.TryEnterUpgradeableReadLock(-1);
    }

    [__DynamicallyInvokable]
    public bool TryEnterUpgradeableReadLock(TimeSpan timeout)
    {
      return this.TryEnterUpgradeableReadLock(new ReaderWriterLockSlim.TimeoutTracker(timeout));
    }

    [__DynamicallyInvokable]
    public bool TryEnterUpgradeableReadLock(int millisecondsTimeout)
    {
      return this.TryEnterUpgradeableReadLock(new ReaderWriterLockSlim.TimeoutTracker(millisecondsTimeout));
    }

    [__DynamicallyInvokable]
    public void ExitReadLock()
    {
      this.EnterMyLock();
      ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
      if (threadRwCount == null || threadRwCount.readercount < 1)
      {
        this.ExitMyLock();
        throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedRead"));
      }
      else
      {
        if (this.fIsReentrant)
        {
          if (threadRwCount.readercount > 1)
          {
            --threadRwCount.readercount;
            this.ExitMyLock();
            Thread.EndCriticalRegion();
            return;
          }
          else if (Thread.CurrentThread.ManagedThreadId == this.upgradeLockOwnerId)
            this.fUpgradeThreadHoldingRead = false;
        }
        --this.owners;
        --threadRwCount.readercount;
        this.ExitAndWakeUpAppropriateWaiters();
        Thread.EndCriticalRegion();
      }
    }

    [__DynamicallyInvokable]
    public void ExitWriteLock()
    {
      if (!this.fIsReentrant)
      {
        if (Thread.CurrentThread.ManagedThreadId != this.writeLockOwnerId)
          throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedWrite"));
        this.EnterMyLock();
      }
      else
      {
        this.EnterMyLock();
        ReaderWriterCount threadRwCount = this.GetThreadRWCount(false);
        if (threadRwCount == null)
        {
          this.ExitMyLock();
          throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedWrite"));
        }
        else if (threadRwCount.writercount < 1)
        {
          this.ExitMyLock();
          throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedWrite"));
        }
        else
        {
          --threadRwCount.writercount;
          if (threadRwCount.writercount > 0)
          {
            this.ExitMyLock();
            Thread.EndCriticalRegion();
            return;
          }
        }
      }
      this.ClearWriterAcquired();
      this.writeLockOwnerId = -1;
      this.ExitAndWakeUpAppropriateWaiters();
      Thread.EndCriticalRegion();
    }

    [__DynamicallyInvokable]
    public void ExitUpgradeableReadLock()
    {
      if (!this.fIsReentrant)
      {
        if (Thread.CurrentThread.ManagedThreadId != this.upgradeLockOwnerId)
          throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedUpgrade"));
        this.EnterMyLock();
      }
      else
      {
        this.EnterMyLock();
        ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount == null)
        {
          this.ExitMyLock();
          throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedUpgrade"));
        }
        else if (threadRwCount.upgradecount < 1)
        {
          this.ExitMyLock();
          throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_MisMatchedUpgrade"));
        }
        else
        {
          --threadRwCount.upgradecount;
          if (threadRwCount.upgradecount > 0)
          {
            this.ExitMyLock();
            Thread.EndCriticalRegion();
            return;
          }
          else
            this.fUpgradeThreadHoldingRead = false;
        }
      }
      --this.owners;
      this.upgradeLockOwnerId = -1;
      this.ExitAndWakeUpAppropriateWaiters();
      Thread.EndCriticalRegion();
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Dispose()
    {
      this.Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      if (!disposing || this.fDisposed)
        return;
      if (this.WaitingReadCount > 0 || this.WaitingUpgradeCount > 0 || this.WaitingWriteCount > 0)
        throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_IncorrectDispose"));
      if (this.IsReadLockHeld || this.IsUpgradeableReadLockHeld || this.IsWriteLockHeld)
        throw new SynchronizationLockException(SR.GetString("SynchronizationLockException_IncorrectDispose"));
      if (this.writeEvent != null)
      {
        this.writeEvent.Close();
        this.writeEvent = (EventWaitHandle) null;
      }
      if (this.readEvent != null)
      {
        this.readEvent.Close();
        this.readEvent = (EventWaitHandle) null;
      }
      if (this.upgradeEvent != null)
      {
        this.upgradeEvent.Close();
        this.upgradeEvent = (EventWaitHandle) null;
      }
      if (this.waitUpgradeEvent != null)
      {
        this.waitUpgradeEvent.Close();
        this.waitUpgradeEvent = (EventWaitHandle) null;
      }
      this.fDisposed = true;
    }

    private void InitializeThreadCounts()
    {
      this.upgradeLockOwnerId = -1;
      this.writeLockOwnerId = -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsRWEntryEmpty(ReaderWriterCount rwc)
    {
      return rwc.lockID == 0L || rwc.readercount == 0 && rwc.writercount == 0 && rwc.upgradecount == 0;
    }

    private bool IsRwHashEntryChanged(ReaderWriterCount lrwc)
    {
      return lrwc.lockID != this.lockID;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReaderWriterCount GetThreadRWCount(bool dontAllocate)
    {
      ReaderWriterCount rwc = ReaderWriterLockSlim.t_rwc;
      ReaderWriterCount readerWriterCount = (ReaderWriterCount) null;
      for (; rwc != null; rwc = rwc.next)
      {
        if (rwc.lockID == this.lockID)
          return rwc;
        if (!dontAllocate && readerWriterCount == null && ReaderWriterLockSlim.IsRWEntryEmpty(rwc))
          readerWriterCount = rwc;
      }
      if (dontAllocate)
        return (ReaderWriterCount) null;
      if (readerWriterCount == null)
      {
        readerWriterCount = new ReaderWriterCount();
        readerWriterCount.next = ReaderWriterLockSlim.t_rwc;
        ReaderWriterLockSlim.t_rwc = readerWriterCount;
      }
      readerWriterCount.lockID = this.lockID;
      return readerWriterCount;
    }

    private bool TryEnterReadLock(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      Thread.BeginCriticalRegion();
      bool flag = false;
      try
      {
        flag = this.TryEnterReadLockCore(timeout);
        return flag;
      }
      finally
      {
        if (!flag)
          Thread.EndCriticalRegion();
      }
    }

    private bool TryEnterReadLockCore(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      if (this.fDisposed)
        throw new ObjectDisposedException((string) null);
      int managedThreadId = Thread.CurrentThread.ManagedThreadId;
      ReaderWriterCount threadRwCount;
      if (!this.fIsReentrant)
      {
        if (managedThreadId == this.writeLockOwnerId)
          throw new LockRecursionException(SR.GetString("LockRecursionException_ReadAfterWriteNotAllowed"));
        this.EnterMyLock();
        threadRwCount = this.GetThreadRWCount(false);
        if (threadRwCount.readercount > 0)
        {
          this.ExitMyLock();
          throw new LockRecursionException(SR.GetString("LockRecursionException_RecursiveReadNotAllowed"));
        }
        else if (managedThreadId == this.upgradeLockOwnerId)
        {
          ++threadRwCount.readercount;
          ++this.owners;
          this.ExitMyLock();
          return true;
        }
      }
      else
      {
        this.EnterMyLock();
        threadRwCount = this.GetThreadRWCount(false);
        if (threadRwCount.readercount > 0)
        {
          ++threadRwCount.readercount;
          this.ExitMyLock();
          return true;
        }
        else if (managedThreadId == this.upgradeLockOwnerId)
        {
          ++threadRwCount.readercount;
          ++this.owners;
          this.ExitMyLock();
          this.fUpgradeThreadHoldingRead = true;
          return true;
        }
        else if (managedThreadId == this.writeLockOwnerId)
        {
          ++threadRwCount.readercount;
          ++this.owners;
          this.ExitMyLock();
          return true;
        }
      }
      bool flag = true;
      int SpinCount = 0;
      while (this.owners >= 268435454U)
      {
        if (SpinCount < 20)
        {
          this.ExitMyLock();
          if (timeout.IsExpired)
            return false;
          ++SpinCount;
          ReaderWriterLockSlim.SpinWait(SpinCount);
          this.EnterMyLock();
          if (this.IsRwHashEntryChanged(threadRwCount))
            threadRwCount = this.GetThreadRWCount(false);
        }
        else if (this.readEvent == null)
        {
          this.LazyCreateEvent(ref this.readEvent, false);
          if (this.IsRwHashEntryChanged(threadRwCount))
            threadRwCount = this.GetThreadRWCount(false);
        }
        else
        {
          flag = this.WaitOnEvent(this.readEvent, ref this.numReadWaiters, timeout);
          if (!flag)
            return false;
          if (this.IsRwHashEntryChanged(threadRwCount))
            threadRwCount = this.GetThreadRWCount(false);
        }
      }
      ++this.owners;
      ++threadRwCount.readercount;
      this.ExitMyLock();
      return flag;
    }

    private bool TryEnterWriteLock(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      Thread.BeginCriticalRegion();
      bool flag = false;
      try
      {
        flag = this.TryEnterWriteLockCore(timeout);
        return flag;
      }
      finally
      {
        if (!flag)
          Thread.EndCriticalRegion();
      }
    }

    private bool TryEnterWriteLockCore(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      if (this.fDisposed)
        throw new ObjectDisposedException((string) null);
      int managedThreadId = Thread.CurrentThread.ManagedThreadId;
      bool flag = false;
      ReaderWriterCount threadRwCount;
      if (!this.fIsReentrant)
      {
        if (managedThreadId == this.writeLockOwnerId)
          throw new LockRecursionException(SR.GetString("LockRecursionException_RecursiveWriteNotAllowed"));
        if (managedThreadId == this.upgradeLockOwnerId)
          flag = true;
        this.EnterMyLock();
        threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount != null && threadRwCount.readercount > 0)
        {
          this.ExitMyLock();
          throw new LockRecursionException(SR.GetString("LockRecursionException_WriteAfterReadNotAllowed"));
        }
      }
      else
      {
        this.EnterMyLock();
        threadRwCount = this.GetThreadRWCount(false);
        if (managedThreadId == this.writeLockOwnerId)
        {
          ++threadRwCount.writercount;
          this.ExitMyLock();
          return true;
        }
        else if (managedThreadId == this.upgradeLockOwnerId)
          flag = true;
        else if (threadRwCount.readercount > 0)
        {
          this.ExitMyLock();
          throw new LockRecursionException(SR.GetString("LockRecursionException_WriteAfterReadNotAllowed"));
        }
      }
      int SpinCount = 0;
      while (!this.IsWriterAcquired())
      {
        if (flag)
        {
          switch (this.GetNumReaders())
          {
            case 1U:
              this.SetWriterAcquired();
              goto label_39;
            case 2U:
              if (threadRwCount != null)
              {
                if (this.IsRwHashEntryChanged(threadRwCount))
                  threadRwCount = this.GetThreadRWCount(false);
                if (threadRwCount.readercount > 0)
                {
                  this.SetWriterAcquired();
                  goto label_39;
                }
                else
                  break;
              }
              else
                break;
          }
        }
        if (SpinCount < 20)
        {
          this.ExitMyLock();
          if (timeout.IsExpired)
            return false;
          ++SpinCount;
          ReaderWriterLockSlim.SpinWait(SpinCount);
          this.EnterMyLock();
        }
        else if (flag)
        {
          if (this.waitUpgradeEvent == null)
            this.LazyCreateEvent(ref this.waitUpgradeEvent, true);
          else if (!this.WaitOnEvent(this.waitUpgradeEvent, ref this.numWriteUpgradeWaiters, timeout))
            return false;
        }
        else if (this.writeEvent == null)
          this.LazyCreateEvent(ref this.writeEvent, true);
        else if (!this.WaitOnEvent(this.writeEvent, ref this.numWriteWaiters, timeout))
          return false;
      }
      this.SetWriterAcquired();
label_39:
      if (this.fIsReentrant)
      {
        if (this.IsRwHashEntryChanged(threadRwCount))
          threadRwCount = this.GetThreadRWCount(false);
        ++threadRwCount.writercount;
      }
      this.ExitMyLock();
      this.writeLockOwnerId = managedThreadId;
      return true;
    }

    private bool TryEnterUpgradeableReadLock(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      Thread.BeginCriticalRegion();
      bool flag = false;
      try
      {
        flag = this.TryEnterUpgradeableReadLockCore(timeout);
        return flag;
      }
      finally
      {
        if (!flag)
          Thread.EndCriticalRegion();
      }
    }

    private bool TryEnterUpgradeableReadLockCore(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      if (this.fDisposed)
        throw new ObjectDisposedException((string) null);
      int managedThreadId = Thread.CurrentThread.ManagedThreadId;
      ReaderWriterCount threadRwCount;
      if (!this.fIsReentrant)
      {
        if (managedThreadId == this.upgradeLockOwnerId)
          throw new LockRecursionException(SR.GetString("LockRecursionException_RecursiveUpgradeNotAllowed"));
        if (managedThreadId == this.writeLockOwnerId)
          throw new LockRecursionException(SR.GetString("LockRecursionException_UpgradeAfterWriteNotAllowed"));
        this.EnterMyLock();
        threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount != null && threadRwCount.readercount > 0)
        {
          this.ExitMyLock();
          throw new LockRecursionException(SR.GetString("LockRecursionException_UpgradeAfterReadNotAllowed"));
        }
      }
      else
      {
        this.EnterMyLock();
        threadRwCount = this.GetThreadRWCount(false);
        if (managedThreadId == this.upgradeLockOwnerId)
        {
          ++threadRwCount.upgradecount;
          this.ExitMyLock();
          return true;
        }
        else if (managedThreadId == this.writeLockOwnerId)
        {
          ++this.owners;
          this.upgradeLockOwnerId = managedThreadId;
          ++threadRwCount.upgradecount;
          if (threadRwCount.readercount > 0)
            this.fUpgradeThreadHoldingRead = true;
          this.ExitMyLock();
          return true;
        }
        else if (threadRwCount.readercount > 0)
        {
          this.ExitMyLock();
          throw new LockRecursionException(SR.GetString("LockRecursionException_UpgradeAfterReadNotAllowed"));
        }
      }
      int SpinCount = 0;
      while (this.upgradeLockOwnerId != -1 || this.owners >= 268435454U)
      {
        if (SpinCount < 20)
        {
          this.ExitMyLock();
          if (timeout.IsExpired)
            return false;
          ++SpinCount;
          ReaderWriterLockSlim.SpinWait(SpinCount);
          this.EnterMyLock();
        }
        else if (this.upgradeEvent == null)
          this.LazyCreateEvent(ref this.upgradeEvent, true);
        else if (!this.WaitOnEvent(this.upgradeEvent, ref this.numUpgradeWaiters, timeout))
          return false;
      }
      ++this.owners;
      this.upgradeLockOwnerId = managedThreadId;
      if (this.fIsReentrant)
      {
        if (this.IsRwHashEntryChanged(threadRwCount))
          threadRwCount = this.GetThreadRWCount(false);
        ++threadRwCount.upgradecount;
      }
      this.ExitMyLock();
      return true;
    }

    private void LazyCreateEvent(ref EventWaitHandle waitEvent, bool makeAutoResetEvent)
    {
      this.ExitMyLock();
      EventWaitHandle eventWaitHandle = !makeAutoResetEvent ? (EventWaitHandle) new ManualResetEvent(false) : (EventWaitHandle) new AutoResetEvent(false);
      this.EnterMyLock();
      if (waitEvent == null)
        waitEvent = eventWaitHandle;
      else
        eventWaitHandle.Close();
    }

    private bool WaitOnEvent(EventWaitHandle waitEvent, ref uint numWaiters, ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      waitEvent.Reset();
      ++numWaiters;
      this.fNoWaiters = false;
      if ((int) this.numWriteWaiters == 1)
        this.SetWritersWaiting();
      if ((int) this.numWriteUpgradeWaiters == 1)
        this.SetUpgraderWaiting();
      bool flag = false;
      this.ExitMyLock();
      try
      {
        flag = waitEvent.WaitOne(timeout.RemainingMilliseconds);
        return flag;
      }
      finally
      {
        this.EnterMyLock();
        --numWaiters;
        if ((int) this.numWriteWaiters == 0 && (int) this.numWriteUpgradeWaiters == 0 && ((int) this.numUpgradeWaiters == 0 && (int) this.numReadWaiters == 0))
          this.fNoWaiters = true;
        if ((int) this.numWriteWaiters == 0)
          this.ClearWritersWaiting();
        if ((int) this.numWriteUpgradeWaiters == 0)
          this.ClearUpgraderWaiting();
        if (!flag)
          this.ExitMyLock();
      }
    }

    private void ExitAndWakeUpAppropriateWaiters()
    {
      if (this.fNoWaiters)
        this.ExitMyLock();
      else
        this.ExitAndWakeUpAppropriateWaitersPreferringWriters();
    }

    private void ExitAndWakeUpAppropriateWaitersPreferringWriters()
    {
      bool flag1 = false;
      bool flag2 = false;
      uint numReaders = this.GetNumReaders();
      if (this.fIsReentrant && this.numWriteUpgradeWaiters > 0U && (this.fUpgradeThreadHoldingRead && (int) numReaders == 2))
      {
        this.ExitMyLock();
        this.waitUpgradeEvent.Set();
      }
      else if ((int) numReaders == 1 && this.numWriteUpgradeWaiters > 0U)
      {
        this.ExitMyLock();
        this.waitUpgradeEvent.Set();
      }
      else if ((int) numReaders == 0 && this.numWriteWaiters > 0U)
      {
        this.ExitMyLock();
        this.writeEvent.Set();
      }
      else if (numReaders >= 0U)
      {
        if ((int) this.numReadWaiters != 0 || (int) this.numUpgradeWaiters != 0)
        {
          if ((int) this.numReadWaiters != 0)
            flag2 = true;
          if ((int) this.numUpgradeWaiters != 0 && this.upgradeLockOwnerId == -1)
            flag1 = true;
          this.ExitMyLock();
          if (flag2)
            this.readEvent.Set();
          if (!flag1)
            return;
          this.upgradeEvent.Set();
        }
        else
          this.ExitMyLock();
      }
      else
        this.ExitMyLock();
    }

    private bool IsWriterAcquired()
    {
      return ((int) this.owners & -1073741825) == 0;
    }

    private void SetWriterAcquired()
    {
      this.owners |= (uint) int.MinValue;
    }

    private void ClearWriterAcquired()
    {
      this.owners &= (uint) int.MaxValue;
    }

    private void SetWritersWaiting()
    {
      this.owners |= 1073741824U;
    }

    private void ClearWritersWaiting()
    {
      this.owners &= 3221225471U;
    }

    private void SetUpgraderWaiting()
    {
      this.owners |= 536870912U;
    }

    private void ClearUpgraderWaiting()
    {
      this.owners &= 3758096383U;
    }

    private uint GetNumReaders()
    {
      return this.owners & 268435455U;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnterMyLock()
    {
      if (Interlocked.CompareExchange(ref this.myLock, 1, 0) == 0)
        return;
      this.EnterMyLockSpin();
    }

    private void EnterMyLockSpin()
    {
      int processorCount = Environment.ProcessorCount;
      int num = 0;
      while (true)
      {
        if (num < 10 && processorCount > 1)
          Thread.SpinWait(20 * (num + 1));
        else if (num < 15)
          Thread.Sleep(0);
        else
          Thread.Sleep(1);
        if (this.myLock != 0 || Interlocked.CompareExchange(ref this.myLock, 1, 0) != 0)
          ++num;
        else
          break;
      }
    }

    private void ExitMyLock()
    {
      Volatile.Write(ref this.myLock, 0);
    }

    private static void SpinWait(int SpinCount)
    {
      if (SpinCount < 5 && Environment.ProcessorCount > 1)
        Thread.SpinWait(20 * SpinCount);
      else if (SpinCount < 17)
        Thread.Sleep(0);
      else
        Thread.Sleep(1);
    }

    private struct TimeoutTracker
    {
      private int m_total;
      private int m_start;

      public int RemainingMilliseconds
      {
        get
        {
          if (this.m_total == -1 || this.m_total == 0)
            return this.m_total;
          int num = Environment.TickCount - this.m_start;
          if (num < 0 || num >= this.m_total)
            return 0;
          else
            return this.m_total - num;
        }
      }

      public bool IsExpired
      {
        get
        {
          return this.RemainingMilliseconds == 0;
        }
      }

      public TimeoutTracker(TimeSpan timeout)
      {
        long num = (long) timeout.TotalMilliseconds;
        if (num < -1L || num > (long) int.MaxValue)
          throw new ArgumentOutOfRangeException("timeout");
        this.m_total = (int) num;
        if (this.m_total != -1 && this.m_total != 0)
          this.m_start = Environment.TickCount;
        else
          this.m_start = 0;
      }

      public TimeoutTracker(int millisecondsTimeout)
      {
        if (millisecondsTimeout < -1)
          throw new ArgumentOutOfRangeException("millisecondsTimeout");
        this.m_total = millisecondsTimeout;
        if (this.m_total != -1 && this.m_total != 0)
          this.m_start = Environment.TickCount;
        else
          this.m_start = 0;
      }
    }
  }
}
