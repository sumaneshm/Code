// Type: System.Runtime.CompilerServices.Closure
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime;

namespace System.Runtime.CompilerServices
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  [DebuggerStepThrough]
  [__DynamicallyInvokable]
  public sealed class Closure
  {
    [__DynamicallyInvokable]
    public readonly object[] Constants;
    [__DynamicallyInvokable]
    public readonly object[] Locals;

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public Closure(object[] constants, object[] locals)
    {
      this.Constants = constants;
      this.Locals = locals;
    }
  }
}
