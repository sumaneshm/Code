﻿// Type: System.Linq.Parallel.NullableFloatMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class NullableFloatMinMaxAggregationOperator : InlinedAggregationOperator<float?, float?, float?>
  {
    private readonly int m_sign;

    internal NullableFloatMinMaxAggregationOperator(IEnumerable<float?> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override float? InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<float?> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
          return new float?();
        float? nullable1 = enumerator.Current;
        if (this.m_sign == -1)
        {
          while (enumerator.MoveNext())
          {
            float? current = enumerator.Current;
            if (current.HasValue)
            {
              if (nullable1.HasValue)
              {
                float? nullable2 = current;
                float? nullable3 = nullable1;
                if (((double) nullable2.GetValueOrDefault() >= (double) nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0 && !float.IsNaN(current.GetValueOrDefault()))
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
            float? current = enumerator.Current;
            if (current.HasValue)
            {
              if (nullable1.HasValue)
              {
                float? nullable2 = current;
                float? nullable3 = nullable1;
                if (((double) nullable2.GetValueOrDefault() <= (double) nullable3.GetValueOrDefault() ? 0 : (nullable2.HasValue & nullable3.HasValue ? 1 : 0)) == 0 && !float.IsNaN(nullable1.GetValueOrDefault()))
                  continue;
              }
              nullable1 = current;
            }
          }
        }
        return nullable1;
      }
    }

    protected override QueryOperatorEnumerator<float?, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<float?, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<float?, int>) new NullableFloatMinMaxAggregationOperator.NullableFloatMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class NullableFloatMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<float?>
    {
      private QueryOperatorEnumerator<float?, TKey> m_source;
      private int m_sign;

      internal NullableFloatMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<float?, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref float? currentElement)
      {
        QueryOperatorEnumerator<float?, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          float? currentElement1 = new float?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1.HasValue)
            {
              if (currentElement.HasValue)
              {
                float? nullable1 = currentElement1;
                float? nullable2 = currentElement;
                if (((double) nullable1.GetValueOrDefault() >= (double) nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 && !float.IsNaN(currentElement1.GetValueOrDefault()))
                  continue;
              }
              currentElement = currentElement1;
            }
          }
        }
        else
        {
          float? currentElement1 = new float?();
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1.HasValue)
            {
              if (currentElement.HasValue)
              {
                float? nullable1 = currentElement1;
                float? nullable2 = currentElement;
                if (((double) nullable1.GetValueOrDefault() <= (double) nullable2.GetValueOrDefault() ? 0 : (nullable1.HasValue & nullable2.HasValue ? 1 : 0)) == 0 && !float.IsNaN(currentElement.GetValueOrDefault()))
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
