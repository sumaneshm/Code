// Type: System.Linq.Parallel.DecimalAverageAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class DecimalAverageAggregationOperator : InlinedAggregationOperator<Decimal, Pair<Decimal, long>, Decimal>
  {
    internal DecimalAverageAggregationOperator(IEnumerable<Decimal> child)
      : base(child)
    {
    }

    protected override Decimal InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Pair<Decimal, long>> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
        {
          singularExceptionToThrow = (Exception) new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          return new Decimal(0);
        }
        else
        {
          Pair<Decimal, long> current = enumerator.Current;
          while (enumerator.MoveNext())
          {
            current.First += enumerator.Current.First;
            checked { current.Second += enumerator.Current.Second; }
          }
          return current.First / (Decimal) current.Second;
        }
      }
    }

    protected override QueryOperatorEnumerator<Pair<Decimal, long>, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<Decimal, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Pair<Decimal, long>, int>) new DecimalAverageAggregationOperator.DecimalAverageAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class DecimalAverageAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Pair<Decimal, long>>
    {
      private QueryOperatorEnumerator<Decimal, TKey> m_source;

      internal DecimalAverageAggregationOperatorEnumerator(QueryOperatorEnumerator<Decimal, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref Pair<Decimal, long> currentElement)
      {
        Decimal first = new Decimal(0, 0, 0, false, (byte) 1);
        long second = 0L;
        QueryOperatorEnumerator<Decimal, TKey> operatorEnumerator = this.m_source;
        Decimal currentElement1 = new Decimal(0);
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        int num = 0;
        do
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          first += currentElement1;
          checked { ++second; }
        }
        while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey));
        currentElement = new Pair<Decimal, long>(first, second);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
