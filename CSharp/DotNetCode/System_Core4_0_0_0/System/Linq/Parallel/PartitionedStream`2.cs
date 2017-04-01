// Type: System.Linq.Parallel.PartitionedStream`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class PartitionedStream<TElement, TKey>
  {
    protected QueryOperatorEnumerator<TElement, TKey>[] m_partitions;
    private readonly IComparer<TKey> m_keyComparer;
    private readonly OrdinalIndexState m_indexState;

    internal QueryOperatorEnumerator<TElement, TKey> this[int index]
    {
      get
      {
        return this.m_partitions[index];
      }
      set
      {
        this.m_partitions[index] = value;
      }
    }

    public int PartitionCount
    {
      get
      {
        return this.m_partitions.Length;
      }
    }

    internal IComparer<TKey> KeyComparer
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_keyComparer;
      }
    }

    internal OrdinalIndexState OrdinalIndexState
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_indexState;
      }
    }

    internal PartitionedStream(int partitionCount, IComparer<TKey> keyComparer, OrdinalIndexState indexState)
    {
      this.m_partitions = new QueryOperatorEnumerator<TElement, TKey>[partitionCount];
      this.m_keyComparer = keyComparer;
      this.m_indexState = indexState;
    }
  }
}
