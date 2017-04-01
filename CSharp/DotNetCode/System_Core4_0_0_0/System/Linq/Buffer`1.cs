// Type: System.Linq.Buffer`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;

namespace System.Linq
{
  internal struct Buffer<TElement>
  {
    internal TElement[] items;
    internal int count;

    internal Buffer(IEnumerable<TElement> source)
    {
      TElement[] array = (TElement[]) null;
      int length = 0;
      ICollection<TElement> collection = source as ICollection<TElement>;
      if (collection != null)
      {
        length = collection.Count;
        if (length > 0)
        {
          array = new TElement[length];
          collection.CopyTo(array, 0);
        }
      }
      else
      {
        foreach (TElement element in source)
        {
          if (array == null)
            array = new TElement[4];
          else if (array.Length == length)
          {
            TElement[] elementArray = new TElement[checked (length * 2)];
            Array.Copy((Array) array, 0, (Array) elementArray, 0, length);
            array = elementArray;
          }
          array[length] = element;
          ++length;
        }
      }
      this.items = array;
      this.count = length;
    }

    internal TElement[] ToArray()
    {
      if (this.count == 0)
        return new TElement[0];
      if (this.items.Length == this.count)
        return this.items;
      TElement[] elementArray = new TElement[this.count];
      Array.Copy((Array) this.items, 0, (Array) elementArray, 0, this.count);
      return elementArray;
    }
  }
}
