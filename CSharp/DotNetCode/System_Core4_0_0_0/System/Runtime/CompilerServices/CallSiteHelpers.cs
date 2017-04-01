// Type: System.Runtime.CompilerServices.CallSiteHelpers
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic;
using System.Reflection;

namespace System.Runtime.CompilerServices
{
  [__DynamicallyInvokable]
  public static class CallSiteHelpers
  {
    private static Type _knownNonDynamicMethodType = typeof (object).GetMethod("ToString").GetType();

    static CallSiteHelpers()
    {
    }

    [__DynamicallyInvokable]
    public static bool IsInternalFrame(MethodBase mb)
    {
      return mb.Name == "CallSite.Target" && mb.GetType() != CallSiteHelpers._knownNonDynamicMethodType || mb.DeclaringType == typeof (UpdateDelegates);
    }
  }
}
