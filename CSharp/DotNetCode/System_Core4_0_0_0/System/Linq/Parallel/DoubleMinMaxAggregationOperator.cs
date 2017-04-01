// Type: System.Linq.Parallel.DoubleMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class DoubleMinMaxAggregationOperator : InlinedAggregationOperator<double, double, double>
  {
    private readonly int m_sign;

    internal DoubleMinMaxAggregationOperator(IEnumerable<double> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override double InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<double> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
        {
          singularExceptionToThrow = (Exception) new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          return 0.0;
        }
        else
        {
          double d = enumerator.Current;
          if (this.m_sign == -1)
          {
            while (enumerator.MoveNext())
            {
              double current = enumerator.Current;
              if (current < d || double.IsNaN(current))
                d = current;
            }
          }
          else
          {
            while (enumerator.MoveNext())
            {
              double current = enumerator.Current;
              if (current > d || double.IsNaN(d))
                d = current;
            }
          }
          return d;
        }
      }
    }

    protected override QueryOperatorEnumerator<double, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<double, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<double, int>) new DoubleMinMaxAggregationOperator.DoubleMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class DoubleMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<double>
    {
      private QueryOperatorEnumerator<double, TKey> m_source;
      private int m_sign;

      internal DoubleMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<double, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref double currentElement)
      {
        QueryOperatorEnumerator<double, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          double currentElement1 = 0.0;
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1 < currentElement || double.IsNaN(currentElement1))
              currentElement = currentElement1;
          }
        }
        else
        {
          double currentElement1 = 0.0;
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1 > currentElement || double.IsNaN(currentElement))
              currentElement = currentElement1;
          }
        }
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
