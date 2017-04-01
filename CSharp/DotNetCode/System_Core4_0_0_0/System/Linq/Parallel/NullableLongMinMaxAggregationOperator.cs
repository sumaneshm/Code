// Type: System.Linq.Parallel.NullableLongMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableLongMinMaxAggregationOperator : InlinedAggregationOperator<long?, long?, long?>
  {
    private readonly int m_sign;

    internal NullableLongMinMaxAggregationOperator(IEnumerable<long?> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override long? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<long?> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
          return new long?();
        long? nullable1 = enumerator.Current;
        if (this.m_sign == -1)
        {
          while (enumerator.MoveNext())
          {
            long? current = enumerator.Current;
            if (nullable1.HasValue)
            {
              long? nullable2 = current;
              long? nullable3 = nullable1;
              if ((nullable2.GetValueOrDefault() >= nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0)
                continue;
            }
            nullable1 = current;
          }
        }
        else
        {
          while (enumerator.MoveNext())
          {
            long? current = enumerator.Current;
            if (nullable1.HasValue)
            {
              long? nullable2 = current;
              long? nullable3 = nullable1;
              if ((nullable2.GetValueOrDefault() <= nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0)
                continue;
            }
            nullable1 = current;
          }
        }
        return nullable1;
      }
    }

    protected override QueryOperatorEnumerator<long?, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<long?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<long?, int>) new NullableLongMinMaxAggregationOperator.NullableLongMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class NullableLongMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<long?>
    {
      private QueryOperatorEnumerator<long?, TKey> m_source;
      private int m_sign;

      internal NullableLongMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<long?, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref long? currentElement)
      {
        QueryOperatorEnumerator<long?, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          long? currentElement1 = new long?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement.HasValue)
            {
              long? nullable1 = currentElement1;
              long? nullable2 = currentElement;
              if ((nullable1.GetValueOrDefault() >= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0)
                continue;
            }
            currentElement = currentElement1;
          }
        }
        else
        {
          long? currentElement1 = new long?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement.HasValue)
            {
              long? nullable1 = currentElement1;
              long? nullable2 = currentElement;
              if ((nullable1.GetValueOrDefault() <= nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0)
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
