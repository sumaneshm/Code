// Type: System.Linq.Parallel.PartitionedStreamMerger`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class PartitionedStreamMerger<TOutput> : IPartitionedStreamRecipient<TOutput>
  {
    private bool m_forEffectMerge;
    private ParallelMergeOptions m_mergeOptions;
    private bool m_isOrdered;
    private MergeExecutor<TOutput> m_mergeExecutor;
    private TaskScheduler m_taskScheduler;
    private int m_queryId;
    private CancellationState m_cancellationState;

    internal MergeExecutor<TOutput> MergeExecutor
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_mergeExecutor;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal PartitionedStreamMerger(bool forEffectMerge, ParallelMergeOptions mergeOptions, TaskScheduler taskScheduler, bool outputOrdered, CancellationState cancellationState, int queryId)
    {
      this.m_forEffectMerge = forEffectMerge;
      this.m_mergeOptions = mergeOptions;
      this.m_isOrdered = outputOrdered;
      this.m_taskScheduler = taskScheduler;
      this.m_cancellationState = cancellationState;
      this.m_queryId = queryId;
    }

    public void Receive<TKey>(PartitionedStream<TOutput, TKey> partitionedStream)
    {
      this.m_mergeExecutor = MergeExecutor<TOutput>.Execute<TKey>(partitionedStream, this.m_forEffectMerge, this.m_mergeOptions, this.m_taskScheduler, this.m_isOrdered, this.m_cancellationState, this.m_queryId);
    }
  }
}
