// Type: System.Linq.Expressions.LabelTarget
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Runtime;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public sealed class LabelTarget
  {
    private readonly Type _type;
    private readonly string _name;

    [__DynamicallyInvokable]
    public string Name
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._name;
      }
    }

    [__DynamicallyInvokable]
    public Type Type
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._type;
      }
    }

    internal LabelTarget(Type type, string name)
    {
      this._type = type;
      this._name = name;
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      if (!string.IsNullOrEmpty(this.Name))
        return this.Name;
      else
        return "UnamedLabel";
    }
  }
}
