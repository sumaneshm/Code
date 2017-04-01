// Type: System.Diagnostics.PerformanceData.CounterSet
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics.PerformanceData
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public class CounterSet : IDisposable
  {
    private static readonly bool s_platformNotSupported = Environment.OSVersion.Version.Major < 6;
    internal PerfProvider m_provider;
    internal Guid m_providerGuid;
    internal Guid m_counterSet;
    internal CounterSetInstanceType m_instType;
    private readonly object m_lockObject;
    private bool m_instanceCreated;
    internal Dictionary<string, int> m_stringToId;
    internal Dictionary<int, CounterType> m_idToCounter;

    static CounterSet()
    {
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
    public CounterSet(Guid providerGuid, Guid counterSetGuid, CounterSetInstanceType instanceType)
    {
      if (CounterSet.s_platformNotSupported)
        throw new PlatformNotSupportedException(SR.GetString("Perflib_PlatformNotSupported"));
      if (!PerfProviderCollection.ValidateCounterSetInstanceType(instanceType))
      {
        throw new ArgumentException(SR.GetString("Perflib_Argument_InvalidCounterSetInstanceType", new object[1]
        {
          (object) instanceType
        }), "instanceType");
      }
      else
      {
        this.m_providerGuid = providerGuid;
        this.m_counterSet = counterSetGuid;
        this.m_instType = instanceType;
        PerfProviderCollection.RegisterCounterSet(this.m_counterSet);
        this.m_provider = PerfProviderCollection.QueryProvider(this.m_providerGuid);
        this.m_lockObject = new object();
        this.m_stringToId = new Dictionary<string, int>();
        this.m_idToCounter = new Dictionary<int, CounterType>();
      }
    }

    ~CounterSet()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    [SecuritySafeCritical]
    protected virtual void Dispose(bool disposing)
    {
      lock (this)
      {
        PerfProviderCollection.UnregisterCounterSet(this.m_counterSet);
        if (!this.m_instanceCreated || this.m_provider == null)
          return;
        lock (this.m_lockObject)
        {
          if (this.m_provider == null)
            return;
          Interlocked.Decrement(ref this.m_provider.m_counterSet);
          if (this.m_provider.m_counterSet <= 0)
            PerfProviderCollection.RemoveProvider(this.m_providerGuid);
          this.m_provider = (PerfProvider) null;
        }
      }
    }

    public void AddCounter(int counterId, CounterType counterType)
    {
      if (this.m_provider == null)
        throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_NoActiveProvider", new object[1]
        {
          (object) this.m_providerGuid
        }));
      else if (!PerfProviderCollection.ValidateCounterType(counterType))
        throw new ArgumentException(SR.GetString("Perflib_Argument_InvalidCounterType", new object[1]
        {
          (object) counterType
        }), "counterType");
      else if (this.m_instanceCreated)
      {
        throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_AddCounterAfterInstance", new object[1]
        {
          (object) this.m_counterSet
        }));
      }
      else
      {
        lock (this.m_lockObject)
        {
          if (this.m_instanceCreated)
            throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_AddCounterAfterInstance", new object[1]
            {
              (object) this.m_counterSet
            }));
          else if (this.m_idToCounter.ContainsKey(counterId))
            throw new ArgumentException(SR.GetString("Perflib_Argument_CounterAlreadyExists", (object) counterId, (object) this.m_counterSet), "CounterId");
          else
            this.m_idToCounter.Add(counterId, counterType);
        }
      }
    }

    public void AddCounter(int counterId, CounterType counterType, string counterName)
    {
      if (counterName == null)
        throw new ArgumentNullException("CounterName");
      if (counterName.Length == 0)
        throw new ArgumentException(SR.GetString("Perflib_Argument_EmptyCounterName"), "counterName");
      if (!PerfProviderCollection.ValidateCounterType(counterType))
        throw new ArgumentException(SR.GetString("Perflib_Argument_InvalidCounterType", new object[1]
        {
          (object) counterType
        }), "counterType");
      else if (this.m_provider == null)
        throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_NoActiveProvider", new object[1]
        {
          (object) this.m_providerGuid
        }));
      else if (this.m_instanceCreated)
      {
        throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_AddCounterAfterInstance", new object[1]
        {
          (object) this.m_counterSet
        }));
      }
      else
      {
        lock (this.m_lockObject)
        {
          if (this.m_instanceCreated)
            throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_AddCounterAfterInstance", new object[1]
            {
              (object) this.m_counterSet
            }));
          else if (this.m_stringToId.ContainsKey(counterName))
            throw new ArgumentException(SR.GetString("Perflib_Argument_CounterNameAlreadyExists", (object) counterName, (object) this.m_counterSet), "CounterName");
          else if (this.m_idToCounter.ContainsKey(counterId))
          {
            throw new ArgumentException(SR.GetString("Perflib_Argument_CounterAlreadyExists", (object) counterId, (object) this.m_counterSet), "CounterId");
          }
          else
          {
            this.m_stringToId.Add(counterName, counterId);
            this.m_idToCounter.Add(counterId, counterType);
          }
        }
      }
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Unrestricted = true)]
    public unsafe CounterSetInstance CreateCounterSetInstance(string instanceName)
    {
      if (instanceName == null)
        throw new ArgumentNullException("instanceName");
      if (instanceName.Length == 0)
        throw new ArgumentException(SR.GetString("Perflib_Argument_EmptyInstanceName"), "instanceName");
      if (this.m_provider == null)
      {
        throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_NoActiveProvider", new object[1]
        {
          (object) this.m_providerGuid
        }));
      }
      else
      {
        if (!this.m_instanceCreated)
        {
          lock (this.m_lockObject)
          {
            if (!this.m_instanceCreated)
            {
              if (this.m_provider == null)
                throw new ArgumentException(SR.GetString("Perflib_Argument_ProviderNotFound", new object[1]
                {
                  (object) this.m_providerGuid
                }), "ProviderGuid");
              else if (this.m_provider.m_hProvider.IsInvalid)
                throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_NoActiveProvider", new object[1]
                {
                  (object) this.m_providerGuid
                }));
              else if (this.m_idToCounter.Count == 0)
              {
                throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_CounterSetContainsNoCounter", new object[1]
                {
                  (object) this.m_counterSet
                }));
              }
              else
              {
                uint local_1 = (uint) (sizeof (Microsoft.Win32.UnsafeNativeMethods.PerfCounterSetInfoStruct) + this.m_idToCounter.Count * sizeof (Microsoft.Win32.UnsafeNativeMethods.PerfCounterInfoStruct));
                byte* local_3 = stackalloc byte[(int) local_1];
                if ((IntPtr) local_3 == IntPtr.Zero)
                {
                  throw new InsufficientMemoryException(SR.GetString("Perflib_InsufficientMemory_CounterSetTemplate", (object) this.m_counterSet, (object) local_1));
                }
                else
                {
                  uint local_6 = 0U;
                  uint local_7 = 0U;
                  Microsoft.Win32.UnsafeNativeMethods.PerfCounterSetInfoStruct* local_4 = (Microsoft.Win32.UnsafeNativeMethods.PerfCounterSetInfoStruct*) local_3;
                  local_4->CounterSetGuid = this.m_counterSet;
                  local_4->ProviderGuid = this.m_providerGuid;
                  local_4->NumCounters = (uint) this.m_idToCounter.Count;
                  local_4->InstanceType = (uint) this.m_instType;
                  foreach (KeyValuePair<int, CounterType> item_0 in this.m_idToCounter)
                  {
                    uint local_2_1 = (uint) (sizeof (Microsoft.Win32.UnsafeNativeMethods.PerfCounterSetInfoStruct) + (int) local_6 * sizeof (Microsoft.Win32.UnsafeNativeMethods.PerfCounterInfoStruct));
                    if (local_2_1 < local_1)
                    {
                      Microsoft.Win32.UnsafeNativeMethods.PerfCounterInfoStruct* local_5 = (Microsoft.Win32.UnsafeNativeMethods.PerfCounterInfoStruct*) (local_3 + (int) local_2_1);
                      local_5->CounterId = (uint) item_0.Key;
                      local_5->CounterType = (uint) item_0.Value;
                      local_5->Attrib = 1L;
                      local_5->Size = (uint) sizeof (void*);
                      local_5->DetailLevel = 100U;
                      local_5->Scale = 0U;
                      local_5->Offset = local_7;
                      local_7 += local_5->Size;
                    }
                    ++local_6;
                  }
                  uint local_0_1 = Microsoft.Win32.UnsafeNativeMethods.PerfSetCounterSetInfo(this.m_provider.m_hProvider, local_4, local_1);
                  switch (local_0_1)
                  {
                    case 0U:
                      Interlocked.Increment(ref this.m_provider.m_counterSet);
                      this.m_instanceCreated = true;
                      break;
                    case 183U:
                      throw new ArgumentException(SR.GetString("Perflib_Argument_CounterSetAlreadyRegister", new object[1]
                      {
                        (object) this.m_counterSet
                      }), "CounterSetGuid");
                    default:
                      throw new Win32Exception((int) local_0_1);
                  }
                }
              }
            }
          }
        }
        return new CounterSetInstance(this, instanceName);
      }
    }
  }
}
