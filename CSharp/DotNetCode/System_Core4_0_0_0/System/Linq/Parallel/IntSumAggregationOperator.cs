// Type: System.Linq.Parallel.IntSumAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class IntSumAggregationOperator : InlinedAggregationOperator<int, int, int>
  {
    internal IntSumAggregationOperator(IEnumerable<int> child)
      : base(child)
    {
    }

    protected override int InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<int> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        int num = 0;
        while (enumerator.MoveNext())
          checked { num += enumerator.Current; }
        return num;
      }
    }

    protected override QueryOperatorEnumerator<int, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<int, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<int, int>) new IntSumAggregationOperator.IntSumAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class IntSumAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<int>
    {
      private readonly QueryOperatorEnumerator<int, TKey> m_source;

      internal IntSumAggregationOperatorEnumerator(QueryOperatorEnumerator<int, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref int currentElement)
      {
        int currentElement1 = 0;
        TKey currentKey = default (TKey);
        QueryOperatorEnumerator<int, TKey> operatorEnumerator = this.m_source;
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        int num1 = 0;
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
