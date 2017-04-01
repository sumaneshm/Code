// Type: System.Linq.Parallel.FixedMaxHeap`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal class FixedMaxHeap<TElement>
  {
    private TElement[] m_elements;
    private int m_count;
    private IComparer<TElement> m_comparer;

    internal int Count
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.m_count;
      }
    }

    internal int Size
    {
      get
      {
        return this.m_elements.Length;
      }
    }

    internal TElement MaxValue
    {
      get
      {
        if (this.m_count == 0)
          throw new InvalidOperationException(System.Linq.SR.GetString("NoElements"));
        else
          return this.m_elements[0];
      }
    }

    internal FixedMaxHeap(int maximumSize)
      : this(maximumSize, (IComparer<TElement>) Util.GetDefaultComparer<TElement>())
    {
    }

    internal FixedMaxHeap(int maximumSize, IComparer<TElement> comparer)
    {
      this.m_elements = new TElement[maximumSize];
      this.m_comparer = comparer;
    }

    internal void Clear()
    {
      this.m_count = 0;
    }

    internal bool Insert(TElement e)
    {
      if (this.m_count < this.m_elements.Length)
      {
        this.m_elements[this.m_count] = e;
        ++this.m_count;
        this.HeapifyLastLeaf();
        return true;
      }
      else
      {
        if (this.m_comparer.Compare(e, this.m_elements[0]) >= 0)
          return false;
        this.m_elements[0] = e;
        this.HeapifyRoot();
        return true;
      }
    }

    internal void ReplaceMax(TElement newValue)
    {
      this.m_elements[0] = newValue;
      this.HeapifyRoot();
    }

    internal void RemoveMax()
    {
      --this.m_count;
      if (this.m_count <= 0)
        return;
      this.m_elements[0] = this.m_elements[this.m_count];
      this.HeapifyRoot();
    }

    private void Swap(int i, int j)
    {
      TElement element = this.m_elements[i];
      this.m_elements[i] = this.m_elements[j];
      this.m_elements[j] = element;
    }

    private void HeapifyRoot()
    {
      int i = 0;
      int num = this.m_count;
      while (i < num)
      {
        int j1 = (i + 1) * 2 - 1;
        int j2 = j1 + 1;
        if (j1 < num && this.m_comparer.Compare(this.m_elements[i], this.m_elements[j1]) < 0)
        {
          if (j2 < num && this.m_comparer.Compare(this.m_elements[j1], this.m_elements[j2]) < 0)
          {
            this.Swap(i, j2);
            i = j2;
          }
          else
          {
            this.Swap(i, j1);
            i = j1;
          }
        }
        else
        {
          if (j2 >= num || this.m_comparer.Compare(this.m_elements[i], this.m_elements[j2]) >= 0)
            break;
          this.Swap(i, j2);
          i = j2;
        }
      }
    }

    private void HeapifyLastLeaf()
    {
      for (int i = this.m_count - 1; i > 0; {
        int j;
        i = j;
      }
      )
      {
        j = (i + 1) / 2 - 1;
        if (this.m_comparer.Compare(this.m_elements[i], this.m_elements[j]) <= 0)
          break;
        this.Swap(i, j);
      }
    }
  }
}
