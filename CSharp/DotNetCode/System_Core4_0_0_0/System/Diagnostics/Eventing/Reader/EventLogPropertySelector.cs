// Type: System.Diagnostics.Eventing.Reader.EventLogPropertySelector
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class EventLogPropertySelector : IDisposable
  {
    private EventLogHandle renderContextHandleValues;

    internal EventLogHandle Handle
    {
      get
      {
        return this.renderContextHandleValues;
      }
    }

    [SecurityCritical]
    public EventLogPropertySelector(IEnumerable<string> propertyQueries)
    {
      EventLogPermissionHolder.GetEventLogPermission().Demand();
      if (propertyQueries == null)
        throw new ArgumentNullException("propertyQueries");
      ICollection<string> collection = propertyQueries as ICollection<string>;
      string[] strArray;
      if (collection != null)
      {
        strArray = new string[collection.Count];
        collection.CopyTo(strArray, 0);
      }
      else
        strArray = new List<string>(propertyQueries).ToArray();
      this.renderContextHandleValues = NativeWrapper.EvtCreateRenderContext(strArray.Length, strArray, Microsoft.Win32.UnsafeNativeMethods.EvtRenderContextFlags.EvtRenderContextValues);
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
      if (this.renderContextHandleValues == null || this.renderContextHandleValues.IsInvalid)
        return;
      this.renderContextHandleValues.Dispose();
    }
  }
}
