// Type: System.Linq.Parallel.InlinedAggregationOperatorEnumerator`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;
using System.Threading;

namespace System.Linq.Parallel
{
  internal abstract class InlinedAggregationOperatorEnumerator<TIntermediate> : QueryOperatorEnumerator<TIntermediate, int>
  {
    private int m_partitionIndex;
    private bool m_done;
    protected CancellationToken m_cancellationToken;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal InlinedAggregationOperatorEnumerator(int partitionIndex, CancellationToken cancellationToken)
    {
      this.m_partitionIndex = partitionIndex;
      this.m_cancellationToken = cancellationToken;
    }

    internal override sealed bool MoveNext(ref TIntermediate currentElement, ref int currentKey)
    {
      if (this.m_done || !this.MoveNextCore(ref currentElement))
        return false;
      currentKey = this.m_partitionIndex;
      this.m_done = true;
      return true;
    }

    protected abstract bool MoveNextCore(ref TIntermediate currentElement);
  }
}
