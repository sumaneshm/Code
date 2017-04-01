// Type: System.Linq.Parallel.ScanQueryOperator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal sealed class ScanQueryOperator<TElement> : QueryOperator<TElement>
  {
    private readonly IEnumerable<TElement> m_data;

    public IEnumerable<TElement> Data
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_data;
      }
    }

    internal override OrdinalIndexState OrdinalIndexState
    {
      get
      {
        return !(this.m_data is IList<TElement>) ? OrdinalIndexState.Correct : OrdinalIndexState.Indexible;
      }
    }

    internal override bool LimitsParallelism
    {
      get
      {
        return false;
      }
    }

    internal ScanQueryOperator(IEnumerable<TElement> data)
      : base(false, QuerySettings.Empty)
    {
      ParallelEnumerableWrapper<TElement> enumerableWrapper = data as ParallelEnumerableWrapper<TElement>;
      if (enumerableWrapper != null)
        data = enumerableWrapper.WrappedEnumerable;
      this.m_data = data;
    }

    internal override QueryResults<TElement> Open(QuerySettings settings, bool preferStriping)
    {
      IList<TElement> source = this.m_data as IList<TElement>;
      if (source != null)
        return (QueryResults<TElement>) new ListQueryResults<TElement>(source, settings.DegreeOfParallelism.GetValueOrDefault(), preferStriping);
      else
        return (QueryResults<TElement>) new ScanQueryOperator<TElement>.ScanEnumerableQueryOperatorResults(this.m_data, settings);
    }

    internal override IEnumerator<TElement> GetEnumerator(ParallelMergeOptions? mergeOptions, bool suppressOrderPreservation)
    {
      return this.m_data.GetEnumerator();
    }

    internal override IEnumerable<TElement> AsSequentialQuery(CancellationToken token)
    {
      return this.m_data;
    }

    private class ScanEnumerableQueryOperatorResults : QueryResults<TElement>
    {
      private IEnumerable<TElement> m_data;
      private QuerySettings m_settings;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ScanEnumerableQueryOperatorResults(IEnumerable<TElement> data, QuerySettings settings)
      {
        this.m_data = data;
        this.m_settings = settings;
      }

      internal override void GivePartitionedStream(IPartitionedStreamRecipient<TElement> recipient)
      {
        PartitionedStream<TElement, int> partitionedStream = ExchangeUtilities.PartitionDataSource<TElement>(this.m_data, this.m_settings.DegreeOfParallelism.Value, false);
        recipient.Receive<int>(partitionedStream);
      }
    }
  }
}
