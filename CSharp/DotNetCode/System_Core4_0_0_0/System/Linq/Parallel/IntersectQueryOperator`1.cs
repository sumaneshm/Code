// Type: System.Linq.Parallel.IntersectQueryOperator`1
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
  internal sealed class IntersectQueryOperator<TInputOutput> : BinaryQueryOperator<TInputOutput, TInputOutput, TInputOutput>
  {
    private readonly IEqualityComparer<TInputOutput> m_comparer;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal IntersectQueryOperator(ParallelQuery<TInputOutput> left, ParallelQuery<TInputOutput> right, IEqualityComparer<TInputOutput> comparer)
      : base(left, right)
    {
      this.m_comparer = comparer;
      this.m_outputOrdered = this.LeftChild.OutputOrdered;
      this.SetOrdinalIndex(OrdinalIndexState.Shuffled);
    }

    internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInputOutput>) new BinaryQueryOperator<TInputOutput, TInputOutput, TInputOutput>.BinaryQueryOperatorResults(this.LeftChild.Open(settings, false), this.RightChild.Open(settings, false), (BinaryQueryOperator<TInputOutput, TInputOutput, TInputOutput>) this, settings, false);
    }

    public override void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TInputOutput, TLeftKey> leftPartitionedStream, PartitionedStream<TInputOutput, TRightKey> rightPartitionedStream, IPartitionedStreamRecipient<TInputOutput> outputRecipient, bool preferStriping, QuerySettings settings)
    {
      if (this.OutputOrdered)
        this.WrapPartitionedStreamHelper<TLeftKey, TRightKey>(ExchangeUtilities.HashRepartitionOrdered<TInputOutput, NoKeyMemoizationRequired, TLeftKey>(leftPartitionedStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, settings.CancellationState.MergedCancellationToken), rightPartitionedStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
      else
        this.WrapPartitionedStreamHelper<int, TRightKey>(ExchangeUtilities.HashRepartition<TInputOutput, NoKeyMemoizationRequired, TLeftKey>(leftPartitionedStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, settings.CancellationState.MergedCancellationToken), rightPartitionedStream, outputRecipient, settings.CancellationState.MergedCancellationToken);
    }

    private void WrapPartitionedStreamHelper<TLeftKey, TRightKey>(PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftHashStream, PartitionedStream<TInputOutput, TRightKey> rightPartitionedStream, IPartitionedStreamRecipient<TInputOutput> outputRecipient, CancellationToken cancellationToken)
    {
      int partitionCount = leftHashStream.PartitionCount;
      PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, int> partitionedStream1 = ExchangeUtilities.HashRepartition<TInputOutput, NoKeyMemoizationRequired, TRightKey>(rightPartitionedStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, cancellationToken);
      PartitionedStream<TInputOutput, TLeftKey> partitionedStream2 = new PartitionedStream<TInputOutput, TLeftKey>(partitionCount, leftHashStream.KeyComparer, OrdinalIndexState.Shuffled);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream2[index] = !this.OutputOrdered ? (QueryOperatorEnumerator<TInputOutput, TLeftKey>) new IntersectQueryOperator<TInputOutput>.IntersectQueryOperatorEnumerator<TLeftKey>(leftHashStream[index], partitionedStream1[index], this.m_comparer, cancellationToken) : (QueryOperatorEnumerator<TInputOutput, TLeftKey>) new IntersectQueryOperator<TInputOutput>.OrderedIntersectQueryOperatorEnumerator<TLeftKey>(leftHashStream[index], partitionedStream1[index], this.m_comparer, leftHashStream.KeyComparer, cancellationToken);
      outputRecipient.Receive<TLeftKey>(partitionedStream2);
    }

    internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Intersect<TInputOutput>(CancellableEnumerable.Wrap<TInputOutput>(this.LeftChild.AsSequentialQuery(token), token), CancellableEnumerable.Wrap<TInputOutput>(this.RightChild.AsSequentialQuery(token), token), this.m_comparer);
    }

    private class IntersectQueryOperatorEnumerator<TLeftKey> : QueryOperatorEnumerator<TInputOutput, int>
    {
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> m_leftSource;
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, int> m_rightSource;
      private IEqualityComparer<TInputOutput> m_comparer;
      private Set<TInputOutput> m_hashLookup;
      private CancellationToken m_cancellationToken;
      private Shared<int> m_outputLoopCount;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal IntersectQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftSource, QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, int> rightSource, IEqualityComparer<TInputOutput> comparer, CancellationToken cancellationToken)
      {
        this.m_leftSource = leftSource;
        this.m_rightSource = rightSource;
        this.m_comparer = comparer;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref int currentKey)
      {
        if (this.m_hashLookup == null)
        {
          this.m_outputLoopCount = new Shared<int>(0);
          this.m_hashLookup = new Set<TInputOutput>(this.m_comparer);
          Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
          int currentKey1 = 0;
          int num = 0;
          while (this.m_rightSource.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            this.m_hashLookup.Add(currentElement1.First);
          }
        }
        Pair<TInputOutput, NoKeyMemoizationRequired> currentElement2 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
        TLeftKey currentKey2 = default (TLeftKey);
        while (this.m_leftSource.MoveNext(ref currentElement2, ref currentKey2))
        {
          if ((this.m_outputLoopCount.Value++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (this.m_hashLookup.Contains(currentElement2.First))
          {
            this.m_hashLookup.Remove(currentElement2.First);
            currentElement = currentElement2.First;
            return true;
          }
        }
        return false;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_leftSource.Dispose();
        this.m_rightSource.Dispose();
      }
    }

    private class OrderedIntersectQueryOperatorEnumerator<TLeftKey> : QueryOperatorEnumerator<TInputOutput, TLeftKey>
    {
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> m_leftSource;
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, int> m_rightSource;
      private IEqualityComparer<Wrapper<TInputOutput>> m_comparer;
      private IComparer<TLeftKey> m_leftKeyComparer;
      private Dictionary<Wrapper<TInputOutput>, Pair<TInputOutput, TLeftKey>> m_hashLookup;
      private CancellationToken m_cancellationToken;

      internal OrderedIntersectQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TLeftKey> leftSource, QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, int> rightSource, IEqualityComparer<TInputOutput> comparer, IComparer<TLeftKey> leftKeyComparer, CancellationToken cancellationToken)
      {
        this.m_leftSource = leftSource;
        this.m_rightSource = rightSource;
        this.m_comparer = (IEqualityComparer<Wrapper<TInputOutput>>) new WrapperEqualityComparer<TInputOutput>(comparer);
        this.m_leftKeyComparer = leftKeyComparer;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref TLeftKey currentKey)
      {
        int num = 0;
        if (this.m_hashLookup == null)
        {
          this.m_hashLookup = new Dictionary<Wrapper<TInputOutput>, Pair<TInputOutput, TLeftKey>>(this.m_comparer);
          Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
          TLeftKey currentKey1 = default (TLeftKey);
          while (this.m_leftSource.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            Wrapper<TInputOutput> key = new Wrapper<TInputOutput>(currentElement1.First);
            Pair<TInputOutput, TLeftKey> pair;
            if (!this.m_hashLookup.TryGetValue(key, out pair) || this.m_leftKeyComparer.Compare(currentKey1, pair.Second) < 0)
              this.m_hashLookup[key] = new Pair<TInputOutput, TLeftKey>(currentElement1.First, currentKey1);
          }
        }
        Pair<TInputOutput, NoKeyMemoizationRequired> currentElement2 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
        int currentKey2 = 0;
        while (this.m_rightSource.MoveNext(ref currentElement2, ref currentKey2))
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          Pair<TInputOutput, TLeftKey> pair;
          if (this.m_hashLookup.TryGetValue(new Wrapper<TInputOutput>(currentElement2.First), out pair))
          {
            currentElement = pair.First;
            currentKey = pair.Second;
            this.m_hashLookup.Remove(new Wrapper<TInputOutput>(pair.First));
            return true;
          }
        }
        return false;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_leftSource.Dispose();
        this.m_rightSource.Dispose();
      }
    }
  }
}
