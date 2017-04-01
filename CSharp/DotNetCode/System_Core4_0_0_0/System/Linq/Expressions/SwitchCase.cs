// Type: System.Linq.Expressions.SwitchCase
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.SwitchCaseProxy))]
  [__DynamicallyInvokable]
  public sealed class SwitchCase
  {
    private readonly ReadOnlyCollection<Expression> _testValues;
    private readonly Expression _body;

    [__DynamicallyInvokable]
    public ReadOnlyCollection<Expression> TestValues
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._testValues;
      }
    }

    [__DynamicallyInvokable]
    public Expression Body
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._body;
      }
    }

    internal SwitchCase(Expression body, ReadOnlyCollection<Expression> testValues)
    {
      this._body = body;
      this._testValues = testValues;
    }

    [__DynamicallyInvokable]
    public override string ToString()
    {
      return ExpressionStringBuilder.SwitchCaseToString(this);
    }

    [__DynamicallyInvokable]
    public SwitchCase Update(IEnumerable<Expression> testValues, Expression body)
    {
      if (testValues == this.TestValues && body == this.Body)
        return this;
      else
        return Expression.SwitchCase(body, testValues);
    }
  }
}
