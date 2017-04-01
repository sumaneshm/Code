// Type: System.Linq.Queryable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public static class Queryable
  {
    [__DynamicallyInvokable]
    public static IQueryable<TElement> AsQueryable<TElement>(this IEnumerable<TElement> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (source is IQueryable<TElement>)
        return (IQueryable<TElement>) source;
      else
        return (IQueryable<TElement>) new EnumerableQuery<TElement>(source);
    }

    [__DynamicallyInvokable]
    public static IQueryable AsQueryable(this IEnumerable source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (source is IQueryable)
        return (IQueryable) source;
      Type genericType = TypeHelper.FindGenericType(typeof (IEnumerable<>), source.GetType());
      if (genericType == (Type) null)
        throw Error.ArgumentNotIEnumerableGeneric((object) "source");
      else
        return EnumerableQuery.Create(genericType.GetGenericArguments()[0], source);
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> OfType<TResult>(this IQueryable source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TResult)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Cast<TResult>(this IQueryable source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TResult)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TResult)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TResult)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TResult)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TResult)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (collectionSelector == null)
        throw Error.ArgumentNull("collectionSelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TCollection), typeof (TResult)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) collectionSelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (collectionSelector == null)
        throw Error.ArgumentNull("collectionSelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TCollection), typeof (TResult)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) collectionSelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    private static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
    {
      IQueryable<TSource> queryable = source as IQueryable<TSource>;
      if (queryable != null)
        return queryable.Expression;
      else
        return (Expression) Expression.Constant((object) source, typeof (IEnumerable<TSource>));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
    {
      if (outer == null)
        throw Error.ArgumentNull("outer");
      if (inner == null)
        throw Error.ArgumentNull("inner");
      if (outerKeySelector == null)
        throw Error.ArgumentNull("outerKeySelector");
      if (innerKeySelector == null)
        throw Error.ArgumentNull("innerKeySelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TOuter), typeof (TInner), typeof (TKey), typeof (TResult)), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector)));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw Error.ArgumentNull("outer");
      if (inner == null)
        throw Error.ArgumentNull("inner");
      if (outerKeySelector == null)
        throw Error.ArgumentNull("outerKeySelector");
      if (innerKeySelector == null)
        throw Error.ArgumentNull("innerKeySelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TOuter), typeof (TInner), typeof (TKey), typeof (TResult)), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
    {
      if (outer == null)
        throw Error.ArgumentNull("outer");
      if (inner == null)
        throw Error.ArgumentNull("inner");
      if (outerKeySelector == null)
        throw Error.ArgumentNull("outerKeySelector");
      if (innerKeySelector == null)
        throw Error.ArgumentNull("innerKeySelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TOuter), typeof (TInner), typeof (TKey), typeof (TResult)), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector)));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw Error.ArgumentNull("outer");
      if (inner == null)
        throw Error.ArgumentNull("inner");
      if (outerKeySelector == null)
        throw Error.ArgumentNull("outerKeySelector");
      if (innerKeySelector == null)
        throw Error.ArgumentNull("innerKeySelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return outer.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TOuter), typeof (TInner), typeof (TKey), typeof (TResult)), outer.Expression, Queryable.GetSourceExpression<TInner>(inner), (Expression) Expression.Quote((Expression) outerKeySelector), (Expression) Expression.Quote((Expression) innerKeySelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> OrderByDescending<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IOrderedQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Take<TSource>(this IQueryable<TSource> source, int count)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) count)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> TakeWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> TakeWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Skip<TSource>(this IQueryable<TSource> source, int count)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) count)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> SkipWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> SkipWhile<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return source.Provider.CreateQuery<IGrouping<TKey, TSource>>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (elementSelector == null)
        throw Error.ArgumentNull("elementSelector");
      return source.Provider.CreateQuery<IGrouping<TKey, TElement>>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey), typeof (TElement)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) elementSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      return source.Provider.CreateQuery<IGrouping<TKey, TSource>>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (elementSelector == null)
        throw Error.ArgumentNull("elementSelector");
      return source.Provider.CreateQuery<IGrouping<TKey, TElement>>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey), typeof (TElement)), new Expression[4]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) elementSelector),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (elementSelector == null)
        throw Error.ArgumentNull("elementSelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey), typeof (TElement), typeof (TResult)), new Expression[4]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) elementSelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey), typeof (TResult)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey), typeof (TResult)), new Expression[4]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) keySelector),
        (Expression) Expression.Quote((Expression) resultSelector),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (keySelector == null)
        throw Error.ArgumentNull("keySelector");
      if (elementSelector == null)
        throw Error.ArgumentNull("elementSelector");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TKey), typeof (TElement), typeof (TResult)), source.Expression, (Expression) Expression.Quote((Expression) keySelector), (Expression) Expression.Quote((Expression) elementSelector), (Expression) Expression.Quote((Expression) resultSelector), (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TKey>))));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Concat<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IQueryable<TFirst> source1, IEnumerable<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      if (resultSelector == null)
        throw Error.ArgumentNull("resultSelector");
      return source1.Provider.CreateQuery<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TFirst), typeof (TSecond), typeof (TResult)), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSecond>(source2),
        (Expression) Expression.Quote((Expression) resultSelector)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource ElementAt<TSource>(this IQueryable<TSource> source, int index)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (index < 0)
        throw Error.ArgumentOutOfRange("index");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) index)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource ElementAtOrDefault<TSource>(this IQueryable<TSource> source, int index)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) index)
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> DefaultIfEmpty<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> DefaultIfEmpty<TSource>(this IQueryable<TSource> source, TSource defaultValue)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) defaultValue, typeof (TSource))
      }));
    }

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Constant((object) item, typeof (TSource))
      }));
    }

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Constant((object) item, typeof (TSource)),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static IQueryable<TSource> Reverse<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.CreateQuery<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2)
      }));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
    {
      if (source1 == null)
        throw Error.ArgumentNull("source1");
      if (source2 == null)
        throw Error.ArgumentNull("source2");
      return source1.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[3]
      {
        source1.Expression,
        Queryable.GetSourceExpression<TSource>(source2),
        (Expression) Expression.Constant((object) comparer, typeof (IEqualityComparer<TSource>))
      }));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static bool All<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<bool>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (predicate == null)
        throw Error.ArgumentNull("predicate");
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) predicate)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Min<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TResult Min<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TResult)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Max<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static TResult Max<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TResult)), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static int Sum(this IQueryable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static int? Sum(this IQueryable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<int?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static long Sum(this IQueryable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static long? Sum(this IQueryable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<long?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static float Sum(this IQueryable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static float? Sum(this IQueryable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double Sum(this IQueryable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double? Sum(this IQueryable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal Sum(this IQueryable<Decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum(this IQueryable<Decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static int Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<int>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static int? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<int?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static long Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<long>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static long? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<long?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Decimal>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Decimal?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Average(this IQueryable<int> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average(this IQueryable<int?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double Average(this IQueryable<long> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average(this IQueryable<long?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static float Average(this IQueryable<float> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static float? Average(this IQueryable<float?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double Average(this IQueryable<double> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average(this IQueryable<double?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal Average(this IQueryable<Decimal> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal? Average(this IQueryable<Decimal?> source)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetCurrentMethod(), new Expression[1]
      {
        source.Expression
      }));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<float>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static float? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<float?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<double?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Decimal>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<Decimal>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static Decimal? Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Decimal?>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<Decimal?>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) selector)
      }));
    }

    [__DynamicallyInvokable]
    public static TSource Aggregate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> func)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (func == null)
        throw Error.ArgumentNull("func");
      return source.Provider.Execute<TSource>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[1]
      {
        typeof (TSource)
      }), new Expression[2]
      {
        source.Expression,
        (Expression) Expression.Quote((Expression) func)
      }));
    }

    [__DynamicallyInvokable]
    public static TAccumulate Aggregate<TSource, TAccumulate>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (func == null)
        throw Error.ArgumentNull("func");
      return source.Provider.Execute<TAccumulate>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TAccumulate)), new Expression[3]
      {
        source.Expression,
        (Expression) Expression.Constant((object) seed),
        (Expression) Expression.Quote((Expression) func)
      }));
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(this IQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector)
    {
      if (source == null)
        throw Error.ArgumentNull("source");
      if (func == null)
        throw Error.ArgumentNull("func");
      if (selector == null)
        throw Error.ArgumentNull("selector");
      return source.Provider.Execute<TResult>((Expression) Expression.Call((Expression) null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof (TSource), typeof (TAccumulate), typeof (TResult)), new Expression[4]
      {
        source.Expression,
        (Expression) Expression.Constant((object) seed),
        (Expression) Expression.Quote((Expression) func),
        (Expression) Expression.Quote((Expression) selector)
      }));
    }
  }
}
