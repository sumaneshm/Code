// Type: System.Runtime.CompilerServices.StrongBox`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Runtime;

namespace System.Runtime.CompilerServices
{
  [__DynamicallyInvokable]
  public class StrongBox<T> : IStrongBox
  {
    [__DynamicallyInvokable]
    public T Value;

    [__DynamicallyInvokable]
    object IStrongBox.Value
    {
      [__DynamicallyInvokable] get
      {
        return (object) this.Value;
      }
      [__DynamicallyInvokable] set
      {
        this.Value = (T) value;
      }
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public StrongBox()
    {
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public StrongBox(T value)
    {
      this.Value = value;
    }
  }
}
