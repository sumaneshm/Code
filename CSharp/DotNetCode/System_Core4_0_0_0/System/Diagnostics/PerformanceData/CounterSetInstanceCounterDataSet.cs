// Type: System.Diagnostics.PerformanceData.CounterSetInstanceCounterDataSet
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics.PerformanceData
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CounterSetInstanceCounterDataSet : IDisposable
  {
    internal CounterSetInstance m_instance;
    private Dictionary<int, CounterData> m_counters;
    private int m_disposed;
    [SecurityCritical]
    internal unsafe byte* m_dataBlock;

    public CounterData this[int counterId]
    {
      get
      {
        if (this.m_disposed != 0)
          return (CounterData) null;
        try
        {
          return this.m_counters[counterId];
        }
        catch (KeyNotFoundException ex)
        {
          return (CounterData) null;
        }
        catch
        {
          throw;
        }
      }
    }

    public CounterData this[string counterName]
    {
      get
      {
        if (counterName == null)
          throw new ArgumentNullException("CounterName");
        if (counterName.Length == 0)
          throw new ArgumentNullException("CounterName");
        if (this.m_disposed != 0)
          return (CounterData) null;
        try
        {
          int index = this.m_instance.m_counterSet.m_stringToId[counterName];
          try
          {
            return this.m_counters[index];
          }
          catch (KeyNotFoundException ex)
          {
            return (CounterData) null;
          }
          catch
          {
            throw;
          }
        }
        catch (KeyNotFoundException ex)
        {
          return (CounterData) null;
        }
        catch
        {
          throw;
        }
      }
    }

    [SecurityCritical]
    internal unsafe CounterSetInstanceCounterDataSet(CounterSetInstance thisInst)
    {
      this.m_instance = thisInst;
      this.m_counters = new Dictionary<int, CounterData>();
      if (this.m_instance.m_counterSet.m_provider == null)
        throw new ArgumentException(SR.GetString("Perflib_Argument_ProviderNotFound", new object[1]
        {
          (object) this.m_instance.m_counterSet.m_providerGuid
        }), "ProviderGuid");
      else if (this.m_instance.m_counterSet.m_provider.m_hProvider.IsInvalid)
      {
        throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_NoActiveProvider", new object[1]
        {
          (object) this.m_instance.m_counterSet.m_providerGuid
        }));
      }
      else
      {
        this.m_dataBlock = (byte*) (void*) Marshal.AllocHGlobal(this.m_instance.m_counterSet.m_idToCounter.Count * 8);
        if ((IntPtr) this.m_dataBlock == IntPtr.Zero)
        {
          throw new InsufficientMemoryException(SR.GetString("Perflib_InsufficientMemory_InstanceCounterBlock", (object) this.m_instance.m_counterSet.m_counterSet, (object) this.m_instance.m_instName));
        }
        else
        {
          int num1 = 0;
          foreach (KeyValuePair<int, CounterType> keyValuePair in this.m_instance.m_counterSet.m_idToCounter)
          {
            CounterData counterData = new CounterData((long*) (this.m_dataBlock + ((IntPtr) num1 * 8).ToInt64()));
            this.m_counters.Add(keyValuePair.Key, counterData);
            uint num2 = Microsoft.Win32.UnsafeNativeMethods.PerfSetCounterRefValue(this.m_instance.m_counterSet.m_provider.m_hProvider, this.m_instance.m_nativeInst, (uint) keyValuePair.Key, (void*) (this.m_dataBlock + ((IntPtr) num1 * 8).ToInt64()));
            if ((int) num2 != 0)
            {
              this.Dispose(true);
              if ((int) num2 != 1168)
                throw new Win32Exception((int) num2);
              throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_CounterRefValue", (object) this.m_instance.m_counterSet.m_counterSet, (object) keyValuePair.Key, (object) this.m_instance.m_instName));
            }
            else
              ++num1;
          }
        }
      }
    }

    [SecurityCritical]
    ~CounterSetInstanceCounterDataSet()
    {
      this.Dispose(false);
    }

    [SecurityCritical]
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SecurityCritical]
    private unsafe void Dispose(bool disposing)
    {
      if (Interlocked.Exchange(ref this.m_disposed, 1) != 0 || (IntPtr) this.m_dataBlock == IntPtr.Zero)
        return;
      Marshal.FreeHGlobal((IntPtr) ((void*) this.m_dataBlock));
      this.m_dataBlock = (byte*) null;
    }
  }
}
