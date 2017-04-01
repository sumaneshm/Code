// Type: System.Linq.IdentityFunction`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;

namespace System.Linq
{
  internal class IdentityFunction<TElement>
  {
    public static Func<TElement, TElement> Instance
    {
      get
      {
        return (Func<TElement, TElement>) (x => x);
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public IdentityFunction()
    {
    }
  }
}
