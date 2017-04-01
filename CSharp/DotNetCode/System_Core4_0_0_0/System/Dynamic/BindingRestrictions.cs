// Type: System.Dynamic.BindingRestrictions
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Dynamic
{
  [DebuggerDisplay("{DebugView}")]
  [DebuggerTypeProxy(typeof (BindingRestrictions.BindingRestrictionsProxy))]
  [__DynamicallyInvokable]
  public abstract class BindingRestrictions
  {
    [__DynamicallyInvokable]
    public static readonly BindingRestrictions Empty = (BindingRestrictions) new BindingRestrictions.CustomRestriction((Expression) Expression.Constant((object) true));
    private const int TypeRestrictionHash = 268435456;
    private const int InstanceRestrictionHash = 536870912;
    private const int CustomRestrictionHash = 1073741824;

    private string DebugView
    {
      get
      {
        return this.ToExpression().ToString();
      }
    }

    static BindingRestrictions()
    {
    }

    private BindingRestrictions()
    {
    }

    internal abstract Expression GetExpression();

    [__DynamicallyInvokable]
    public BindingRestrictions Merge(BindingRestrictions restrictions)
    {
      ContractUtils.RequiresNotNull((object) restrictions, "restrictions");
      if (this == BindingRestrictions.Empty)
        return restrictions;
      if (restrictions == BindingRestrictions.Empty)
        return this;
      else
        return (BindingRestrictions) new BindingRestrictions.MergedRestriction(this, restrictions);
    }

    [__DynamicallyInvokable]
    public static BindingRestrictions GetTypeRestriction(Expression expression, Type type)
    {
      ContractUtils.RequiresNotNull((object) expression, "expression");
      ContractUtils.RequiresNotNull((object) type, "type");
      return (BindingRestrictions) new BindingRestrictions.TypeRestriction(expression, type);
    }

    [__DynamicallyInvokable]
    public static BindingRestrictions GetInstanceRestriction(Expression expression, object instance)
    {
      ContractUtils.RequiresNotNull((object) expression, "expression");
      return (BindingRestrictions) new BindingRestrictions.InstanceRestriction(expression, instance);
    }

    [__DynamicallyInvokable]
    public static BindingRestrictions GetExpressionRestriction(Expression expression)
    {
      ContractUtils.RequiresNotNull((object) expression, "expression");
      ContractUtils.Requires(expression.Type == typeof (bool), "expression");
      return (BindingRestrictions) new BindingRestrictions.CustomRestriction(expression);
    }

    [__DynamicallyInvokable]
    public static BindingRestrictions Combine(IList<DynamicMetaObject> contributingObjects)
    {
      BindingRestrictions bindingRestrictions = BindingRestrictions.Empty;
      if (contributingObjects != null)
      {
        foreach (DynamicMetaObject dynamicMetaObject in (IEnumerable<DynamicMetaObject>) contributingObjects)
        {
          if (dynamicMetaObject != null)
            bindingRestrictions = bindingRestrictions.Merge(dynamicMetaObject.Restrictions);
        }
      }
      return bindingRestrictions;
    }

    [__DynamicallyInvokable]
    public Expression ToExpression()
    {
      if (this == BindingRestrictions.Empty)
        return (Expression) Expression.Constant((object) true);
      BindingRestrictions.TestBuilder testBuilder = new BindingRestrictions.TestBuilder();
      Stack<BindingRestrictions> stack = new Stack<BindingRestrictions>();
      stack.Push(this);
      do
      {
        BindingRestrictions restrictions = stack.Pop();
        BindingRestrictions.MergedRestriction mergedRestriction = restrictions as BindingRestrictions.MergedRestriction;
        if (mergedRestriction != null)
        {
          stack.Push(mergedRestriction.Right);
          stack.Push(mergedRestriction.Left);
        }
        else
          testBuilder.Append(restrictions);
      }
      while (stack.Count > 0);
      return testBuilder.ToExpression();
    }

    internal static BindingRestrictions GetTypeRestriction(DynamicMetaObject obj)
    {
      if (obj.Value == null && obj.HasValue)
        return BindingRestrictions.GetInstanceRestriction(obj.Expression, (object) null);
      else
        return BindingRestrictions.GetTypeRestriction(obj.Expression, obj.LimitType);
    }

    private sealed class TestBuilder
    {
      private readonly Set<BindingRestrictions> _unique = new Set<BindingRestrictions>();
      private readonly Stack<BindingRestrictions.TestBuilder.AndNode> _tests = new Stack<BindingRestrictions.TestBuilder.AndNode>();

      internal void Append(BindingRestrictions restrictions)
      {
        if (this._unique.Contains(restrictions))
          return;
        this._unique.Add(restrictions);
        this.Push(restrictions.GetExpression(), 0);
      }

      internal Expression ToExpression()
      {
        Expression right = this._tests.Pop().Node;
        while (this._tests.Count > 0)
          right = (Expression) Expression.AndAlso(this._tests.Pop().Node, right);
        return right;
      }

      private void Push(Expression node, int depth)
      {
        for (; this._tests.Count > 0 && this._tests.Peek().Depth == depth; ++depth)
          node = (Expression) Expression.AndAlso(this._tests.Pop().Node, node);
        this._tests.Push(new BindingRestrictions.TestBuilder.AndNode()
        {
          Node = node,
          Depth = depth
        });
      }

      private struct AndNode
      {
        internal int Depth;
        internal Expression Node;
      }
    }

    private sealed class MergedRestriction : BindingRestrictions
    {
      internal readonly BindingRestrictions Left;
      internal readonly BindingRestrictions Right;

      internal MergedRestriction(BindingRestrictions left, BindingRestrictions right)
      {
        this.Left = left;
        this.Right = right;
      }

      internal override Expression GetExpression()
      {
        throw ContractUtils.Unreachable;
      }
    }

    private sealed class CustomRestriction : BindingRestrictions
    {
      private readonly Expression _expression;

      internal CustomRestriction(Expression expression)
      {
        this._expression = expression;
      }

      public override bool Equals(object obj)
      {
        BindingRestrictions.CustomRestriction customRestriction = obj as BindingRestrictions.CustomRestriction;
        if (customRestriction != null)
          return customRestriction._expression == this._expression;
        else
          return false;
      }

      public override int GetHashCode()
      {
        return 1073741824 ^ this._expression.GetHashCode();
      }

      internal override Expression GetExpression()
      {
        return this._expression;
      }
    }

    private sealed class TypeRestriction : BindingRestrictions
    {
      private readonly Expression _expression;
      private readonly Type _type;

      internal TypeRestriction(Expression parameter, Type type)
      {
        this._expression = parameter;
        this._type = type;
      }

      public override bool Equals(object obj)
      {
        BindingRestrictions.TypeRestriction typeRestriction = obj as BindingRestrictions.TypeRestriction;
        if (typeRestriction != null && TypeUtils.AreEquivalent(typeRestriction._type, this._type))
          return typeRestriction._expression == this._expression;
        else
          return false;
      }

      public override int GetHashCode()
      {
        return 268435456 ^ this._expression.GetHashCode() ^ this._type.GetHashCode();
      }

      internal override Expression GetExpression()
      {
        return (Expression) Expression.TypeEqual(this._expression, this._type);
      }
    }

    private sealed class InstanceRestriction : BindingRestrictions
    {
      private readonly Expression _expression;
      private readonly object _instance;

      internal InstanceRestriction(Expression parameter, object instance)
      {
        this._expression = parameter;
        this._instance = instance;
      }

      public override bool Equals(object obj)
      {
        BindingRestrictions.InstanceRestriction instanceRestriction = obj as BindingRestrictions.InstanceRestriction;
        if (instanceRestriction != null && instanceRestriction._instance == this._instance)
          return instanceRestriction._expression == this._expression;
        else
          return false;
      }

      public override int GetHashCode()
      {
        return 536870912 ^ RuntimeHelpers.GetHashCode(this._instance) ^ this._expression.GetHashCode();
      }

      internal override Expression GetExpression()
      {
        if (this._instance == null)
          return (Expression) Expression.Equal((Expression) Expression.Convert(this._expression, typeof (object)), (Expression) Expression.Constant((object) null));
        ParameterExpression parameterExpression = Expression.Parameter(typeof (object), (string) null);
        return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression
        }, new Expression[2]
        {
          (Expression) Expression.Assign((Expression) parameterExpression, (Expression) Expression.Property((Expression) Expression.Constant((object) new WeakReference(this._instance)), typeof (WeakReference).GetProperty("Target"))),
          (Expression) Expression.AndAlso((Expression) Expression.NotEqual((Expression) parameterExpression, (Expression) Expression.Constant((object) null)), (Expression) Expression.Equal((Expression) Expression.Convert(this._expression, typeof (object)), (Expression) parameterExpression))
        });
      }
    }

    private sealed class BindingRestrictionsProxy
    {
      private readonly BindingRestrictions _node;

      public bool IsEmpty
      {
        get
        {
          return this._node == BindingRestrictions.Empty;
        }
      }

      public Expression Test
      {
        get
        {
          return this._node.ToExpression();
        }
      }

      public BindingRestrictions[] Restrictions
      {
        get
        {
          List<BindingRestrictions> list = new List<BindingRestrictions>();
          Stack<BindingRestrictions> stack = new Stack<BindingRestrictions>();
          stack.Push(this._node);
          do
          {
            BindingRestrictions bindingRestrictions = stack.Pop();
            BindingRestrictions.MergedRestriction mergedRestriction = bindingRestrictions as BindingRestrictions.MergedRestriction;
            if (mergedRestriction != null)
            {
              stack.Push(mergedRestriction.Right);
              stack.Push(mergedRestriction.Left);
            }
            else
              list.Add(bindingRestrictions);
          }
          while (stack.Count > 0);
          return list.ToArray();
        }
      }

      public BindingRestrictionsProxy(BindingRestrictions node)
      {
        this._node = node;
      }

      public override string ToString()
      {
        return this._node.DebugView;
      }
    }
  }
}
