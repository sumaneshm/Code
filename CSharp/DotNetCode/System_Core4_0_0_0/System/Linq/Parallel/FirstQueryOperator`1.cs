// Type: System.Linq.Parallel.FirstQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class FirstQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
  {
    private readonly Func<TSource, bool> m_predicate;
    private readonly bool m_prematureMergeNeeded;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal FirstQueryOperator(IEnumerable<TSource> child, Func<TSource, bool> predicate)
      : base(child)
    {
      this.m_predicate = predicate;
      this.m_prematureMergeNeeded = ExchangeUtilities.IsWorseThan(this.Child.OrdinalIndexState, OrdinalIndexState.Increasing);
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TSource>) new UnaryQueryOperator<TSource, TSource>.UnaryQueryOperatorResults(this.Child.Open(settings, false), (UnaryQueryOperator<TSource, TSource>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
    {
      if (this.m_prematureMergeNeeded)
        this.WrapHelper<int>(QueryOperator<TSource>.ExecuteAndCollectResults<TKey>(inputStream, inputStream.PartitionCount, this.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream(), recipient, settings);
      else
        this.WrapHelper<TKey>(inputStream, recipient, settings);
    }

    private void WrapHelper<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      FirstQueryOperator<TSource>.FirstQueryOperatorState<TKey> operatorState = new FirstQueryOperator<TSource>.FirstQueryOperatorState<TKey>();
      CountdownEvent sharedBarrier = new CountdownEvent(partitionCount);
      PartitionedStream<TSource, int> partitionedStream = new PartitionedStream<TSource, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Shuffled);
      for (int partitionId = 0; partitionId < partitionCount; ++partitionId)
        partitionedStream[partitionId] = (QueryOperatorEnumerator<TSource, int>) new FirstQueryOperator<TSource>.FirstQueryOperatorEnumerator<TKey>(inputStream[partitionId], this.m_predicate, operatorState, sharedBarrier, settings.CancellationState.MergedCancellationToken, inputStream.KeyComparer, partitionId);
      recipient.Receive<int>(partitionedStream);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }

    private class FirstQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TSource, int>
    {
      private QueryOperatorEnumerator<TSource, TKey> m_source;
      private Func<TSource, bool> m_predicate;
      private bool m_alreadySearched;
      private int m_partitionId;
      private FirstQueryOperator<TSource>.FirstQueryOperatorState<TKey> m_operatorState;
      private CountdownEvent m_sharedBarrier;
      private CancellationToken m_cancellationToken;
      private IComparer<TKey> m_keyComparer;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal FirstQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TKey> source, Func<TSource, bool> predicate, FirstQueryOperator<TSource>.FirstQueryOperatorState<TKey> operatorState, CountdownEvent sharedBarrier, CancellationToken cancellationToken, IComparer<TKey> keyComparer, int partitionId)
      {
        this.m_source = source;
        this.m_predicate = predicate;
        this.m_operatorState = operatorState;
        this.m_sharedBarrier = sharedBarrier;
        this.m_cancellationToken = cancellationToken;
        this.m_keyComparer = keyComparer;
        this.m_partitionId = partitionId;
      }

      internal override bool MoveNext(ref TSource currentElement, ref int currentKey)
      {
        if (this.m_alreadySearched)
          return false;
        TSource source = default (TSource);
        TKey key = default (TKey);
        try
        {
          TSource currentElement1 = default (TSource);
          TKey currentKey1 = default (TKey);
          int num = 0;
          while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            if (this.m_predicate == null || this.m_predicate(currentElement1))
            {
              source = currentElement1;
              TKey x = currentKey1;
              lock (this.m_operatorState)
              {
                if (this.m_operatorState.m_partitionId != -1)
                {
                  if (this.m_keyComparer.Compare(x, this.m_operatorState.m_key) >= 0)
                    break;
                }
                this.m_operatorState.m_key = x;
                this.m_operatorState.m_partitionId = this.m_partitionId;
                break;
              }
            }
          }
        }
        finally
        {
          this.m_sharedBarrier.Signal();
        }
        this.m_alreadySearched = true;
        if (this.m_partitionId == this.m_operatorState.m_partitionId)
        {
          this.m_sharedBarrier.Wait(this.m_cancellationToken);
          if (this.m_partitionId == this.m_operatorState.m_partitionId)
          {
            currentElement = source;
            currentKey = 0;
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

    private class FirstQueryOperatorState<TKey>
    {
      internal int m_partitionId = -1;
      internal TKey m_key;
    }
  }
}
