// Type: System.Linq.EnumerableSorter`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Runtime;

namespace System.Linq
{
  internal class EnumerableSorter<TElement, TKey> : EnumerableSorter<TElement>
  {
    internal Func<TElement, TKey> keySelector;
    internal IComparer<TKey> comparer;
    internal bool descending;
    internal EnumerableSorter<TElement> next;
    internal TKey[] keys;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal EnumerableSorter(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, EnumerableSorter<TElement> next)
    {
      this.keySelector = keySelector;
      this.comparer = comparer;
      this.descending = descending;
      this.next = next;
    }

    internal override void ComputeKeys(TElement[] elements, int count)
    {
      this.keys = new TKey[count];
      for (int index = 0; index < count; ++index)
        this.keys[index] = this.keySelector(elements[index]);
      if (this.next == null)
        return;
      this.next.ComputeKeys(elements, count);
    }

    internal override int CompareKeys(int index1, int index2)
    {
      int num = this.comparer.Compare(this.keys[index1], this.keys[index2]);
      if (num == 0)
      {
        if (this.next == null)
          return index1 - index2;
        else
          return this.next.CompareKeys(index1, index2);
      }
      else if (!this.descending)
        return num;
      else
        return -num;
    }
  }
}
