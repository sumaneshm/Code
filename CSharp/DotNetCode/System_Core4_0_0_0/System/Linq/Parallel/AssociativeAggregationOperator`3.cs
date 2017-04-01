// Type: System.Linq.Parallel.AssociativeAggregationOperator`3
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
  internal sealed class AssociativeAggregationOperator<TInput, TIntermediate, TOutput> : UnaryQueryOperator<TInput, TIntermediate>
  {
    private readonly TIntermediate m_seed;
    private readonly bool m_seedIsSpecified;
    private readonly bool m_throwIfEmpty;
    private Func<TIntermediate, TInput, TIntermediate> m_intermediateReduce;
    private Func<TIntermediate, TIntermediate, TIntermediate> m_finalReduce;
    private Func<TIntermediate, TOutput> m_resultSelector;
    private Func<TIntermediate> m_seedFactory;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal AssociativeAggregationOperator(IEnumerable<TInput> child, TIntermediate seed, Func<TIntermediate> seedFactory, bool seedIsSpecified, Func<TIntermediate, TInput, TIntermediate> intermediateReduce, Func<TIntermediate, TIntermediate, TIntermediate> finalReduce, Func<TIntermediate, TOutput> resultSelector, bool throwIfEmpty, QueryAggregationOptions options)
      : base(child)
    {
      this.m_seed = seed;
      this.m_seedFactory = seedFactory;
      this.m_seedIsSpecified = seedIsSpecified;
      this.m_intermediateReduce = intermediateReduce;
      this.m_finalReduce = finalReduce;
      this.m_resultSelector = resultSelector;
      this.m_throwIfEmpty = throwIfEmpty;
    }

    internal TOutput Aggregate()
    {
      TIntermediate intermediate = default (TIntermediate);
      bool flag = false;
      using (IEnumerator<TIntermediate> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true))
      {
        while (enumerator.MoveNext())
        {
          if (flag)
          {
            try
            {
              intermediate = this.m_finalReduce(intermediate, enumerator.Current);
            }
            catch (ThreadAbortException ex)
            {
              throw;
            }
            catch (Exception ex)
            {
              throw new AggregateException(new Exception[1]
              {
                ex
              });
            }
          }
          else
          {
            intermediate = enumerator.Current;
            flag = true;
          }
        }
        if (!flag)
        {
          if (this.m_throwIfEmpty)
            throw new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
          intermediate = this.m_seedFactory == null ? this.m_seed : this.m_seedFactory();
        }
      }
      try
      {
        return this.m_resultSelector(intermediate);
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new AggregateException(new Exception[1]
        {
          ex
        });
      }
    }

    internal override QueryResults<TIntermediate> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TIntermediate>) new UnaryQueryOperator<TInput, TIntermediate>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TInput, TIntermediate>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<TIntermediate> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TIntermediate, int> partitionedStream = new PartitionedStream<TIntermediate, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
      for (int partitionIndex = 0; partitionIndex < partitionCount; ++partitionIndex)
        partitionedStream[partitionIndex] = (QueryOperatorEnumerator<TIntermediate, int>) new AssociativeAggregationOperator<TInput, TIntermediate, TOutput>.AssociativeAggregationOperatorEnumerator<TKey>(inputStream[partitionIndex], this, partitionIndex, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream);
    }

    internal override IEnumerable<TIntermediate> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }

    private class AssociativeAggregationOperatorEnumerator<TKey> : QueryOperatorEnumerator<TIntermediate, int>
    {
      private readonly QueryOperatorEnumerator<TInput, TKey> m_source;
      private readonly AssociativeAggregationOperator<TInput, TIntermediate, TOutput> m_reduceOperator;
      private readonly int m_partitionIndex;
      private readonly CancellationToken m_cancellationToken;
      private bool m_accumulated;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal AssociativeAggregationOperatorEnumerator(QueryOperatorEnumerator<TInput, TKey> source, AssociativeAggregationOperator<TInput, TIntermediate, TOutput> reduceOperator, int partitionIndex, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_reduceOperator = reduceOperator;
        this.m_partitionIndex = partitionIndex;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TIntermediate currentElement, ref int currentKey)
      {
        if (this.m_accumulated)
          return false;
        this.m_accumulated = true;
        bool flag = false;
        TIntermediate intermediate1 = default (TIntermediate);
        TIntermediate intermediate2;
        if (this.m_reduceOperator.m_seedIsSpecified)
        {
          intermediate2 = this.m_reduceOperator.m_seedFactory == null ? this.m_reduceOperator.m_seed : this.m_reduceOperator.m_seedFactory();
        }
        else
        {
          TInput currentElement1 = default (TInput);
          TKey currentKey1 = default (TKey);
          if (!this.m_source.MoveNext(ref currentElement1, ref currentKey1))
            return false;
          flag = true;
          intermediate2 = (TIntermediate) (object) currentElement1;
        }
        TInput currentElement2 = default (TInput);
        TKey currentKey2 = default (TKey);
        int num = 0;
        while (this.m_source.MoveNext(ref currentElement2, ref currentKey2))
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          flag = true;
          intermediate2 = this.m_reduceOperator.m_intermediateReduce(intermediate2, currentElement2);
        }
        if (!flag)
          return false;
        currentElement = intermediate2;
        currentKey = this.m_partitionIndex;
        return true;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
