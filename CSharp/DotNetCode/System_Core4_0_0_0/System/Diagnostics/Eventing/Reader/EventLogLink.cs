// Type: System.Diagnostics.Eventing.Reader.EventLogLink
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Security.Permissions;

namespace System.Diagnostics.Eventing.Reader
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class EventLogLink
  {
    private string channelName;
    private bool isImported;
    private string displayName;
    private uint channelId;
    private bool dataReady;
    private ProviderMetadata pmReference;
    private object syncObject;

    public string LogName
    {
      get
      {
        this.PrepareData();
        return this.channelName;
      }
    }

    public bool IsImported
    {
      get
      {
        this.PrepareData();
        return this.isImported;
      }
    }

    public string DisplayName
    {
      get
      {
        this.PrepareData();
        return this.displayName;
      }
    }

    internal uint ChannelId
    {
      get
      {
        return this.channelId;
      }
    }

    internal EventLogLink(uint channelId, ProviderMetadata pmReference)
    {
      this.channelId = channelId;
      this.pmReference = pmReference;
      this.syncObject = new object();
    }

    internal EventLogLink(string channelName, bool isImported, string displayName, uint channelId)
    {
      this.channelName = channelName;
      this.isImported = isImported;
      this.displayName = displayName;
      this.channelId = channelId;
      this.dataReady = true;
      this.syncObject = new object();
    }

    private void PrepareData()
    {
      if (this.dataReady)
        return;
      lock (this.syncObject)
      {
        if (this.dataReady)
          return;
        IEnumerable<EventLogLink> local_0 = (IEnumerable<EventLogLink>) this.pmReference.LogLinks;
        this.channelName = (string) null;
        this.isImported = false;
        this.displayName = (string) null;
        this.dataReady = true;
        foreach (EventLogLink item_0 in local_0)
        {
          if ((int) item_0.ChannelId == (int) this.channelId)
          {
            this.channelName = item_0.LogName;
            this.isImported = item_0.IsImported;
            this.displayName = item_0.DisplayName;
            this.dataReady = true;
            break;
          }
        }
      }
    }
  }
}
