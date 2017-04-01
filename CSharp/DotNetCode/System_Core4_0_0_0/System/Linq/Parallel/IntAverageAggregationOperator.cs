// Type: System.Linq.Parallel.IntAverageAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class IntAverageAggregationOperator : InlinedAggregationOperator<int, Pair<long, long>, double>
  {
    internal IntAverageAggregationOperator(IEnumerable<int> child)
      : base(child)
    {
    }

    protected override double InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Pair<long, long>> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
        {
          singularExceptionToThrow = (Exception) new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          return 0.0;
        }
        else
        {
          Pair<long, long> current = enumerator.Current;
          while (enumerator.MoveNext())
          {
            checked { current.First += enumerator.Current.First; }
            checked { current.Second += enumerator.Current.Second; }
          }
          return (double) current.First / (double) current.Second;
        }
      }
    }

    protected override QueryOperatorEnumerator<Pair<long, long>, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<int, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Pair<long, long>, int>) new IntAverageAggregationOperator.IntAverageAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class IntAverageAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Pair<long, long>>
    {
      private QueryOperatorEnumerator<int, TKey> m_source;

      internal IntAverageAggregationOperatorEnumerator(QueryOperatorEnumerator<int, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref Pair<long, long> currentElement)
      {
        long first = 0L;
        long second = 0L;
        QueryOperatorEnumerator<int, TKey> operatorEnumerator = this.m_source;
        int currentElement1 = 0;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        int num = 0;
        do
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          checked { first += (long) currentElement1; }
          checked { ++second; }
        }
        while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey));
        currentElement = new Pair<long, long>(first, second);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
