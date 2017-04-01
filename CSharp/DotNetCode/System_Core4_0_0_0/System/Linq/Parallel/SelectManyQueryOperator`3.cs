// Type: System.Linq.Parallel.SelectManyQueryOperator`3
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
  internal sealed class SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> : UnaryQueryOperator<TLeftInput, TOutput>
  {
    private readonly Func<TLeftInput, IEnumerable<TRightInput>> m_rightChildSelector;
    private readonly Func<TLeftInput, int, IEnumerable<TRightInput>> m_indexedRightChildSelector;
    private readonly Func<TLeftInput, TRightInput, TOutput> m_resultSelector;
    private bool m_prematureMerge;
    private bool m_limitsParallelism;

    internal override bool LimitsParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_limitsParallelism;
      }
    }

    internal SelectManyQueryOperator(IEnumerable<TLeftInput> leftChild, Func<TLeftInput, IEnumerable<TRightInput>> rightChildSelector, Func<TLeftInput, int, IEnumerable<TRightInput>> indexedRightChildSelector, Func<TLeftInput, TRightInput, TOutput> resultSelector)
      : base(leftChild)
    {
      this.m_rightChildSelector = rightChildSelector;
      this.m_indexedRightChildSelector = indexedRightChildSelector;
      this.m_resultSelector = resultSelector;
      this.m_outputOrdered = this.Child.OutputOrdered || indexedRightChildSelector != null;
      this.InitOrderIndex();
    }

    private void InitOrderIndex()
    {
      OrdinalIndexState ordinalIndexState = this.Child.OrdinalIndexState;
      if (this.m_indexedRightChildSelector != null)
      {
        this.m_prematureMerge = ExchangeUtilities.IsWorseThan(ordinalIndexState, OrdinalIndexState.Correct);
        this.m_limitsParallelism = this.m_prematureMerge && ordinalIndexState != OrdinalIndexState.Shuffled;
      }
      else if (this.OutputOrdered)
        this.m_prematureMerge = ExchangeUtilities.IsWorseThan(ordinalIndexState, OrdinalIndexState.Increasing);
      this.SetOrdinalIndexState(OrdinalIndexState.Increasing);
    }

    internal override void WrapPartitionedStream<TLeftKey>(PartitionedStream<TLeftInput, TLeftKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      if (this.m_indexedRightChildSelector != null)
        this.WrapPartitionedStreamIndexed(!this.m_prematureMerge ? (PartitionedStream<TLeftInput, int>) inputStream : QueryOperator<TLeftInput>.ExecuteAndCollectResults<TLeftKey>(inputStream, partitionCount, this.OutputOrdered, preferStriping, settings).GetPartitionedStream(), recipient, settings);
      else if (this.m_prematureMerge)
        this.WrapPartitionedStreamNotIndexed<int>(QueryOperator<TLeftInput>.ExecuteAndCollectResults<TLeftKey>(inputStream, partitionCount, this.OutputOrdered, preferStriping, settings).GetPartitionedStream(), recipient, settings);
      else
        this.WrapPartitionedStreamNotIndexed<TLeftKey>(inputStream, recipient, settings);
    }

    private void WrapPartitionedStreamNotIndexed<TLeftKey>(PartitionedStream<TLeftInput, TLeftKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PairComparer<TLeftKey, int> pairComparer = new PairComparer<TLeftKey, int>(inputStream.KeyComparer, (IComparer<int>) Util.GetDefaultComparer<int>());
      PartitionedStream<TOutput, Pair<TLeftKey, int>> partitionedStream = new PartitionedStream<TOutput, Pair<TLeftKey, int>>(partitionCount, (IComparer<Pair<TLeftKey, int>>) pairComparer, this.OrdinalIndexState);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TOutput, Pair<TLeftKey, int>>) new SelectManyQueryOperator<TLeftInput, TRightInput, TOutput>.SelectManyQueryOperatorEnumerator<TLeftKey>(inputStream[index], this, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<Pair<TLeftKey, int>>(partitionedStream);
    }

    private void WrapPartitionedStreamIndexed(PartitionedStream<TLeftInput, int> inputStream, IPartitionedStreamRecipient<TOutput> recipient, QuerySettings settings)
    {
      PairComparer<int, int> pairComparer = new PairComparer<int, int>(inputStream.KeyComparer, (IComparer<int>) Util.GetDefaultComparer<int>());
      PartitionedStream<TOutput, Pair<int, int>> partitionedStream = new PartitionedStream<TOutput, Pair<int, int>>(inputStream.PartitionCount, (IComparer<Pair<int, int>>) pairComparer, this.OrdinalIndexState);
      for (int index = 0; index < inputStream.PartitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TOutput, Pair<int, int>>) new SelectManyQueryOperator<TLeftInput, TRightInput, TOutput>.IndexedSelectManyQueryOperatorEnumerator(inputStream[index], this, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<Pair<int, int>>(partitionedStream);
    }

    internal override QueryResults<TOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TOutput>) new UnaryQueryOperator<TLeftInput, TOutput>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TLeftInput, TOutput>) this, settings, preferStriping);
    }

    internal override IEnumerable<TOutput> AsSequentialQuery(CancellationToken token)
    {
      if (this.m_rightChildSelector != null)
      {
        if (this.m_resultSelector != null)
          return Enumerable.SelectMany<TLeftInput, TRightInput, TOutput>(CancellableEnumerable.Wrap<TLeftInput>(this.Child.AsSequentialQuery(token), token), this.m_rightChildSelector, this.m_resultSelector);
        else
          return (IEnumerable<TOutput>) Enumerable.SelectMany<TLeftInput, TRightInput>(CancellableEnumerable.Wrap<TLeftInput>(this.Child.AsSequentialQuery(token), token), this.m_rightChildSelector);
      }
      else if (this.m_resultSelector != null)
        return Enumerable.SelectMany<TLeftInput, TRightInput, TOutput>(CancellableEnumerable.Wrap<TLeftInput>(this.Child.AsSequentialQuery(token), token), this.m_indexedRightChildSelector, this.m_resultSelector);
      else
        return (IEnumerable<TOutput>) Enumerable.SelectMany<TLeftInput, TRightInput>(CancellableEnumerable.Wrap<TLeftInput>(this.Child.AsSequentialQuery(token), token), this.m_indexedRightChildSelector);
    }

    private class IndexedSelectManyQueryOperatorEnumerator : QueryOperatorEnumerator<TOutput, Pair<int, int>>
    {
      private readonly QueryOperatorEnumerator<TLeftInput, int> m_leftSource;
      private readonly SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> m_selectManyOperator;
      private IEnumerator<TRightInput> m_currentRightSource;
      private IEnumerator<TOutput> m_currentRightSourceAsOutput;
      private SelectManyQueryOperator<TLeftInput, TRightInput, TOutput>.IndexedSelectManyQueryOperatorEnumerator.Mutables m_mutables;
      private readonly CancellationToken m_cancellationToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal IndexedSelectManyQueryOperatorEnumerator(QueryOperatorEnumerator<TLeftInput, int> leftSource, SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> selectManyOperator, CancellationToken cancellationToken)
      {
        this.m_leftSource = leftSource;
        this.m_selectManyOperator = selectManyOperator;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TOutput currentElement, ref Pair<int, int> currentKey)
      {
        while (true)
        {
          if (this.m_currentRightSource == null)
          {
            this.m_mutables = new SelectManyQueryOperator<TLeftInput, TRightInput, TOutput>.IndexedSelectManyQueryOperatorEnumerator.Mutables();
            if ((this.m_mutables.m_lhsCount++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (this.m_leftSource.MoveNext(ref this.m_mutables.m_currentLeftElement, ref this.m_mutables.m_currentLeftSourceIndex))
            {
              this.m_currentRightSource = this.m_selectManyOperator.m_indexedRightChildSelector(this.m_mutables.m_currentLeftElement, this.m_mutables.m_currentLeftSourceIndex).GetEnumerator();
              if (this.m_selectManyOperator.m_resultSelector == null)
                this.m_currentRightSourceAsOutput = (IEnumerator<TOutput>) this.m_currentRightSource;
            }
            else
              break;
          }
          if (!this.m_currentRightSource.MoveNext())
          {
            this.m_currentRightSource.Dispose();
            this.m_currentRightSource = (IEnumerator<TRightInput>) null;
            this.m_currentRightSourceAsOutput = (IEnumerator<TOutput>) null;
          }
          else
            goto label_8;
        }
        return false;
label_8:
        ++this.m_mutables.m_currentRightSourceIndex;
        currentElement = this.m_selectManyOperator.m_resultSelector == null ? this.m_currentRightSourceAsOutput.Current : this.m_selectManyOperator.m_resultSelector(this.m_mutables.m_currentLeftElement, this.m_currentRightSource.Current);
        currentKey = new Pair<int, int>(this.m_mutables.m_currentLeftSourceIndex, this.m_mutables.m_currentRightSourceIndex);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_leftSource.Dispose();
        if (this.m_currentRightSource == null)
          return;
        this.m_currentRightSource.Dispose();
      }

      private class Mutables
      {
        internal int m_currentRightSourceIndex = -1;
        internal TLeftInput m_currentLeftElement;
        internal int m_currentLeftSourceIndex;
        internal int m_lhsCount;
      }
    }

    private class SelectManyQueryOperatorEnumerator<TLeftKey> : QueryOperatorEnumerator<TOutput, Pair<TLeftKey, int>>
    {
      private readonly QueryOperatorEnumerator<TLeftInput, TLeftKey> m_leftSource;
      private readonly SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> m_selectManyOperator;
      private IEnumerator<TRightInput> m_currentRightSource;
      private IEnumerator<TOutput> m_currentRightSourceAsOutput;
      private SelectManyQueryOperator<TLeftInput, TRightInput, TOutput>.SelectManyQueryOperatorEnumerator<TLeftKey>.Mutables m_mutables;
      private readonly CancellationToken m_cancellationToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal SelectManyQueryOperatorEnumerator(QueryOperatorEnumerator<TLeftInput, TLeftKey> leftSource, SelectManyQueryOperator<TLeftInput, TRightInput, TOutput> selectManyOperator, CancellationToken cancellationToken)
      {
        this.m_leftSource = leftSource;
        this.m_selectManyOperator = selectManyOperator;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TOutput currentElement, ref Pair<TLeftKey, int> currentKey)
      {
        while (true)
        {
          if (this.m_currentRightSource == null)
          {
            this.m_mutables = new SelectManyQueryOperator<TLeftInput, TRightInput, TOutput>.SelectManyQueryOperatorEnumerator<TLeftKey>.Mutables();
            if ((this.m_mutables.m_lhsCount++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (this.m_leftSource.MoveNext(ref this.m_mutables.m_currentLeftElement, ref this.m_mutables.m_currentLeftKey))
            {
              this.m_currentRightSource = this.m_selectManyOperator.m_rightChildSelector(this.m_mutables.m_currentLeftElement).GetEnumerator();
              if (this.m_selectManyOperator.m_resultSelector == null)
                this.m_currentRightSourceAsOutput = (IEnumerator<TOutput>) this.m_currentRightSource;
            }
            else
              break;
          }
          if (!this.m_currentRightSource.MoveNext())
          {
            this.m_currentRightSource.Dispose();
            this.m_currentRightSource = (IEnumerator<TRightInput>) null;
            this.m_currentRightSourceAsOutput = (IEnumerator<TOutput>) null;
          }
          else
            goto label_8;
        }
        return false;
label_8:
        ++this.m_mutables.m_currentRightSourceIndex;
        currentElement = this.m_selectManyOperator.m_resultSelector == null ? this.m_currentRightSourceAsOutput.Current : this.m_selectManyOperator.m_resultSelector(this.m_mutables.m_currentLeftElement, this.m_currentRightSource.Current);
        currentKey = new Pair<TLeftKey, int>(this.m_mutables.m_currentLeftKey, this.m_mutables.m_currentRightSourceIndex);
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_leftSource.Dispose();
        if (this.m_currentRightSource == null)
          return;
        this.m_currentRightSource.Dispose();
      }

      private class Mutables
      {
        internal int m_currentRightSourceIndex = -1;
        internal TLeftInput m_currentLeftElement;
        internal TLeftKey m_currentLeftKey;
        internal int m_lhsCount;
      }
    }
  }
}
