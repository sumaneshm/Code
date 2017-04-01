// Type: System.Linq.Parallel.GroupByGrouping`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class GroupByGrouping<TGroupKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
  {
    private KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>> m_keyValues;

    TGroupKey IGrouping<TGroupKey, TElement>.Key
    {
      get
      {
        return this.m_keyValues.Key.Value;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal GroupByGrouping(KeyValuePair<Wrapper<TGroupKey>, ListChunk<TElement>> keyValues)
    {
      this.m_keyValues = keyValues;
    }

    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
    {
      return this.m_keyValues.Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
