// Type: System.Linq.Parallel.ListQueryResults`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class ListQueryResults<T> : QueryResults<T>
  {
    private IList<T> m_source;
    private int m_partitionCount;
    private bool m_useStriping;

    internal override bool IsIndexible
    {
      get
      {
        return true;
      }
    }

    internal override int ElementsCount
    {
      get
      {
        return this.m_source.Count;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ListQueryResults(IList<T> source, int partitionCount, bool useStriping)
    {
      this.m_source = source;
      this.m_partitionCount = partitionCount;
      this.m_useStriping = useStriping;
    }

    internal override void GivePartitionedStream(IPartitionedStreamRecipient<T> recipient)
    {
      PartitionedStream<T, int> partitionedStream = this.GetPartitionedStream();
      recipient.Receive<int>(partitionedStream);
    }

    internal override T GetElement(int index)
    {
      return this.m_source[index];
    }

    internal PartitionedStream<T, int> GetPartitionedStream()
    {
      return ExchangeUtilities.PartitionDataSource<T>((IEnumerable<T>) this.m_source, this.m_partitionCount, this.m_useStriping);
    }
  }
}
