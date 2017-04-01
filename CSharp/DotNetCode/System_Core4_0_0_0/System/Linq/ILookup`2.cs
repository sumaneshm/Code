// Type: System.Linq.ILookup`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
  {
    [__DynamicallyInvokable]
    int Count { [__DynamicallyInvokable] get; }

    [__DynamicallyInvokable]
    IEnumerable<TElement> this[TKey key] { [__DynamicallyInvokable] get; }

    [__DynamicallyInvokable]
    bool Contains(TKey key);
  }
}
