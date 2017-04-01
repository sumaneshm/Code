// Type: System.Linq.Parallel.SortQueryOperatorEnumerator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class SortQueryOperatorEnumerator<TInputOutput, TKey, TSortKey> : QueryOperatorEnumerator<TInputOutput, TSortKey>
  {
    private readonly QueryOperatorEnumerator<TInputOutput, TKey> m_source;
    private readonly Func<TInputOutput, TSortKey> m_keySelector;
    private readonly IComparer<TSortKey> m_keyComparer;

    public IComparer<TSortKey> KeyComparer
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_keyComparer;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal SortQueryOperatorEnumerator(QueryOperatorEnumerator<TInputOutput, TKey> source, Func<TInputOutput, TSortKey> keySelector, IComparer<TSortKey> keyComparer)
    {
      this.m_source = source;
      this.m_keySelector = keySelector;
      this.m_keyComparer = keyComparer;
    }

    internal override bool MoveNext(ref TInputOutput currentElement, ref TSortKey currentKey)
    {
      TKey currentKey1 = default (TKey);
      if (!this.m_source.MoveNext(ref currentElement, ref currentKey1))
        return false;
      currentKey = this.m_keySelector(currentElement);
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      this.m_source.Dispose();
    }
  }
}
