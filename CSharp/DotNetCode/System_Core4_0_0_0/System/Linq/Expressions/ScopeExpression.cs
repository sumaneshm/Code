// Type: System.Linq.Expressions.ScopeExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions
{
  internal class ScopeExpression : BlockExpression
  {
    private IList<ParameterExpression> _variables;

    internal override int VariableCount
    {
      get
      {
        return this._variables.Count;
      }
    }

    protected IList<ParameterExpression> VariablesList
    {
      get
      {
        return this._variables;
      }
    }

    internal ScopeExpression(IList<ParameterExpression> variables)
    {
      this._variables = variables;
    }

    internal override ParameterExpression GetVariable(int index)
    {
      return this._variables[index];
    }

    internal override ReadOnlyCollection<ParameterExpression> GetOrMakeVariables()
    {
      return Expression.ReturnReadOnly<ParameterExpression>(ref this._variables);
    }

    internal IList<ParameterExpression> ReuseOrValidateVariables(ReadOnlyCollection<ParameterExpression> variables)
    {
      if (variables == null || variables == this.VariablesList)
        return this.VariablesList;
      Expression.ValidateVariables(variables, "variables");
      return (IList<ParameterExpression>) variables;
    }
  }
}
