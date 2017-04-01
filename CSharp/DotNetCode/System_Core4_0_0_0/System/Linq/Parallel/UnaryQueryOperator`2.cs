// Type: System.Linq.Parallel.UnaryQueryOperator`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal abstract class UnaryQueryOperator<TInput, TOutput> : QueryOperator<TOutput>
  {
    private OrdinalIndexState m_indexState = OrdinalIndexState.Shuffled;
    private readonly QueryOperator<TInput> m_child;

    internal QueryOperator<TInput> Child
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_child;
      }
    }

    internal override sealed OrdinalIndexState OrdinalIndexState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_indexState;
      }
    }

    internal UnaryQueryOperator(IEnumerable<TInput> child)
      : this(QueryOperator<TInput>.AsQueryOperator(child))
    {
    }

    internal UnaryQueryOperator(IEnumerable<TInput> child, bool outputOrdered)
      : this(QueryOperator<TInput>.AsQueryOperator(child), outputOrdered)
    {
    }

    private UnaryQueryOperator(QueryOperator<TInput> child)
      : this(child, child.OutputOrdered, child.SpecifiedQuerySettings)
    {
    }

    internal UnaryQueryOperator(QueryOperator<TInput> child, bool outputOrdered)
      : this(child, outputOrdered, child.SpecifiedQuerySettings)
    {
    }

    private UnaryQueryOperator(QueryOperator<TInput> child, bool outputOrdered, QuerySettings settings)
      : base(outputOrdered, settings)
    {
      this.m_child = child;
    }

    protected void SetOrdinalIndexState(OrdinalIndexState indexState)
    {
      this.m_indexState = indexState;
    }

    internal abstract void WrapPartitionedStream<TKey>(PartitionedStream<TInput, TKey> inputStream, IPartitionedStreamRecipient<TOutput> recipient, bool preferStriping, QuerySettings settings);

    internal class UnaryQueryOperatorResults : QueryResults<TOutput>
    {
      protected QueryResults<TInput> m_childQueryResults;
      private UnaryQueryOperator<TInput, TOutput> m_op;
      private QuerySettings m_settings;
      private bool m_preferStriping;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal UnaryQueryOperatorResults(QueryResults<TInput> childQueryResults, UnaryQueryOperator<TInput, TOutput> op, QuerySettings settings, bool preferStriping)
      {
        this.m_childQueryResults = childQueryResults;
        this.m_op = op;
        this.m_settings = settings;
        this.m_preferStriping = preferStriping;
      }

      internal override void GivePartitionedStream(IPartitionedStreamRecipient<TOutput> recipient)
      {
        if (this.m_settings.ExecutionMode.Value == ParallelExecutionMode.Default && this.m_op.LimitsParallelism)
        {
          PartitionedStream<TOutput, int> partitionedStream = ExchangeUtilities.PartitionDataSource<TOutput>(this.m_op.AsSequentialQuery(this.m_settings.CancellationState.ExternalCancellationToken), this.m_settings.DegreeOfParallelism.Value, this.m_preferStriping);
          recipient.Receive<int>(partitionedStream);
        }
        else if (this.IsIndexible)
        {
          PartitionedStream<TOutput, int> partitionedStream = ExchangeUtilities.PartitionDataSource<TOutput>((IEnumerable<TOutput>) this, this.m_settings.DegreeOfParallelism.Value, this.m_preferStriping);
          recipient.Receive<int>(partitionedStream);
        }
        else
          this.m_childQueryResults.GivePartitionedStream((IPartitionedStreamRecipient<TInput>) new UnaryQueryOperator<TInput, TOutput>.UnaryQueryOperatorResults.ChildResultsRecipient(recipient, this.m_op, this.m_preferStriping, this.m_settings));
      }

      private class ChildResultsRecipient : IPartitionedStreamRecipient<TInput>
      {
        private IPartitionedStreamRecipient<TOutput> m_outputRecipient;
        private UnaryQueryOperator<TInput, TOutput> m_op;
        private bool m_preferStriping;
        private QuerySettings m_settings;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal ChildResultsRecipient(IPartitionedStreamRecipient<TOutput> outputRecipient, UnaryQueryOperator<TInput, TOutput> op, bool preferStriping, QuerySettings settings)
        {
          this.m_outputRecipient = outputRecipient;
          this.m_op = op;
          this.m_preferStriping = preferStriping;
          this.m_settings = settings;
        }

        public void Receive<TKey>(PartitionedStream<TInput, TKey> inputStream)
        {
          this.m_op.WrapPartitionedStream<TKey>(inputStream, this.m_outputRecipient, this.m_preferStriping, this.m_settings);
        }
      }
    }
  }
}
