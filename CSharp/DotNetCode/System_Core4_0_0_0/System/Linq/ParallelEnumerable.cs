// Type: System.Linq.ParallelEnumerable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Parallel;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public static class ParallelEnumerable
  {
    private const string RIGHT_SOURCE_NOT_PARALLEL_STR = "The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.";

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsParallel<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery<TSource>) new ParallelEnumerableWrapper<TSource>(source);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsParallel<TSource>(this Partitioner<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery<TSource>) new PartitionerQueryOperator<TSource>(source);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsOrdered<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (!(source is ParallelEnumerableWrapper<TSource>) && !(source is IParallelPartitionable<TSource>))
      {
        PartitionerQueryOperator<TSource> partitionerQueryOperator = source as PartitionerQueryOperator<TSource>;
        if (partitionerQueryOperator == null)
          throw new InvalidOperationException(SR.GetString("ParallelQuery_InvalidAsOrderedCall"));
        if (!partitionerQueryOperator.Orderable)
          throw new InvalidOperationException(SR.GetString("ParallelQuery_PartitionerNotOrderable"));
      }
      return (ParallelQuery<TSource>) new OrderingQueryOperator<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), true);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery AsOrdered(this ParallelQuery source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      ParallelEnumerableWrapper enumerableWrapper = source as ParallelEnumerableWrapper;
      if (enumerableWrapper == null)
        throw new InvalidOperationException(SR.GetString("ParallelQuery_InvalidNonGenericAsOrderedCall"));
      else
        return (ParallelQuery) new OrderingQueryOperator<object>(QueryOperator<object>.AsQueryOperator((IEnumerable<object>) enumerableWrapper), true);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> AsUnordered<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery<TSource>) new OrderingQueryOperator<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), false);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery AsParallel(this IEnumerable source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery) new ParallelEnumerableWrapper(source);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<TSource> AsSequential<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      ParallelEnumerableWrapper<TSource> enumerableWrapper = source as ParallelEnumerableWrapper<TSource>;
      if (enumerableWrapper != null)
        return enumerableWrapper.WrappedEnumerable;
      else
        return (IEnumerable<TSource>) source;
    }

    internal static ParallelQuery<TSource> WithTaskScheduler<TSource>(this ParallelQuery<TSource> source, TaskScheduler taskScheduler)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (taskScheduler == null)
        throw new ArgumentNullException("taskScheduler");
      QuerySettings empty = QuerySettings.Empty;
      empty.TaskScheduler = taskScheduler;
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), empty);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithDegreeOfParallelism<TSource>(this ParallelQuery<TSource> source, int degreeOfParallelism)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (degreeOfParallelism < 1 || degreeOfParallelism > 512)
        throw new ArgumentOutOfRangeException("degreeOfParallelism");
      QuerySettings empty = QuerySettings.Empty;
      empty.DegreeOfParallelism = new int?(degreeOfParallelism);
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), empty);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithCancellation<TSource>(this ParallelQuery<TSource> source, CancellationToken cancellationToken)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      CancellationTokenRegistration tokenRegistration = new CancellationTokenRegistration();
      try
      {
        tokenRegistration = cancellationToken.Register((Action) (() => {}));
      }
      catch (ObjectDisposedException ex)
      {
        throw new ArgumentException(SR.GetString("ParallelEnumerable_WithCancellation_TokenSourceDisposed"), "cancellationToken");
      }
      finally
      {
        tokenRegistration.Dispose();
      }
      QuerySettings empty = QuerySettings.Empty;
      empty.CancellationState = new CancellationState(cancellationToken);
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), empty);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithExecutionMode<TSource>(this ParallelQuery<TSource> source, ParallelExecutionMode executionMode)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (executionMode != ParallelExecutionMode.Default && executionMode != ParallelExecutionMode.ForceParallelism)
        throw new ArgumentException(SR.GetString("ParallelEnumerable_WithQueryExecutionMode_InvalidMode"));
      QuerySettings empty = QuerySettings.Empty;
      empty.ExecutionMode = new ParallelExecutionMode?(executionMode);
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), empty);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> WithMergeOptions<TSource>(this ParallelQuery<TSource> source, ParallelMergeOptions mergeOptions)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (mergeOptions != ParallelMergeOptions.Default && mergeOptions != ParallelMergeOptions.AutoBuffered && (mergeOptions != ParallelMergeOptions.NotBuffered && mergeOptions != ParallelMergeOptions.FullyBuffered))
        throw new ArgumentException(SR.GetString("ParallelEnumerable_WithMergeOptions_InvalidOptions"));
      QuerySettings empty = QuerySettings.Empty;
      empty.MergeOptions = new ParallelMergeOptions?(mergeOptions);
      return (ParallelQuery<TSource>) new QueryExecutionOption<TSource>(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) source), empty);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<int> Range(int start, int count)
    {
      if (count < 0 || count > 0 && int.MaxValue - (count - 1) < start)
        throw new ArgumentOutOfRangeException("count");
      else
        return (ParallelQuery<int>) new RangeEnumerable(start, count);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Repeat<TResult>(TResult element, int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("count");
      else
        return (ParallelQuery<TResult>) new RepeatEnumerable<TResult>(element, count);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Empty<TResult>()
    {
      return (ParallelQuery<TResult>) EmptyEnumerable<TResult>.Instance;
    }

    [__DynamicallyInvokable]
    public static void ForAll<TSource>(this ParallelQuery<TSource> source, Action<TSource> action)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (action == null)
        throw new ArgumentNullException("action");
      new ForAllOperator<TSource>((IEnumerable<TSource>) source, action).RunSynchronously();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Where<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return (ParallelQuery<TSource>) new WhereQueryOperator<TSource>((IEnumerable<TSource>) source, predicate);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Where<TSource>(this ParallelQuery<TSource> source, Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return (ParallelQuery<TSource>) new IndexedWhereQueryOperator<TSource>((IEnumerable<TSource>) source, predicate);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Select<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (selector == null)
        throw new ArgumentNullException("selector");
      else
        return (ParallelQuery<TResult>) new SelectQueryOperator<TSource, TResult>((IEnumerable<TSource>) source, selector);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Select<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, int, TResult> selector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (selector == null)
        throw new ArgumentNullException("selector");
      else
        return (ParallelQuery<TResult>) new IndexedSelectQueryOperator<TSource, TResult>((IEnumerable<TSource>) source, selector);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Zip<TFirst, TSecond, TResult>(this ParallelQuery<TFirst> first, ParallelQuery<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return (ParallelQuery<TResult>) new ZipQueryOperator<TFirst, TSecond, TResult>(first, (IEnumerable<TSecond>) second, resultSelector);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Zip<TFirst, TSecond, TResult>(this ParallelQuery<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
      return ParallelEnumerable.Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw new ArgumentNullException("outer");
      if (inner == null)
        throw new ArgumentNullException("inner");
      if (outerKeySelector == null)
        throw new ArgumentNullException("outerKeySelector");
      if (innerKeySelector == null)
        throw new ArgumentNullException("innerKeySelector");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return (ParallelQuery<TResult>) new JoinQueryOperator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Join<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
    {
      return ParallelEnumerable.GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, (IEqualityComparer<TKey>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, ParallelQuery<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (outer == null)
        throw new ArgumentNullException("outer");
      if (inner == null)
        throw new ArgumentNullException("inner");
      if (outerKeySelector == null)
        throw new ArgumentNullException("outerKeySelector");
      if (innerKeySelector == null)
        throw new ArgumentNullException("innerKeySelector");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return (ParallelQuery<TResult>) new GroupJoinQueryOperator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this ParallelQuery<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (selector == null)
        throw new ArgumentNullException("selector");
      else
        return (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TResult, TResult>((IEnumerable<TSource>) source, selector, (Func<TSource, int, IEnumerable<TResult>>) null, (Func<TSource, TResult, TResult>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (selector == null)
        throw new ArgumentNullException("selector");
      else
        return (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TResult, TResult>((IEnumerable<TSource>) source, (Func<TSource, IEnumerable<TResult>>) null, selector, (Func<TSource, TResult, TResult>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TCollection, TResult>(this ParallelQuery<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (collectionSelector == null)
        throw new ArgumentNullException("collectionSelector");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TCollection, TResult>((IEnumerable<TSource>) source, collectionSelector, (Func<TSource, int, IEnumerable<TCollection>>) null, resultSelector);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> SelectMany<TSource, TCollection, TResult>(this ParallelQuery<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (collectionSelector == null)
        throw new ArgumentNullException("collectionSelector");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return (ParallelQuery<TResult>) new SelectManyQueryOperator<TSource, TCollection, TResult>((IEnumerable<TSource>) source, (Func<TSource, IEnumerable<TCollection>>) null, collectionSelector, resultSelector);
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, (IComparer<TKey>) null, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, comparer, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderByDescending<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, (IComparer<TKey>) null, true));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> OrderByDescending<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) new SortQueryOperator<TSource, TKey>((IEnumerable<TSource>) source, keySelector, comparer, true));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenBy<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, (IComparer<TKey>) null, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenBy<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, comparer, false));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenByDescending<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, (IComparer<TKey>) null, true));
    }

    [__DynamicallyInvokable]
    public static OrderedParallelQuery<TSource> ThenByDescending<TSource, TKey>(this OrderedParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return new OrderedParallelQuery<TSource>((QueryOperator<TSource>) source.OrderedEnumerable.CreateOrderedEnumerable<TKey>(keySelector, comparer, true));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      return ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      else
        return (ParallelQuery<IGrouping<TKey, TSource>>) new GroupByQueryOperator<TSource, TKey, TSource>((IEnumerable<TSource>) source, keySelector, (Func<TSource, TSource>) null, comparer);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
      return ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      if (elementSelector == null)
        throw new ArgumentNullException("elementSelector");
      else
        return (ParallelQuery<IGrouping<TKey, TElement>>) new GroupByQueryOperator<TSource, TKey, TElement>((IEnumerable<TSource>) source, keySelector, elementSelector, comparer);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
    {
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return ParallelEnumerable.Select<IGrouping<TKey, TSource>, TResult>(ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector), (Func<IGrouping<TKey, TSource>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TSource>) grouping)));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return ParallelEnumerable.Select<IGrouping<TKey, TSource>, TResult>(ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector, comparer), (Func<IGrouping<TKey, TSource>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TSource>) grouping)));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return ParallelEnumerable.Select<IGrouping<TKey, TElement>, TResult>(ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector), (Func<IGrouping<TKey, TElement>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TElement>) grouping)));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> GroupBy<TSource, TKey, TElement, TResult>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return ParallelEnumerable.Select<IGrouping<TKey, TElement>, TResult>(ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer), (Func<IGrouping<TKey, TElement>, TResult>) (grouping => resultSelector(grouping.Key, (IEnumerable<TElement>) grouping)));
    }

    private static T PerformAggregation<T>(this ParallelQuery<T> source, Func<T, T, T> reduce, T seed, bool seedIsSpecified, bool throwIfEmpty, QueryAggregationOptions options)
    {
      return new AssociativeAggregationOperator<T, T, T>((IEnumerable<T>) source, seed, (Func<T>) null, seedIsSpecified, reduce, reduce, (Func<T, T>) (obj => obj), throwIfEmpty, options).Aggregate();
    }

    private static TAccumulate PerformSequentialAggregation<TSource, TAccumulate>(this ParallelQuery<TSource> source, TAccumulate seed, bool seedIsSpecified, Func<TAccumulate, TSource, TAccumulate> func)
    {
      using (IEnumerator<TSource> enumerator = source.GetEnumerator())
      {
        TAccumulate accumulate;
        if (seedIsSpecified)
        {
          accumulate = seed;
        }
        else
        {
          if (!enumerator.MoveNext())
            throw new InvalidOperationException(SR.GetString("NoElements"));
          accumulate = (TAccumulate) (object) enumerator.Current;
        }
        while (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          try
          {
            accumulate = func(accumulate, current);
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new AggregateException(new Exception[1]
            {
              ex
            });
          }
        }
        return accumulate;
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static TSource Aggregate<TSource>(this ParallelQuery<TSource> source, Func<TSource, TSource, TSource> func)
    {
      return ParallelEnumerable.Aggregate<TSource>(source, func, QueryAggregationOptions.AssociativeCommutative);
    }

    internal static TSource Aggregate<TSource>(this ParallelQuery<TSource> source, Func<TSource, TSource, TSource> func, QueryAggregationOptions options)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (func == null)
        throw new ArgumentNullException("func");
      if ((~QueryAggregationOptions.AssociativeCommutative & options) != QueryAggregationOptions.None)
        throw new ArgumentOutOfRangeException("options");
      if ((options & QueryAggregationOptions.Associative) != QueryAggregationOptions.Associative)
        return ParallelEnumerable.PerformSequentialAggregation<TSource, TSource>(source, default (TSource), false, func);
      else
        return ParallelEnumerable.PerformAggregation<TSource>(source, func, default (TSource), false, true, options);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static TAccumulate Aggregate<TSource, TAccumulate>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
    {
      return ParallelEnumerable.Aggregate<TSource, TAccumulate>(source, seed, func, QueryAggregationOptions.AssociativeCommutative);
    }

    internal static TAccumulate Aggregate<TSource, TAccumulate>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, QueryAggregationOptions options)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (func == null)
        throw new ArgumentNullException("func");
      if ((~QueryAggregationOptions.AssociativeCommutative & options) != QueryAggregationOptions.None)
        throw new ArgumentOutOfRangeException("options");
      else
        return ParallelEnumerable.PerformSequentialAggregation<TSource, TAccumulate>(source, seed, true, func);
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (func == null)
        throw new ArgumentNullException("func");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      TAccumulate accumulate = ParallelEnumerable.PerformSequentialAggregation<TSource, TAccumulate>(source, seed, true, func);
      try
      {
        return resultSelector(accumulate);
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new AggregateException(new Exception[1]
        {
          ex
        });
      }
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(this ParallelQuery<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> updateAccumulatorFunc, Func<TAccumulate, TAccumulate, TAccumulate> combineAccumulatorsFunc, Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (updateAccumulatorFunc == null)
        throw new ArgumentNullException("updateAccumulatorFunc");
      if (combineAccumulatorsFunc == null)
        throw new ArgumentNullException("combineAccumulatorsFunc");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return new AssociativeAggregationOperator<TSource, TAccumulate, TResult>((IEnumerable<TSource>) source, seed, (Func<TAccumulate>) null, true, updateAccumulatorFunc, combineAccumulatorsFunc, resultSelector, false, QueryAggregationOptions.AssociativeCommutative).Aggregate();
    }

    [__DynamicallyInvokable]
    public static TResult Aggregate<TSource, TAccumulate, TResult>(this ParallelQuery<TSource> source, Func<TAccumulate> seedFactory, Func<TAccumulate, TSource, TAccumulate> updateAccumulatorFunc, Func<TAccumulate, TAccumulate, TAccumulate> combineAccumulatorsFunc, Func<TAccumulate, TResult> resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (seedFactory == null)
        throw new ArgumentNullException("seedFactory");
      if (updateAccumulatorFunc == null)
        throw new ArgumentNullException("updateAccumulatorFunc");
      if (combineAccumulatorsFunc == null)
        throw new ArgumentNullException("combineAccumulatorsFunc");
      if (resultSelector == null)
        throw new ArgumentNullException("resultSelector");
      else
        return new AssociativeAggregationOperator<TSource, TAccumulate, TResult>((IEnumerable<TSource>) source, default (TAccumulate), seedFactory, true, updateAccumulatorFunc, combineAccumulatorsFunc, resultSelector, false, QueryAggregationOptions.AssociativeCommutative).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      ParallelEnumerableWrapper<TSource> enumerableWrapper = source as ParallelEnumerableWrapper<TSource>;
      if (enumerableWrapper != null)
      {
        ICollection<TSource> collection = enumerableWrapper.WrappedEnumerable as ICollection<TSource>;
        if (collection != null)
          return collection.Count;
      }
      return new CountAggregationOperator<TSource>((IEnumerable<TSource>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int Count<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return new CountAggregationOperator<TSource>((IEnumerable<TSource>) ParallelEnumerable.Where<TSource>(source, predicate)).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      ParallelEnumerableWrapper<TSource> enumerableWrapper = source as ParallelEnumerableWrapper<TSource>;
      if (enumerableWrapper != null)
      {
        ICollection<TSource> collection = enumerableWrapper.WrappedEnumerable as ICollection<TSource>;
        if (collection != null)
          return (long) collection.Count;
      }
      return new LongCountAggregationOperator<TSource>((IEnumerable<TSource>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long LongCount<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return new LongCountAggregationOperator<TSource>((IEnumerable<TSource>) ParallelEnumerable.Where<TSource>(source, predicate)).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int Sum(this ParallelQuery<int> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new IntSumAggregationOperator((IEnumerable<int>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int? Sum(this ParallelQuery<int?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableIntSumAggregationOperator((IEnumerable<int?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long Sum(this ParallelQuery<long> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new LongSumAggregationOperator((IEnumerable<long>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long? Sum(this ParallelQuery<long?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableLongSumAggregationOperator((IEnumerable<long?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float Sum(this ParallelQuery<float> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new FloatSumAggregationOperator((IEnumerable<float>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float? Sum(this ParallelQuery<float?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableFloatSumAggregationOperator((IEnumerable<float?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double Sum(this ParallelQuery<double> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DoubleSumAggregationOperator((IEnumerable<double>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double? Sum(this ParallelQuery<double?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDoubleSumAggregationOperator((IEnumerable<double?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal Sum(this ParallelQuery<Decimal> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DecimalSumAggregationOperator((IEnumerable<Decimal>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum(this ParallelQuery<Decimal?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDecimalSumAggregationOperator((IEnumerable<Decimal?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, int>(source, selector));
    }

    [__DynamicallyInvokable]
    public static int? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, int?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, long>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, long?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, float>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, float?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, double>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, double?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, Decimal>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal? Sum<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Sum(ParallelEnumerable.Select<TSource, Decimal?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static int Min(this ParallelQuery<int> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new IntMinMaxAggregationOperator((IEnumerable<int>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int? Min(this ParallelQuery<int?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableIntMinMaxAggregationOperator((IEnumerable<int?>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long Min(this ParallelQuery<long> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new LongMinMaxAggregationOperator((IEnumerable<long>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long? Min(this ParallelQuery<long?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableLongMinMaxAggregationOperator((IEnumerable<long?>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float Min(this ParallelQuery<float> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new FloatMinMaxAggregationOperator((IEnumerable<float>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float? Min(this ParallelQuery<float?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableFloatMinMaxAggregationOperator((IEnumerable<float?>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double Min(this ParallelQuery<double> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DoubleMinMaxAggregationOperator((IEnumerable<double>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double? Min(this ParallelQuery<double?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDoubleMinMaxAggregationOperator((IEnumerable<double?>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal Min(this ParallelQuery<Decimal> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DecimalMinMaxAggregationOperator((IEnumerable<Decimal>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal? Min(this ParallelQuery<Decimal?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDecimalMinMaxAggregationOperator((IEnumerable<Decimal?>) source, -1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static TSource Min<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return AggregationMinMaxHelpers<TSource>.ReduceMin((IEnumerable<TSource>) source);
    }

    [__DynamicallyInvokable]
    public static int Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector)
    {
      return ParallelEnumerable.Min<int>(ParallelEnumerable.Select<TSource, int>(source, selector));
    }

    [__DynamicallyInvokable]
    public static int? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Min<int?>(ParallelEnumerable.Select<TSource, int?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector)
    {
      return ParallelEnumerable.Min<long>(ParallelEnumerable.Select<TSource, long>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Min<long?>(ParallelEnumerable.Select<TSource, long?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector)
    {
      return ParallelEnumerable.Min<float>(ParallelEnumerable.Select<TSource, float>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Min<float?>(ParallelEnumerable.Select<TSource, float?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector)
    {
      return ParallelEnumerable.Min<double>(ParallelEnumerable.Select<TSource, double>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Min<double?>(ParallelEnumerable.Select<TSource, double?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Min<Decimal>(ParallelEnumerable.Select<TSource, Decimal>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal? Min<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Min<Decimal?>(ParallelEnumerable.Select<TSource, Decimal?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static TResult Min<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector)
    {
      return ParallelEnumerable.Min<TResult>(ParallelEnumerable.Select<TSource, TResult>(source, selector));
    }

    [__DynamicallyInvokable]
    public static int Max(this ParallelQuery<int> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new IntMinMaxAggregationOperator((IEnumerable<int>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static int? Max(this ParallelQuery<int?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableIntMinMaxAggregationOperator((IEnumerable<int?>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long Max(this ParallelQuery<long> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new LongMinMaxAggregationOperator((IEnumerable<long>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static long? Max(this ParallelQuery<long?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableLongMinMaxAggregationOperator((IEnumerable<long?>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float Max(this ParallelQuery<float> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new FloatMinMaxAggregationOperator((IEnumerable<float>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float? Max(this ParallelQuery<float?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableFloatMinMaxAggregationOperator((IEnumerable<float?>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double Max(this ParallelQuery<double> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DoubleMinMaxAggregationOperator((IEnumerable<double>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double? Max(this ParallelQuery<double?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDoubleMinMaxAggregationOperator((IEnumerable<double?>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal Max(this ParallelQuery<Decimal> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DecimalMinMaxAggregationOperator((IEnumerable<Decimal>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal? Max(this ParallelQuery<Decimal?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDecimalMinMaxAggregationOperator((IEnumerable<Decimal?>) source, 1).Aggregate();
    }

    [__DynamicallyInvokable]
    public static TSource Max<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return AggregationMinMaxHelpers<TSource>.ReduceMax((IEnumerable<TSource>) source);
    }

    [__DynamicallyInvokable]
    public static int Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector)
    {
      return ParallelEnumerable.Max<int>(ParallelEnumerable.Select<TSource, int>(source, selector));
    }

    [__DynamicallyInvokable]
    public static int? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Max<int?>(ParallelEnumerable.Select<TSource, int?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector)
    {
      return ParallelEnumerable.Max<long>(ParallelEnumerable.Select<TSource, long>(source, selector));
    }

    [__DynamicallyInvokable]
    public static long? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Max<long?>(ParallelEnumerable.Select<TSource, long?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector)
    {
      return ParallelEnumerable.Max<float>(ParallelEnumerable.Select<TSource, float>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Max<float?>(ParallelEnumerable.Select<TSource, float?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector)
    {
      return ParallelEnumerable.Max<double>(ParallelEnumerable.Select<TSource, double>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Max<double?>(ParallelEnumerable.Select<TSource, double?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Max<Decimal>(ParallelEnumerable.Select<TSource, Decimal>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal? Max<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Max<Decimal?>(ParallelEnumerable.Select<TSource, Decimal?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static TResult Max<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector)
    {
      return ParallelEnumerable.Max<TResult>(ParallelEnumerable.Select<TSource, TResult>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Average(this ParallelQuery<int> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new IntAverageAggregationOperator((IEnumerable<int>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double? Average(this ParallelQuery<int?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableIntAverageAggregationOperator((IEnumerable<int?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double Average(this ParallelQuery<long> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new LongAverageAggregationOperator((IEnumerable<long>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double? Average(this ParallelQuery<long?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableLongAverageAggregationOperator((IEnumerable<long?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float Average(this ParallelQuery<float> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new FloatAverageAggregationOperator((IEnumerable<float>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static float? Average(this ParallelQuery<float?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableFloatAverageAggregationOperator((IEnumerable<float?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double Average(this ParallelQuery<double> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DoubleAverageAggregationOperator((IEnumerable<double>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double? Average(this ParallelQuery<double?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDoubleAverageAggregationOperator((IEnumerable<double?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal Average(this ParallelQuery<Decimal> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new DecimalAverageAggregationOperator((IEnumerable<Decimal>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static Decimal? Average(this ParallelQuery<Decimal?> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new NullableDecimalAverageAggregationOperator((IEnumerable<Decimal?>) source).Aggregate();
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, int> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, int>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, int?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, int?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, long> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, long>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, long?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, long?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, float> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, float>(source, selector));
    }

    [__DynamicallyInvokable]
    public static float? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, float?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, float?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, double> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, double>(source, selector));
    }

    [__DynamicallyInvokable]
    public static double? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, double?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, double?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, Decimal>(source, selector));
    }

    [__DynamicallyInvokable]
    public static Decimal? Average<TSource>(this ParallelQuery<TSource> source, Func<TSource, Decimal?> selector)
    {
      return ParallelEnumerable.Average(ParallelEnumerable.Select<TSource, Decimal?>(source, selector));
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return new AnyAllSearchOperator<TSource>((IEnumerable<TSource>) source, true, predicate).Aggregate();
    }

    [__DynamicallyInvokable]
    public static bool Any<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return ParallelEnumerable.Any<TSource>(source, (Func<TSource, bool>) (x => true));
    }

    [__DynamicallyInvokable]
    public static bool All<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return new AnyAllSearchOperator<TSource>((IEnumerable<TSource>) source, false, predicate).Aggregate();
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static bool Contains<TSource>(this ParallelQuery<TSource> source, TSource value)
    {
      return ParallelEnumerable.Contains<TSource>(source, value, (IEqualityComparer<TSource>) null);
    }

    [__DynamicallyInvokable]
    public static bool Contains<TSource>(this ParallelQuery<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return new ContainsSearchOperator<TSource>((IEnumerable<TSource>) source, value, comparer).Aggregate();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Take<TSource>(this ParallelQuery<TSource> source, int count)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (count > 0)
        return (ParallelQuery<TSource>) new TakeOrSkipQueryOperator<TSource>((IEnumerable<TSource>) source, count, true);
      else
        return ParallelEnumerable.Empty<TSource>();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> TakeWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, predicate, (Func<TSource, int, bool>) null, true);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> TakeWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null, predicate, true);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Skip<TSource>(this ParallelQuery<TSource> source, int count)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (count <= 0)
        return source;
      else
        return (ParallelQuery<TSource>) new TakeOrSkipQueryOperator<TSource>((IEnumerable<TSource>) source, count, false);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> SkipWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, predicate, (Func<TSource, int, bool>) null, false);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> SkipWhile<TSource>(this ParallelQuery<TSource> source, Func<TSource, int, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return (ParallelQuery<TSource>) new TakeOrSkipWhileQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null, predicate, false);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Concat<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      else
        return (ParallelQuery<TSource>) new ConcatQueryOperator<TSource>(first, second);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Concat<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      else
        return ParallelEnumerable.SequenceEqual<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      comparer = comparer ?? (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      QuerySettings querySettings = QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) first).SpecifiedQuerySettings.Merge(QueryOperator<TSource>.AsQueryOperator((IEnumerable<TSource>) second).SpecifiedQuerySettings).WithDefaults().WithPerExecutionSettings(new CancellationTokenSource(), new Shared<bool>(false));
      IEnumerator<TSource> enumerator1 = first.GetEnumerator();
      try
      {
        IEnumerator<TSource> enumerator2 = second.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            if (!enumerator2.MoveNext() || !comparer.Equals(enumerator1.Current, enumerator2.Current))
              return false;
          }
          if (enumerator2.MoveNext())
            return false;
        }
        catch (ThreadAbortException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          ExceptionAggregator.ThrowOCEorAggregateException(ex, querySettings.CancellationState);
        }
        finally
        {
          ParallelEnumerable.DisposeEnumerator<TSource>(enumerator2, querySettings.CancellationState);
        }
      }
      finally
      {
        ParallelEnumerable.DisposeEnumerator<TSource>(enumerator1, querySettings.CancellationState);
      }
      return true;
    }

    private static void DisposeEnumerator<TSource>(IEnumerator<TSource> e, CancellationState cancelState)
    {
      try
      {
        e.Dispose();
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        ExceptionAggregator.ThrowOCEorAggregateException(ex, cancelState);
      }
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static bool SequenceEqual<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<TSource> Distinct<TSource>(this ParallelQuery<TSource> source)
    {
      return ParallelEnumerable.Distinct<TSource>(source, (IEqualityComparer<TSource>) null);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Distinct<TSource>(this ParallelQuery<TSource> source, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery<TSource>) new DistinctQueryOperator<TSource>((IEnumerable<TSource>) source, comparer);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second)
    {
      return ParallelEnumerable.Union<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      else
        return (ParallelQuery<TSource>) new UnionQueryOperator<TSource>(first, second, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Union<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second)
    {
      return ParallelEnumerable.Intersect<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      else
        return (ParallelQuery<TSource>) new IntersectQueryOperator<TSource>(first, second, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Intersect<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second)
    {
      return ParallelEnumerable.Except<TSource>(first, second, (IEqualityComparer<TSource>) null);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, ParallelQuery<TSource> second, IEqualityComparer<TSource> comparer)
    {
      if (first == null)
        throw new ArgumentNullException("first");
      if (second == null)
        throw new ArgumentNullException("second");
      else
        return (ParallelQuery<TSource>) new ExceptQueryOperator<TSource>(first, second, comparer);
    }

    [Obsolete("The second data source of a binary operator must be of type System.Linq.ParallelQuery<T> rather than System.Collections.Generic.IEnumerable<T>. To fix this problem, use the AsParallel() extension method to convert the right data source to System.Linq.ParallelQuery<T>.")]
    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Except<TSource>(this ParallelQuery<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
      throw new NotSupportedException(SR.GetString("ParallelEnumerable_BinaryOpMustUseAsParallel"));
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public static IEnumerable<TSource> AsEnumerable<TSource>(this ParallelQuery<TSource> source)
    {
      return ParallelEnumerable.AsSequential<TSource>(source);
    }

    [__DynamicallyInvokable]
    public static TSource[] ToArray<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      QueryOperator<TSource> queryOperator = source as QueryOperator<TSource>;
      if (queryOperator != null)
        return queryOperator.ExecuteAndGetResultsAsArray();
      else
        return Enumerable.ToArray<TSource>((IEnumerable<TSource>) ParallelEnumerable.ToList<TSource>(source));
    }

    [__DynamicallyInvokable]
    public static List<TSource> ToList<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      List<TSource> list = new List<TSource>();
      QueryOperator<TSource> queryOperator = source as QueryOperator<TSource>;
      IEnumerator<TSource> enumerator;
      if (queryOperator != null)
      {
        if (queryOperator.OrdinalIndexState == OrdinalIndexState.Indexible && queryOperator.OutputOrdered)
          return new List<TSource>((IEnumerable<TSource>) ParallelEnumerable.ToArray<TSource>(source));
        enumerator = queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered));
      }
      else
        enumerator = source.GetEnumerator();
      using (enumerator)
      {
        while (enumerator.MoveNext())
          list.Add(enumerator.Current);
      }
      return list;
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      return ParallelEnumerable.ToDictionary<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(comparer);
      QueryOperator<TSource> queryOperator = source as QueryOperator<TSource>;
      IEnumerator<TSource> enumerator = queryOperator == null ? source.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true);
      using (enumerator)
      {
        while (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          try
          {
            TKey key = keySelector(current);
            dictionary.Add(key, current);
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new AggregateException(new Exception[1]
            {
              ex
            });
          }
        }
      }
      return dictionary;
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
      return ParallelEnumerable.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      if (elementSelector == null)
        throw new ArgumentNullException("elementSelector");
      Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
      QueryOperator<TSource> queryOperator = source as QueryOperator<TSource>;
      IEnumerator<TSource> enumerator = queryOperator == null ? source.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true);
      using (enumerator)
      {
        while (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          try
          {
            dictionary.Add(keySelector(current), elementSelector(current));
          }
          catch (ThreadAbortException ex)
          {
            throw;
          }
          catch (Exception ex)
          {
            throw new AggregateException(new Exception[1]
            {
              ex
            });
          }
        }
      }
      return dictionary;
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector)
    {
      return ParallelEnumerable.ToLookup<TSource, TKey>(source, keySelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
      ParallelQuery<IGrouping<TKey, TSource>> parallelQuery = ParallelEnumerable.GroupBy<TSource, TKey>(source, keySelector, comparer);
      Lookup<TKey, TSource> lookup = new Lookup<TKey, TSource>(comparer);
      QueryOperator<IGrouping<TKey, TSource>> queryOperator = parallelQuery as QueryOperator<IGrouping<TKey, TSource>>;
      IEnumerator<IGrouping<TKey, TSource>> enumerator = queryOperator == null ? parallelQuery.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered));
      using (enumerator)
      {
        while (enumerator.MoveNext())
          lookup.Add(enumerator.Current);
      }
      return (ILookup<TKey, TSource>) lookup;
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
      return ParallelEnumerable.ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default);
    }

    [__DynamicallyInvokable]
    public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this ParallelQuery<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (keySelector == null)
        throw new ArgumentNullException("keySelector");
      if (elementSelector == null)
        throw new ArgumentNullException("elementSelector");
      comparer = comparer ?? (IEqualityComparer<TKey>) EqualityComparer<TKey>.Default;
      ParallelQuery<IGrouping<TKey, TElement>> parallelQuery = ParallelEnumerable.GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
      Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
      QueryOperator<IGrouping<TKey, TElement>> queryOperator = parallelQuery as QueryOperator<IGrouping<TKey, TElement>>;
      IEnumerator<IGrouping<TKey, TElement>> enumerator = queryOperator == null ? parallelQuery.GetEnumerator() : queryOperator.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered));
      using (enumerator)
      {
        while (enumerator.MoveNext())
          lookup.Add(enumerator.Current);
      }
      return (ILookup<TKey, TElement>) lookup;
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> Reverse<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery<TSource>) new ReverseQueryOperator<TSource>((IEnumerable<TSource>) source);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> OfType<TResult>(this ParallelQuery source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return source.OfType<TResult>();
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TResult> Cast<TResult>(this ParallelQuery source)
    {
      return source.Cast<TResult>();
    }

    private static TSource GetOneWithPossibleDefault<TSource>(QueryOperator<TSource> queryOp, bool throwIfTwo, bool defaultIfEmpty)
    {
      using (IEnumerator<TSource> enumerator = queryOp.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered)))
      {
        if (enumerator.MoveNext())
        {
          TSource current = enumerator.Current;
          if (throwIfTwo && enumerator.MoveNext())
            throw new InvalidOperationException(SR.GetString("MoreThanOneMatch"));
          else
            return current;
        }
      }
      if (defaultIfEmpty)
        return default (TSource);
      else
        throw new InvalidOperationException(SR.GetString("NoElements"));
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      FirstQueryOperator<TSource> firstQueryOperator = new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null);
      QuerySettings querySettings = firstQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (firstQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.First<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(firstQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) firstQueryOperator, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource First<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      FirstQueryOperator<TSource> firstQueryOperator = new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, predicate);
      QuerySettings querySettings = firstQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (firstQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.First<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(firstQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState), ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) firstQueryOperator, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      FirstQueryOperator<TSource> firstQueryOperator = new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null);
      QuerySettings querySettings = firstQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (firstQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.FirstOrDefault<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(firstQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) firstQueryOperator, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource FirstOrDefault<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      FirstQueryOperator<TSource> firstQueryOperator = new FirstQueryOperator<TSource>((IEnumerable<TSource>) source, predicate);
      QuerySettings querySettings = firstQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (firstQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.FirstOrDefault<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(firstQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState), ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) firstQueryOperator, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      LastQueryOperator<TSource> lastQueryOperator = new LastQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null);
      QuerySettings querySettings = lastQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (lastQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.Last<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(lastQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) lastQueryOperator, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource Last<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      LastQueryOperator<TSource> lastQueryOperator = new LastQueryOperator<TSource>((IEnumerable<TSource>) source, predicate);
      QuerySettings querySettings = lastQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (lastQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.Last<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(lastQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState), ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) lastQueryOperator, false, false);
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      LastQueryOperator<TSource> lastQueryOperator = new LastQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null);
      QuerySettings querySettings = lastQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (lastQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.LastOrDefault<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(lastQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) lastQueryOperator, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource LastOrDefault<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      LastQueryOperator<TSource> lastQueryOperator = new LastQueryOperator<TSource>((IEnumerable<TSource>) source, predicate);
      QuerySettings querySettings = lastQueryOperator.SpecifiedQuerySettings.WithDefaults();
      if (lastQueryOperator.LimitsParallelism)
      {
        ParallelExecutionMode? executionMode = querySettings.ExecutionMode;
        if ((executionMode.GetValueOrDefault() != ParallelExecutionMode.ForceParallelism ? 1 : (!executionMode.HasValue ? 1 : 0)) != 0)
          return Enumerable.LastOrDefault<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(CancellableEnumerable.Wrap<TSource>(lastQueryOperator.Child.AsSequentialQuery(querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState.ExternalCancellationToken), querySettings.CancellationState), ExceptionAggregator.WrapFunc<TSource, bool>(predicate, querySettings.CancellationState));
      }
      return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) lastQueryOperator, false, true);
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null), true, false);
    }

    [__DynamicallyInvokable]
    public static TSource Single<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, predicate), true, false);
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(this ParallelQuery<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, (Func<TSource, bool>) null), true, true);
    }

    [__DynamicallyInvokable]
    public static TSource SingleOrDefault<TSource>(this ParallelQuery<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");
      else
        return ParallelEnumerable.GetOneWithPossibleDefault<TSource>((QueryOperator<TSource>) new SingleQueryOperator<TSource>((IEnumerable<TSource>) source, predicate), true, true);
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> DefaultIfEmpty<TSource>(this ParallelQuery<TSource> source)
    {
      return ParallelEnumerable.DefaultIfEmpty<TSource>(source, default (TSource));
    }

    [__DynamicallyInvokable]
    public static ParallelQuery<TSource> DefaultIfEmpty<TSource>(this ParallelQuery<TSource> source, TSource defaultValue)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      else
        return (ParallelQuery<TSource>) new DefaultIfEmptyQueryOperator<TSource>((IEnumerable<TSource>) source, defaultValue);
    }

    [__DynamicallyInvokable]
    public static TSource ElementAt<TSource>(this ParallelQuery<TSource> source, int index)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (index < 0)
        throw new ArgumentOutOfRangeException("index");
      TSource result;
      if (new ElementAtQueryOperator<TSource>((IEnumerable<TSource>) source, index).Aggregate(out result, false))
        return result;
      else
        throw new ArgumentOutOfRangeException("index");
    }

    [__DynamicallyInvokable]
    public static TSource ElementAtOrDefault<TSource>(this ParallelQuery<TSource> source, int index)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      TSource result;
      if (index >= 0 && new ElementAtQueryOperator<TSource>((IEnumerable<TSource>) source, index).Aggregate(out result, true))
        return result;
      else
        return default (TSource);
    }
  }
}
