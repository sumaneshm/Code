// Type: System.Linq.Parallel.NullableDecimalMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableDecimalMinMaxAggregationOperator : InlinedAggregationOperator<Decimal?, Decimal?, Decimal?>
  {
    private readonly int m_sign;

    internal NullableDecimalMinMaxAggregationOperator(IEnumerable<Decimal?> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override Decimal? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<Decimal?> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
          return new Decimal?();
        Decimal? nullable1 = enumerator.Current;
        if (this.m_sign == -1)
        {
          while (enumerator.MoveNext())
          {
            Decimal? current = enumerator.Current;
            if (nullable1.HasValue)
            {
              Decimal? nullable2 = current;
              Decimal? nullable3 = nullable1;
              if ((!(nullable2.GetValueOrDefault() < nullable3.GetValueOrDefault()) ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0)
                continue;
            }
            nullable1 = current;
          }
        }
        else
        {
          while (enumerator.MoveNext())
          {
            Decimal? current = enumerator.Current;
            if (nullable1.HasValue)
            {
              Decimal? nullable2 = current;
              Decimal? nullable3 = nullable1;
              if ((!(nullable2.GetValueOrDefault() > nullable3.GetValueOrDefault()) ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0)
                continue;
            }
            nullable1 = current;
          }
        }
        return nullable1;
      }
    }

    protected override QueryOperatorEnumerator<Decimal?, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<Decimal?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<Decimal?, int>) new NullableDecimalMinMaxAggregationOperator.NullableDecimalMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class NullableDecimalMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<Decimal?>
    {
      private QueryOperatorEnumerator<Decimal?, TKey> m_source;
      private int m_sign;

      internal NullableDecimalMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<Decimal?, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref Decimal? currentElement)
      {
        QueryOperatorEnumerator<Decimal?, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          Decimal? currentElement1 = new Decimal?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement.HasValue)
            {
              Decimal? nullable1 = currentElement1;
              Decimal? nullable2 = currentElement;
              if ((!(nullable1.GetValueOrDefault() < nullable2.GetValueOrDefault()) ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0)
                continue;
            }
            currentElement = currentElement1;
          }
        }
        else
        {
          Decimal? currentElement1 = new Decimal?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement.HasValue)
            {
              Decimal? nullable1 = currentElement1;
              Decimal? nullable2 = currentElement;
              if ((!(nullable1.GetValueOrDefault() > nullable2.GetValueOrDefault()) ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0)
                continue;
            }
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
