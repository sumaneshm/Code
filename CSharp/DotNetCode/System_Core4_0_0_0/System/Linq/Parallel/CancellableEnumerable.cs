// Type: System.Linq.Parallel.CancellableEnumerable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel
{
  internal static class CancellableEnumerable
  {
    internal static IEnumerable<TElement> Wrap<TElement>(IEnumerable<TElement> source, CancellationToken token)
    {
      int count = 0;
      foreach (TElement element in source)
      {
        if ((count++ & 63) == 0)
          CancellationState.ThrowIfCanceled(token);
        yield return element;
      }
    }
  }
}
