// Type: System.Linq.Parallel.NullableDecimalAverageAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableDecimalAverageAggregationOperator : InlinedAggregationOperator<Decimal?, Pair<Decimal, long>, Decimal?>
  {
    internal NullableDecimalAverageAggregationOperator(IEnumerable<Decimal?> child)
      : base(child)
    {
    }

    protected override Decimal? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Pair<Decimal, long>> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
          return new Decimal?();
        Pair<Decimal, long> current = enumerator.Current;
        while (enumerator.MoveNext())
        {
          current.First += enumerator.Current.First;
          checked { current.Second += enumerator.Current.Second; }
        }
        return new Decimal?(current.First / (Decimal) current.Second);
      }
    }

    protected override QueryOperatorEnumerator<Pair<Decimal, long>, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<Decimal?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Pair<Decimal, long>, int>) new NullableDecimalAverageAggregationOperator.NullableDecimalAverageAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class NullableDecimalAverageAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Pair<Decimal, long>>
    {
      private QueryOperatorEnumerator<Decimal?, TKey> m_source;

      internal NullableDecimalAverageAggregationOperatorEnumerator(QueryOperatorEnumerator<Decimal?, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref Pair<Decimal, long> currentElement)
      {
        Decimal first = new Decimal(0, 0, 0, false, (byte) 1);
        long second = 0L;
        QueryOperatorEnumerator<Decimal?, TKey> operatorEnumerator = this.m_source;
        Decimal? currentElement1 = new Decimal?();
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
        currentElement = new Pair<Decimal, long>(first, second);
        return second > 0L;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
