// Type: System.Linq.Parallel.NullableLongAverageAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableLongAverageAggregationOperator : InlinedAggregationOperator<long?, Pair<long, long>, double?>
  {
    internal NullableLongAverageAggregationOperator(IEnumerable<long?> child)
      : base(child)
    {
    }

    protected override double? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Pair<long, long>> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
          return new double?();
        Pair<long, long> current = enumerator.Current;
        while (enumerator.MoveNext())
        {
          checked { current.First += enumerator.Current.First; }
          checked { current.Second += enumerator.Current.Second; }
        }
        return new double?((double) current.First / (double) current.Second);
      }
    }

    protected override QueryOperatorEnumerator<Pair<long, long>, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<long?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Pair<long, long>, int>) new NullableLongAverageAggregationOperator.NullableLongAverageAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class NullableLongAverageAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Pair<long, long>>
    {
      private QueryOperatorEnumerator<long?, TKey> m_source;

      internal NullableLongAverageAggregationOperatorEnumerator(QueryOperatorEnumerator<long?, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref Pair<long, long> currentElement)
      {
        long first = 0L;
        long second = 0L;
        QueryOperatorEnumerator<long?, TKey> operatorEnumerator = this.m_source;
        long? currentElement1 = new long?();
        TKey currentKey = default (TKey);
        int num = 0;
        while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (currentElement1.HasValue)
          {
            first += currentElement1.GetValueOrDefault();
            ++second;
          }
        }
        currentElement = new Pair<long, long>(first, second);
        return second > 0L;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
