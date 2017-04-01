// Type: System.Linq.Expressions.MemberExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Diagnostics;
using System.Dynamic.Utils;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.MemberExpressionProxy))]
  [__DynamicallyInvokable]
  public class MemberExpression : Expression
  {
    private readonly Expression _expression;

    [__DynamicallyInvokable]
    public MemberInfo Member
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetMember();
      }
    }

    [__DynamicallyInvokable]
    public Expression Expression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._expression;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.MemberAccess;
      }
    }

    internal MemberExpression(Expression expression)
    {
      this._expression = expression;
    }

    internal virtual MemberInfo GetMember()
    {
      throw ContractUtils.Unreachable;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitMember(this);
    }

    [__DynamicallyInvokable]
    public MemberExpression Update(Expression expression)
    {
      if (expression == this.Expression)
        return this;
      else
        return Expression.MakeMemberAccess(expression, this.Member);
    }

    internal static MemberExpression Make(Expression expression, MemberInfo member)
    {
      if (member.MemberType == MemberTypes.Field)
      {
        FieldInfo member1 = (FieldInfo) member;
        return (MemberExpression) new FieldExpression(expression, member1);
      }
      else
      {
        PropertyInfo member1 = (PropertyInfo) member;
        return (MemberExpression) new PropertyExpression(expression, member1);
      }
    }
  }
}
