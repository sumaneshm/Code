// Type: System.Linq.Expressions.MemberMemberBinding
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public sealed class MemberMemberBinding : MemberBinding
  {
    private ReadOnlyCollection<MemberBinding> _bindings;

    [__DynamicallyInvokable]
    public ReadOnlyCollection<MemberBinding> Bindings
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._bindings;
      }
    }

    internal MemberMemberBinding(MemberInfo member, ReadOnlyCollection<MemberBinding> bindings)
      : base(MemberBindingType.MemberBinding, member)
    {
      this._bindings = bindings;
    }

    [__DynamicallyInvokable]
    public MemberMemberBinding Update(IEnumerable<MemberBinding> bindings)
    {
      if (bindings == this.Bindings)
        return this;
      else
        return Expression.MemberBind(this.Member, bindings);
    }
  }
}
