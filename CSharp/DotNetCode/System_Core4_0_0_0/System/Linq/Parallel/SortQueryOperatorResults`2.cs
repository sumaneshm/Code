// Type: System.Linq.Parallel.SortQueryOperatorResults`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;

namespace System.Linq.Parallel
{
  internal class SortQueryOperatorResults<TInputOutput, TSortKey> : QueryResults<TInputOutput>
  {
    protected QueryResults<TInputOutput> m_childQueryResults;
    private SortQueryOperator<TInputOutput, TSortKey> m_op;
    private QuerySettings m_settings;
    private bool m_preferStriping;

    internal override bool IsIndexible
    {
      get
      {
        return false;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal SortQueryOperatorResults(QueryResults<TInputOutput> childQueryResults, SortQueryOperator<TInputOutput, TSortKey> op, QuerySettings settings, bool preferStriping)
    {
      this.m_childQueryResults = childQueryResults;
      this.m_op = op;
      this.m_settings = settings;
      this.m_preferStriping = preferStriping;
    }

    internal override void GivePartitionedStream(IPartitionedStreamRecipient<TInputOutput> recipient)
    {
      this.m_childQueryResults.GivePartitionedStream((IPartitionedStreamRecipient<TInputOutput>) new SortQueryOperatorResults<TInputOutput, TSortKey>.ChildResultsRecipient(recipient, this.m_op, this.m_settings));
    }

    private class ChildResultsRecipient : IPartitionedStreamRecipient<TInputOutput>
    {
      private IPartitionedStreamRecipient<TInputOutput> m_outputRecipient;
      private SortQueryOperator<TInputOutput, TSortKey> m_op;
      private QuerySettings m_settings;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ChildResultsRecipient(IPartitionedStreamRecipient<TInputOutput> outputRecipient, SortQueryOperator<TInputOutput, TSortKey> op, QuerySettings settings)
      {
        this.m_outputRecipient = outputRecipient;
        this.m_op = op;
        this.m_settings = settings;
      }

      public void Receive<TKey>(PartitionedStream<TInputOutput, TKey> childPartitionedStream)
      {
        this.m_op.WrapPartitionedStream<TKey>(childPartitionedStream, this.m_outputRecipient, false, this.m_settings);
      }
    }
  }
}
