// Type: System.Linq.Parallel.OrderPreservingMergeHelper`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq.Parallel
{
  internal class OrderPreservingMergeHelper<TInputOutput, TKey> : IMergeHelper<TInputOutput>
  {
    private QueryTaskGroupState m_taskGroupState;
    private PartitionedStream<TInputOutput, TKey> m_partitions;
    private Shared<TInputOutput[]> m_results;
    private TaskScheduler m_taskScheduler;

    internal OrderPreservingMergeHelper(PartitionedStream<TInputOutput, TKey> partitions, TaskScheduler taskScheduler, CancellationState cancellationState, int queryId)
    {
      this.m_taskGroupState = new QueryTaskGroupState(cancellationState, queryId);
      this.m_partitions = partitions;
      this.m_results = new Shared<TInputOutput[]>((TInputOutput[]) null);
      this.m_taskScheduler = taskScheduler;
    }

    void IMergeHelper<TInputOutput>.Execute()
    {
      OrderPreservingSpoolingTask<TInputOutput, TKey>.Spool(this.m_taskGroupState, this.m_partitions, this.m_results, this.m_taskScheduler);
    }

    IEnumerator<TInputOutput> IMergeHelper<TInputOutput>.GetEnumerator()
    {
      return ((IEnumerable<TInputOutput>) this.m_results.Value).GetEnumerator();
    }

    public TInputOutput[] GetResultsAsArray()
    {
      return this.m_results.Value;
    }
  }
}
