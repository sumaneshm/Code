// Type: System.Linq.Expressions.MemberListBinding
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
  public sealed class MemberListBinding : MemberBinding
  {
    private ReadOnlyCollection<ElementInit> _initializers;

    [__DynamicallyInvokable]
    public ReadOnlyCollection<ElementInit> Initializers
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._initializers;
      }
    }

    internal MemberListBinding(MemberInfo member, ReadOnlyCollection<ElementInit> initializers)
      : base(MemberBindingType.ListBinding, member)
    {
      this._initializers = initializers;
    }

    [__DynamicallyInvokable]
    public MemberListBinding Update(IEnumerable<ElementInit> initializers)
    {
      if (initializers == this.Initializers)
        return this;
      else
        return Expression.ListBind(this.Member, initializers);
    }
  }
}
