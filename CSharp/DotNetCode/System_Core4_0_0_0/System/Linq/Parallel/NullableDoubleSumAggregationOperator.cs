﻿// Type: System.Linq.Parallel.NullableDoubleSumAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableDoubleSumAggregationOperator : InlinedAggregationOperator<double?, double?, double?>
  {
    internal NullableDoubleSumAggregationOperator(IEnumerable<double?> child)
      : base(child)
    {
    }

    protected override double? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<double?> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        double num = 0.0;
        while (enumerator.MoveNext())
          num += enumerator.Current.GetValueOrDefault();
        return new double?(num);
      }
    }

    protected override QueryOperatorEnumerator<double?, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<double?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<double?, int>) new NullableDoubleSumAggregationOperator.NullableDoubleSumAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class NullableDoubleSumAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<double?>
    {
      private readonly QueryOperatorEnumerator<double?, TKey> m_source;

      internal NullableDoubleSumAggregationOperatorEnumerator(QueryOperatorEnumerator<double?, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref double? currentElement)
      {
        double? currentElement1 = new double?();
        TKey currentKey = default (TKey);
        QueryOperatorEnumerator<double?, TKey> operatorEnumerator = this.m_source;
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        double num1 = 0.0;
        int num2 = 0;
        do
        {
          if ((num2++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          num1 += currentElement1.GetValueOrDefault();
        }
        while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey));
        currentElement = new double?(num1);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
