// Type: System.Linq.Parallel.DefaultIfEmptyQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class DefaultIfEmptyQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
  {
    private readonly TSource m_defaultValue;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal DefaultIfEmptyQueryOperator(IEnumerable<TSource> child, TSource defaultValue)
      : base(child)
    {
      this.m_defaultValue = defaultValue;
      this.SetOrdinalIndexState(ExchangeUtilities.Worse(this.Child.OrdinalIndexState, OrdinalIndexState.Correct));
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TSource>) new UnaryQueryOperator<TSource, TSource>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TSource, TSource>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      Shared<int> sharedEmptyCount = new Shared<int>(0);
      CountdownEvent sharedLatch = new CountdownEvent(partitionCount - 1);
      PartitionedStream<TSource, TKey> partitionedStream = new PartitionedStream<TSource, TKey>(partitionCount, inputStream.KeyComparer, this.OrdinalIndexState);
      for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
        partitionedStream[partitionIndex] = (QueryOperatorEnumerator<TSource, TKey>) new DefaultIfEmptyQueryOperator<TSource>.DefaultIfEmptyQueryOperatorEnumerator<TKey>(inputStream[partitionIndex], this.m_defaultValue, partitionIndex, partitionCount, sharedEmptyCount, sharedLatch, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.DefaultIfEmpty<TSource>(this.Child.AsSequentialQuery(token), this.m_defaultValue);
    }

    private class DefaultIfEmptyQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TSource, TKey>
    {
      private QueryOperatorEnumerator<TSource, TKey> m_source;
      private bool m_lookedForEmpty;
      private int m_partitionIndex;
      private int m_partitionCount;
      private TSource m_defaultValue;
      private Shared<int> m_sharedEmptyCount;
      private CountdownEvent m_sharedLatch;
      private CancellationToken m_cancelToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal DefaultIfEmptyQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TKey> source, TSource defaultValue, int partitionIndex, int partitionCount, Shared<int> sharedEmptyCount, CountdownEvent sharedLatch, CancellationToken cancelToken)
      {
        this.m_source = source;
        this.m_defaultValue = defaultValue;
        this.m_partitionIndex = partitionIndex;
        this.m_partitionCount = partitionCount;
        this.m_sharedEmptyCount = sharedEmptyCount;
        this.m_sharedLatch = sharedLatch;
        this.m_cancelToken = cancelToken;
      }

      internal override bool MoveNext(ref TSource currentElement, ref TKey currentKey)
      {
        bool flag = this.m_source.MoveNext(ref currentElement, ref currentKey);
        if (!this.m_lookedForEmpty)
        {
          this.m_lookedForEmpty = true;
          if (!flag)
          {
            if (this.m_partitionIndex == 0)
            {
              this.m_sharedLatch.Wait(this.m_cancelToken);
              this.m_sharedLatch.Dispose();
              if (this.m_sharedEmptyCount.Value != this.m_partitionCount - 1)
                return false;
              currentElement = this.m_defaultValue;
              currentKey = default (TKey);
              return true;
            }
            else
              Interlocked.Increment(ref this.m_sharedEmptyCount.Value);
          }
          if (this.m_partitionIndex != 0)
            this.m_sharedLatch.Signal();
        }
        return flag;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
