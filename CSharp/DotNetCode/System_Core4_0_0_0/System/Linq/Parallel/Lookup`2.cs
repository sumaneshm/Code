// Type: System.Linq.Parallel.Lookup`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Parallel
{
  internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
  {
    private IDictionary<TKey, IGrouping<TKey, TElement>> m_dict;
    private IEqualityComparer<TKey> m_comparer;
    private IGrouping<TKey, TElement> m_defaultKeyGrouping;

    public int Count
    {
      get
      {
        int count = this.m_dict.Count;
        if (this.m_defaultKeyGrouping != null)
          ++count;
        return count;
      }
    }

    public IEnumerable<TElement> this[TKey key]
    {
      get
      {
        if (this.m_comparer.Equals(key, default (TKey)))
        {
          if (this.m_defaultKeyGrouping != null)
            return (IEnumerable<TElement>) this.m_defaultKeyGrouping;
          else
            return Enumerable.Empty<TElement>();
        }
        else
        {
          IGrouping<TKey, TElement> grouping;
          if (this.m_dict.TryGetValue(key, out grouping))
            return (IEnumerable<TElement>) grouping;
          else
            return Enumerable.Empty<TElement>();
        }
      }
    }

    internal Lookup(IEqualityComparer<TKey> comparer)
    {
      this.m_comparer = comparer;
      this.m_dict = (IDictionary<TKey, IGrouping<TKey, TElement>>) new Dictionary<TKey, IGrouping<TKey, TElement>>(this.m_comparer);
    }

    public bool Contains(TKey key)
    {
      if (this.m_comparer.Equals(key, default (TKey)))
        return this.m_defaultKeyGrouping != null;
      else
        return this.m_dict.ContainsKey(key);
    }

    internal void Add(IGrouping<TKey, TElement> grouping)
    {
      if (this.m_comparer.Equals(grouping.Key, default (TKey)))
        this.m_defaultKeyGrouping = grouping;
      else
        this.m_dict.Add(grouping.Key, grouping);
    }

    public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
    {
      foreach (IGrouping<TKey, TElement> grouping in (IEnumerable<IGrouping<TKey, TElement>>) this.m_dict.Values)
        yield return grouping;
      if (this.m_defaultKeyGrouping != null)
        yield return this.m_defaultKeyGrouping;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
