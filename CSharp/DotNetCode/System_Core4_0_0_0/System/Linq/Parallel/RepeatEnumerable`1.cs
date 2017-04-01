// Type: System.Linq.Parallel.RepeatEnumerable`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class RepeatEnumerable<TResult> : ParallelQuery<TResult>, IParallelPartitionable<TResult>
  {
    private TResult m_element;
    private int m_count;

    internal RepeatEnumerable(TResult element, int count)
      : base(QuerySettings.Empty)
    {
      this.m_element = element;
      this.m_count = count;
    }

    public QueryOperatorEnumerator<TResult, int>[] GetPartitions(int partitionCount)
    {
      int count = (this.m_count + partitionCount - 1) / partitionCount;
      QueryOperatorEnumerator<TResult, int>[] operatorEnumeratorArray = new QueryOperatorEnumerator<TResult, int>[partitionCount];
      int index = 0;
      int indexOffset = 0;
      while (index < partitionCount)
      {
        operatorEnumeratorArray[index] = indexOffset + count <= this.m_count ? (QueryOperatorEnumerator<TResult, int>) new RepeatEnumerable<TResult>.RepeatEnumerator(this.m_element, count, indexOffset) : (QueryOperatorEnumerator<TResult, int>) new RepeatEnumerable<TResult>.RepeatEnumerator(this.m_element, indexOffset < this.m_count ? this.m_count - indexOffset : 0, indexOffset);
        ++index;
        indexOffset += count;
      }
      return operatorEnumeratorArray;
    }

    public override IEnumerator<TResult> GetEnumerator()
    {
      return new RepeatEnumerable<TResult>.RepeatEnumerator(this.m_element, this.m_count, 0).AsClassicEnumerator();
    }

    private class RepeatEnumerator : QueryOperatorEnumerator<TResult, int>
    {
      private readonly TResult m_element;
      private readonly int m_count;
      private readonly int m_indexOffset;
      private Shared<int> m_currentIndex;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal RepeatEnumerator(TResult element, int count, int indexOffset)
      {
        this.m_element = element;
        this.m_count = count;
        this.m_indexOffset = indexOffset;
      }

      internal override bool MoveNext(ref TResult currentElement, ref int currentKey)
      {
        if (this.m_currentIndex == null)
          this.m_currentIndex = new Shared<int>(-1);
        if (this.m_currentIndex.Value >= this.m_count - 1)
          return false;
        ++this.m_currentIndex.Value;
        currentElement = this.m_element;
        currentKey = this.m_currentIndex.Value + this.m_indexOffset;
        return true;
      }

      internal override void Reset()
      {
        this.m_currentIndex = (Shared<int>) null;
      }
    }
  }
}
