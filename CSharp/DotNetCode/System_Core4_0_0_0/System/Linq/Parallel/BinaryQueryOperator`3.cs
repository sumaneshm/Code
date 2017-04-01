// Type: System.Linq.Parallel.BinaryQueryOperator`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal abstract class BinaryQueryOperator<TLeftInput, TRightInput, TOutput> : QueryOperator<TOutput>
  {
    private OrdinalIndexState m_indexState = OrdinalIndexState.Shuffled;
    private readonly QueryOperator<TLeftInput> m_leftChild;
    private readonly QueryOperator<TRightInput> m_rightChild;

    internal QueryOperator<TLeftInput> LeftChild
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_leftChild;
      }
    }

    internal QueryOperator<TRightInput> RightChild
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_rightChild;
      }
    }

    internal override sealed OrdinalIndexState OrdinalIndexState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_indexState;
      }
    }

    internal BinaryQueryOperator(ParallelQuery<TLeftInput> leftChild, ParallelQuery<TRightInput> rightChild)
      : this(QueryOperator<TLeftInput>.AsQueryOperator((IEnumerable<TLeftInput>) leftChild), QueryOperator<TRightInput>.AsQueryOperator((IEnumerable<TRightInput>) rightChild))
    {
    }

    internal BinaryQueryOperator(QueryOperator<TLeftInput> leftChild, QueryOperator<TRightInput> rightChild)
      : base(false, leftChild.SpecifiedQuerySettings.Merge(rightChild.SpecifiedQuerySettings))
    {
      this.m_leftChild = leftChild;
      this.m_rightChild = rightChild;
    }

    protected void SetOrdinalIndex(OrdinalIndexState indexState)
    {
      this.m_indexState = indexState;
    }

    public abstract void WrapPartitionedStream<TLeftKey, TRightKey>(PartitionedStream<TLeftInput, TLeftKey> leftPartitionedStream, PartitionedStream<TRightInput, TRightKey> rightPartitionedStream, IPartitionedStreamRecipient<TOutput> outputRecipient, bool preferStriping, QuerySettings settings);

    internal class BinaryQueryOperatorResults : QueryResults<TOutput>
    {
      protected QueryResults<TLeftInput> m_leftChildQueryResults;
      protected QueryResults<TRightInput> m_rightChildQueryResults;
      private BinaryQueryOperator<TLeftInput, TRightInput, TOutput> m_op;
      private QuerySettings m_settings;
      private bool m_preferStriping;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal BinaryQueryOperatorResults(QueryResults<TLeftInput> leftChildQueryResults, QueryResults<TRightInput> rightChildQueryResults, BinaryQueryOperator<TLeftInput, TRightInput, TOutput> op, QuerySettings settings, bool preferStriping)
      {
        this.m_leftChildQueryResults = leftChildQueryResults;
        this.m_rightChildQueryResults = rightChildQueryResults;
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
          this.m_leftChildQueryResults.GivePartitionedStream((IPartitionedStreamRecipient<TLeftInput>) new BinaryQueryOperator<TLeftInput, TRightInput, TOutput>.BinaryQueryOperatorResults.LeftChildResultsRecipient(recipient, this, this.m_preferStriping, this.m_settings));
      }

      private class LeftChildResultsRecipient : IPartitionedStreamRecipient<TLeftInput>
      {
        private IPartitionedStreamRecipient<TOutput> m_outputRecipient;
        private BinaryQueryOperator<TLeftInput, TRightInput, TOutput>.BinaryQueryOperatorResults m_results;
        private bool m_preferStriping;
        private QuerySettings m_settings;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal LeftChildResultsRecipient(IPartitionedStreamRecipient<TOutput> outputRecipient, BinaryQueryOperator<TLeftInput, TRightInput, TOutput>.BinaryQueryOperatorResults results, bool preferStriping, QuerySettings settings)
        {
          this.m_outputRecipient = outputRecipient;
          this.m_results = results;
          this.m_preferStriping = preferStriping;
          this.m_settings = settings;
        }

        public void Receive<TLeftKey>(PartitionedStream<TLeftInput, TLeftKey> source)
        {
          this.m_results.m_rightChildQueryResults.GivePartitionedStream((IPartitionedStreamRecipient<TRightInput>) new BinaryQueryOperator<TLeftInput, TRightInput, TOutput>.BinaryQueryOperatorResults.RightChildResultsRecipient<TLeftKey>(this.m_outputRecipient, this.m_results.m_op, source, this.m_preferStriping, this.m_settings));
        }
      }

      private class RightChildResultsRecipient<TLeftKey> : IPartitionedStreamRecipient<TRightInput>
      {
        private IPartitionedStreamRecipient<TOutput> m_outputRecipient;
        private PartitionedStream<TLeftInput, TLeftKey> m_leftPartitionedStream;
        private BinaryQueryOperator<TLeftInput, TRightInput, TOutput> m_op;
        private bool m_preferStriping;
        private QuerySettings m_settings;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal RightChildResultsRecipient(IPartitionedStreamRecipient<TOutput> outputRecipient, BinaryQueryOperator<TLeftInput, TRightInput, TOutput> op, PartitionedStream<TLeftInput, TLeftKey> leftPartitionedStream, bool preferStriping, QuerySettings settings)
        {
          this.m_outputRecipient = outputRecipient;
          this.m_op = op;
          this.m_preferStriping = preferStriping;
          this.m_leftPartitionedStream = leftPartitionedStream;
          this.m_settings = settings;
        }

        public void Receive<TRightKey>(PartitionedStream<TRightInput, TRightKey> rightPartitionedStream)
        {
          this.m_op.WrapPartitionedStream<TLeftKey, TRightKey>(this.m_leftPartitionedStream, rightPartitionedStream, this.m_outputRecipient, this.m_preferStriping, this.m_settings);
        }
      }
    }
  }
}
