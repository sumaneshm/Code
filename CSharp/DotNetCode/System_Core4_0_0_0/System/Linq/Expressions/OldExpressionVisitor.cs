﻿// Type: System.Linq.Expressions.OldExpressionVisitor
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime;

namespace System.Linq.Expressions
{
  internal abstract class OldExpressionVisitor
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal OldExpressionVisitor()
    {
    }

    internal virtual Expression Visit(Expression exp)
    {
      if (exp == null)
        return exp;
      switch (exp.NodeType)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.ArrayIndex:
        case ExpressionType.Coalesce:
        case ExpressionType.Divide:
        case ExpressionType.Equal:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LeftShift:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.NotEqual:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.Power:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return this.VisitBinary((BinaryExpression) exp);
        case ExpressionType.ArrayLength:
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
          return this.VisitUnary((UnaryExpression) exp);
        case ExpressionType.Call:
          return this.VisitMethodCall((MethodCallExpression) exp);
        case ExpressionType.Conditional:
          return this.VisitConditional((ConditionalExpression) exp);
        case ExpressionType.Constant:
          return this.VisitConstant((ConstantExpression) exp);
        case ExpressionType.Invoke:
          return this.VisitInvocation((InvocationExpression) exp);
        case ExpressionType.Lambda:
          return this.VisitLambda((LambdaExpression) exp);
        case ExpressionType.ListInit:
          return this.VisitListInit((ListInitExpression) exp);
        case ExpressionType.MemberAccess:
          return this.VisitMemberAccess((MemberExpression) exp);
        case ExpressionType.MemberInit:
          return this.VisitMemberInit((MemberInitExpression) exp);
        case ExpressionType.New:
          return (Expression) this.VisitNew((NewExpression) exp);
        case ExpressionType.NewArrayInit:
        case ExpressionType.NewArrayBounds:
          return this.VisitNewArray((NewArrayExpression) exp);
        case ExpressionType.Parameter:
          return this.VisitParameter((ParameterExpression) exp);
        case ExpressionType.TypeIs:
          return this.VisitTypeIs((TypeBinaryExpression) exp);
        default:
          throw Error.UnhandledExpressionType((object) exp.NodeType);
      }
    }

    internal virtual MemberBinding VisitBinding(MemberBinding binding)
    {
      switch (binding.BindingType)
      {
        case MemberBindingType.Assignment:
          return (MemberBinding) this.VisitMemberAssignment((MemberAssignment) binding);
        case MemberBindingType.MemberBinding:
          return (MemberBinding) this.VisitMemberMemberBinding((MemberMemberBinding) binding);
        case MemberBindingType.ListBinding:
          return (MemberBinding) this.VisitMemberListBinding((MemberListBinding) binding);
        default:
          throw Error.UnhandledBindingType((object) binding.BindingType);
      }
    }

    internal virtual ElementInit VisitElementInitializer(ElementInit initializer)
    {
      ReadOnlyCollection<Expression> readOnlyCollection = this.VisitExpressionList(initializer.Arguments);
      if (readOnlyCollection != initializer.Arguments)
        return Expression.ElementInit(initializer.AddMethod, (IEnumerable<Expression>) readOnlyCollection);
      else
        return initializer;
    }

    internal virtual Expression VisitUnary(UnaryExpression u)
    {
      Expression operand = this.Visit(u.Operand);
      if (operand != u.Operand)
        return (Expression) Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
      else
        return (Expression) u;
    }

    internal virtual Expression VisitBinary(BinaryExpression b)
    {
      Expression left = this.Visit(b.Left);
      Expression right = this.Visit(b.Right);
      Expression expression = this.Visit((Expression) b.Conversion);
      if (left == b.Left && right == b.Right && expression == b.Conversion)
        return (Expression) b;
      if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
        return (Expression) Expression.Coalesce(left, right, expression as LambdaExpression);
      else
        return (Expression) Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
    }

    internal virtual Expression VisitTypeIs(TypeBinaryExpression b)
    {
      Expression expression = this.Visit(b.Expression);
      if (expression != b.Expression)
        return (Expression) Expression.TypeIs(expression, b.TypeOperand);
      else
        return (Expression) b;
    }

    internal virtual Expression VisitConstant(ConstantExpression c)
    {
      return (Expression) c;
    }

    internal virtual Expression VisitConditional(ConditionalExpression c)
    {
      Expression test = this.Visit(c.Test);
      Expression ifTrue = this.Visit(c.IfTrue);
      Expression ifFalse = this.Visit(c.IfFalse);
      if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
        return (Expression) Expression.Condition(test, ifTrue, ifFalse);
      else
        return (Expression) c;
    }

    internal virtual Expression VisitParameter(ParameterExpression p)
    {
      return (Expression) p;
    }

    internal virtual Expression VisitMemberAccess(MemberExpression m)
    {
      Expression expression = this.Visit(m.Expression);
      if (expression != m.Expression)
        return (Expression) Expression.MakeMemberAccess(expression, m.Member);
      else
        return (Expression) m;
    }

    internal virtual Expression VisitMethodCall(MethodCallExpression m)
    {
      Expression instance = this.Visit(m.Object);
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(m.Arguments);
      if (instance != m.Object || arguments != m.Arguments)
        return (Expression) Expression.Call(instance, m.Method, arguments);
      else
        return (Expression) m;
    }

    internal virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
    {
      List<Expression> list = (List<Expression>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        Expression expression = this.Visit(original[index1]);
        if (list != null)
          list.Add(expression);
        else if (expression != original[index1])
        {
          list = new List<Expression>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            list.Add(original[index2]);
          list.Add(expression);
        }
      }
      if (list != null)
        return ReadOnlyCollectionExtensions.ToReadOnlyCollection<Expression>((IEnumerable<Expression>) list);
      else
        return original;
    }

    internal virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
    {
      Expression expression = this.Visit(assignment.Expression);
      if (expression != assignment.Expression)
        return Expression.Bind(assignment.Member, expression);
      else
        return assignment;
    }

    internal virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
      if (bindings != binding.Bindings)
        return Expression.MemberBind(binding.Member, bindings);
      else
        return binding;
    }

    internal virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
    {
      IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
      if (initializers != binding.Initializers)
        return Expression.ListBind(binding.Member, initializers);
      else
        return binding;
    }

    internal virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
    {
      List<MemberBinding> list = (List<MemberBinding>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        MemberBinding memberBinding = this.VisitBinding(original[index1]);
        if (list != null)
          list.Add(memberBinding);
        else if (memberBinding != original[index1])
        {
          list = new List<MemberBinding>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            list.Add(original[index2]);
          list.Add(memberBinding);
        }
      }
      return (IEnumerable<MemberBinding>) list ?? (IEnumerable<MemberBinding>) original;
    }

    internal virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
    {
      List<ElementInit> list = (List<ElementInit>) null;
      int index1 = 0;
      for (int count = original.Count; index1 < count; ++index1)
      {
        ElementInit elementInit = this.VisitElementInitializer(original[index1]);
        if (list != null)
          list.Add(elementInit);
        else if (elementInit != original[index1])
        {
          list = new List<ElementInit>(count);
          for (int index2 = 0; index2 < index1; ++index2)
            list.Add(original[index2]);
          list.Add(elementInit);
        }
      }
      return (IEnumerable<ElementInit>) list ?? (IEnumerable<ElementInit>) original;
    }

    internal virtual Expression VisitLambda(LambdaExpression lambda)
    {
      Expression body = this.Visit(lambda.Body);
      if (body != lambda.Body)
        return (Expression) Expression.Lambda(lambda.Type, body, (IEnumerable<ParameterExpression>) lambda.Parameters);
      else
        return (Expression) lambda;
    }

    internal virtual NewExpression VisitNew(NewExpression nex)
    {
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(nex.Arguments);
      if (arguments == nex.Arguments)
        return nex;
      if (nex.Members != null)
        return Expression.New(nex.Constructor, arguments, (IEnumerable<MemberInfo>) nex.Members);
      else
        return Expression.New(nex.Constructor, arguments);
    }

    internal virtual Expression VisitMemberInit(MemberInitExpression init)
    {
      NewExpression newExpression = this.VisitNew(init.NewExpression);
      IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
      if (newExpression != init.NewExpression || bindings != init.Bindings)
        return (Expression) Expression.MemberInit(newExpression, bindings);
      else
        return (Expression) init;
    }

    internal virtual Expression VisitListInit(ListInitExpression init)
    {
      NewExpression newExpression = this.VisitNew(init.NewExpression);
      IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
      if (newExpression != init.NewExpression || initializers != init.Initializers)
        return (Expression) Expression.ListInit(newExpression, initializers);
      else
        return (Expression) init;
    }

    internal virtual Expression VisitNewArray(NewArrayExpression na)
    {
      IEnumerable<Expression> enumerable = (IEnumerable<Expression>) this.VisitExpressionList(na.Expressions);
      if (enumerable == na.Expressions)
        return (Expression) na;
      if (na.NodeType == ExpressionType.NewArrayInit)
        return (Expression) Expression.NewArrayInit(na.Type.GetElementType(), enumerable);
      else
        return (Expression) Expression.NewArrayBounds(na.Type.GetElementType(), enumerable);
    }

    internal virtual Expression VisitInvocation(InvocationExpression iv)
    {
      IEnumerable<Expression> arguments = (IEnumerable<Expression>) this.VisitExpressionList(iv.Arguments);
      Expression expression = this.Visit(iv.Expression);
      if (arguments != iv.Arguments || expression != iv.Expression)
        return (Expression) Expression.Invoke(expression, arguments);
      else
        return (Expression) iv;
    }
  }
}
