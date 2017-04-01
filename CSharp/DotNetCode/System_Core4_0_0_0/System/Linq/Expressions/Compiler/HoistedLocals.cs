// Type: System.Linq.Expressions.Compiler.HoistedLocals
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class HoistedLocals
  {
    internal readonly HoistedLocals Parent;
    internal readonly ReadOnlyDictionary<Expression, int> Indexes;
    internal readonly ReadOnlyCollection<ParameterExpression> Variables;
    internal readonly ParameterExpression SelfVariable;

    internal ParameterExpression ParentVariable
    {
      get
      {
        if (this.Parent == null)
          return (ParameterExpression) null;
        else
          return this.Parent.SelfVariable;
      }
    }

    internal HoistedLocals(HoistedLocals parent, ReadOnlyCollection<ParameterExpression> vars)
    {
      if (parent != null)
        vars = (ReadOnlyCollection<ParameterExpression>) new TrueReadOnlyCollection<ParameterExpression>(CollectionExtensions.AddFirst<ParameterExpression>((IList<ParameterExpression>) vars, parent.SelfVariable));
      Dictionary<Expression, int> dictionary = new Dictionary<Expression, int>(vars.Count);
      for (int index = 0; index < vars.Count; ++index)
        dictionary.Add((Expression) vars[index], index);
      this.SelfVariable = Expression.Variable(typeof (object[]), (string) null);
      this.Parent = parent;
      this.Variables = vars;
      this.Indexes = new ReadOnlyDictionary<Expression, int>((IDictionary<Expression, int>) dictionary);
    }

    internal static object[] GetParent(object[] locals)
    {
      return ((StrongBox<object[]>) locals[0]).Value;
    }
  }
}
