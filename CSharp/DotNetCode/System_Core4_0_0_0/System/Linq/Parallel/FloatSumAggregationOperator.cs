// Type: System.Linq.Parallel.FloatSumAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class FloatSumAggregationOperator : InlinedAggregationOperator<float, double, float>
  {
    internal FloatSumAggregationOperator(IEnumerable<float> child)
      : base(child)
    {
    }

    protected override float InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<double> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        double num = 0.0;
        while (enumerator.MoveNext())
          num += enumerator.Current;
        return (float) num;
      }
    }

    protected override QueryOperatorEnumerator<double, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<float, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<double, int>) new FloatSumAggregationOperator.FloatSumAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class FloatSumAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<double>
    {
      private readonly QueryOperatorEnumerator<float, TKey> m_source;

      internal FloatSumAggregationOperatorEnumerator(QueryOperatorEnumerator<float, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref double currentElement)
      {
        float currentElement1 = 0.0f;
        TKey currentKey = default (TKey);
        QueryOperatorEnumerator<float, TKey> operatorEnumerator = this.m_source;
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        double num1 = 0.0;
        int num2 = 0;
        do
        {
          if ((num2++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          num1 += (double) currentElement1;
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
