// Type: System.Linq.Expressions.MemberAssignment
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public sealed class MemberAssignment : MemberBinding
  {
    private Expression _expression;

    [__DynamicallyInvokable]
    public Expression Expression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._expression;
      }
    }

    internal MemberAssignment(MemberInfo member, Expression expression)
      : base(MemberBindingType.Assignment, member)
    {
      this._expression = expression;
    }

    [__DynamicallyInvokable]
    public MemberAssignment Update(Expression expression)
    {
      if (expression == this.Expression)
        return this;
      else
        return Expression.Bind(this.Member, expression);
    }
  }
}
