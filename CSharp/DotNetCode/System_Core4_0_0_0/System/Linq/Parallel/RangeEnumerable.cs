// Type: System.Linq.Parallel.RangeEnumerable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Parallel
{
  internal class RangeEnumerable : ParallelQuery<int>, IParallelPartitionable<int>
  {
    private int m_from;
    private int m_count;

    internal RangeEnumerable(int from, int count)
      : base(QuerySettings.Empty)
    {
      this.m_from = from;
      this.m_count = count;
    }

    public QueryOperatorEnumerator<int, int>[] GetPartitions(int partitionCount)
    {
      int num1 = this.m_count / partitionCount;
      int num2 = this.m_count % partitionCount;
      int initialIndex = 0;
      QueryOperatorEnumerator<int, int>[] operatorEnumeratorArray = new QueryOperatorEnumerator<int, int>[partitionCount];
      for (int index = 0; index < partitionCount; ++index)
      {
        int count = index < num2 ? num1 + 1 : num1;
        operatorEnumeratorArray[index] = (QueryOperatorEnumerator<int, int>) new RangeEnumerable.RangeEnumerator(this.m_from + initialIndex, count, initialIndex);
        initialIndex += count;
      }
      return operatorEnumeratorArray;
    }

    public override IEnumerator<int> GetEnumerator()
    {
      return new RangeEnumerable.RangeEnumerator(this.m_from, this.m_count, 0).AsClassicEnumerator();
    }

    private class RangeEnumerator : QueryOperatorEnumerator<int, int>
    {
      private readonly int m_from;
      private readonly int m_count;
      private readonly int m_initialIndex;
      private Shared<int> m_currentCount;

      internal RangeEnumerator(int from, int count, int initialIndex)
      {
        this.m_from = from;
        this.m_count = count;
        this.m_initialIndex = initialIndex;
      }

      internal override bool MoveNext(ref int currentElement, ref int currentKey)
      {
        if (this.m_currentCount == null)
          this.m_currentCount = new Shared<int>(-1);
        int num = this.m_currentCount.Value + 1;
        if (num >= this.m_count)
          return false;
        this.m_currentCount.Value = num;
        currentElement = num + this.m_from;
        currentKey = num + this.m_initialIndex;
        return true;
      }

      internal override void Reset()
      {
        this.m_currentCount = (Shared<int>) null;
      }
    }
  }
}
