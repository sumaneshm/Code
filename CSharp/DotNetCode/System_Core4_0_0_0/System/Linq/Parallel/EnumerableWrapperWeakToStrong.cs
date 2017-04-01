// Type: System.Linq.Parallel.EnumerableWrapperWeakToStrong
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
  internal class EnumerableWrapperWeakToStrong : IEnumerable<object>, IEnumerable
  {
    private readonly IEnumerable m_wrappedEnumerable;

    internal EnumerableWrapperWeakToStrong(IEnumerable wrappedEnumerable)
    {
      this.m_wrappedEnumerable = wrappedEnumerable;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public IEnumerator<object> GetEnumerator()
    {
      return (IEnumerator<object>) new EnumerableWrapperWeakToStrong.WrapperEnumeratorWeakToStrong(this.m_wrappedEnumerable.GetEnumerator());
    }

    private class WrapperEnumeratorWeakToStrong : IEnumerator<object>, IDisposable, IEnumerator
    {
      private IEnumerator m_wrappedEnumerator;

      object IEnumerator.Current
      {
        get
        {
          return this.m_wrappedEnumerator.Current;
        }
      }

      object IEnumerator<object>.Current
      {
        get
        {
          return this.m_wrappedEnumerator.Current;
        }
      }

      internal WrapperEnumeratorWeakToStrong(IEnumerator wrappedEnumerator)
      {
        this.m_wrappedEnumerator = wrappedEnumerator;
      }

      void IDisposable.Dispose()
      {
        IDisposable disposable = this.m_wrappedEnumerator as IDisposable;
        if (disposable == null)
          return;
        disposable.Dispose();
      }

      bool IEnumerator.MoveNext()
      {
        return this.m_wrappedEnumerator.MoveNext();
      }

      void IEnumerator.Reset()
      {
        this.m_wrappedEnumerator.Reset();
      }
    }
  }
}
