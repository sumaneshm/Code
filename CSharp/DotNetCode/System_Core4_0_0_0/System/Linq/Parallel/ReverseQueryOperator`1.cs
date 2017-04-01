// Type: System.Linq.Parallel.ReverseQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class ReverseQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
  {
    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal ReverseQueryOperator(IEnumerable<TSource> child)
      : base(child)
    {
      if (this.Child.OrdinalIndexState == OrdinalIndexState.Indexible)
        this.SetOrdinalIndexState(OrdinalIndexState.Indexible);
      else
        this.SetOrdinalIndexState(OrdinalIndexState.Shuffled);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TSource, TKey> partitionedStream = new PartitionedStream<TSource, TKey>(partitionCount, (IComparer<TKey>) new ReverseComparer<TKey>(inputStream.KeyComparer), OrdinalIndexState.Shuffled);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TSource, TKey>) new ReverseQueryOperator<TSource>.ReverseQueryOperatorEnumerator<TKey>(inputStream[index], settings.CancellationState.MergedCancellationToken);
      recipient.Receive<TKey>(partitionedStream);
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return ReverseQueryOperator<TSource>.ReverseQueryOperatorResults.NewResults(this.Child.Open(settings, false), this, settings, preferStriping);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      return Enumerable.Reverse<TSource>(CancellableEnumerable.Wrap<TSource>(this.Child.AsSequentialQuery(token), token));
    }

    private class ReverseQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TSource, TKey>
    {
      private readonly QueryOperatorEnumerator<TSource, TKey> m_source;
      private readonly CancellationToken m_cancellationToken;
      private List<Pair<TSource, TKey>> m_buffer;
      private Shared<int> m_bufferIndex;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ReverseQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TKey> source, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TSource currentElement, ref TKey currentKey)
      {
        if (this.m_buffer == null)
        {
          this.m_bufferIndex = new Shared<int>(0);
          this.m_buffer = new List<Pair<TSource, TKey>>();
          TSource currentElement1 = default (TSource);
          TKey currentKey1 = default (TKey);
          int num = 0;
          while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          {
            if ((num++ & 63) == 0)
              CancellationState.ThrowIfCanceled(this.m_cancellationToken);
            this.m_buffer.Add(new Pair<TSource, TKey>(currentElement1, currentKey1));
            ++this.m_bufferIndex.Value;
          }
        }
        if (--this.m_bufferIndex.Value < 0)
          return false;
        currentElement = this.m_buffer[this.m_bufferIndex.Value].First;
        currentKey = this.m_buffer[this.m_bufferIndex.Value].Second;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }

    private class ReverseQueryOperatorResults : UnaryQueryOperator<TSource, TSource>.UnaryQueryOperatorResults
    {
      private int m_count;

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
          return this.m_count;
        }
      }

      private ReverseQueryOperatorResults(QueryResults<TSource> childQueryResults, ReverseQueryOperator<TSource> op, QuerySettings settings, bool preferStriping)
        : base(childQueryResults, (UnaryQueryOperator<TSource, TSource>) op, settings, preferStriping)
      {
        this.m_count = this.m_childQueryResults.ElementsCount;
      }

      public static QueryResults<TSource> NewResults(QueryResults<TSource> childQueryResults, ReverseQueryOperator<TSource> op, QuerySettings settings, bool preferStriping)
      {
        if (childQueryResults.IsIndexible)
          return (QueryResults<TSource>) new ReverseQueryOperator<TSource>.ReverseQueryOperatorResults(childQueryResults, op, settings, preferStriping);
        else
          return (QueryResults<TSource>) new UnaryQueryOperator<TSource, TSource>.UnaryQueryOperatorResults(childQueryResults, (UnaryQueryOperator<TSource, TSource>) op, settings, preferStriping);
      }

      internal override TSource GetElement(int index)
      {
        return this.m_childQueryResults.GetElement(this.m_count - index - 1);
      }
    }
  }
}
