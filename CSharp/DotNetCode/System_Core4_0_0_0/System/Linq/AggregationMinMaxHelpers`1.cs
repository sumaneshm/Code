// Type: System.Linq.AggregationMinMaxHelpers`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq.Parallel;
using System.Runtime;

namespace System.Linq
{
  internal static class AggregationMinMaxHelpers<T>
  {
    private static T Reduce(IEnumerable<T> source, int sign)
    {
      Func<Pair<bool, T>, T, Pair<bool, T>> intermediateReduce = AggregationMinMaxHelpers<T>.MakeIntermediateReduceFunction(sign);
      Func<Pair<bool, T>, Pair<bool, T>, Pair<bool, T>> finalReduce = AggregationMinMaxHelpers<T>.MakeFinalReduceFunction(sign);
      Func<Pair<bool, T>, T> resultSelector = AggregationMinMaxHelpers<T>.MakeResultSelectorFunction();
      return new AssociativeAggregationOperator<T, Pair<bool, T>, T>(source, new Pair<bool, T>(false, default (T)), (Func<Pair<bool, T>>) null, true, intermediateReduce, finalReduce, resultSelector, (object) default (T) != null, QueryAggregationOptions.AssociativeCommutative).Aggregate();
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal static T ReduceMin(IEnumerable<T> source)
    {
      return AggregationMinMaxHelpers<T>.Reduce(source, -1);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal static T ReduceMax(IEnumerable<T> source)
    {
      return AggregationMinMaxHelpers<T>.Reduce(source, 1);
    }

    private static Func<Pair<bool, T>, T, Pair<bool, T>> MakeIntermediateReduceFunction(int sign)
    {
      Comparer<T> comparer = Util.GetDefaultComparer<T>();
      return (Func<Pair<bool, T>, T, Pair<bool, T>>) ((accumulator, element) =>
      {
        if (((object) default (T) != null || (object) element != null) && (!accumulator.First || Util.Sign(comparer.Compare(element, accumulator.Second)) == sign))
          return new Pair<bool, T>(true, element);
        else
          return accumulator;
      });
    }

    private static Func<Pair<bool, T>, Pair<bool, T>, Pair<bool, T>> MakeFinalReduceFunction(int sign)
    {
      Comparer<T> comparer = Util.GetDefaultComparer<T>();
      return (Func<Pair<bool, T>, Pair<bool, T>, Pair<bool, T>>) ((accumulator, element) =>
      {
        if (element.First && (!accumulator.First || Util.Sign(comparer.Compare(element.Second, accumulator.Second)) == sign))
          return new Pair<bool, T>(true, element.Second);
        else
          return accumulator;
      });
    }

    private static Func<Pair<bool, T>, T> MakeResultSelectorFunction()
    {
      return (Func<Pair<bool, T>, T>) (accumulator => accumulator.Second);
    }
  }
}
