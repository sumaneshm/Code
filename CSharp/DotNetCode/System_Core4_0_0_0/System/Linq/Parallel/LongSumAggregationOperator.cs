// Type: System.Linq.Parallel.LongSumAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class LongSumAggregationOperator : InlinedAggregationOperator<long, long, long>
  {
    internal LongSumAggregationOperator(IEnumerable<long> child)
      : base(child)
    {
    }

    protected override long InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<long> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        long num = 0L;
        while (enumerator.MoveNext())
          checked { num += enumerator.Current; }
        return num;
      }
    }

    protected override QueryOperatorEnumerator<long, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<long, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<long, int>) new LongSumAggregationOperator.LongSumAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class LongSumAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<long>
    {
      private readonly QueryOperatorEnumerator<long, TKey> m_source;

      internal LongSumAggregationOperatorEnumerator(QueryOperatorEnumerator<long, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref long currentElement)
      {
        long currentElement1 = 0L;
        TKey currentKey = default (TKey);
        QueryOperatorEnumerator<long, TKey> operatorEnumerator = this.m_source;
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        long num1 = 0L;
        int num2 = 0;
        do
        {
          if ((num2++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          checked { num1 += currentElement1; }
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
