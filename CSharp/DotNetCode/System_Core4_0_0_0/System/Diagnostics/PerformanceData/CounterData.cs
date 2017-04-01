// Type: System.Diagnostics.PerformanceData.CounterData
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics.PerformanceData
{
  [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
  public sealed class CounterData
  {
    [SecurityCritical]
    private unsafe long* m_offset;

    public unsafe long Value
    {
      [SecurityCritical] get
      {
        return Interlocked.Read(ref *this.m_offset);
      }
      [SecurityCritical] set
      {
        Interlocked.Exchange(ref *this.m_offset, value);
      }
    }

    public unsafe long RawValue
    {
      [SecurityCritical] get
      {
        return *this.m_offset;
      }
      [SecurityCritical] set
      {
        *this.m_offset = value;
      }
    }

    [SecurityCritical]
    internal unsafe CounterData(long* pCounterData)
    {
      this.m_offset = pCounterData;
      *this.m_offset = 0L;
    }

    [SecurityCritical]
    public unsafe void Increment()
    {
      Interlocked.Increment(ref *this.m_offset);
    }

    [SecurityCritical]
    public unsafe void Decrement()
    {
      Interlocked.Decrement(ref *this.m_offset);
    }

    [SecurityCritical]
    public unsafe void IncrementBy(long value)
    {
      Interlocked.Add(ref *this.m_offset, value);
    }
  }
}
