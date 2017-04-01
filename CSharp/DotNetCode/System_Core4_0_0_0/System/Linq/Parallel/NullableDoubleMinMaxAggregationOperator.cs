// Type: System.Linq.Parallel.NullableDoubleMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableDoubleMinMaxAggregationOperator : InlinedAggregationOperator<double?, double?, double?>
  {
    private readonly int m_sign;

    internal NullableDoubleMinMaxAggregationOperator(IEnumerable<double?> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override double? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<double?> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
          return new double?();
        double? nullable1 = enumerator.Current;
        if (this.m_sign == -1)
        {
          while (enumerator.MoveNext())
          {
            double? current = enumerator.Current;
            if (current.HasValue)
            {
              if (nullable1.HasValue)
              {
                double? nullable2 = current;
                double? nullable3 = nullable1;
                if ((nullable2.GetValueOrDefault() >= nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0 && !double.IsNaN(current.GetValueOrDefault()))
                  continue;
              }
              nullable1 = current;
            }
          }
        }
        else
        {
          while (enumerator.MoveNext())
          {
            double? current = enumerator.Current;
            if (current.HasValue)
            {
              if (nullable1.HasValue)
              {
                double? nullable2 = current;
                double? nullable3 = nullable1;
                if ((nullable2.GetValueOrDefault() <= nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0 && !double.IsNaN(nullable1.GetValueOrDefault()))
                  continue;
              }
              nullable1 = current;
            }
          }
        }
        return nullable1;
      }
    }

    protected override QueryOperatorEnumerator<double?, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<double?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<double?, int>) new NullableDoubleMinMaxAggregationOperator.NullableDoubleMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class NullableDoubleMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<double?>
    {
      private QueryOperatorEnumerator<double?, TKey> m_source;
      private int m_sign;

      internal NullableDoubleMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<double?, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref double? currentElement)
      {
        QueryOperatorEnumerator<double?, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          double? currentElement1 = new double?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1.HasValue)
            {
              if (currentElement.HasValue)
              {
                double? nullable1 = currentElement1;
                double? nullable2 = currentElement;
                if ((nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 && !double.IsNaN(currentElement1.GetValueOrDefault()))
                  continue;
              }
              currentElement = currentElement1;
            }
          }
        }
        else
        {
          double? currentElement1 = new double?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1.HasValue)
            {
              if (currentElement.HasValue)
              {
                double? nullable1 = currentElement1;
                double? nullable2 = currentElement;
                if ((nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 && !double.IsNaN(currentElement.GetValueOrDefault()))
                  continue;
              }
              currentElement = currentElement1;
            }
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
