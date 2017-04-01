// Type: System.Linq.Expressions.MemberBinding
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public abstract class MemberBinding
  {
    private MemberBindingType _type;
    private MemberInfo _member;

    [__DynamicallyInvokable]
    public MemberBindingType BindingType
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._type;
      }
    }

    [__DynamicallyInvokable]
    public MemberInfo Member
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._member;
      }
    }

    [Obsolete("Do not use this constructor. It will be removed in future releases.")]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected MemberBinding(MemberBindingType type, MemberInfo member)
    {
      this._type = type;
      this._member = member;
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      return ExpressionStringBuilder.MemberBindingToString(this);
    }
  }
}
