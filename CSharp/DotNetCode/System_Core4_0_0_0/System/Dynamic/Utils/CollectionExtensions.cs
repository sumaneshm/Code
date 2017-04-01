// Type: System.Dynamic.Utils.CollectionExtensions
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Dynamic.Utils
{
  internal static class CollectionExtensions
  {
    internal static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> enumerable)
    {
      if (enumerable == null)
        return EmptyReadOnlyCollection<T>.Instance;
      TrueReadOnlyCollection<T> readOnlyCollection = enumerable as TrueReadOnlyCollection<T>;
      if (readOnlyCollection != null)
        return (ReadOnlyCollection<T>) readOnlyCollection;
      ReadOnlyCollectionBuilder<T> collectionBuilder = enumerable as ReadOnlyCollectionBuilder<T>;
      if (collectionBuilder != null)
        return collectionBuilder.ToReadOnlyCollection();
      ICollection<T> collection = enumerable as ICollection<T>;
      if (collection == null)
        return (ReadOnlyCollection<T>) new TrueReadOnlyCollection<T>(new List<T>(enumerable).ToArray());
      int count = collection.Count;
      if (count == 0)
        return EmptyReadOnlyCollection<T>.Instance;
      T[] objArray = new T[count];
      collection.CopyTo(objArray, 0);
      return (ReadOnlyCollection<T>) new TrueReadOnlyCollection<T>(objArray);
    }

    internal static int ListHashCode<T>(this IEnumerable<T> list)
    {
      EqualityComparer<T> @default = EqualityComparer<T>.Default;
      int num = 6551;
      foreach (T obj in list)
        num ^= num << 5 ^ @default.GetHashCode(obj);
      return num;
    }

    internal static bool ListEquals<T>(this ICollection<T> first, ICollection<T> second)
    {
      if (first.Count != second.Count)
        return false;
      EqualityComparer<T> @default = EqualityComparer<T>.Default;
      IEnumerator<T> enumerator1 = first.GetEnumerator();
      IEnumerator<T> enumerator2 = second.GetEnumerator();
      while (enumerator1.MoveNext())
      {
        enumerator2.MoveNext();
        if (!@default.Equals(enumerator1.Current, enumerator2.Current))
          return false;
      }
      return true;
    }

    internal static IEnumerable<U> Select<T, U>(this IEnumerable<T> enumerable, Func<T, U> select)
    {
      foreach (T obj in enumerable)
        yield return select(obj);
    }

    internal static U[] Map<T, U>(this ICollection<T> collection, Func<T, U> select)
    {
      U[] uArray = new U[collection.Count];
      int num = 0;
      foreach (T obj in (IEnumerable<T>) collection)
        uArray[num++] = select(obj);
      return uArray;
    }

    internal static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, Func<T, bool> where)
    {
      foreach (T obj in enumerable)
      {
        if (where(obj))
          yield return obj;
      }
    }

    internal static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      foreach (T obj in source)
      {
        if (predicate(obj))
          return true;
      }
      return false;
    }

    internal static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
      foreach (T obj in source)
      {
        if (!predicate(obj))
          return false;
      }
      return true;
    }

    internal static T[] RemoveFirst<T>(this T[] array)
    {
      T[] objArray = new T[array.Length - 1];
      Array.Copy((Array) array, 1, (Array) objArray, 0, objArray.Length);
      return objArray;
    }

    internal static T[] RemoveLast<T>(this T[] array)
    {
      T[] objArray = new T[array.Length - 1];
      Array.Copy((Array) array, 0, (Array) objArray, 0, objArray.Length);
      return objArray;
    }

    internal static T[] AddFirst<T>(this IList<T> list, T item)
    {
      T[] array = new T[list.Count + 1];
      array[0] = item;
      list.CopyTo(array, 1);
      return array;
    }

    internal static T[] AddLast<T>(this IList<T> list, T item)
    {
      T[] array = new T[list.Count + 1];
      list.CopyTo(array, 0);
      array[list.Count] = item;
      return array;
    }

    internal static T First<T>(this IEnumerable<T> source)
    {
      IList<T> list = source as IList<T>;
      if (list != null)
        return list[0];
      using (IEnumerator<T> enumerator = source.GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
      throw new InvalidOperationException();
    }

    internal static T Last<T>(this IList<T> list)
    {
      return list[list.Count - 1];
    }

    internal static T[] Copy<T>(this T[] array)
    {
      T[] objArray = new T[array.Length];
      Array.Copy((Array) array, (Array) objArray, array.Length);
      return objArray;
    }
  }
}
