// Type: System.Diagnostics.PerformanceData.PerfProviderCollection
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;

namespace System.Diagnostics.PerformanceData
{
  internal static class PerfProviderCollection
  {
    private static List<PerfProvider> s_providerList = new List<PerfProvider>();
    private static Dictionary<object, int> s_counterSetList = new Dictionary<object, int>();
    private static CounterType[] s_counterTypes = (CounterType[]) Enum.GetValues(typeof (CounterType));
    private static CounterSetInstanceType[] s_counterSetInstanceTypes = (CounterSetInstanceType[]) Enum.GetValues(typeof (CounterSetInstanceType));
    private static object s_hiddenInternalSyncObject;

    private static object s_lockObject
    {
      get
      {
        if (PerfProviderCollection.s_hiddenInternalSyncObject == null)
        {
          object obj = new object();
          Interlocked.CompareExchange(ref PerfProviderCollection.s_hiddenInternalSyncObject, obj, (object) null);
        }
        return PerfProviderCollection.s_hiddenInternalSyncObject;
      }
    }

    static PerfProviderCollection()
    {
    }

    [SecurityCritical]
    internal static PerfProvider QueryProvider(Guid providerGuid)
    {
      lock (PerfProviderCollection.s_lockObject)
      {
        foreach (PerfProvider item_0 in PerfProviderCollection.s_providerList)
        {
          if (item_0.m_providerGuid == providerGuid)
            return item_0;
        }
        PerfProvider local_1 = new PerfProvider(providerGuid);
        PerfProviderCollection.s_providerList.Add(local_1);
        return local_1;
      }
    }

    [SecurityCritical]
    internal static void RemoveProvider(Guid providerGuid)
    {
      lock (PerfProviderCollection.s_lockObject)
      {
        PerfProvider local_0 = (PerfProvider) null;
        foreach (PerfProvider item_0 in PerfProviderCollection.s_providerList)
        {
          if (item_0.m_providerGuid == providerGuid)
            local_0 = item_0;
        }
        if (local_0 == null)
          return;
        local_0.m_hProvider.Dispose();
        PerfProviderCollection.s_providerList.Remove(local_0);
      }
    }

    internal static void RegisterCounterSet(Guid counterSetGuid)
    {
      lock (PerfProviderCollection.s_lockObject)
      {
        if (PerfProviderCollection.s_counterSetList.ContainsKey((object) counterSetGuid))
          throw new ArgumentException(SR.GetString("Perflib_Argument_CounterSetAlreadyRegister", new object[1]
          {
            (object) counterSetGuid
          }), "CounterSetGuid");
        else
          PerfProviderCollection.s_counterSetList.Add((object) counterSetGuid, 0);
      }
    }

    internal static void UnregisterCounterSet(Guid counterSetGuid)
    {
      lock (PerfProviderCollection.s_lockObject)
        PerfProviderCollection.s_counterSetList.Remove((object) counterSetGuid);
    }

    internal static bool ValidateCounterType(CounterType inCounterType)
    {
      foreach (CounterType counterType in PerfProviderCollection.s_counterTypes)
      {
        if (counterType == inCounterType)
          return true;
      }
      return false;
    }

    internal static bool ValidateCounterSetInstanceType(CounterSetInstanceType inCounterSetInstanceType)
    {
      foreach (CounterSetInstanceType counterSetInstanceType in PerfProviderCollection.s_counterSetInstanceTypes)
      {
        if (counterSetInstanceType == inCounterSetInstanceType)
          return true;
      }
      return false;
    }
  }
}
