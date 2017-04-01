// Type: System.Linq.Parallel.TakeOrSkipQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class TakeOrSkipQueryOperator<TResult> : UnaryQueryOperator<TResult, TResult>
  {
    private readonly int m_count;
    private readonly bool m_take;
    private bool m_prematureMerge;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal TakeOrSkipQueryOperator(IEnumerable<TResult> child, int count, bool take)
      : base(child)
    {
      this.m_count = count;
      this.m_take = take;
      this.SetOrdinalIndexState(this.OutputOrdinalIndexState());
    }

    private OrdinalIndexState OutputOrdinalIndexState()
    {
      OrdinalIndexState state1 = this.Child.OrdinalIndexState;
      if (state1 == OrdinalIndexState.Indexible)
        return OrdinalIndexState.Indexible;
      if (ExchangeUtilities.IsWorseThan(state1, OrdinalIndexState.Increasing))
      {
        this.m_prematureMerge = true;
        state1 = OrdinalIndexState.Correct;
      }
      if (!this.m_take && state1 == OrdinalIndexState.Correct)
        state1 = OrdinalIndexState.Increasing;
      return state1;
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TResult, TKey> inputStream, IPartitionedStreamRecipient<TResult> recipient, bool preferStriping, QuerySettings settings)
    {
      if (this.m_prematureMerge)
        this.WrapHelper<int>(QueryOperator<TResult>.ExecuteAndCollectResults<TKey>(inputStream, inputStream.PartitionCount, this.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream(), recipient, settings);
      else
        this.WrapHelper<TKey>(inputStream, recipient, settings);
    }

    private void WrapHelper<TKey>(PartitionedStream<TResult, TKey> inputStream, IPartitionedStreamRecipient<TResult> recipient, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      FixedMaxHeap<TKey> sharedIndices = new FixedMaxHeap<TKey>(this.m_count, inputStream.KeyComparer);
      CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
      PartitionedStream<TResult, TKey> partitionedStream = new PartitionedStream<TResult, TKey>(partitionCount, inputStream.KeyComparer, this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TResult, TKey>) new TakeOrSkipQueryOperator<TResult>.TakeOrSkipQueryOperatorEnumerator<TKey>(inputStream[index], this.m_take, sharedIndices, sharedBarrier, settings.CancellationState.MergedCancellationToken, inputStream.KeyComparer);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override QueryResults<TResult> Open(QuerySettings settings, bool preferStriping)
    {
      return TakeOrSkipQueryOperator<TResult>.TakeOrSkipQueryOperatorResults.NewResults(this.Child.Open(settings, true), this, settings, preferStriping);
    }

    internal override IEnumerable<TResult> AsSequentialQuery(CancellationToken token)
    {
      if (this.m_take)
        return Enumerable.Take<TResult>(this.Child.AsSequentialQuery(token), this.m_count);
      else
        return Enumerable.Skip<TResult>(CancellableEnumerable.Wrap<TResult>(this.Child.AsSequentialQuery(token), token), this.m_count);
    }

    private class TakeOrSkipQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TResult, TKey>
    {
      private readonly QueryOperatorEnumerator<TResult, TKey> m_source;
      private readonly int m_count;
      private readonly bool m_take;
      private readonly IComparer<TKey> m_keyComparer;
      private readonly FixedMaxHeap<TKey> m_sharedIndices;
      private readonly CountdownEvent m_sharedBarrier;
      private readonly CancellationToken m_cancellationToken;
      private List<Pair<TResult, TKey>> m_buffer;
      private Shared<int> m_bufferIndex;

      internal TakeOrSkipQueryOperatorEnumerator(QueryOperatorEnumerator<TResult, TKey> source, bool take, FixedMaxHeap<TKey> sharedIndices, CountdownEvent sharedBarrier, CancellationToken cancellationToken, IComparer<TKey> keyComparer)
      {
        this.m_source = source;
        this.m_count = sharedIndices.Size;
        this.m_take = take;
        this.m_sharedIndices = sharedIndices;
        this.m_sharedBarrier = sharedBarrier;
        this.m_cancellationToken = cancellationToken;
        this.m_keyComparer = keyComparer;
      }

      internal override bool MoveNext(ref TResult currentElement, ref TKey currentKey)
      {
        if (this.m_buffer == null && this.m_count > 0)
        {
          List<Pair<TResult, TKey>> list = new List<Pair<TResult, TKey>>();
          TResult currentElement1 = default (TResult);
          TKey currentKey1 = default (TKey);
          int num = 0;
          while (list.Count < this.m_count && this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            list.Add(new Pair<TResult, TKey>(currentElement1, currentKey1));
            lock (this.m_sharedIndices)
            {
              if (!this.m_sharedIndices.Insert(currentKey1))
                break;
            }
          }
          this.m_sharedBarrier.Signal();
          this.m_sharedBarrier.Wait(this.m_cancellationToken);
          this.m_buffer = list;
          this.m_bufferIndex = new Shared<int>(-1);
        }
        if (this.m_take)
        {
          if (this.m_count == 0 || this.m_bufferIndex.Value >= this.m_buffer.Count - 1)
            return false;
          ++this.m_bufferIndex.Value;
          currentElement = this.m_buffer[this.m_bufferIndex.Value].First;
          currentKey = this.m_buffer[this.m_bufferIndex.Value].Second;
          if (this.m_sharedIndices.Count != 0)
            return this.m_keyComparer.Compare(this.m_buffer[this.m_bufferIndex.Value].Second, this.m_sharedIndices.MaxValue) <= 0;
          else
            return true;
        }
        else
        {
          TKey key = default (TKey);
          if (this.m_count > 0)
          {
            if (this.m_sharedIndices.Count < this.m_count)
              return false;
            TKey maxValue = this.m_sharedIndices.MaxValue;
            if (this.m_bufferIndex.Value < this.m_buffer.Count - 1)
            {
              for (++this.m_bufferIndex.Value; this.m_bufferIndex.Value < this.m_buffer.Count; ++this.m_bufferIndex.Value)
              {
                if (this.m_keyComparer.Compare(this.m_buffer[this.m_bufferIndex.Value].Second, maxValue) > 0)
                {
                  currentElement = this.m_buffer[this.m_bufferIndex.Value].First;
                  currentKey = this.m_buffer[this.m_bufferIndex.Value].Second;
                  return true;
                }
              }
            }
          }
          return this.m_source.MoveNext(ref currentElement, ref currentKey);
        }
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }

    private class TakeOrSkipQueryOperatorResults : UnaryQueryOperator<TResult, TResult>.UnaryQueryOperatorResults
    {
      private TakeOrSkipQueryOperator<TResult> m_takeOrSkipOp;
      private int m_childCount;

      internal override bool IsIndexible
      {
        get
        {
          return this.m_childCount >= 0;
        }
      }

      internal override int ElementsCount
      {
        get
        {
          if (this.m_takeOrSkipOp.m_take)
            return Math.Min(this.m_childCount, this.m_takeOrSkipOp.m_count);
          else
            return Math.Max(this.m_childCount - this.m_takeOrSkipOp.m_count, 0);
        }
      }

      private TakeOrSkipQueryOperatorResults(QueryResults<TResult> childQueryResults, TakeOrSkipQueryOperator<TResult> takeOrSkipOp, QuerySettings settings, bool preferStriping)
        : base(childQueryResults, (UnaryQueryOperator<TResult, TResult>) takeOrSkipOp, settings, preferStriping)
      {
        this.m_takeOrSkipOp = takeOrSkipOp;
        this.m_childCount = this.m_childQueryResults.ElementsCount;
      }

      public static QueryResults<TResult> NewResults(QueryResults<TResult> childQueryResults, TakeOrSkipQueryOperator<TResult> op, QuerySettings settings, bool preferStriping)
      {
        if (childQueryResults.IsIndexible)
          return (QueryResults<TResult>) new TakeOrSkipQueryOperator<TResult>.TakeOrSkipQueryOperatorResults(childQueryResults, op, settings, preferStriping);
        else
          return (QueryResults<TResult>) new UnaryQueryOperator<TResult, TResult>.UnaryQueryOperatorResults(childQueryResults, (UnaryQueryOperator<TResult, TResult>) op, settings, preferStriping);
      }

      internal override TResult GetElement(int index)
      {
        if (this.m_takeOrSkipOp.m_take)
          return this.m_childQueryResults.GetElement(index);
        else
          return this.m_childQueryResults.GetElement(this.m_takeOrSkipOp.m_count + index);
      }
    }
  }
}
