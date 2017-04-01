// Type: System.Linq.Parallel.ProducerComparerInt
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class ProducerComparerInt : IComparer<Producer<int>>
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public ProducerComparerInt()
    {
    }

    public int Compare(Producer<int> x, Producer<int> y)
    {
      return y.MaxKey - x.MaxKey;
    }
  }
}
