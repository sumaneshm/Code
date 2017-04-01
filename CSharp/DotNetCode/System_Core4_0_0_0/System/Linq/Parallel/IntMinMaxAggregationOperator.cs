// Type: System.Linq.Parallel.IntMinMaxAggregationOperator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class IntMinMaxAggregationOperator : InlinedAggregationOperator<int, int, int>
  {
    private readonly int m_sign;

    internal IntMinMaxAggregationOperator(IEnumerable<int> child, int sign)
      : base(child)
    {
      this.m_sign = sign;
    }

    protected override int InternalAggregate(ref Exception singularExceptionToThrow)
    {
      using (IEnumerator<int> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        if (!enumerator.MoveNext())
        {
          singularExceptionToThrow = (Exception) new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          return 0;
        }
        else
        {
          int num = enumerator.Current;
          if (this.m_sign == -1)
          {
            while (enumerator.MoveNext())
            {
              int current = enumerator.Current;
              if (current < num)
                num = current;
            }
          }
          else
          {
            while (enumerator.MoveNext())
            {
              int current = enumerator.Current;
              if (current > num)
                num = current;
            }
          }
          return num;
        }
      }
    }

    protected override QueryOperatorEnumerator<int, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<int, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<int, int>) new IntMinMaxAggregationOperator.IntMinMaxAggregationOperatorEnumerator<TKey>(source, index, this.m_sign, cancellationToken);
    }

    private class IntMinMaxAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<int>
    {
      private readonly QueryOperatorEnumerator<int, TKey> m_source;
      private readonly int m_sign;

      internal IntMinMaxAggregationOperatorEnumerator(QueryOperatorEnumerator<int, TKey> source, int partitionIndex, int sign, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
        this.m_sign = sign;
      }

      protected override bool MoveNextCore(ref int currentElement)
      {
        QueryOperatorEnumerator<int, TKey> operatorEnumerator = this.m_source;
        TKey currentKey = default (TKey);
        if (!operatorEnumerator.MoveNext(ref currentElement, ref currentKey))
          return false;
        int num = 0;
        if (this.m_sign == -1)
        {
          int currentElement1 = 0;
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1 < currentElement)
              currentElement = currentElement1;
          }
        }
        else
        {
          int currentElement1 = 0;
          while (operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (currentElement1 > currentElement)
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
