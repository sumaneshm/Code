// Type: System.Linq.Expressions.ElementInit
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
  public sealed class ElementInit : IArgumentProvider
  {
    private MethodInfo _addMethod;
    private ReadOnlyCollection<Expression> _arguments;

    [__DynamicallyInvokable]
    public MethodInfo AddMethod
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._addMethod;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<Expression> Arguments
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._arguments;
      }
    }

    int IArgumentProvider.ArgumentCount
    {
      get
      {
        return this._arguments.Count;
      }
    }

    internal ElementInit(MethodInfo addMethod, ReadOnlyCollection<Expression> arguments)
    {
      this._addMethod = addMethod;
      this._arguments = arguments;
    }

    Expression IArgumentProvider.GetArgument(int index)
    {
      return this._arguments[index];
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      return ExpressionStringBuilder.ElementInitBindingToString(this);
    }

    [__DynamicallyInvokable]
    public ElementInit Update(IEnumerable<Expression> arguments)
    {
      if (arguments == this.Arguments)
        return this;
      else
        return Expression.ElementInit(this.AddMethod, arguments);
    }
  }
}
