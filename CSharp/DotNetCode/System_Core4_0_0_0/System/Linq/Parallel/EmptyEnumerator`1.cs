// Type: System.Linq.Parallel.EmptyEnumerator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class EmptyEnumerator<T> : QueryOperatorEnumerator<T, int>, IEnumerator<T>, IDisposable, IEnumerator
  {
    public T Current
    {
      get
      {
        return default (T);
      }
    }

    object IEnumerator.Current
    {
      get
      {
        return (object) null;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public EmptyEnumerator()
    {
    }

    internal override bool MoveNext(ref T currentElement, ref int currentKey)
    {
      return false;
    }

    public bool MoveNext()
    {
      return false;
    }

    void IEnumerator.Reset()
    {
    }
  }
}
