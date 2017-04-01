// Type: System.Linq.Parallel.ListChunk`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class ListChunk<TInputOutput> : IEnumerable<TInputOutput>, IEnumerable
  {
    internal TInputOutput[] m_chunk;
    private int m_chunkCount;
    private ListChunk<TInputOutput> m_nextChunk;
    private ListChunk<TInputOutput> m_tailChunk;

    internal ListChunk<TInputOutput> Next
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_nextChunk;
      }
    }

    internal int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_chunkCount;
      }
    }

    internal ListChunk(int size)
    {
      this.m_chunk = new TInputOutput[size];
      this.m_chunkCount = 0;
      this.m_tailChunk = this;
    }

    internal void Add(TInputOutput e)
    {
      ListChunk<TInputOutput> listChunk = this.m_tailChunk;
      if (listChunk.m_chunkCount == listChunk.m_chunk.Length)
      {
        this.m_tailChunk = new ListChunk<TInputOutput>(listChunk.m_chunkCount * 2);
        listChunk = listChunk.m_nextChunk = this.m_tailChunk;
      }
      listChunk.m_chunk[listChunk.m_chunkCount++] = e;
    }

    public IEnumerator<TInputOutput> GetEnumerator()
    {
      for (ListChunk<TInputOutput> curr = this; curr != null; curr = curr.m_nextChunk)
      {
        for (int i = 0; i < curr.m_chunkCount; ++i)
          yield return curr.m_chunk[i];
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
