// Type: System.Linq.Parallel.GroupByElementSelectorQueryOperatorEnumerator`4
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class GroupByElementSelectorQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey> : GroupByQueryOperatorEnumerator<TSource, TGroupKey, TElement, TOrderKey>
  {
    private readonly Func<TSource, TElement> m_elementSelector;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal GroupByElementSelectorQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TSource, TGroupKey>, TOrderKey> source, IEqualityComparer<TGroupKey> keyComparer, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
      : base(source, keyComparer, cancellationToken)
    {
      this.m_elementSelector = elementSelector;
    }

    protected override HashLookup<Wrapper<TGroupKey>, ListChunk<TElement>> BuildHashLookup()
    {
      HashLookup<Wrapper<TGroupKey>, ListChunk<TElement>> hashLookup = new HashLookup<Wrapper<TGroupKey>, ListChunk<TElement>>((IEqualityComparer<Wrapper<TGroupKey>>) new WrapperEqualityComparer<TGroupKey>(this.m_keyComparer));
      Pair<TSource, TGroupKey> currentElement = new Pair<TSource, TGroupKey>();
      TOrderKey currentKey = default (TOrderKey);
      int num = 0;
      while (this.m_source.MoveNext(ref currentElement, ref currentKey))
      {
        if ((num++ & 63) == 0)
          CancellationState.ThrowIfCanceled(this.m_cancellationToken);
        Wrapper<TGroupKey> key = new Wrapper<TGroupKey>(currentElement.Second);
        ListChunk<TElement> listChunk = (ListChunk<TElement>) null;
        if (!hashLookup.TryGetValue(key, ref listChunk))
        {
          listChunk = new ListChunk<TElement>(2);
          hashLookup.Add(key, listChunk);
        }
        listChunk.Add(this.m_elementSelector(currentElement.First));
      }
      return hashLookup;
    }
  }
}
