// Type: System.Linq.Parallel.IntValueEvent
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Threading;

namespace System.Linq.Parallel
{
  internal class IntValueEvent : ManualResetEventSlim
  {
    internal int Value;

    internal IntValueEvent()
      : base(false)
    {
      this.Value = 0;
    }

    internal void Set(int index)
    {
      this.Value = index;
      base.Set();
    }
  }
}
