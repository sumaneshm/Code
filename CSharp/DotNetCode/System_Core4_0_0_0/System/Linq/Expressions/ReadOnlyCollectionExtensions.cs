// Type: System.Linq.Expressions.ReadOnlyCollectionExtensions
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Linq.Expressions
{
  internal static class ReadOnlyCollectionExtensions
  {
    internal static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> sequence)
    {
      if (sequence == null)
        return ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>.Empty;
      else
        return sequence as ReadOnlyCollection<T> ?? new ReadOnlyCollection<T>((IList<T>) Enumerable.ToArray<T>(sequence));
    }

    private static class DefaultReadOnlyCollection<T>
    {
      private static volatile ReadOnlyCollection<T> _defaultCollection;

      internal static ReadOnlyCollection<T> Empty
      {
        get
        {
          if (ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>._defaultCollection == null)
            ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>._defaultCollection = new ReadOnlyCollection<T>((IList<T>) new T[0]);
          return ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>._defaultCollection;
        }
      }
    }
  }
}
