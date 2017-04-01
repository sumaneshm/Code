// Type: System.Linq.EnumerableSorter`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;

namespace System.Linq
{
  internal abstract class EnumerableSorter<TElement>
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EnumerableSorter()
    {
    }

    internal abstract void ComputeKeys(TElement[] elements, int count);

    internal abstract int CompareKeys(int index1, int index2);

    internal int[] Sort(TElement[] elements, int count)
    {
      this.ComputeKeys(elements, count);
      int[] map = new int[count];
      for (int index = 0; index < count; ++index)
        map[index] = index;
      this.QuickSort(map, 0, count - 1);
      return map;
    }

    private void QuickSort(int[] map, int left, int right)
    {
      do
      {
        int left1 = left;
        int right1 = right;
        int index1 = map[left1 + (right1 - left1 >> 1)];
        while (true)
        {
          do
          {
            if (left1 >= map.Length || this.CompareKeys(index1, map[left1]) <= 0)
            {
              while (right1 >= 0 && this.CompareKeys(index1, map[right1]) < 0)
                --right1;
              if (left1 <= right1)
              {
                if (left1 < right1)
                {
                  int num = map[left1];
                  map[left1] = map[right1];
                  map[right1] = num;
                }
                ++left1;
                --right1;
              }
              else
                break;
            }
            else
              goto label_1;
          }
          while (left1 <= right1);
          break;
label_1:
          ++left1;
        }
        if (right1 - left <= right - left1)
        {
          if (left < right1)
            this.QuickSort(map, left, right1);
          left = left1;
        }
        else
        {
          if (left1 < right)
            this.QuickSort(map, left1, right);
          right = right1;
        }
      }
      while (left < right);
    }
  }
}
