// Type: System.Dynamic.Utils.EmptyReadOnlyCollection`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Dynamic.Utils
{
  internal static class EmptyReadOnlyCollection<T>
  {
    internal static ReadOnlyCollection<T> Instance = (ReadOnlyCollection<T>) new TrueReadOnlyCollection<T>(new T[0]);

    static EmptyReadOnlyCollection()
    {
    }
  }
}
