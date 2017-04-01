// Type: System.Linq.Parallel.ConcatKey`2
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Runtime;

namespace System.Linq.Parallel
{
  internal struct ConcatKey<TLeftKey, TRightKey>
  {
    private readonly TLeftKey m_leftKey;
    private readonly TRightKey m_rightKey;
    private readonly bool m_isLeft;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    private ConcatKey(TLeftKey leftKey, TRightKey rightKey, bool isLeft)
    {
      this.m_leftKey = leftKey;
      this.m_rightKey = rightKey;
      this.m_isLeft = isLeft;
    }

    internal static ConcatKey<TLeftKey, TRightKey> MakeLeft(TLeftKey leftKey)
    {
      return new ConcatKey<TLeftKey, TRightKey>(leftKey, default (TRightKey), true);
    }

    internal static ConcatKey<TLeftKey, TRightKey> MakeRight(TRightKey rightKey)
    {
      return new ConcatKey<TLeftKey, TRightKey>(default (TLeftKey), rightKey, false);
    }

    internal static IComparer<ConcatKey<TLeftKey, TRightKey>> MakeComparer(IComparer<TLeftKey> leftComparer, IComparer<TRightKey> rightComparer)
    {
      return (IComparer<ConcatKey<TLeftKey, TRightKey>>) new ConcatKey<TLeftKey, TRightKey>.ConcatKeyComparer(leftComparer, rightComparer);
    }

    private class ConcatKeyComparer : IComparer<ConcatKey<TLeftKey, TRightKey>>
    {
      private IComparer<TLeftKey> m_leftComparer;
      private IComparer<TRightKey> m_rightComparer;

      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
      internal ConcatKeyComparer(IComparer<TLeftKey> leftComparer, IComparer<TRightKey> rightComparer)
      {
        this.m_leftComparer = leftComparer;
        this.m_rightComparer = rightComparer;
      }

      public int Compare(ConcatKey<TLeftKey, TRightKey> x, ConcatKey<TLeftKey, TRightKey> y)
      {
        if (x.m_isLeft != y.m_isLeft)
          return !x.m_isLeft ? 1 : -1;
        else if (x.m_isLeft)
          return this.m_leftComparer.Compare(x.m_leftKey, y.m_leftKey);
        else
          return this.m_rightComparer.Compare(x.m_rightKey, y.m_rightKey);
      }
    }
  }
}
