// Type: System.Linq.Expressions.InstanceMethodCallExpressionN
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal class InstanceMethodCallExpressionN : MethodCallExpression, IArgumentProvider
  {
    private IList<Expression> _arguments;
    private readonly Expression _instance;

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return this._arguments.Count;
      }
    }

    public InstanceMethodCallExpressionN(MethodInfo method, Expression instance, IList<Expression> args)
      : base(method)
    {
      this._instance = instance;
      this._arguments = args;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      return this._arguments[index];
    }

    internal override Expression GetInstance()
    {
      return this._instance;
    }

    internal override ReadOnlyCollection<Expression> GetOrMakeArguments()
    {
      return Expression.ReturnReadOnly<Expression>(ref this._arguments);
    }

    internal override MethodCallExpression Rewrite(Expression instance, IList<Expression> args)
    {
      return Expression.Call(instance, this.Method, (IEnumerable<Expression>) (args ?? this._arguments));
    }
  }
}
