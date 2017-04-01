// Type: System.Linq.Parallel.UnionQueryOperator`1
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
  internal sealed class UnionQueryOperator<TInputOutput> : BinaryQueryOperator<TInputOutput, TInputOutput, TInputOutput>
  {
    private readonly IEqualityComparer<TInputOutput> m_comparer;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal UnionQueryOperator(ParallelQuery<TInputOutput> left, ParallelQuery<TInputOutput> right, IEqualityComparer<TInputOutput> comparer)
      : base(left, right)
    {
      this.m_comparer = comparer;
      this.m_outputOrdered = this.LeftChild.OutputOrdered || this.RightChild.OutputOrdered;
    }

    internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInputOutput>) new BinaryQueryOperator<TInputOutput, TInputOutput, TInputOutput>.BinaryQueryOperatorResults(this.LeftChild.Open(settings, false), this.RightChild.Open(settings, false), (BinaryQueryOperator<TInputOutput, TInputOutput, TInputOutput>) this, settings, false);
    }

    public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TInputOutput, TLeftKey> leftStream, PartitionedStream<TInputOutput, TRightKey> rightStream, IPartitionedStreamRecipient<TInputOutput> outputRecipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = leftStream.PartitionCount;
      if (this.LeftChild.OutputOrdered)
        this.WrapPartitionedStreamFixedLeftType<TLeftKey, TRightKey>(ExchangeUtilities.HashRepartitionOrdered<TInputOutput, NoKeyMemoizationRequired, TLeftKey>(leftStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, partitionCount, settings.CancellationState.MergedCancellationToken);
      else
        this.WrapPartitionedStreamFixedLeftType<int, TRightKey>(ExchangeUtilities.HashRepartition<TInputOutput, NoKeyMemoizationRequired, TLeftKey>(leftStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, settings.CancellationState.MergedCancellationToken), rightStream, outputRecipient, partitionCount, settings.CancellationState.MergedCancellationToken);
    }

    private void WrapPartitionedStreamFixedLeftType<TLeftKey, TRightKey>(PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftHashStream, PartitionedStream<TInputOutput, TRightKey> rightStream, IPartitionedStreamRecipient<TInputOutput> outputRecipient, int partitionCount, CancellationToken cancellationToken)
    {
      if (this.RightChild.OutputOrdered)
      {
        PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey> rightHashStream = ExchangeUtilities.HashRepartitionOrdered<TInputOutput, NoKeyMemoizationRequired, TRightKey>(rightStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, cancellationToken);
        this.WrapPartitionedStreamFixedBothTypes<TLeftKey, TRightKey>(leftHashStream, rightHashStream, outputRecipient, partitionCount, cancellationToken);
      }
      else
      {
        PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, int> rightHashStream = ExchangeUtilities.HashRepartition<TInputOutput, NoKeyMemoizationRequired, TRightKey>(rightStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, cancellationToken);
        this.WrapPartitionedStreamFixedBothTypes<TLeftKey, int>(leftHashStream, rightHashStream, outputRecipient, partitionCount, cancellationToken);
      }
    }

    private void WrapPartitionedStreamFixedBothTypes<TLeftKey, TRightKey>(PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftHashStream, PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey> rightHashStream, IPartitionedStreamRecipient<TInputOutput> outputRecipient, int partitionCount, CancellationToken cancellationToken)
    {
      if (this.LeftChild.OutputOrdered || this.RightChild.OutputOrdered)
      {
        IComparer<ConcatKey<TLeftKey, TRightKey>> keyComparer = ConcatKey<TLeftKey, TRightKey>.MakeComparer(leftHashStream.KeyComparer, rightHashStream.KeyComparer);
        PartitionedStream<TInputOutput, ConcatKey<TLeftKey, TRightKey>> partitionedStream = new PartitionedStream<TInputOutput, ConcatKey<TLeftKey, TRightKey>>(partitionCount, keyComparer, OrdinalIndexState.Shuffled);
        for (int index = 0; index < partitionCount; ++index)
          partitionedStream[index] = (QueryOperatorEnumerator<TInputOutput, ConcatKey<TLeftKey, TRightKey>>) new UnionQueryOperator<TInputOutput>.OrderedUnionQueryOperatorEnumerator<TLeftKey, TRightKey>(leftHashStream[index], rightHashStream[index], this.LeftChild.OutputOrdered, this.RightChild.OutputOrdered, this.m_comparer, keyComparer, cancellationToken);
        outputRecipient.Receive<ConcatKey<TLeftKey, TRightKey>>(partitionedStream);
      }
      else
      {
        PartitionedStream<TInputOutput, int> partitionedStream = new PartitionedStream<TInputOutput, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Shuffled);
        for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
          partitionedStream[partitionIndex] = (QueryOperatorEnumerator<TInputOutput, int>) new UnionQueryOperator<TInputOutput>.UnionQueryOperatorEnumerator<TLeftKey, TRightKey>(leftHashStream[partitionIndex], rightHashStream[partitionIndex], partitionIndex, this.m_comparer, cancellationToken);
        outputRecipient.Receive<int>(partitionedStream);
      }
    }

    internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Union<TInputOutput>(CancellableEnumerable.Wrap<TInputOutput>(this.LeftChild.AsSequentialQuery(token), token), CancellableEnumerable.Wrap<TInputOutput>(this.RightChild.AsSequentialQuery(token), token), this.m_comparer);
    }

    private class UnionQueryOperatorEnumerator<TLeftKey, TRightKey> : QueryOperatorEnumerator<TInputOutput, int>
    {
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> m_leftSource;
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey> m_rightSource;
      private readonly int m_partitionIndex;
      private Set<TInputOutput> m_hashLookup;
      private CancellationToken m_cancellationToken;
      private Shared<int> m_outputLoopCount;
      private readonly IEqualityComparer<TInputOutput> m_comparer;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal UnionQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftSource, QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey> rightSource, int partitionIndex, IEqualityComparer<TInputOutput> comparer, CancellationToken cancellationToken)
      {
        this.m_leftSource = leftSource;
        this.m_rightSource = rightSource;
        this.m_partitionIndex = partitionIndex;
        this.m_comparer = comparer;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref int currentKey)
      {
        if (this.m_hashLookup == null)
        {
          this.m_hashLookup = new Set<TInputOutput>(this.m_comparer);
          this.m_outputLoopCount = new Shared<int>(0);
        }
        if (this.m_leftSource != null)
        {
          TLeftKey currentKey1 = default (TLeftKey);
          Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
          int num = 0;
          while (this.m_leftSource.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (this.m_hashLookup.Add(currentElement1.First))
            {
              currentElement = currentElement1.First;
              return true;
            }
          }
          this.m_leftSource.Dispose();
          this.m_leftSource = (QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey>) null;
        }
        if (this.m_rightSource != null)
        {
          TRightKey currentKey1 = default (TRightKey);
          Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
          while (this.m_rightSource.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((this.m_outputLoopCount.Value++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (this.m_hashLookup.Add(currentElement1.First))
            {
              currentElement = currentElement1.First;
              return true;
            }
          }
          this.m_rightSource.Dispose();
          this.m_rightSource = (QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey>) null;
        }
        return false;
      }

      protected override void Dispose(bool disposing)
      {
        if (this.m_leftSource != null)
          this.m_leftSource.Dispose();
        if (this.m_rightSource == null)
          return;
        this.m_rightSource.Dispose();
      }
    }

    private class OrderedUnionQueryOperatorEnumerator<TLeftKey, TRightKey> : QueryOperatorEnumerator<TInputOutput, ConcatKey<TLeftKey, TRightKey>>
    {
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> m_leftSource;
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey> m_rightSource;
      private IComparer<ConcatKey<TLeftKey, TRightKey>> m_keyComparer;
      private IEnumerator<KeyValuePair<Wrapper<TInputOutput>, Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>>>> m_outputEnumerator;
      private bool m_leftOrdered;
      private bool m_rightOrdered;
      private IEqualityComparer<TInputOutput> m_comparer;
      private CancellationToken m_cancellationToken;

      internal OrderedUnionQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftSource, QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TRightKey> rightSource, bool leftOrdered, bool rightOrdered, IEqualityComparer<TInputOutput> comparer, IComparer<ConcatKey<TLeftKey, TRightKey>> keyComparer, CancellationToken cancellationToken)
      {
        this.m_leftSource = leftSource;
        this.m_rightSource = rightSource;
        this.m_keyComparer = keyComparer;
        this.m_leftOrdered = leftOrdered;
        this.m_rightOrdered = rightOrdered;
        this.m_comparer = comparer;
        if (this.m_comparer == null)
          this.m_comparer = (IEqualityComparer<TInputOutput>) EqualityComparer<TInputOutput>.Default;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref ConcatKey<TLeftKey, TRightKey> currentKey)
      {
        if (this.m_outputEnumerator == null)
        {
          Dictionary<Wrapper<TInputOutput>, Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>>> dictionary = new Dictionary<Wrapper<TInputOutput>, Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>>>((IEqualityComparer<Wrapper<TInputOutput>>) new WrapperEqualityComparer<TInputOutput>(this.m_comparer));
          Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
          TLeftKey currentKey1 = default (TLeftKey);
          int num = 0;
          while (this.m_leftSource.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            ConcatKey<TLeftKey, TRightKey> concatKey = ConcatKey<TLeftKey, TRightKey>.MakeLeft(this.m_leftOrdered ? currentKey1 : default (TLeftKey));
            Wrapper<TInputOutput> key = new Wrapper<TInputOutput>(currentElement1.First);
            Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>> pair;
            if (!dictionary.TryGetValue(key, out pair) || this.m_keyComparer.Compare(concatKey, pair.Second) < 0)
              dictionary[key] = new Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>>(currentElement1.First, concatKey);
          }
          TRightKey currentKey2 = default (TRightKey);
          while (this.m_rightSource.MoveNext(ref currentElement1, ref currentKey2))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            ConcatKey<TLeftKey, TRightKey> concatKey = ConcatKey<TLeftKey, TRightKey>.MakeRight(this.m_rightOrdered ? currentKey2 : default (TRightKey));
            Wrapper<TInputOutput> key = new Wrapper<TInputOutput>(currentElement1.First);
            Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>> pair;
            if (!dictionary.TryGetValue(key, out pair) || this.m_keyComparer.Compare(concatKey, pair.Second) < 0)
              dictionary[key] = new Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>>(currentElement1.First, concatKey);
          }
          this.m_outputEnumerator = (IEnumerator<KeyValuePair<Wrapper<TInputOutput>, Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>>>>) dictionary.GetEnumerator();
        }
        if (!this.m_outputEnumerator.MoveNext())
          return false;
        Pair<TInputOutput, ConcatKey<TLeftKey, TRightKey>> pair1 = this.m_outputEnumerator.Current.Value;
        currentElement = pair1.First;
        currentKey = pair1.Second;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_leftSource.Dispose();
        this.m_rightSource.Dispose();
      }
    }
  }
}
