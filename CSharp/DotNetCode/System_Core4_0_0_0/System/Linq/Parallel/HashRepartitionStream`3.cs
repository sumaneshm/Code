// Type: System.Linq.Parallel.HashRepartitionStream`3
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;

namespace System.Linq.Parallel
{
  internal abstract class HashRepartitionStream<TInputOutput, THashKey, TOrderKey> : PartitionedStream<Pair<TInputOutput, THashKey>, TOrderKey>
  {
    private readonly IEqualityComparer<THashKey> m_keyComparer;
    private readonly IEqualityComparer<TInputOutput> m_elementComparer;
    private readonly int m_distributionMod;
    private const int NULL_ELEMENT_HASH_CODE = 0;

    internal HashRepartitionStream(int partitionsCount, IComparer<TOrderKey> orderKeyComparer, IEqualityComparer<THashKey> hashKeyComparer, IEqualityComparer<TInputOutput> elementComparer)
      : base(partitionsCount, orderKeyComparer, OrdinalIndexState.Shuffled)
    {
      this.m_keyComparer = hashKeyComparer;
      this.m_elementComparer = elementComparer;
      this.m_distributionMod = 503;
      while (this.m_distributionMod < partitionsCount)
        checked { this.m_distributionMod *= 2; }
    }

    internal int GetHashCode(TInputOutput element)
    {
      return (int.MaxValue & (this.m_elementComparer == null ? ((object) element == null ? 0 : element.GetHashCode()) : this.m_elementComparer.GetHashCode(element))) % this.m_distributionMod;
    }

    internal int GetHashCode(THashKey key)
    {
      return (int.MaxValue & (this.m_keyComparer == null ? ((object) key == null ? 0 : key.GetHashCode()) : this.m_keyComparer.GetHashCode(key))) % this.m_distributionMod;
    }
  }
}
