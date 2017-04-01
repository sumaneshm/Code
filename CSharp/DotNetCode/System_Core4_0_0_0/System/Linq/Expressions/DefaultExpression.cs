// Type: System.Linq.Expressions.DefaultExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.DefaultExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class DefaultExpression : Expression
  {
    private readonly Type _type;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._type;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Default;
      }
    }

    internal DefaultExpression(Type type)
    {
      this._type = type;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitDefault(this);
    }
  }
}
