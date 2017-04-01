// Type: System.Linq.Parallel.IndexedSelectQueryOperator`2
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
  internal sealed class IndexedSelectQueryOperator<TInput, TOutput> : UnaryQueryOperator<TInput, TOutput>
  {
    private readonly Func<TInput, int, TOutput> m_selector;
    private bool m_prematureMerge;
    private bool m_limitsParallelism;

    internal override bool LimitsParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_limitsParallelism;
      }
    }

    internal IndexedSelectQueryOperator(IEnumerable<TInput> child, Func<TInput, int, TOutput> selector)
      : base(child)
    {
      this.m_selector = selector;
      this.m_outputOrdered = true;
      this.InitOrdinalIndexState();
    }

    private void InitOrdinalIndexState()
    {
      OrdinalIndexState ordinalIndexState = this.Child.OrdinalIndexState;
      OrdinalIndexState indexState = ordinalIndexState;
      if (ExchangeUtilities.IsWorseThan(ordinalIndexState, OrdinalIndexState.Correct))
      {
        this.m_prematureMerge = true;
        this.m_limitsParallelism = ordinalIndexState != OrdinalIndexState.Shuffled;
        indexState = OrdinalIndexState.Correct;
      }
      this.SetOrdinalIndexState(indexState);
    }

    internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return IndexedSelectQueryOperator<TInput, TOutput>.IndexedSelectQueryOperatorResults.NewResults(this.Child.Open(settings, preferStriping), this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TInput, int> partitionedStream1 = !this.m_prematureMerge ? (PartitionedStream<TInput, int>) inputStream : QueryOperator<TInput>.ExecuteAndCollectResults<TKey>(inputStream, partitionCount, this.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream();
      PartitionedStream<TOutput, int> partitionedStream2 = new PartitionedStream<TOutput, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream2[index] = (QueryOperatorEnumerator<TOutput, int>) new IndexedSelectQueryOperator<TInput, TOutput>.IndexedSelectQueryOperatorEnumerator(partitionedStream1[index], this.m_selector);
      recipient.Receive<int>(partitionedStream2);
    }

    internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Select<TInput, TOutput>(this.Child.AsSequentialQuery(token), this.m_selector);
    }

    private class IndexedSelectQueryOperatorEnumerator : QueryOperatorEnumerator<TOutput, int>
    {
      private readonly QueryOperatorEnumerator<TInput, int> m_source;
      private readonly Func<TInput, int, TOutput> m_selector;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal IndexedSelectQueryOperatorEnumerator(QueryOperatorEnumerator<TInput, int> source, Func<TInput, int, TOutput> selector)
      {
        this.m_source = source;
        this.m_selector = selector;
      }

      internal override bool MoveNext(ref TOutput currentElement, ref int currentKey)
      {
        TInput currentElement1 = default (TInput);
        if (!this.m_source.MoveNext(ref currentElement1, ref currentKey))
          return false;
        currentElement = this.m_selector(currentElement1, currentKey);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }

    private class IndexedSelectQueryOperatorResults : UnaryQueryOperator<TInput, TOutput>.UnaryQueryOperatorResults
    {
      private IndexedSelectQueryOperator<TInput, TOutput> m_selectOp;
      private int m_childCount;

      internal override int ElementsCount
      {
        get
        {
          return this.m_childQueryResults.ElementsCount;
        }
      }

      internal override bool IsIndexible
      {
        get
        {
          return true;
        }
      }

      private IndexedSelectQueryOperatorResults(QueryResults<TInput> childQueryResults, IndexedSelectQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
        : base(childQueryResults, (UnaryQueryOperator<TInput, TOutput>) op, settings, preferStriping)
      {
        this.m_selectOp = op;
        this.m_childCount = this.m_childQueryResults.ElementsCount;
      }

      public static QueryResults<TOutput> NewResults(QueryResults<TInput> childQueryResults, IndexedSelectQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
      {
        if (childQueryResults.IsIndexible)
          return (QueryResults<TOutput>) new IndexedSelectQueryOperator<TInput, TOutput>.IndexedSelectQueryOperatorResults(childQueryResults, op, settings, preferStriping);
        else
          return (QueryResults<TOutput>) new UnaryQueryOperator<TInput, TOutput>.UnaryQueryOperatorResults(childQueryResults, (UnaryQueryOperator<TInput, TOutput>) op, settings, preferStriping);
      }

      internal override TOutput GetElement(int index)
      {
        return this.m_selectOp.m_selector(this.m_childQueryResults.GetElement(index), index);
      }
    }
  }
}
