// Type: System.Linq.Parallel.DistinctQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class DistinctQueryOperator<TInputOutput> : UnaryQueryOperator<TInputOutput, TInputOutput>
  {
    private readonly IEqualityComparer<TInputOutput> m_comparer;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal DistinctQueryOperator(IEnumerable<TInputOutput> source, IEqualityComparer<TInputOutput> comparer)
      : base(source)
    {
      this.m_comparer = comparer;
      this.SetOrdinalIndexState(OrdinalIndexState.Shuffled);
    }

    internal override QueryResults<TInputOutput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInputOutput>) new UnaryQueryOperator<TInputOutput, TInputOutput>.UnaryQueryOperatorResults(this.Child.Open(settings, false), (UnaryQueryOperator<TInputOutput, TInputOutput>) this, settings, false);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInputOutput, TKey> inputStream, IPartitionedStreamRecipient<TInputOutput> recipient, bool preferStriping, QuerySettings settings)
    {
      if (this.OutputOrdered)
        this.WrapPartitionedStreamHelper<TKey>(ExchangeUtilities.HashRepartitionOrdered<TInputOutput, NoKeyMemoizationRequired, TKey>(inputStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, settings.CancellationState.MergedCancellationToken), recipient, settings.CancellationState.MergedCancellationToken);
      else
        this.WrapPartitionedStreamHelper<int>(ExchangeUtilities.HashRepartition<TInputOutput, NoKeyMemoizationRequired, TKey>(inputStream, (Func<TInputOutput, NoKeyMemoizationRequired>) null, (IEqualityComparer<NoKeyMemoizationRequired>) null, this.m_comparer, settings.CancellationState.MergedCancellationToken), recipient, settings.CancellationState.MergedCancellationToken);
    }

    private void WrapPartitionedStreamHelper<TKey>(PartitionedStream<Pair<TInputOutput, NoKeyMemoizationRequired>, TKey> hashStream, IPartitionedStreamRecipient<TInputOutput> recipient, CancellationToken cancellationToken)
    {
      int partitionCount = hashStream.PartitionCount;
      PartitionedStream<TInputOutput, TKey> partitionedStream = new PartitionedStream<TInputOutput, TKey>(partitionCount, hashStream.KeyComparer, OrdinalIndexState.Shuffled);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = !this.OutputOrdered ? (QueryOperatorEnumerator<TInputOutput, TKey>) new DistinctQueryOperator<TInputOutput>.DistinctQueryOperatorEnumerator<TKey>(hashStream[index], this.m_comparer, cancellationToken) : (QueryOperatorEnumerator<TInputOutput, TKey>) new DistinctQueryOperator<TInputOutput>.OrderedDistinctQueryOperatorEnumerator<TKey>(hashStream[index], this.m_comparer, hashStream.KeyComparer, cancellationToken);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override IEnumerable<TInputOutput> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Distinct<TInputOutput>(CancellableEnumerable.Wrap<TInputOutput>(this.Child.AsSequentialQuery(token), token), this.m_comparer);
    }

    private class DistinctQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TInputOutput, int>
    {
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TKey> m_source;
      private Set<TInputOutput> m_hashLookup;
      private CancellationToken m_cancellationToken;
      private Shared<int> m_outputLoopCount;

      internal DistinctQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TKey> source, IEqualityComparer<TInputOutput> comparer, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_hashLookup = new Set<TInputOutput>(comparer);
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref int currentKey)
      {
        TKey currentKey1 = default (TKey);
        Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
        if (this.m_outputLoopCount == null)
          this.m_outputLoopCount = new Shared<int>(0);
        while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
        {
          if ((this.m_outputLoopCount.Value++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (this.m_hashLookup.Add(currentElement1.First))
          {
            currentElement = currentElement1.First;
            return true;
          }
        }
        return false;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }

    private class OrderedDistinctQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TInputOutput, TKey>
    {
      private QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TKey> m_source;
      private Dictionary<Wrapper<TInputOutput>, TKey> m_hashLookup;
      private IComparer<TKey> m_keyComparer;
      private IEnumerator<KeyValuePair<Wrapper<TInputOutput>, TKey>> m_hashLookupEnumerator;
      private CancellationToken m_cancellationToken;

      internal OrderedDistinctQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TInputOutput, NoKeyMemoizationRequired>, TKey> source, IEqualityComparer<TInputOutput> comparer, IComparer<TKey> keyComparer, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_keyComparer = keyComparer;
        this.m_hashLookup = new Dictionary<Wrapper<TInputOutput>, TKey>((IEqualityComparer<Wrapper<TInputOutput>>) new WrapperEqualityComparer<TInputOutput>(comparer));
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInputOutput currentElement, ref TKey currentKey)
      {
        if (this.m_hashLookupEnumerator == null)
        {
          Pair<TInputOutput, NoKeyMemoizationRequired> currentElement1 = new Pair<TInputOutput, NoKeyMemoizationRequired>();
          TKey currentKey1 = default (TKey);
          int num = 0;
          while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            Wrapper<TInputOutput> key = new Wrapper<TInputOutput>(currentElement1.First);
            TKey y;
            if (!this.m_hashLookup.TryGetValue(key, out y) || this.m_keyComparer.Compare(currentKey1, y) < 0)
              this.m_hashLookup[key] = currentKey1;
          }
          this.m_hashLookupEnumerator = (IEnumerator<KeyValuePair<Wrapper<TInputOutput>, TKey>>) this.m_hashLookup.GetEnumerator();
        }
        if (!this.m_hashLookupEnumerator.MoveNext())
          return false;
        KeyValuePair<Wrapper<TInputOutput>, TKey> current = this.m_hashLookupEnumerator.Current;
        currentElement = current.Key.Value;
        currentKey = current.Value;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
        if (this.m_hashLookupEnumerator == null)
          return;
        this.m_hashLookupEnumerator.Dispose();
      }
    }
  }
}
