// Type: System.Linq.Parallel.InlinedAggregationOperator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal abstract class InlinedAggregationOperator<TSource, TIntermediate, TResult> : UnaryQueryOperator<TSource, TIntermediate>
  {
    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal InlinedAggregationOperator(IEnumerable<TSource> child)
      : base(child)
    {
    }

    internal TResult Aggregate()
    {
      Exception singularExceptionToThrow = (Exception) null;
      TResult result;
      try
      {
        result = this.InternalAggregate(ref singularExceptionToThrow);
      }
      catch (ThreadAbortException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (!(ex is AggregateException))
        {
          OperationCanceledException canceledException = ex as OperationCanceledException;
          if (canceledException != null && canceledException.CancellationToken == this.SpecifiedQuerySettings.CancellationState.ExternalCancellationToken && this.SpecifiedQuerySettings.CancellationState.ExternalCancellationToken.IsCancellationRequested)
            throw;
          else
            throw new AggregateException(new Exception[1]
            {
              ex
            });
        }
        else
          throw;
      }
      if (singularExceptionToThrow != null)
        throw singularExceptionToThrow;
      else
        return result;
    }

    protected abstract TResult InternalAggregate(ref Exception singularExceptionToThrow);

    internal override QueryResults<TIntermediate> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TIntermediate>) new UnaryQueryOperator<TSource, TIntermediate>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TSource, TIntermediate>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TSource, TKey> inputStream, IPartitionedStreamRecipient<TIntermediate> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TIntermediate, int> partitionedStream = new PartitionedStream<TIntermediate, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = this.CreateEnumerator<TKey>(index, partitionCount, inputStream[index], (object) null, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream);
    }

    protected abstract QueryOperatorEnumerator<TIntermediate, int> CreateEnumerator<TKey>(int index, int count, QueryOperatorEnumerator<TSource, TKey> source, object sharedData, CancellationToken cancellationToken);

    internal override IEnumerable<TIntermediate> AsSequentialQuery(CancellationToken token)
    {
      throw new NotSupportedException();
    }
  }
}
