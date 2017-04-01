// Type: System.Runtime.CompilerServices.TrueReadOnlyCollection`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime;

namespace System.Runtime.CompilerServices
{
  internal sealed class TrueReadOnlyCollection<T> : ReadOnlyCollection<T>
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal TrueReadOnlyCollection(T[] list)
      : base((IList<T>) list)
    {
    }
  }
}
