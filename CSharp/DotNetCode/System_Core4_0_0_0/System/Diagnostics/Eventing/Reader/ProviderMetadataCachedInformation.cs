// Type: System.Diagnostics.Eventing.Reader.ProviderMetadataCachedInformation
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime;
using System.Security;

namespace System.Diagnostics.Eventing.Reader
{
  internal class ProviderMetadataCachedInformation
  {
    private Dictionary<ProviderMetadataCachedInformation.ProviderMetadataId, ProviderMetadataCachedInformation.CacheItem> cache;
    private int maximumCacheSize;
    private EventLogSession session;
    private string logfile;

    public ProviderMetadataCachedInformation(EventLogSession session, string logfile, int maximumCacheSize)
    {
      this.session = session;
      this.logfile = logfile;
      this.cache = new Dictionary<ProviderMetadataCachedInformation.ProviderMetadataId, ProviderMetadataCachedInformation.CacheItem>();
      this.maximumCacheSize = maximumCacheSize;
    }

    [SecuritySafeCritical]
    public string GetFormatDescription(string ProviderName, EventLogHandle eventHandle)
    {
      lock (this)
      {
        ProviderMetadataCachedInformation.ProviderMetadataId local_0 = new ProviderMetadataCachedInformation.ProviderMetadataId(ProviderName, CultureInfo.CurrentCulture);
        try
        {
          return NativeWrapper.EvtFormatMessageRenderName(this.GetProviderMetadata(local_0).Handle, eventHandle, UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageEvent);
        }
        catch (EventLogNotFoundException exception_0)
        {
          return (string) null;
        }
      }
    }

    public string GetFormatDescription(string ProviderName, EventLogHandle eventHandle, string[] values)
    {
      lock (this)
      {
        ProviderMetadata local_1 = this.GetProviderMetadata(new ProviderMetadataCachedInformation.ProviderMetadataId(ProviderName, CultureInfo.CurrentCulture));
        try
        {
          return NativeWrapper.EvtFormatMessageFormatDescription(local_1.Handle, eventHandle, values);
        }
        catch (EventLogNotFoundException exception_0)
        {
          return (string) null;
        }
      }
    }

    [SecuritySafeCritical]
    public string GetLevelDisplayName(string ProviderName, EventLogHandle eventHandle)
    {
      lock (this)
        return NativeWrapper.EvtFormatMessageRenderName(this.GetProviderMetadata(new ProviderMetadataCachedInformation.ProviderMetadataId(ProviderName, CultureInfo.CurrentCulture)).Handle, eventHandle, UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageLevel);
    }

    [SecuritySafeCritical]
    public string GetOpcodeDisplayName(string ProviderName, EventLogHandle eventHandle)
    {
      lock (this)
        return NativeWrapper.EvtFormatMessageRenderName(this.GetProviderMetadata(new ProviderMetadataCachedInformation.ProviderMetadataId(ProviderName, CultureInfo.CurrentCulture)).Handle, eventHandle, UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageOpcode);
    }

    [SecuritySafeCritical]
    public string GetTaskDisplayName(string ProviderName, EventLogHandle eventHandle)
    {
      lock (this)
        return NativeWrapper.EvtFormatMessageRenderName(this.GetProviderMetadata(new ProviderMetadataCachedInformation.ProviderMetadataId(ProviderName, CultureInfo.CurrentCulture)).Handle, eventHandle, UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageTask);
    }

    [SecuritySafeCritical]
    public IEnumerable<string> GetKeywordDisplayNames(string ProviderName, EventLogHandle eventHandle)
    {
      lock (this)
        return NativeWrapper.EvtFormatMessageRenderKeywords(this.GetProviderMetadata(new ProviderMetadataCachedInformation.ProviderMetadataId(ProviderName, CultureInfo.CurrentCulture)).Handle, eventHandle, UnsafeNativeMethods.EvtFormatMessageFlags.EvtFormatMessageKeyword);
    }

    private bool IsCacheFull()
    {
      return this.cache.Count == this.maximumCacheSize;
    }

    private bool IsProviderinCache(ProviderMetadataCachedInformation.ProviderMetadataId key)
    {
      return this.cache.ContainsKey(key);
    }

    private void DeleteCacheEntry(ProviderMetadataCachedInformation.ProviderMetadataId key)
    {
      if (!this.IsProviderinCache(key))
        return;
      ProviderMetadataCachedInformation.CacheItem cacheItem = this.cache[key];
      this.cache.Remove(key);
      cacheItem.ProviderMetadata.Dispose();
    }

    private void AddCacheEntry(ProviderMetadataCachedInformation.ProviderMetadataId key, ProviderMetadata pm)
    {
      if (this.IsCacheFull())
        this.FlushOldestEntry();
      ProviderMetadataCachedInformation.CacheItem cacheItem = new ProviderMetadataCachedInformation.CacheItem(pm);
      this.cache.Add(key, cacheItem);
    }

    private void FlushOldestEntry()
    {
      double num = -10.0;
      DateTime now = DateTime.Now;
      ProviderMetadataCachedInformation.ProviderMetadataId key = (ProviderMetadataCachedInformation.ProviderMetadataId) null;
      foreach (KeyValuePair<ProviderMetadataCachedInformation.ProviderMetadataId, ProviderMetadataCachedInformation.CacheItem> keyValuePair in this.cache)
      {
        TimeSpan timeSpan = now.Subtract(keyValuePair.Value.TheTime);
        if (timeSpan.TotalMilliseconds >= num)
        {
          num = timeSpan.TotalMilliseconds;
          key = keyValuePair.Key;
        }
      }
      if (key == null)
        return;
      this.DeleteCacheEntry(key);
    }

    private static void UpdateCacheValueInfoForHit(ProviderMetadataCachedInformation.CacheItem cacheItem)
    {
      cacheItem.TheTime = DateTime.Now;
    }

    private ProviderMetadata GetProviderMetadata(ProviderMetadataCachedInformation.ProviderMetadataId key)
    {
      if (!this.IsProviderinCache(key))
      {
        ProviderMetadata pm;
        try
        {
          pm = new ProviderMetadata(key.ProviderName, this.session, key.TheCultureInfo, this.logfile);
        }
        catch (EventLogNotFoundException ex)
        {
          pm = new ProviderMetadata(key.ProviderName, this.session, key.TheCultureInfo);
        }
        this.AddCacheEntry(key, pm);
        return pm;
      }
      else
      {
        ProviderMetadataCachedInformation.CacheItem cacheItem = this.cache[key];
        ProviderMetadata pm = cacheItem.ProviderMetadata;
        try
        {
          pm.CheckReleased();
          ProviderMetadataCachedInformation.UpdateCacheValueInfoForHit(cacheItem);
        }
        catch (EventLogException ex1)
        {
          this.DeleteCacheEntry(key);
          try
          {
            pm = new ProviderMetadata(key.ProviderName, this.session, key.TheCultureInfo, this.logfile);
          }
          catch (EventLogNotFoundException ex2)
          {
            pm = new ProviderMetadata(key.ProviderName, this.session, key.TheCultureInfo);
          }
          this.AddCacheEntry(key, pm);
        }
        return pm;
      }
    }

    private class ProviderMetadataId
    {
      private string providerName;
      private CultureInfo cultureInfo;

      public string ProviderName
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.providerName;
        }
      }

      public CultureInfo TheCultureInfo
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.cultureInfo;
        }
      }

      public ProviderMetadataId(string providerName, CultureInfo cultureInfo)
      {
        this.providerName = providerName;
        this.cultureInfo = cultureInfo;
      }

      public override bool Equals(object obj)
      {
        ProviderMetadataCachedInformation.ProviderMetadataId providerMetadataId = obj as ProviderMetadataCachedInformation.ProviderMetadataId;
        return providerMetadataId != null && this.providerName.Equals(providerMetadataId.providerName) && this.cultureInfo == providerMetadataId.cultureInfo;
      }

      public override int GetHashCode()
      {
        return this.providerName.GetHashCode() ^ this.cultureInfo.GetHashCode();
      }
    }

    private class CacheItem
    {
      private ProviderMetadata pm;
      private DateTime theTime;

      public DateTime TheTime
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.theTime;
        }
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] set
        {
          this.theTime = value;
        }
      }

      public ProviderMetadata ProviderMetadata
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.pm;
        }
      }

      public CacheItem(ProviderMetadata pm)
      {
        this.pm = pm;
        this.theTime = DateTime.Now;
      }
    }
  }
}
