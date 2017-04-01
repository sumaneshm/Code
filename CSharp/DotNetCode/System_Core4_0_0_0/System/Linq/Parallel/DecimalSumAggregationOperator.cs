// Type: System.Linq.Parallel.DecimalSumAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class DecimalSumAggregationOperator : InlinedAggregationOperator<Decimal, Decimal, Decimal>
  {
    internal DecimalSumAggregationOperator(IEnumerable<Decimal> child)
      : base(child)
    {
    }

    protected override Decimal InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Decimal> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        Decimal num = new Decimal(0, 0, 0, false, (byte) 1);
        while (enumerator.MoveNext())
          num += enumerator.Current;
        return num;
      }
    }

    protected override QueryOperatorEnumerator<Decimal, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<Decimal, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Decimal, int>) new DecimalSumAggregationOperator.DecimalSumAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class DecimalSumAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Decimal>
    {
      private QueryOperatorEnumerator<Decimal, TKey> m_source;

      internal DecimalSumAggregationOperatorEnumerator(QueryOperatorEnumerator<Decimal, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref Decimal currentElement)
      {
        Decimal currentElement1 = new Decimal(0);
        TKey currentKey = default (TKey);
        QueryOperatorEnumerator<Decimal, TKey> operatorEnumerator = this.m_source;
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        Decimal num1 = new Decimal(0, 0, 0, false, (byte) 1);
        int num2 = 0;
        do
        {
          if ((num2++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          num1 += currentElement1;
        }
        while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey));
        currentElement = num1;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
