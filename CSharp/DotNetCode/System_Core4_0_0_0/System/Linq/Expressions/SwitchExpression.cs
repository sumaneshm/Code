// Type: System.Linq.Expressions.SwitchExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.SwitchExpressionProxy))]
  [__DynamicallyInvokable]
  public sealed class SwitchExpression : Expression
  {
    private readonly Type _type;
    private readonly Expression _switchValue;
    private readonly ReadOnlyCollection<SwitchCase> _cases;
    private readonly Expression _defaultBody;
    private readonly MethodInfo _comparison;

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
        return ExpressionType.Switch;
      }
    }

    [__DynamicallyInvokable]
    public Expression SwitchValue
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._switchValue;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<SwitchCase> Cases
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._cases;
      }
    }

    [__DynamicallyInvokable]
    public Expression DefaultBody
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._defaultBody;
      }
    }

    [__DynamicallyInvokable]
    public MethodInfo Comparison
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._comparison;
      }
    }

    internal bool IsLifted
    {
      get
      {
        if (!TypeUtils.IsNullableType(this._switchValue.Type))
          return false;
        if (!(this._comparison == (MethodInfo) null))
          return !TypeUtils.AreEquivalent(this._switchValue.Type, TypeUtils.GetNonRefType(TypeExtensions.GetParametersCached((MethodBase) this._comparison)[0].ParameterType));
        else
          return true;
      }
    }

    internal SwitchExpression(Type type, Expression switchValue, Expression defaultBody, MethodInfo comparison, ReadOnlyCollection<SwitchCase> cases)
    {
      this._type = type;
      this._switchValue = switchValue;
      this._defaultBody = defaultBody;
      this._comparison = comparison;
      this._cases = cases;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitSwitch(this);
    }

    [__DynamicallyInvokable]
    public SwitchExpression Update(Expression switchValue, IEnumerable<SwitchCase> cases, Expression defaultBody)
    {
      if (switchValue == this.SwitchValue && cases == this.Cases && defaultBody == this.DefaultBody)
        return this;
      else
        return Expression.Switch(this.Type, switchValue, defaultBody, this.Comparison, cases);
    }
  }
}
