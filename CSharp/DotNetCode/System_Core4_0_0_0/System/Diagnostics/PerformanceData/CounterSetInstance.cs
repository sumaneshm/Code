// Type: System.Diagnostics.PerformanceData.CounterSetInstance
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.ComponentModel;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics.PerformanceData
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CounterSetInstance : IDisposable
  {
    internal CounterSet m_counterSet;
    internal string m_instName;
    private int m_active;
    private CounterSetInstanceCounterDataSet m_counters;
    [SecurityCritical]
    internal unsafe Microsoft.Win32.UnsafeNativeMethods.PerfCounterSetInstanceStruct* m_nativeInst;

    public CounterSetInstanceCounterDataSet Counters
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_counters;
      }
    }

    [SecurityCritical]
    internal unsafe CounterSetInstance(CounterSet counterSetDefined, string instanceName)
    {
      if (counterSetDefined == null)
        throw new ArgumentNullException("counterSetDefined");
      if (instanceName == null)
        throw new ArgumentNullException("InstanceName");
      if (instanceName.Length == 0)
        throw new ArgumentException(SR.GetString("Perflib_Argument_EmptyInstanceName"), "InstanceName");
      this.m_counterSet = counterSetDefined;
      this.m_instName = instanceName;
      this.m_nativeInst = Microsoft.Win32.UnsafeNativeMethods.PerfCreateInstance(this.m_counterSet.m_provider.m_hProvider, ref this.m_counterSet.m_counterSet, this.m_instName, 0U);
      int error = (IntPtr) this.m_nativeInst != IntPtr.Zero ? 0 : Marshal.GetLastWin32Error();
      if ((IntPtr) this.m_nativeInst != IntPtr.Zero)
      {
        this.m_counters = new CounterSetInstanceCounterDataSet(this);
        this.m_active = 1;
      }
      else
      {
        switch (error)
        {
          case 87:
            if (this.m_counterSet.m_instType != CounterSetInstanceType.Single)
              throw new Win32Exception(error);
            throw new ArgumentException(SR.GetString("Perflib_Argument_InvalidInstance", new object[1]
            {
              (object) this.m_counterSet.m_counterSet
            }), "InstanceName");
          case 183:
            throw new ArgumentException(SR.GetString("Perflib_Argument_InstanceAlreadyExists", (object) this.m_instName, (object) this.m_counterSet.m_counterSet), "InstanceName");
          case 1168:
            throw new InvalidOperationException(SR.GetString("Perflib_InvalidOperation_CounterSetNotInstalled", new object[1]
            {
              (object) this.m_counterSet.m_counterSet
            }));
          default:
            throw new Win32Exception(error);
        }
      }
    }

    [SecurityCritical]
    ~CounterSetInstance()
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
      if (disposing && this.m_counters != null)
      {
        this.m_counters.Dispose();
        this.m_counters = (CounterSetInstanceCounterDataSet) null;
      }
      if ((IntPtr) this.m_nativeInst == IntPtr.Zero || Interlocked.Exchange(ref this.m_active, 0) == 0 || (IntPtr) this.m_nativeInst == IntPtr.Zero)
        return;
      lock (this.m_counterSet)
      {
        if (this.m_counterSet.m_provider != null)
        {
          int temp_31 = (int) Microsoft.Win32.UnsafeNativeMethods.PerfDeleteInstance(this.m_counterSet.m_provider.m_hProvider, this.m_nativeInst);
        }
        this.m_nativeInst = (Microsoft.Win32.UnsafeNativeMethods.PerfCounterSetInstanceStruct*) null;
      }
    }
  }
}
