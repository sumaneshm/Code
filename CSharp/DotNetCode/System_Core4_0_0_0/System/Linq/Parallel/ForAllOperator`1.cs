// Type: System.Linq.Parallel.ForAllOperator`1
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
  internal sealed class ForAllOperator<TInput> : UnaryQueryOperator<TInput, TInput>
  {
    private readonly Action<TInput> m_elementAction;

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ForAllOperator(IEnumerable<TInput> child, Action<TInput> elementAction)
      : base(child)
    {
      this.m_elementAction = elementAction;
    }

    internal void RunSynchronously()
    {
      QuerySettings querySettings = this.SpecifiedQuerySettings.WithPerExecutionSettings(new CancellationTokenSource(), new Shared<bool>(false)).WithDefaults();
      QueryLifecycle.LogicalQueryExecutionBegin(querySettings.QueryId);
      this.GetOpenedEnumerator(new ParallelMergeOptions?(ParallelMergeOptions.FullyBuffered), true, true, querySettings);
      querySettings.CleanStateAtQueryEnd();
      QueryLifecycle.LogicalQueryExecutionEnd(querySettings.QueryId);
    }

    internal override QueryResults<TInput> Open(QuerySettings settings, bool preferStriping)
    {
      return (QueryResults<TInput>) new UnaryQueryOperator<TInput, TInput>.UnaryQueryOperatorResults(this.Child.Open(settings, preferStriping), (UnaryQueryOperator<TInput, TInput>) this, settings, preferStriping);
    }

    internal override void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<TInput> recipient, bool preferStriping, QuerySettings settings)
    {
      int partitionCount = inputStream.PartitionCount;
      PartitionedStream<TInput, int> partitionedStream = new PartitionedStream<TInput, int>(partitionCount, (IComparer<int>) Util.GetDefaultComparer<int>(), OrdinalIndexState.Correct);
      for (int index = 0; index < partitionCount; ++index)
        partitionedStream[index] = (QueryOperatorEnumerator<TInput, int>) new ForAllOperator<TInput>.ForAllEnumerator<TKey>(inputStream[index], this.m_elementAction, settings.CancellationState.MergedCancellationToken);
      recipient.Receive<int>(partitionedStream);
    }

    internal override IEnumerable<TInput> AsSequentialQuery(CancellationToken token)
    {
      throw new InvalidOperationException();
    }

    private class ForAllEnumerator<TKey> : QueryOperatorEnumerator<TInput, int>
    {
      private readonly QueryOperatorEnumerator<TInput, TKey> m_source;
      private readonly Action<TInput> m_elementAction;
      private CancellationToken m_cancellationToken;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ForAllEnumerator(QueryOperatorEnumerator<TInput, TKey> source, Action<TInput> elementAction, CancellationToken cancellationToken)
      {
        this.m_source = source;
        this.m_elementAction = elementAction;
        this.m_cancellationToken = cancellationToken;
      }

      internal override bool MoveNext(ref TInput currentElement, ref int currentKey)
      {
        TInput currentElement1 = default (TInput);
        TKey currentKey1 = default (TKey);
        int num = 0;
        while (this.m_source.MoveNext(ref currentElement1, ref currentKey1))
        {
          if ((num++ & 63) == 0)
            CancellationState.ThrowIfCanceled(this.m_cancellationToken);
          this.m_elementAction(currentElement1);
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
