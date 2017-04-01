// Type: System.Linq.Parallel.QueryOperatorEnumerator`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal abstract class QueryOperatorEnumerator<TElement, TKey>
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected QueryOperatorEnumerator()
    {
    }

    internal abstract bool MoveNext(ref TElement currentElement, ref TKey currentKey);

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Dispose()
    {
      this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    internal virtual void Reset()
    {
    }

    internal IEnumerator<TElement> AsClassicEnumerator()
    {
      return (IEnumerator<TElement>) new QueryOperatorEnumerator<TElement, TKey>.QueryOperatorClassicEnumerator(this);
    }

    private class QueryOperatorClassicEnumerator : IEnumerator<TElement>, IDisposable, IEnumerator
    {
      private QueryOperatorEnumerator<TElement, TKey> m_operatorEnumerator;
      private TElement m_current;

      public TElement Current
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.m_current;
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.m_current;
        }
      }

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal QueryOperatorClassicEnumerator(QueryOperatorEnumerator<TElement, TKey> operatorEnumerator)
      {
        this.m_operatorEnumerator = operatorEnumerator;
      }

      public bool MoveNext()
      {
        TKey currentKey = default (TKey);
        return this.m_operatorEnumerator.MoveNext(ref this.m_current, ref currentKey);
      }

      public void Dispose()
      {
        this.m_operatorEnumerator.Dispose();
        this.m_operatorEnumerator = (QueryOperatorEnumerator<TElement, TKey>) null;
      }

      public void Reset()
      {
        this.m_operatorEnumerator.Reset();
      }
    }
  }
}
