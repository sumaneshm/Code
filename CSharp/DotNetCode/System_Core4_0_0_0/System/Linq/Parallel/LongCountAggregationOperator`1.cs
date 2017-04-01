// Type: System.Linq.Parallel.LongCountAggregationOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class LongCountAggregationOperator<TSource> : InlinedAggregationOperator<TSource, long, long>
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal LongCountAggregationOperator(IEnumerable<TSource> child)
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

    protected override QueryOperatorEnumerator<long, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<TSource, TKey> source, object sharedData, CancellationToken cancellationToken)
    {
      return (QueryOperatorEnumerator<long, int>) new LongCountAggregationOperator<TSource>.LongCountAggregationOperatorEnumerator<TKey>(source, index, cancellationToken);
    }

    private class LongCountAggregationOperatorEnumerator<TKey> : InlinedAggregationOperatorEnumerator<long>
    {
      private readonly QueryOperatorEnumerator<TSource, TKey> m_source;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal LongCountAggregationOperatorEnumerator(QueryOperatorEnumerator<TSource, TKey> source, int partitionIndex, CancellationToken cancellationToken)
        : base(partitionIndex, cancellationToken)
      {
        this.m_source = source;
      }

      protected override bool MoveNextCore(ref long currentElement)
      {
        TSource currentElement1 = default (TSource);
        TKey currentKey = default (TKey);
        QueryOperatorEnumerator<TSource, TKey> operatorEnumerator = this.m_source;
        if (!operatorEnumerator.MoveNext(ref currentElement1, ref currentKey))
          return false;
        long num1 = 0L;
        int num2 = 0;
        do
        {
          if ((num2++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          checked { ++num1; }
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
