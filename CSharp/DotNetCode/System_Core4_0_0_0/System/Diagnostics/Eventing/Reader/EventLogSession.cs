// Type: System.Diagnostics.Eventing.Reader.EventLogSession
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogSession : IDisposable
  {
    private static EventLogSession globalSession = new EventLogSession();
    internal EventLogHandle renderContextHandleSystem = EventLogHandle.Zero;
    internal EventLogHandle renderContextHandleUser = EventLogHandle.Zero;
    private EventLogHandle handle = EventLogHandle.Zero;
    private object syncObject;
    private string server;
    private string user;
    private string domain;
    private SessionAuthentication logOnType;

    internal EventLogHandle Handle
    {
      get
      {
        return this.handle;
      }
    }

    public static EventLogSession GlobalSession
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return EventLogSession.globalSession;
      }
    }

    static EventLogSession()
    {
    }

    [SecurityCritical]
    public EventLogSession()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      this.syncObject = new object();
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EventLogSession(string server)
      : this(server, (string) null, (string) null, (SecureString) null, SessionAuthentication.Default)
    {
    }

    [SecurityCritical]
    public EventLogSession(string server, string domain, string user, SecureString password, SessionAuthentication logOnType)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (server == null)
        server = "localhost";
      this.syncObject = new object();
      this.server = server;
      this.domain = domain;
      this.user = user;
      this.logOnType = logOnType;
      Microsoft.Win32.UnsafeNativeMethods.EvtRpcLogin login = new Microsoft.Win32.UnsafeNativeMethods.EvtRpcLogin();
      login.Server = this.server;
      login.User = this.user;
      login.Domain = this.domain;
      login.Flags = (int) this.logOnType;
      login.Password = CoTaskMemUnicodeSafeHandle.Zero;
      try
      {
        if (password != null)
          login.Password.SetMemory(Marshal.SecureStringToCoTaskMemUnicode(password));
        this.handle = NativeWrapper.EvtOpenSession(Microsoft.Win32.UnsafeNativeMethods.EvtLoginClass.EvtRpcLogin, ref login, 0, 0);
      }
      finally
      {
        login.Password.Close();
      }
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
        if (this == EventLogSession.globalSession)
          throw new InvalidOperationException();
        EventLogPermissionHolder.GetEventLogPermission().Demand();
      }
      if (this.renderContextHandleSystem != null && !this.renderContextHandleSystem.IsInvalid)
        this.renderContextHandleSystem.Dispose();
      if (this.renderContextHandleUser != null && !this.renderContextHandleUser.IsInvalid)
        this.renderContextHandleUser.Dispose();
      if (this.handle == null || this.handle.IsInvalid)
        return;
      this.handle.Dispose();
    }

    public void CancelCurrentOperations()
    {
      NativeWrapper.EvtCancel(this.handle);
    }

    [SecurityCritical]
    public IEnumerable<string> GetProviderNames()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      List<string> list = new List<string>(100);
      using (EventLogHandle handle = NativeWrapper.EvtOpenProviderEnum(this.Handle, 0))
      {
        bool finish = false;
        do
        {
          string str = NativeWrapper.EvtNextPublisherId(handle, ref finish);
          if (!finish)
            list.Add(str);
        }
        while (!finish);
        return (IEnumerable<string>) list;
      }
    }

    [SecurityCritical]
    public IEnumerable<string> GetLogNames()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      List<string> list = new List<string>(100);
      using (EventLogHandle handle = NativeWrapper.EvtOpenChannelEnum(this.Handle, 0))
      {
        bool finish = false;
        do
        {
          string str = NativeWrapper.EvtNextChannelPath(handle, ref finish);
          if (!finish)
            list.Add(str);
        }
        while (!finish);
        return (IEnumerable<string>) list;
      }
    }

    public EventLogInformation GetLogInformation(string logName, PathType pathType)
    {
      if (logName == null)
        throw new ArgumentNullException("logName");
      else
        return new EventLogInformation(this, logName, pathType);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void ExportLog(string path, PathType pathType, string query, string targetFilePath)
    {
      this.ExportLog(path, pathType, query, targetFilePath, false);
    }

    public void ExportLog(string path, PathType pathType, string query, string targetFilePath, bool tolerateQueryErrors)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (targetFilePath == null)
        throw new ArgumentNullException("targetFilePath");
      Microsoft.Win32.UnsafeNativeMethods.EvtExportLogFlags evtExportLogFlags;
      switch (pathType)
      {
        case PathType.LogName:
          evtExportLogFlags = Microsoft.Win32.UnsafeNativeMethods.EvtExportLogFlags.EvtExportLogChannelPath;
          break;
        case PathType.FilePath:
          evtExportLogFlags = Microsoft.Win32.UnsafeNativeMethods.EvtExportLogFlags.EvtExportLogFilePath;
          break;
        default:
          throw new ArgumentOutOfRangeException("pathType");
      }
      if (!tolerateQueryErrors)
        NativeWrapper.EvtExportLog(this.Handle, path, query, targetFilePath, (int) evtExportLogFlags);
      else
        NativeWrapper.EvtExportLog(this.Handle, path, query, targetFilePath, (int) (evtExportLogFlags | Microsoft.Win32.UnsafeNativeMethods.EvtExportLogFlags.EvtExportLogTolerateQueryErrors));
    }

    public void ExportLogAndMessages(string path, PathType pathType, string query, string targetFilePath)
    {
      this.ExportLogAndMessages(path, pathType, query, targetFilePath, false, CultureInfo.CurrentCulture);
    }

    public void ExportLogAndMessages(string path, PathType pathType, string query, string targetFilePath, bool tolerateQueryErrors, CultureInfo targetCultureInfo)
    {
      if (targetCultureInfo == null)
        targetCultureInfo = CultureInfo.CurrentCulture;
      this.ExportLog(path, pathType, query, targetFilePath, tolerateQueryErrors);
      NativeWrapper.EvtArchiveExportedLog(this.Handle, targetFilePath, targetCultureInfo.LCID, 0);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void ClearLog(string logName)
    {
      this.ClearLog(logName, (string) null);
    }

    public void ClearLog(string logName, string backupPath)
    {
      if (logName == null)
        throw new ArgumentNullException("logName");
      NativeWrapper.EvtClearLog(this.Handle, logName, backupPath, 0);
    }

    [SecuritySafeCritical]
    internal void SetupSystemContext()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (!this.renderContextHandleSystem.IsInvalid)
        return;
      lock (this.syncObject)
      {
        if (!this.renderContextHandleSystem.IsInvalid)
          return;
        this.renderContextHandleSystem = NativeWrapper.EvtCreateRenderContext(0, (string[]) null, Microsoft.Win32.UnsafeNativeMethods.EvtRenderContextFlags.EvtRenderContextSystem);
      }
    }

    [SecuritySafeCritical]
    internal void SetupUserContext()
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      lock (this.syncObject)
      {
        if (!this.renderContextHandleUser.IsInvalid)
          return;
        this.renderContextHandleUser = NativeWrapper.EvtCreateRenderContext(0, (string[]) null, Microsoft.Win32.UnsafeNativeMethods.EvtRenderContextFlags.EvtRenderContextUser);
      }
    }
  }
}
