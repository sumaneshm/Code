// Type: System.Linq.Parallel.SelectQueryOperator`2
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
  internal sealed class SelectQueryOperator<TInput, TOutput> : UnaryQueryOperator<TInput, TOutput>
  {
    private Func<TInput, TOutput> m_selector;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal SelectQueryOperator(IEnumerable<TInput> child, Func<TInput, TOutput> selector)
      : base(child)
    {
      this.m_selector = selector;
      this.SetOrdinalIndexState(this.Child.OrdinalIndexState);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      PartitionedStream<TOutput, TKey> partitionedStream = new PartitionedStream<TOutput, TKey>(inputStream.PartitionCount, inputStream.KeyComparer, this.OrdinalIndexState);
      for (int index = 0; index < inputStream.PartitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TOutput, TKey>) new SelectQueryOperator<TInput, TOutput>.SelectQueryOperatorEnumerator<TKey>(inputStream[index], this.m_selector);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return SelectQueryOperator<TInput, TOutput>.SelectQueryOperatorResults.NewResults(this.Child.Open(settings, preferStriping), this, settings, preferStriping);
    }

    internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Select<TInput, TOutput>(this.Child.AsSequentialQuery(token), this.m_selector);
    }

    private class SelectQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TOutput, TKey>
    {
      private readonly QueryOperatorEnumerator<TInput, TKey> m_source;
      private readonly Func<TInput, TOutput> m_selector;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal SelectQueryOperatorEnumerator(QueryOperatorEnumerator<TInput, TKey> source, Func<TInput, TOutput> selector)
      {
        this.m_source = source;
        this.m_selector = selector;
      }

      internal override bool MoveNext(ref TOutput currentElement, ref TKey currentKey)
      {
        TInput currentElement1 = default (TInput);
        if (!this.m_source.MoveNext(ref currentElement1, ref currentKey))
          return false;
        currentElement = this.m_selector(currentElement1);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }

    private class SelectQueryOperatorResults : UnaryQueryOperator<TInput, TOutput>.UnaryQueryOperatorResults
    {
      private Func<TInput, TOutput> m_selector;
      private int m_childCount;

      internal override bool IsIndexible
      {
        get
        {
          return true;
        }
      }

      internal override int ElementsCount
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this.m_childCount;
        }
      }

      private SelectQueryOperatorResults(QueryResults<TInput> childQueryResults, SelectQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
        : base(childQueryResults, (UnaryQueryOperator<TInput, TOutput>) op, settings, preferStriping)
      {
        this.m_selector = op.m_selector;
        this.m_childCount = this.m_childQueryResults.ElementsCount;
      }

      public static QueryResults<TOutput> NewResults(QueryResults<TInput> childQueryResults, SelectQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
      {
        if (childQueryResults.IsIndexible)
          return (QueryResults<TOutput>) new SelectQueryOperator<TInput, TOutput>.SelectQueryOperatorResults(childQueryResults, op, settings, preferStriping);
        else
          return (QueryResults<TOutput>) new UnaryQueryOperator<TInput, TOutput>.UnaryQueryOperatorResults(childQueryResults, (UnaryQueryOperator<TInput, TOutput>) op, settings, preferStriping);
      }

      internal override TOutput GetElement(int index)
      {
        return this.m_selector(this.m_childQueryResults.GetElement(index));
      }
    }
  }
}
