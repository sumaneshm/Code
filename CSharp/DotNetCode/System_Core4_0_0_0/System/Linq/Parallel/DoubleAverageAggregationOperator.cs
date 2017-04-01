﻿// Type: System.Linq.Parallel.DoubleAverageAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class DoubleAverageAggregationOperator : InlinedAggregationOperator<double, Pair<double, long>, double>
  {
    internal DoubleAverageAggregationOperator(IEnumerable<double> child)
      : base(child)
    {
    }

    protected override double InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Pair<double, long>> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
        {
          singularExceptionToThrow = (Exception) new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          return 0.0;
        }
        else
        {
          Pair<double, long> current = enumerator.Current;
          while (enumerator.MoveNext())
          {
            current.First += enumerator.Current.First;
            checked { current.Second += enumerator.Current.Second; }
          }
          return current.First / (double) current.Second;
        }
      }
    }

    protected override QueryOperatorEnumerator<Pair<double, long>, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<double, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Pair<double, long>, int>) new DoubleAverageAggregationOperator.DoubleAverageAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class DoubleAverageAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Pair<double, long>>
    {
      private QueryOperatorEnumerator<double, TKey> m_source;

      internal DoubleAverageAggregationOperatorEnumerator(QueryOperatorEnumerator<double, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref Pair<double, long> currentElement)
      {
        double first = 0.0;
        long second = 0L;
        QueryOperatorEnumerator<double, TKey> operatorEnumerator = this.m_source;
        double currentElement1 = 0.0;
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
        currentElement = new Pair<double, long>(first, second);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
