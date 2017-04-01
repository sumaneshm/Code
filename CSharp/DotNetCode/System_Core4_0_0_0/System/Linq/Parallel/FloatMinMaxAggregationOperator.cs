// Type: System.Linq.Parallel.FloatMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class FloatMinMaxAggregationOperator : InlinedAggregationOperator<float, float, float>
  {
    private readonly int m_sign;

    internal FloatMinMaxAggregationOperator(IEnumerable<float> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override float InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<float> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
        {
          singularExceptionToThrow = (Exception) new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          return 0.0f;
        }
        else
        {
          float f = enumerator.Current;
          if (this.m_sign == -1)
          {
            while (enumerator.MoveNext())
            {
              float current = enumerator.Current;
              if ((double) current < (double) f || float.IsNaN(current))
                f = current;
            }
          }
          else
          {
            while (enumerator.MoveNext())
            {
              float current = enumerator.Current;
              if ((double) current > (double) f || float.IsNaN(f))
                f = current;
            }
          }
          return f;
        }
      }
    }

    protected override QueryOperatorEnumerator<float, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<float, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<float, int>) new FloatMinMaxAggregationOperator.FloatMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class FloatMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<float>
    {
      private QueryOperatorEnumerator<float, TKey> m_source;
      private int m_sign;

      internal FloatMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<float, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref float currentElement)
      {
        QueryOperatorEnumerator<float, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          float currentElement1 = 0.0f;
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if ((double) currentElement1 < (double) currentElement || float.IsNaN(currentElement1))
              currentElement = currentElement1;
          }
        }
        else
        {
          float currentElement1 = 0.0f;
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if ((double) currentElement1 > (double) currentElement || float.IsNaN(currentElement))
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
