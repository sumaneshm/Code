// Type: System.Linq.Parallel.SingleQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class SingleQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
  {
    private readonly Func<TSource, bool> m_predicate;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal SingleQueryOperator(IEnumerable<TSource> child, Func<TSource, bool> predicate)
      : base(child)
    {
      this.m_predicate = predicate;
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TSource>) new UnaryQueryOperator<TSource, TSource>.UnaryQueryOperatorResults(this.Child.Open(settings, false), (UnaryQueryOperator<TSource, TSource>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TSource, int> partitionedStream = new PartitionedStream<TSource, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Shuffled);
      Shared<int> totalElementCount = new Shared<int>(0);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TSource, int>) new SingleQueryOperator<TSource>.SingleQueryOperatorEnumerator<TKey>(inputStream[index], this.m_predicate, totalElementCount);
      recipient.Receive<int>(partitionedStream);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }

    private class SingleQueryOperatorEnumerator<TKey> : QueryOperatorEnumerator<TSource, int>
    {
      private QueryOperatorEnumerator<TSource, TKey> m_source;
      private Func<TSource, bool> m_predicate;
      private bool m_alreadySearched;
      private bool m_yieldExtra;
      private Shared<int> m_totalElementCount;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal SingleQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, TKey> source, Func<TSource, bool> predicate, Shared<int> totalElementCount)
      {
        this.m_source = source;
        this.m_predicate = predicate;
        this.m_totalElementCount = totalElementCount;
      }

      internal override bool MoveNext(ref TSource currentElement, ref int currentKey)
      {
        if (this.m_alreadySearched)
        {
          if (!this.m_yieldExtra)
            return false;
          this.m_yieldExtra = false;
          currentElement = default (TSource);
          currentKey = 0;
          return true;
        }
        else
        {
          bool flag = false;
          TSource currentElement1 = default (TSource);
          TKey currentKey1 = default (TKey);
          while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
          {
            if (this.m_predicate == null || this.m_predicate(currentElement1))
            {
              Interlocked.Increment(ref this.m_totalElementCount.Value);
              currentElement = currentElement1;
              currentKey = 0;
              if (flag)
              {
                this.m_yieldExtra = true;
                break;
              }
              else
                flag = true;
            }
            if (Volatile.Read(ref this.m_totalElementCount.Value) > 1)
              break;
          }
          this.m_alreadySearched = true;
          return flag;
        }
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
