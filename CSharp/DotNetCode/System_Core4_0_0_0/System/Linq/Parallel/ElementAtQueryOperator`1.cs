// Type: System.Linq.Parallel.ElementAtQueryOperator`1
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
  internal sealed class ElementAtQueryOperator<TSource> : UnaryQueryOperator<TSource, TSource>
  {
    private readonly int m_index;
    private readonly bool m_prematureMerge;
    private readonly bool m_limitsParallelism;

    internal override bool LimitsParallelism
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_limitsParallelism;
      }
    }

    internal ElementAtQueryOperator(IEnumerable<TSource> child, int index)
      : base(child)
    {
      this.m_index = index;
      OrdinalIndexState ordinalIndexState = this.Child.OrdinalIndexState;
      if (!ExchangeUtilities.IsWorseThan(ordinalIndexState, OrdinalIndexState.Correct))
        return;
      this.m_prematureMerge = true;
      this.m_limitsParallelism = ordinalIndexState != OrdinalIndexState.Shuffled;
    }

    internal override QueryResults<TSource> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TSource>) new UnaryQueryOperator<TSource, TSource>.UnaryQueryOperatorResults(this.Child.Open(settings, false), (UnaryQueryOperator<TSource, TSource>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TSource> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TSource, int> partitionedStream1 = !this.m_prematureMerge ? (PartitionedStream<TSource, int>) inputStream : QueryOperator<TSource>.ExecuteAndCollectResults<TKey>(inputStream, partitionCount, this.Child.OutputOrdered, preferStriping, settings).GetPartitionedStream();
      Shared<bool> resultFoundFlag = new Shared<bool>(false);
      PartitionedStream<TSource, int> partitionedStream2 = new PartitionedStream<TSource, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream2[index] = (QueryOperatorEnumerator<TSource, int>) new ElementAtQueryOperator<TSource>.ElementAtQueryOperatorEnumerator(partitionedStream1[index], this.m_index, resultFoundFlag, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream2);
    }

    internal override IEnumerable<TSource> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }

    internal bool Aggregate(out TSource result, bool withDefaultValue)
    {
      if (this.LimitsParallelism && this.SpecifiedQuerySettings.WithDefaults().ExecutionMode.Value != ParallelExecutionMode.ForceParallelism)
      {
        CancellationState cancellationState = this.SpecifiedQuerySettings.CancellationState;
        if (withDefaultValue)
        {
          IEnumerable<TSource> source = CancellableEnumerable.Wrap<TSource>(this.Child.AsSequentialQuery(cancellationState.ExternalCancellationToken), cancellationState.ExternalCancellationToken);
          result = Enumerable.ElementAtOrDefault<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(source, cancellationState), this.m_index);
        }
        else
        {
          IEnumerable<TSource> source = CancellableEnumerable.Wrap<TSource>(this.Child.AsSequentialQuery(cancellationState.ExternalCancellationToken), cancellationState.ExternalCancellationToken);
          result = Enumerable.ElementAt<TSource>(ExceptionAggregator.WrapEnumerable<TSource>(source, cancellationState), this.m_index);
        }
        return true;
      }
      else
      {
        using (IEnumerator<TSource> enumerator = this.GetEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered)))
        {
          if (enumerator.MoveNext())
          {
            TSource current = enumerator.Current;
            result = current;
            return true;
          }
        }
        result = default (TSource);
        return false;
      }
    }

    private class ElementAtQueryOperatorEnumerator : QueryOperatorEnumerator<TSource, int>
    {
      private QueryOperatorEnumerator<TSource, int> m_source;
      private int m_index;
      private Shared<bool> m_resultFoundFlag;
      private CancellationToken m_cancellationToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ElementAtQueryOperatorEnumerator(QueryOperatorEnumerator<TSource, int> source, int index, Shared<bool> resultFoundFlag, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_index = index;
        this.m_resultFoundFlag = resultFoundFlag;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TSource currentElement, ref int currentKey)
      {
        int num = 0;
        while (this.m_source.MoveNext(ref currentElement, ref currentKey))
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          if (!this.m_resultFoundFlag.Value)
          {
            if (currentKey == this.m_index)
            {
              this.m_resultFoundFlag.Value = true;
              return true;
            }
          }
          else
            break;
        }
        return false;
      }

      protected override void Dispose(bool disposing)
      {
        this.m_source.Dispose();
      }
    }
  }
}
