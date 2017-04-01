// Type: System.Runtime.CompilerServices.RuntimeOps
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Linq.Expressions.Compiler;

namespace System.Runtime.CompilerServices
{
  [DebuggerStepThrough]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [__DynamicallyInvokable]
  public static class RuntimeOps
  {
    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static bool ExpandoTryGetValue(ExpandoObject expando, object indexClass, int index, string name, bool ignoreCase, out object value)
    {
      return expando.TryGetValue(indexClass, index, name, ignoreCase, out value);
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static object ExpandoTrySetValue(ExpandoObject expando, object indexClass, int index, object value, string name, bool ignoreCase)
    {
      expando.TrySetValue(indexClass, index, value, name, ignoreCase, false);
      return value;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static bool ExpandoTryDeleteValue(ExpandoObject expando, object indexClass, int index, string name, bool ignoreCase)
    {
      return expando.TryDeleteValue(indexClass, index, name, ignoreCase, ExpandoObject.Uninitialized);
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static bool ExpandoCheckVersion(ExpandoObject expando, object version)
    {
      return expando.Class == version;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static void ExpandoPromoteClass(ExpandoObject expando, object oldClass, object newClass)
    {
      expando.PromoteClass(oldClass, newClass);
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static Expression Quote(Expression expression, object hoistedLocals, object[] locals)
    {
      return new RuntimeOps.ExpressionQuoter((HoistedLocals) hoistedLocals, locals).Visit(expression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static IRuntimeVariables MergeRuntimeVariables(IRuntimeVariables first, IRuntimeVariables second, int[] indexes)
    {
      return (IRuntimeVariables) new RuntimeOps.MergedRuntimeVariables(first, second, indexes);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("do not use this method", true)]
    [__DynamicallyInvokable]
    public static IRuntimeVariables CreateRuntimeVariables(object[] data, long[] indexes)
    {
      return (IRuntimeVariables) new RuntimeOps.RuntimeVariableList(data, indexes);
    }

    [Obsolete("do not use this method", true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [__DynamicallyInvokable]
    public static IRuntimeVariables CreateRuntimeVariables()
    {
      return (IRuntimeVariables) new RuntimeOps.EmptyRuntimeVariables();
    }

    private sealed class ExpressionQuoter : ExpressionVisitor
    {
      private readonly Stack<Set<ParameterExpression>> _shadowedVars = new Stack<Set<ParameterExpression>>();
      private readonly HoistedLocals _scope;
      private readonly object[] _locals;

      internal ExpressionQuoter(HoistedLocals scope, object[] locals)
      {
        this._scope = scope;
        this._locals = locals;
      }

      protected internal override Expression VisitLambda<T>(Expression<T> node)
      {
        this._shadowedVars.Push(new Set<ParameterExpression>((IList<ParameterExpression>) node.Parameters));
        Expression body = this.Visit(node.Body);
        this._shadowedVars.Pop();
        if (body == node.Body)
          return (Expression) node;
        else
          return (Expression) Expression.Lambda<T>(body, node.Name, node.TailCall, (IEnumerable<ParameterExpression>) node.Parameters);
      }

      protected internal override Expression VisitBlock(BlockExpression node)
      {
        if (node.Variables.Count > 0)
          this._shadowedVars.Push(new Set<ParameterExpression>((IList<ParameterExpression>) node.Variables));
        ReadOnlyCollection<Expression> readOnlyCollection = this.Visit(node.Expressions);
        if (node.Variables.Count > 0)
          this._shadowedVars.Pop();
        if (readOnlyCollection == node.Expressions)
          return (Expression) node;
        else
          return (Expression) Expression.Block((IEnumerable<ParameterExpression>) node.Variables, (IEnumerable<Expression>) readOnlyCollection);
      }

      protected override CatchBlock VisitCatchBlock(CatchBlock node)
      {
        if (node.Variable != null)
          this._shadowedVars.Push(new Set<ParameterExpression>((IList<ParameterExpression>) new ParameterExpression[1]
          {
            node.Variable
          }));
        Expression body = this.Visit(node.Body);
        Expression filter = this.Visit(node.Filter);
        if (node.Variable != null)
          this._shadowedVars.Pop();
        if (body == node.Body && filter == node.Filter)
          return node;
        else
          return Expression.MakeCatchBlock(node.Test, node.Variable, body, filter);
      }

      protected internal override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
      {
        int count = node.Variables.Count;
        List<IStrongBox> list1 = new List<IStrongBox>();
        List<ParameterExpression> list2 = new List<ParameterExpression>();
        int[] numArray = new int[count];
        for (int index = 0; index < count; ++index)
        {
          IStrongBox box = this.GetBox(node.Variables[index]);
          if (box == null)
          {
            numArray[index] = list2.Count;
            list2.Add(node.Variables[index]);
          }
          else
          {
            numArray[index] = -1 - list1.Count;
            list1.Add(box);
          }
        }
        if (list1.Count == 0)
          return (Expression) node;
        ConstantExpression constantExpression = Expression.Constant((object) new RuntimeOps.RuntimeVariables(list1.ToArray()), typeof (IRuntimeVariables));
        if (list2.Count == 0)
          return (Expression) constantExpression;
        else
          return (Expression) Expression.Call(typeof (RuntimeOps).GetMethod("MergeRuntimeVariables"), (Expression) Expression.RuntimeVariables((IEnumerable<ParameterExpression>) new TrueReadOnlyCollection<ParameterExpression>(list2.ToArray())), (Expression) constantExpression, (Expression) Expression.Constant((object) numArray));
      }

      protected internal override Expression VisitParameter(ParameterExpression node)
      {
        IStrongBox box = this.GetBox(node);
        if (box == null)
          return (Expression) node;
        else
          return (Expression) Expression.Field((Expression) Expression.Constant((object) box), "Value");
      }

      private IStrongBox GetBox(ParameterExpression variable)
      {
        foreach (Set<ParameterExpression> set in this._shadowedVars)
        {
          if (set.Contains(variable))
            return (IStrongBox) null;
        }
        HoistedLocals hoistedLocals = this._scope;
        object[] locals = this._locals;
        int index;
        while (!hoistedLocals.Indexes.TryGetValue((Expression) variable, out index))
        {
          hoistedLocals = hoistedLocals.Parent;
          if (hoistedLocals == null)
            throw ContractUtils.Unreachable;
          locals = HoistedLocals.GetParent(locals);
        }
        return (IStrongBox) locals[index];
      }
    }

    private sealed class RuntimeVariables : IRuntimeVariables
    {
      private readonly IStrongBox[] _boxes;

      int IRuntimeVariables.Count
      {
        get
        {
          return this._boxes.Length;
        }
      }

      internal RuntimeVariables(IStrongBox[] boxes)
      {
        this._boxes = boxes;
      }

      object IRuntimeVariables.get_Item(int index)
      {
        return this._boxes[index].Value;
      }

      void IRuntimeVariables.set_Item(int index, object value)
      {
        this._boxes[index].Value = value;
      }
    }

    private sealed class MergedRuntimeVariables : IRuntimeVariables
    {
      private readonly IRuntimeVariables _first;
      private readonly IRuntimeVariables _second;
      private readonly int[] _indexes;

      public int Count
      {
        get
        {
          return this._indexes.Length;
        }
      }

      public object this[int index]
      {
        get
        {
          index = this._indexes[index];
          if (index < 0)
            return this._second[-1 - index];
          else
            return this._first[index];
        }
        set
        {
          index = this._indexes[index];
          if (index >= 0)
            this._first[index] = value;
          else
            this._second[-1 - index] = value;
        }
      }

      internal MergedRuntimeVariables(IRuntimeVariables first, IRuntimeVariables second, int[] indexes)
      {
        this._first = first;
        this._second = second;
        this._indexes = indexes;
      }
    }

    private sealed class EmptyRuntimeVariables : IRuntimeVariables
    {
      int IRuntimeVariables.Count
      {
        get
        {
          return 0;
        }
      }

      object IRuntimeVariables.get_Item(int index)
      {
        throw new ArgumentOutOfRangeException("index");
      }

      void IRuntimeVariables.set_Item(int index, object value)
      {
        throw new ArgumentOutOfRangeException("index");
      }
    }

    private sealed class RuntimeVariableList : IRuntimeVariables
    {
      private readonly object[] _data;
      private readonly long[] _indexes;

      public int Count
      {
        get
        {
          return this._indexes.Length;
        }
      }

      public object this[int index]
      {
        get
        {
          return this.GetStrongBox(index).Value;
        }
        set
        {
          this.GetStrongBox(index).Value = value;
        }
      }

      internal RuntimeVariableList(object[] data, long[] indexes)
      {
        this._data = data;
        this._indexes = indexes;
      }

      private IStrongBox GetStrongBox(int index)
      {
        long num = this._indexes[index];
        object[] locals = this._data;
        for (int index1 = (int) (num >> 32); index1 > 0; --index1)
          locals = HoistedLocals.GetParent(locals);
        return (IStrongBox) locals[(int) num];
      }
    }
  }
}
