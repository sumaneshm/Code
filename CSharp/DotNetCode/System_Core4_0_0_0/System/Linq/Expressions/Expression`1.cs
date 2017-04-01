// Type: System.Linq.Expressions.Expression`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions.Compiler;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public sealed class Expression<TDelegate> : LambdaExpression
  {
    internal Expression(Expression body, string name, bool tailCall, ReadOnlyCollection<ParameterExpression> parameters)
      : base(typeof (TDelegate), name, body, tailCall, parameters)
    {
    }

    [__DynamicallyInvokable]
    public TDelegate Compile()
    {
      return (TDelegate) LambdaCompiler.Compile((LambdaExpression) this, (DebugInfoGenerator) null);
    }

    public TDelegate Compile(DebugInfoGenerator debugInfoGenerator)
    {
      ContractUtils.RequiresNotNull((object) debugInfoGenerator, "debugInfoGenerator");
      return (TDelegate) LambdaCompiler.Compile((LambdaExpression) this, debugInfoGenerator);
    }

    [__DynamicallyInvokable]
    public Expression<TDelegate> Update(Expression body, IEnumerable<ParameterExpression> parameters)
    {
      if (body == this.Body && parameters == this.Parameters)
        return this;
      else
        return Expression.Lambda<TDelegate>(body, this.Name, this.TailCall, parameters);
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitLambda<TDelegate>(this);
    }

    internal override LambdaExpression Accept(StackSpiller spiller)
    {
      return (LambdaExpression) spiller.Rewrite<TDelegate>(this);
    }

    internal static LambdaExpression Create(Expression body, string name, bool tailCall, ReadOnlyCollection<ParameterExpression> parameters)
    {
      return (LambdaExpression) new Expression<TDelegate>(body, name, tailCall, parameters);
    }
  }
}
