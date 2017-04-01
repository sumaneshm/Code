// Type: System.Linq.Parallel.TakeOrSkipWhileQueryOperator`1
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
  internal sealed class TakeOrSkipWhileQueryOperator<TResult> : UnaryQueryOperator<TResult, TResult>
  {
    private Func<TResult, bool> m_predicate;
    private Func<TResult, int, bool> m_indexedPredicate;
    private readonly bool m_take;
    private bool m_prematureMerge;
    private bool m_limitsParallelism;

    internal override bool LimitsParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_limitsParallelism;
      }
    }

    internal TakeOrSkipWhileQueryOperator(IEnumerable<TResult> child, Func<TResult, bool> predicate, Func<TResult, int, bool> indexedPredicate, bool take)
      : base(child)
    {
      this.m_predicate = predicate;
      this.m_indexedPredicate = indexedPredicate;
      this.m_take = take;
      this.InitOrderIndexState();
    }

    private void InitOrderIndexState()
    {
      OrdinalIndexState state2 = OrdinalIndexState.Increasing;
      OrdinalIndexState ordinalIndexState1 = this.Child.OrdinalIndexState;
      if (this.m_indexedPredicate != null)
      {
        state2 = OrdinalIndexState.Correct;
        this.m_limitsParallelism = ordinalIndexState1 == OrdinalIndexState.Increasing;
      }
      OrdinalIndexState ordinalIndexState2 = ExchangeUtilities.Worse(ordinalIndexState1, OrdinalIndexState.Correct);
      if (ExchangeUtilities.IsWorseThan(ordinalIndexState2, state2))
        this.m_prematureMerge = true;
      if (!this.m_take)
        ordinalIndexState2 = ExchangeUtilities.Worse(ordinalIndexState2, OrdinalIndexState.Increasing);
      this.SetOrdinalIndexState(ordinalIndexState2);
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
      TakeOrSkipWhileQueryOperator<TResult>.OperatorState<TKey> operatorState = new TakeOrSkipWhileQueryOperator<TResult>.OperatorState<TKey>();
      CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
      Func<TResult, TKey, bool> indexedPredicate = (Func<TResult, TKey, bool>) this.m_indexedPredicate;
      PartitionedStream<TResult, TKey> partitionedStream = new PartitionedStream<TResult, TKey>(partitionCount, inputStream.KeyComparer, this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TResult, TKey>) new TakeOrSkipWhileQueryOperator<TResult>.TakeOrSkipWhileQueryOperatorEnumerator<TKey>(inputStream[index], this.m_predicate, indexedPredicate, this.m_take, operatorState, sharedBarrier, settings.CancellationState.MergedCancellationToken, inputStream.KeyComparer);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override QueryResults<TResult> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TResult>) new UnaryQueryOperator<TResult, TResult>.UnaryQueryOperatorResults(this.Child.Open(settings, true), (UnaryQueryOperator<TResult, TResult>) this, settings, preferStriping);
    }

    internal override IEnumerable<TResult> AsSequentialQuery(CancellationToken token)
    {
      if (this.m_take)
      {
        if (this.m_indexedPredicate != null)
          return Enumerable.TakeWhile<TResult>(this.Child.AsSequentialQuery(token), this.m_indexedPredicate);
        else
          return Enumerable.TakeWhile<TResult>(this.Child.AsSequentialQuery(token), this.m_predicate);
      }
      else if (this.m_indexedPredicate != null)
        return Enumerable.SkipWhile<TResult>(CancellableEnumerable.Wrap<TResult>(this.Child.AsSequentialQuery(token), token), this.m_indexedPredicate);
      else
        return Enumerable.SkipWhile<TResult>(CancellableEnumerable.Wrap<TResult>(this.Child.AsSequentialQuery(token), token), this.m_predicate);
    }

    private class TakeOrSkipWhileQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TResult, TKey>
    {
      private readonly QueryOperatorEnumerator<TResult, TKey> m_source;
      private readonly Func<TResult, bool> m_predicate;
      private readonly Func<TResult, TKey, bool> m_indexedPredicate;
      private readonly bool m_take;
      private readonly IComparer<TKey> m_keyComparer;
      private readonly TakeOrSkipWhileQueryOperator<TResult>.OperatorState<TKey> m_operatorState;
      private readonly CountdownEvent m_sharedBarrier;
      private readonly CancellationToken m_cancellationToken;
      private List<Pair<TResult, TKey>> m_buffer;
      private Shared<int> m_bufferIndex;
      private int m_updatesSeen;
      private TKey m_currentLowKey;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal TakeOrSkipWhileQueryOperatorEnumerator(QueryOperatorEnumerator<TResult, TKey> source, Func<TResult, bool> predicate, Func<TResult, TKey, bool> indexedPredicate, bool take, TakeOrSkipWhileQueryOperator<TResult>.OperatorState<TKey> operatorState, CountdownEvent sharedBarrier, CancellationToken cancelToken, IComparer<TKey> keyComparer)
      {
        this.m_source = source;
        this.m_predicate = predicate;
        this.m_indexedPredicate = indexedPredicate;
        this.m_take = take;
        this.m_operatorState = operatorState;
        this.m_sharedBarrier = sharedBarrier;
        this.m_cancellationToken = cancelToken;
        this.m_keyComparer = keyComparer;
      }

      internal override bool MoveNext(ref TResult currentElement, ref TKey currentKey)
      {
        if (this.m_buffer == null)
        {
          List<Pair<TResult, TKey>> list = new List<Pair<TResult, TKey>>();
          try
          {
            TResult currentElement1 = default (TResult);
            TKey currentKey1 = default (TKey);
            int num = 0;
            while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
            {
              if ((num++ & 63) == 0)
                CancellationState.ThrowIfCanceled(this.m_cancellationToken);
              list.Add(new Pair<TResult, TKey>(currentElement1, currentKey1));
              if (this.m_updatesSeen != this.m_operatorState.m_updatesDone)
              {
                lock (this.m_operatorState)
                {
                  this.m_currentLowKey = this.m_operatorState.m_currentLowKey;
                  this.m_updatesSeen = this.m_operatorState.m_updatesDone;
                }
              }
              if (this.m_updatesSeen > 0)
              {
                if (this.m_keyComparer.Compare(currentKey1, this.m_currentLowKey) > 0)
                  break;
              }
              if (!(this.m_predicate == null ? this.m_indexedPredicate(currentElement1, currentKey1) : this.m_predicate(currentElement1)))
              {
                lock (this.m_operatorState)
                {
                  if (this.m_operatorState.m_updatesDone != 0)
                  {
                    if (this.m_keyComparer.Compare(this.m_operatorState.m_currentLowKey, currentKey1) <= 0)
                      break;
                  }
                  this.m_currentLowKey = this.m_operatorState.m_currentLowKey = currentKey1;
                  this.m_updatesSeen = ++this.m_operatorState.m_updatesDone;
                  break;
                }
              }
            }
          }
          finally
          {
            this.m_sharedBarrier.Signal();
          }
          this.m_sharedBarrier.Wait(this.m_cancellationToken);
          this.m_buffer = list;
          this.m_bufferIndex = new Shared<int>(-1);
        }
        if (this.m_take)
        {
          if (this.m_bufferIndex.Value >= this.m_buffer.Count - 1)
            return false;
          ++this.m_bufferIndex.Value;
          currentElement = this.m_buffer[this.m_bufferIndex.Value].First;
          currentKey = this.m_buffer[this.m_bufferIndex.Value].Second;
          if (this.m_operatorState.m_updatesDone != 0)
            return this.m_keyComparer.Compare(this.m_operatorState.m_currentLowKey, currentKey) > 0;
          else
            return true;
        }
        else
        {
          if (this.m_operatorState.m_updatesDone == 0)
            return false;
          if (this.m_bufferIndex.Value < this.m_buffer.Count - 1)
          {
            for (++this.m_bufferIndex.Value; this.m_bufferIndex.Value < this.m_buffer.Count; ++this.m_bufferIndex.Value)
            {
              if (this.m_keyComparer.Compare(this.m_buffer[this.m_bufferIndex.Value].Second, this.m_operatorState.m_currentLowKey) >= 0)
              {
                currentElement = this.m_buffer[this.m_bufferIndex.Value].First;
                currentKey = this.m_buffer[this.m_bufferIndex.Value].Second;
                return true;
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

    private class OperatorState<TKey>
    {
      internal volatile int m_updatesDone;
      internal TKey m_currentLowKey;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      public OperatorState()
      {
      }
    }
  }
}
