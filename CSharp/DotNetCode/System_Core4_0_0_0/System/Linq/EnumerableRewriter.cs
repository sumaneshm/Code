// Type: System.Linq.EnumerableRewriter
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;

namespace System.Linq
{
  internal class EnumerableRewriter : OldExpressionVisitor
  {
    private static volatile ILookup<string, MethodInfo> _seqMethods;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal EnumerableRewriter()
    {
    }

    internal override Expression VisitMethodCall(MethodCallExpression m)
    {
      Expression instance = this.Visit(m.Object);
      ReadOnlyCollection<Expression> readOnlyCollection1 = this.VisitExpressionList(m.Arguments);
      if (instance == m.Object && readOnlyCollection1 == m.Arguments)
        return (Expression) m;
      Enumerable.ToArray<Expression>((IEnumerable<Expression>) readOnlyCollection1);
      Type[] typeArgs = m.Method.IsGenericMethod ? m.Method.GetGenericArguments() : (Type[]) null;
      if ((m.Method.IsStatic || m.Method.DeclaringType.IsAssignableFrom(instance.Type)) && EnumerableRewriter.ArgsMatch(m.Method, readOnlyCollection1, typeArgs))
        return (Expression) Expression.Call(instance, m.Method, (IEnumerable<Expression>) readOnlyCollection1);
      if (m.Method.DeclaringType == typeof (Queryable))
      {
        MethodInfo enumerableMethod = EnumerableRewriter.FindEnumerableMethod(m.Method.Name, readOnlyCollection1, typeArgs);
        ReadOnlyCollection<Expression> readOnlyCollection2 = this.FixupQuotedArgs(enumerableMethod, readOnlyCollection1);
        return (Expression) Expression.Call(instance, enumerableMethod, (IEnumerable<Expression>) readOnlyCollection2);
      }
      else
      {
        BindingFlags flags = (BindingFlags) (8 | (m.Method.IsPublic ? 16 : 32));
        MethodInfo method = EnumerableRewriter.FindMethod(m.Method.DeclaringType, m.Method.Name, readOnlyCollection1, typeArgs, flags);
        ReadOnlyCollection<Expression> readOnlyCollection2 = this.FixupQuotedArgs(method, readOnlyCollection1);
        return (Expression) Expression.Call(instance, method, (IEnumerable<Expression>) readOnlyCollection2);
      }
    }

    internal override Expression VisitLambda(LambdaExpression lambda)
    {
      return (Expression) lambda;
    }

    internal override Expression VisitConstant(ConstantExpression c)
    {
      EnumerableQuery enumerableQuery = c.Value as EnumerableQuery;
      if (enumerableQuery == null)
        return (Expression) c;
      if (enumerableQuery.Enumerable == null)
        return this.Visit(enumerableQuery.Expression);
      Type publicType = EnumerableRewriter.GetPublicType(enumerableQuery.Enumerable.GetType());
      return (Expression) Expression.Constant((object) enumerableQuery.Enumerable, publicType);
    }

    internal override Expression VisitParameter(ParameterExpression p)
    {
      return (Expression) p;
    }

    private ReadOnlyCollection<Expression> FixupQuotedArgs(MethodInfo mi, ReadOnlyCollection<Expression> argList)
    {
      ParameterInfo[] parameters = mi.GetParameters();
      if (parameters.Length > 0)
      {
        List<Expression> list = (List<Expression>) null;
        int index1 = 0;
        for (int length = parameters.Length; index1 < length; ++index1)
        {
          Expression expression1 = argList[index1];
          Expression expression2 = this.FixupQuotedExpression(parameters[index1].ParameterType, expression1);
          if (list == null && expression2 != argList[index1])
          {
            list = new List<Expression>(argList.Count);
            for (int index2 = 0; index2 < index1; ++index2)
              list.Add(argList[index2]);
          }
          if (list != null)
            list.Add(expression2);
        }
        if (list != null)
          argList = ReadOnlyCollectionExtensions.ToReadOnlyCollection<Expression>((IEnumerable<Expression>) list);
      }
      return argList;
    }

    private Expression FixupQuotedExpression(Type type, Expression expression)
    {
      Expression expression1;
      for (expression1 = expression; !type.IsAssignableFrom(expression1.Type); expression1 = ((UnaryExpression) expression1).Operand)
      {
        if (expression1.NodeType != ExpressionType.Quote)
        {
          if (!type.IsAssignableFrom(expression1.Type) && type.IsArray && expression1.NodeType == ExpressionType.NewArrayInit)
          {
            Type c = EnumerableRewriter.StripExpression(expression1.Type);
            if (type.IsAssignableFrom(c))
            {
              Type elementType = type.GetElementType();
              NewArrayExpression newArrayExpression = (NewArrayExpression) expression1;
              List<Expression> list = new List<Expression>(newArrayExpression.Expressions.Count);
              int index = 0;
              for (int count = newArrayExpression.Expressions.Count; index < count; ++index)
                list.Add(this.FixupQuotedExpression(elementType, newArrayExpression.Expressions[index]));
              expression = (Expression) Expression.NewArrayInit(elementType, (IEnumerable<Expression>) list);
            }
          }
          return expression;
        }
      }
      return expression1;
    }

    private static Type GetPublicType(Type t)
    {
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Lookup<,>.Grouping))
        return typeof (IGrouping<,>).MakeGenericType(t.GetGenericArguments());
      if (!t.IsNestedPrivate)
        return t;
      foreach (Type type in t.GetInterfaces())
      {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>))
          return type;
      }
      if (typeof (IEnumerable).IsAssignableFrom(t))
        return typeof (IEnumerable);
      else
        return t;
    }

    private static MethodInfo FindEnumerableMethod(string name, ReadOnlyCollection<Expression> args, params Type[] typeArgs)
    {
      if (EnumerableRewriter._seqMethods == null)
        EnumerableRewriter._seqMethods = Enumerable.ToLookup<MethodInfo, string>((IEnumerable<MethodInfo>) typeof (Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public), (Func<MethodInfo, string>) (m => m.Name));
      MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(EnumerableRewriter._seqMethods[name], (Func<MethodInfo, bool>) (m => EnumerableRewriter.ArgsMatch(m, args, typeArgs)));
      if (methodInfo == (MethodInfo) null)
        throw Error.NoMethodOnTypeMatchingArguments((object) name, (object) typeof (Enumerable));
      if (typeArgs != null)
        return methodInfo.MakeGenericMethod(typeArgs);
      else
        return methodInfo;
    }

    internal static MethodInfo FindMethod(Type type, string name, ReadOnlyCollection<Expression> args, Type[] typeArgs, BindingFlags flags)
    {
      MethodInfo[] methodInfoArray = Enumerable.ToArray<MethodInfo>(Enumerable.Where<MethodInfo>((IEnumerable<MethodInfo>) type.GetMethods(flags), (Func<MethodInfo, bool>) (m => m.Name == name)));
      if (methodInfoArray.Length == 0)
        throw Error.NoMethodOnType((object) name, (object) type);
      MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>((IEnumerable<MethodInfo>) methodInfoArray, (Func<MethodInfo, bool>) (m => EnumerableRewriter.ArgsMatch(m, args, typeArgs)));
      if (methodInfo == (MethodInfo) null)
        throw Error.NoMethodOnTypeMatchingArguments((object) name, (object) type);
      if (typeArgs != null)
        return methodInfo.MakeGenericMethod(typeArgs);
      else
        return methodInfo;
    }

    private static bool ArgsMatch(MethodInfo m, ReadOnlyCollection<Expression> args, Type[] typeArgs)
    {
      ParameterInfo[] parameters = m.GetParameters();
      if (parameters.Length != args.Count || !m.IsGenericMethod && typeArgs != null && typeArgs.Length > 0)
        return false;
      if (!m.IsGenericMethodDefinition && m.IsGenericMethod && m.ContainsGenericParameters)
        m = m.GetGenericMethodDefinition();
      if (m.IsGenericMethodDefinition)
      {
        if (typeArgs == null || typeArgs.Length == 0 || m.GetGenericArguments().Length != typeArgs.Length)
          return false;
        m = m.MakeGenericMethod(typeArgs);
        parameters = m.GetParameters();
      }
      int index = 0;
      for (int count = args.Count; index < count; ++index)
      {
        Type type = parameters[index].ParameterType;
        if (type == (Type) null)
          return false;
        if (type.IsByRef)
          type = type.GetElementType();
        Expression expression = args[index];
        if (!type.IsAssignableFrom(expression.Type))
        {
          if (expression.NodeType == ExpressionType.Quote)
            expression = ((UnaryExpression) expression).Operand;
          if (!type.IsAssignableFrom(expression.Type) && !type.IsAssignableFrom(EnumerableRewriter.StripExpression(expression.Type)))
            return false;
        }
      }
      return true;
    }

    private static Type StripExpression(Type type)
    {
      bool isArray = type.IsArray;
      Type type1 = isArray ? type.GetElementType() : type;
      Type genericType = TypeHelper.FindGenericType(typeof (Expression<>), type1);
      if (genericType != (Type) null)
        type1 = genericType.GetGenericArguments()[0];
      if (!isArray)
        return type;
      int arrayRank = type.GetArrayRank();
      if (arrayRank != 1)
        return type1.MakeArrayType(arrayRank);
      else
        return type1.MakeArrayType();
    }
  }
}
