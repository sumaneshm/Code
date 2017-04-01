// Type: System.Linq.Parallel.GroupByIdentityQueryOperatorEnumerator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class GroupByIdentityQueryOperatorEnumerator<TSource, TGroupKey, TOrderKey> : GroupByQueryOperatorEnumerator<TSource, TGroupKey, TSource, TOrderKey>
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal GroupByIdentityQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TSource, TGroupKey>, TOrderKey> source, IEqualityComparer<TGroupKey> keyComparer, CancellationToken cancellationToken)
      : base(source, keyComparer, cancellationToken)
    {
    }

    protected override HashLookup<Wrapper<TGroupKey>, ListChunk<TSource>> BuildHashLookup()
    {
      HashLookup<Wrapper<TGroupKey>, ListChunk<TSource>> hashLookup = new HashLookup<Wrapper<TGroupKey>, ListChunk<TSource>>((IEqualityComparer<Wrapper<TGroupKey>>) new WrapperEqualityComparer<TGroupKey>(this.m_keyComparer));
      Pair<TSource, TGroupKey> currentElement = new Pair<TSource, TGroupKey>();
      TOrderKey currentKey = default (TOrderKey);
      int num = 0;
      while (this.m_source.MoveNext(ref currentElement, ref currentKey))
      {
        if ((num++ & 63) == 0)
          CancellationState.ThrowIfCanceled(this.m_cancellationToken);
        Wrapper<TGroupKey> key = new Wrapper<TGroupKey>(currentElement.Second);
        ListChunk<TSource> listChunk = (ListChunk<TSource>) null;
        if (!hashLookup.TryGetValue(key, ref listChunk))
        {
          listChunk = new ListChunk<TSource>(2);
          hashLookup.Add(key, listChunk);
        }
        listChunk.Add(currentElement.First);
      }
      return hashLookup;
    }
  }
}
