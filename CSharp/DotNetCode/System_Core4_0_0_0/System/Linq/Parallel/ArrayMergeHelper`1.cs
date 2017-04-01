// Type: System.Linq.Parallel.ArrayMergeHelper`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Linq.Parallel
{
  internal class ArrayMergeHelper<TInputOutput> : IMergeHelper<TInputOutput>
  {
    private QueryResults<TInputOutput> m_queryResults;
    private TInputOutput[] m_outputArray;
    private QuerySettings m_settings;

    public ArrayMergeHelper(QuerySettings settings, QueryResults<TInputOutput> queryResults)
    {
      this.m_settings = settings;
      this.m_queryResults = queryResults;
      this.m_outputArray = new TInputOutput[this.m_queryResults.Count];
    }

    private void ToArrayElement(int index)
    {
      this.m_outputArray[index] = this.m_queryResults[index];
    }

    public void Execute()
    {
      ParallelEnumerable.ForAll<int>((ParallelQuery<int>) new QueryExecutionOption<int>(QueryOperator<int>.AsQueryOperator((IEnumerable<int>) ParallelEnumerable.Range(0, this.m_queryResults.Count)), this.m_settings), new Action<int>(this.ToArrayElement));
    }

    public IEnumerator<TInputOutput> GetEnumerator()
    {
      return ((IEnumerable<TInputOutput>) this.GetResultsAsArray()).GetEnumerator();
    }

    public TInputOutput[] GetResultsAsArray()
    {
      return this.m_outputArray;
    }
  }
}
