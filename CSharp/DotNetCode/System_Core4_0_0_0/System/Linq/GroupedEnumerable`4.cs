﻿// Type: System.Linq.GroupedEnumerable`4
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq
{
  internal class GroupedEnumerable<TSource, TKey, TElement, TResult> : IEnumerable<TResult>, IEnumerable
  {
    private IEnumerable<TSource> source;
    private Func<TSource, TKey> keySelector;
    private Func<TSource, TElement> elementSelector;
    private IEqualityComparer<TKey> comparer;
    private Func<TKey, IEnumerable<TElement>, TResult> resultSelector;

    public GroupedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (elementSelector == null)
        throw Error.ArgumentNull("elementSelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      this.source = source;
      this.keySelector = keySelector;
      this.elementSelector = elementSelector;
      this.comparer = comparer;
      this.resultSelector = resultSelector;
    }

    public IEnumerator<TResult> GetEnumerator()
    {
      return Lookup<TKey, TElement>.Create<TSource>(this.source, this.keySelector, this.elementSelector, this.comparer).ApplyResultSelector<TResult>(this.resultSelector).GetEnumerator();
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
