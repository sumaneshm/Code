// Type: System.Linq.Expressions.Compiler.StackSpiller
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Compiler
{
  internal class StackSpiller
  {
    private readonly StackSpiller.TempMaker _tm = new StackSpiller.TempMaker();
    private readonly StackSpiller.Stack _startingStack;
    private StackSpiller.RewriteAction _lambdaRewrite;

    private StackSpiller(StackSpiller.Stack stack)
    {
      this._startingStack = stack;
    }

    internal Expression<T> Rewrite<T>(Expression<T> lambda)
    {
      StackSpiller.Result result = this.RewriteExpressionFreeTemps(lambda.Body, this._startingStack);
      this._lambdaRewrite = result.Action;
      if (result.Action == StackSpiller.RewriteAction.None)
        return lambda;
      Expression body = result.Node;
      if (this._tm.Temps.Count > 0)
        body = (Expression) Expression.Block((IEnumerable<ParameterExpression>) this._tm.Temps, new Expression[1]
        {
          body
        });
      return new Expression<T>(body, lambda.Name, lambda.TailCall, lambda.Parameters);
    }

    private StackSpiller.Result RewriteExpressionFreeTemps(Expression expression, StackSpiller.Stack stack)
    {
      int mark = this.Mark();
      StackSpiller.Result result = this.RewriteExpression(expression, stack);
      this.Free(mark);
      return result;
    }

    private static T[] Clone<T>(ReadOnlyCollection<T> original, int max)
    {
      T[] objArray = new T[original.Count];
      for (int index = 0; index < max; ++index)
        objArray[index] = original[index];
      return objArray;
    }

    internal static LambdaExpression AnalyzeLambda(LambdaExpression lambda)
    {
      return lambda.Accept(new StackSpiller(StackSpiller.Stack.Empty));
    }

    [Conditional("DEBUG")]
    private static void VerifyRewrite(StackSpiller.Result result, Expression node)
    {
    }

    private StackSpiller.Result RewriteDynamicExpression(Expression expr, StackSpiller.Stack stack)
    {
      DynamicExpression dynamicExpression = (DynamicExpression) expr;
      IArgumentProvider expressions = (IArgumentProvider) dynamicExpression;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, StackSpiller.Stack.NonEmpty, expressions.ArgumentCount);
      childRewriter.AddArguments(expressions);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNoRefArgs((MethodBase) dynamicExpression.DelegateType.GetMethod("Invoke"));
      return childRewriter.Finish(childRewriter.Rewrite ? (Expression) dynamicExpression.Rewrite(childRewriter[0, -1]) : expr);
    }

    private StackSpiller.Result RewriteIndexAssignment(BinaryExpression node, StackSpiller.Stack stack)
    {
      IndexExpression indexExpression = (IndexExpression) node.Left;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, 2 + indexExpression.Arguments.Count);
      childRewriter.Add(indexExpression.Object);
      childRewriter.Add((IList<Expression>) indexExpression.Arguments);
      childRewriter.Add(node.Right);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNotRefInstance(indexExpression.Object);
      if (childRewriter.Rewrite)
        node = (BinaryExpression) new AssignBinaryExpression((Expression) new IndexExpression(childRewriter[0], indexExpression.Indexer, (IList<Expression>) childRewriter[1, -2]), childRewriter[-1]);
      return childRewriter.Finish((Expression) node);
    }

    private StackSpiller.Result RewriteLogicalBinaryExpression(Expression expr, StackSpiller.Stack stack)
    {
      BinaryExpression binaryExpression = (BinaryExpression) expr;
      StackSpiller.Result result1 = this.RewriteExpression(binaryExpression.Left, stack);
      StackSpiller.Result result2 = this.RewriteExpression(binaryExpression.Right, stack);
      StackSpiller.Result result3 = this.RewriteExpression((Expression) binaryExpression.Conversion, stack);
      StackSpiller.RewriteAction action = result1.Action | result2.Action | result3.Action;
      if (action != StackSpiller.RewriteAction.None)
        expr = BinaryExpression.Create(binaryExpression.NodeType, result1.Node, result2.Node, binaryExpression.Type, binaryExpression.Method, (LambdaExpression) result3.Node);
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteReducibleExpression(Expression expr, StackSpiller.Stack stack)
    {
      StackSpiller.Result result = this.RewriteExpression(expr.Reduce(), stack);
      return new StackSpiller.Result(result.Action | StackSpiller.RewriteAction.Copy, result.Node);
    }

    private StackSpiller.Result RewriteBinaryExpression(Expression expr, StackSpiller.Stack stack)
    {
      BinaryExpression binaryExpression = (BinaryExpression) expr;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, 3);
      childRewriter.Add(binaryExpression.Left);
      childRewriter.Add(binaryExpression.Right);
      childRewriter.Add((Expression) binaryExpression.Conversion);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNoRefArgs((MethodBase) binaryExpression.Method);
      return childRewriter.Finish(childRewriter.Rewrite ? BinaryExpression.Create(binaryExpression.NodeType, childRewriter[0], childRewriter[1], binaryExpression.Type, binaryExpression.Method, (LambdaExpression) childRewriter[2]) : expr);
    }

    private StackSpiller.Result RewriteVariableAssignment(BinaryExpression node, StackSpiller.Stack stack)
    {
      StackSpiller.Result result = this.RewriteExpression(node.Right, stack);
      if (result.Action != StackSpiller.RewriteAction.None)
        node = Expression.Assign(node.Left, result.Node);
      return new StackSpiller.Result(result.Action, (Expression) node);
    }

    private StackSpiller.Result RewriteAssignBinaryExpression(Expression expr, StackSpiller.Stack stack)
    {
      BinaryExpression node = (BinaryExpression) expr;
      switch (node.Left.NodeType)
      {
        case ExpressionType.Extension:
          return this.RewriteExtensionAssignment(node, stack);
        case ExpressionType.Index:
          return this.RewriteIndexAssignment(node, stack);
        case ExpressionType.MemberAccess:
          return this.RewriteMemberAssignment(node, stack);
        case ExpressionType.Parameter:
          return this.RewriteVariableAssignment(node, stack);
        default:
          throw System.Linq.Expressions.Error.InvalidLvalue((object) node.Left.NodeType);
      }
    }

    private StackSpiller.Result RewriteExtensionAssignment(BinaryExpression node, StackSpiller.Stack stack)
    {
      node = Expression.Assign(node.Left.ReduceExtensions(), node.Right);
      StackSpiller.Result result = this.RewriteAssignBinaryExpression((Expression) node, stack);
      return new StackSpiller.Result(result.Action | StackSpiller.RewriteAction.Copy, result.Node);
    }

    private static StackSpiller.Result RewriteLambdaExpression(Expression expr, StackSpiller.Stack stack)
    {
      LambdaExpression lambda = (LambdaExpression) expr;
      expr = (Expression) StackSpiller.AnalyzeLambda(lambda);
      return new StackSpiller.Result(expr == lambda ? StackSpiller.RewriteAction.None : StackSpiller.RewriteAction.Copy, expr);
    }

    private StackSpiller.Result RewriteConditionalExpression(Expression expr, StackSpiller.Stack stack)
    {
      ConditionalExpression conditionalExpression = (ConditionalExpression) expr;
      StackSpiller.Result result1 = this.RewriteExpression(conditionalExpression.Test, stack);
      StackSpiller.Result result2 = this.RewriteExpression(conditionalExpression.IfTrue, stack);
      StackSpiller.Result result3 = this.RewriteExpression(conditionalExpression.IfFalse, stack);
      StackSpiller.RewriteAction action = result1.Action | result2.Action | result3.Action;
      if (action != StackSpiller.RewriteAction.None)
        expr = (Expression) Expression.Condition(result1.Node, result2.Node, result3.Node, conditionalExpression.Type);
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteMemberAssignment(BinaryExpression node, StackSpiller.Stack stack)
    {
      MemberExpression memberExpression = (MemberExpression) node.Left;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, 2);
      childRewriter.Add(memberExpression.Expression);
      childRewriter.Add(node.Right);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNotRefInstance(memberExpression.Expression);
      if (childRewriter.Rewrite)
        return childRewriter.Finish((Expression) new AssignBinaryExpression((Expression) MemberExpression.Make(childRewriter[0], memberExpression.Member), childRewriter[1]));
      else
        return new StackSpiller.Result(StackSpiller.RewriteAction.None, (Expression) node);
    }

    private StackSpiller.Result RewriteMemberExpression(Expression expr, StackSpiller.Stack stack)
    {
      MemberExpression memberExpression = (MemberExpression) expr;
      StackSpiller.Result result = this.RewriteExpression(memberExpression.Expression, stack);
      if (result.Action != StackSpiller.RewriteAction.None)
      {
        if (result.Action == StackSpiller.RewriteAction.SpillStack && memberExpression.Member.MemberType == MemberTypes.Property)
          StackSpiller.RequireNotRefInstance(memberExpression.Expression);
        expr = (Expression) MemberExpression.Make(result.Node, memberExpression.Member);
      }
      return new StackSpiller.Result(result.Action, expr);
    }

    private StackSpiller.Result RewriteIndexExpression(Expression expr, StackSpiller.Stack stack)
    {
      IndexExpression indexExpression = (IndexExpression) expr;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, indexExpression.Arguments.Count + 1);
      childRewriter.Add(indexExpression.Object);
      childRewriter.Add((IList<Expression>) indexExpression.Arguments);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNotRefInstance(indexExpression.Object);
      if (childRewriter.Rewrite)
        expr = (Expression) new IndexExpression(childRewriter[0], indexExpression.Indexer, (IList<Expression>) childRewriter[1, -1]);
      return childRewriter.Finish(expr);
    }

    private StackSpiller.Result RewriteMethodCallExpression(Expression expr, StackSpiller.Stack stack)
    {
      MethodCallExpression methodCallExpression = (MethodCallExpression) expr;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, methodCallExpression.Arguments.Count + 1);
      childRewriter.Add(methodCallExpression.Object);
      childRewriter.AddArguments((IArgumentProvider) methodCallExpression);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
      {
        StackSpiller.RequireNotRefInstance(methodCallExpression.Object);
        StackSpiller.RequireNoRefArgs((MethodBase) methodCallExpression.Method);
      }
      return childRewriter.Finish(childRewriter.Rewrite ? (Expression) methodCallExpression.Rewrite(childRewriter[0], (IList<Expression>) childRewriter[1, -1]) : expr);
    }

    private StackSpiller.Result RewriteNewArrayExpression(Expression expr, StackSpiller.Stack stack)
    {
      NewArrayExpression newArrayExpression = (NewArrayExpression) expr;
      if (newArrayExpression.NodeType == ExpressionType.NewArrayInit)
        stack = StackSpiller.Stack.NonEmpty;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, newArrayExpression.Expressions.Count);
      childRewriter.Add((IList<Expression>) newArrayExpression.Expressions);
      if (childRewriter.Rewrite)
      {
        Type elementType = newArrayExpression.Type.GetElementType();
        expr = newArrayExpression.NodeType != ExpressionType.NewArrayInit ? (Expression) Expression.NewArrayBounds(elementType, childRewriter[0, -1]) : (Expression) Expression.NewArrayInit(elementType, childRewriter[0, -1]);
      }
      return childRewriter.Finish(expr);
    }

    private StackSpiller.Result RewriteInvocationExpression(Expression expr, StackSpiller.Stack stack)
    {
      InvocationExpression invocationExpression = (InvocationExpression) expr;
      LambdaExpression lambdaOperand = invocationExpression.LambdaOperand;
      if (lambdaOperand != null)
      {
        StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, invocationExpression.Arguments.Count);
        childRewriter.Add((IList<Expression>) invocationExpression.Arguments);
        if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
          StackSpiller.RequireNoRefArgs((MethodBase) Expression.GetInvokeMethod(invocationExpression.Expression));
        StackSpiller spiller = new StackSpiller(stack);
        LambdaExpression lambdaExpression = lambdaOperand.Accept(spiller);
        if (childRewriter.Rewrite || spiller._lambdaRewrite != StackSpiller.RewriteAction.None)
          invocationExpression = new InvocationExpression((Expression) lambdaExpression, (IList<Expression>) childRewriter[0, -1], invocationExpression.Type);
        StackSpiller.Result result = childRewriter.Finish((Expression) invocationExpression);
        return new StackSpiller.Result(result.Action | spiller._lambdaRewrite, result.Node);
      }
      else
      {
        StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, invocationExpression.Arguments.Count + 1);
        childRewriter.Add(invocationExpression.Expression);
        childRewriter.Add((IList<Expression>) invocationExpression.Arguments);
        if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
          StackSpiller.RequireNoRefArgs((MethodBase) Expression.GetInvokeMethod(invocationExpression.Expression));
        return childRewriter.Finish(childRewriter.Rewrite ? (Expression) new InvocationExpression(childRewriter[0], (IList<Expression>) childRewriter[1, -1], invocationExpression.Type) : expr);
      }
    }

    private StackSpiller.Result RewriteNewExpression(Expression expr, StackSpiller.Stack stack)
    {
      NewExpression newExpression = (NewExpression) expr;
      StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, stack, newExpression.Arguments.Count);
      childRewriter.AddArguments((IArgumentProvider) newExpression);
      if (childRewriter.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNoRefArgs((MethodBase) newExpression.Constructor);
      return childRewriter.Finish(childRewriter.Rewrite ? (Expression) new NewExpression(newExpression.Constructor, (IList<Expression>) childRewriter[0, -1], newExpression.Members) : expr);
    }

    private StackSpiller.Result RewriteTypeBinaryExpression(Expression expr, StackSpiller.Stack stack)
    {
      TypeBinaryExpression binaryExpression = (TypeBinaryExpression) expr;
      StackSpiller.Result result = this.RewriteExpression(binaryExpression.Expression, stack);
      if (result.Action != StackSpiller.RewriteAction.None)
        expr = binaryExpression.NodeType != ExpressionType.TypeIs ? (Expression) Expression.TypeEqual(result.Node, binaryExpression.TypeOperand) : (Expression) Expression.TypeIs(result.Node, binaryExpression.TypeOperand);
      return new StackSpiller.Result(result.Action, expr);
    }

    private StackSpiller.Result RewriteThrowUnaryExpression(Expression expr, StackSpiller.Stack stack)
    {
      UnaryExpression unaryExpression = (UnaryExpression) expr;
      StackSpiller.Result result = this.RewriteExpressionFreeTemps(unaryExpression.Operand, StackSpiller.Stack.Empty);
      StackSpiller.RewriteAction action = result.Action;
      if (stack != StackSpiller.Stack.Empty)
        action = StackSpiller.RewriteAction.SpillStack;
      if (action != StackSpiller.RewriteAction.None)
        expr = (Expression) Expression.Throw(result.Node, unaryExpression.Type);
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteUnaryExpression(Expression expr, StackSpiller.Stack stack)
    {
      UnaryExpression unaryExpression = (UnaryExpression) expr;
      StackSpiller.Result result = this.RewriteExpression(unaryExpression.Operand, stack);
      if (result.Action == StackSpiller.RewriteAction.SpillStack)
        StackSpiller.RequireNoRefArgs((MethodBase) unaryExpression.Method);
      if (result.Action != StackSpiller.RewriteAction.None)
        expr = (Expression) new UnaryExpression(unaryExpression.NodeType, result.Node, unaryExpression.Type, unaryExpression.Method);
      return new StackSpiller.Result(result.Action, expr);
    }

    private StackSpiller.Result RewriteListInitExpression(Expression expr, StackSpiller.Stack stack)
    {
      ListInitExpression listInitExpression = (ListInitExpression) expr;
      StackSpiller.Result result1 = this.RewriteExpression((Expression) listInitExpression.NewExpression, stack);
      Expression right = result1.Node;
      StackSpiller.RewriteAction action = result1.Action;
      ReadOnlyCollection<ElementInit> initializers = listInitExpression.Initializers;
      StackSpiller.ChildRewriter[] childRewriterArray = new StackSpiller.ChildRewriter[initializers.Count];
      for (int index = 0; index < initializers.Count; ++index)
      {
        ElementInit elementInit = initializers[index];
        StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(this, StackSpiller.Stack.NonEmpty, elementInit.Arguments.Count);
        childRewriter.Add((IList<Expression>) elementInit.Arguments);
        action |= childRewriter.Action;
        childRewriterArray[index] = childRewriter;
      }
      switch (action)
      {
        case StackSpiller.RewriteAction.None:
          return new StackSpiller.Result(action, expr);
        case StackSpiller.RewriteAction.Copy:
          ElementInit[] list = new ElementInit[initializers.Count];
          for (int index = 0; index < initializers.Count; ++index)
          {
            StackSpiller.ChildRewriter childRewriter = childRewriterArray[index];
            list[index] = childRewriter.Action != StackSpiller.RewriteAction.None ? Expression.ElementInit(initializers[index].AddMethod, childRewriter[0, -1]) : initializers[index];
          }
          expr = (Expression) Expression.ListInit((NewExpression) right, (IEnumerable<ElementInit>) new TrueReadOnlyCollection<ElementInit>(list));
          goto case 0;
        case StackSpiller.RewriteAction.SpillStack:
          StackSpiller.RequireNotRefInstance((Expression) listInitExpression.NewExpression);
          ParameterExpression parameterExpression = this.MakeTemp(right.Type);
          Expression[] expressionArray = new Expression[initializers.Count + 2];
          expressionArray[0] = (Expression) Expression.Assign((Expression) parameterExpression, right);
          for (int index = 0; index < initializers.Count; ++index)
          {
            StackSpiller.ChildRewriter childRewriter = childRewriterArray[index];
            StackSpiller.Result result2 = childRewriter.Finish((Expression) Expression.Call((Expression) parameterExpression, initializers[index].AddMethod, childRewriter[0, -1]));
            expressionArray[index + 1] = result2.Node;
          }
          expressionArray[initializers.Count + 1] = (Expression) parameterExpression;
          expr = StackSpiller.MakeBlock(expressionArray);
          goto case 0;
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private StackSpiller.Result RewriteMemberInitExpression(Expression expr, StackSpiller.Stack stack)
    {
      MemberInitExpression memberInitExpression = (MemberInitExpression) expr;
      StackSpiller.Result result = this.RewriteExpression((Expression) memberInitExpression.NewExpression, stack);
      Expression right = result.Node;
      StackSpiller.RewriteAction action = result.Action;
      ReadOnlyCollection<MemberBinding> bindings = memberInitExpression.Bindings;
      StackSpiller.BindingRewriter[] bindingRewriterArray = new StackSpiller.BindingRewriter[bindings.Count];
      for (int index = 0; index < bindings.Count; ++index)
      {
        StackSpiller.BindingRewriter bindingRewriter = StackSpiller.BindingRewriter.Create(bindings[index], this, StackSpiller.Stack.NonEmpty);
        bindingRewriterArray[index] = bindingRewriter;
        action |= bindingRewriter.Action;
      }
      switch (action)
      {
        case StackSpiller.RewriteAction.None:
          return new StackSpiller.Result(action, expr);
        case StackSpiller.RewriteAction.Copy:
          MemberBinding[] list = new MemberBinding[bindings.Count];
          for (int index = 0; index < bindings.Count; ++index)
            list[index] = bindingRewriterArray[index].AsBinding();
          expr = (Expression) Expression.MemberInit((NewExpression) right, (IEnumerable<MemberBinding>) new TrueReadOnlyCollection<MemberBinding>(list));
          goto case 0;
        case StackSpiller.RewriteAction.SpillStack:
          StackSpiller.RequireNotRefInstance((Expression) memberInitExpression.NewExpression);
          ParameterExpression parameterExpression = this.MakeTemp(right.Type);
          Expression[] expressionArray = new Expression[bindings.Count + 2];
          expressionArray[0] = (Expression) Expression.Assign((Expression) parameterExpression, right);
          for (int index = 0; index < bindings.Count; ++index)
          {
            Expression expression = bindingRewriterArray[index].AsExpression((Expression) parameterExpression);
            expressionArray[index + 1] = expression;
          }
          expressionArray[bindings.Count + 1] = (Expression) parameterExpression;
          expr = StackSpiller.MakeBlock(expressionArray);
          goto case 0;
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private StackSpiller.Result RewriteBlockExpression(Expression expr, StackSpiller.Stack stack)
    {
      BlockExpression blockExpression = (BlockExpression) expr;
      int expressionCount = blockExpression.ExpressionCount;
      StackSpiller.RewriteAction action = StackSpiller.RewriteAction.None;
      Expression[] args = (Expression[]) null;
      for (int index = 0; index < expressionCount; ++index)
      {
        StackSpiller.Result result = this.RewriteExpression(blockExpression.GetExpression(index), stack);
        action |= result.Action;
        if (args == null && result.Action != StackSpiller.RewriteAction.None)
          args = StackSpiller.Clone<Expression>(blockExpression.Expressions, index);
        if (args != null)
          args[index] = result.Node;
      }
      if (action != StackSpiller.RewriteAction.None)
        expr = (Expression) blockExpression.Rewrite((ReadOnlyCollection<ParameterExpression>) null, args);
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteLabelExpression(Expression expr, StackSpiller.Stack stack)
    {
      LabelExpression labelExpression = (LabelExpression) expr;
      StackSpiller.Result result = this.RewriteExpression(labelExpression.DefaultValue, stack);
      if (result.Action != StackSpiller.RewriteAction.None)
        expr = (Expression) Expression.Label(labelExpression.Target, result.Node);
      return new StackSpiller.Result(result.Action, expr);
    }

    private StackSpiller.Result RewriteLoopExpression(Expression expr, StackSpiller.Stack stack)
    {
      LoopExpression loopExpression = (LoopExpression) expr;
      StackSpiller.Result result = this.RewriteExpression(loopExpression.Body, StackSpiller.Stack.Empty);
      StackSpiller.RewriteAction action = result.Action;
      if (stack != StackSpiller.Stack.Empty)
        action = StackSpiller.RewriteAction.SpillStack;
      if (action != StackSpiller.RewriteAction.None)
        expr = (Expression) new LoopExpression(result.Node, loopExpression.BreakLabel, loopExpression.ContinueLabel);
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteGotoExpression(Expression expr, StackSpiller.Stack stack)
    {
      GotoExpression gotoExpression = (GotoExpression) expr;
      StackSpiller.Result result = this.RewriteExpressionFreeTemps(gotoExpression.Value, StackSpiller.Stack.Empty);
      StackSpiller.RewriteAction action = result.Action;
      if (stack != StackSpiller.Stack.Empty)
        action = StackSpiller.RewriteAction.SpillStack;
      if (action != StackSpiller.RewriteAction.None)
        expr = (Expression) Expression.MakeGoto(gotoExpression.Kind, gotoExpression.Target, result.Node, gotoExpression.Type);
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteSwitchExpression(Expression expr, StackSpiller.Stack stack)
    {
      SwitchExpression switchExpression = (SwitchExpression) expr;
      StackSpiller.Result result1 = this.RewriteExpressionFreeTemps(switchExpression.SwitchValue, stack);
      StackSpiller.RewriteAction rewriteAction = result1.Action;
      ReadOnlyCollection<SwitchCase> readOnlyCollection1 = switchExpression.Cases;
      SwitchCase[] switchCaseArray = (SwitchCase[]) null;
      for (int max1 = 0; max1 < readOnlyCollection1.Count; ++max1)
      {
        SwitchCase switchCase = readOnlyCollection1[max1];
        Expression[] expressionArray = (Expression[]) null;
        ReadOnlyCollection<Expression> readOnlyCollection2 = switchCase.TestValues;
        for (int max2 = 0; max2 < readOnlyCollection2.Count; ++max2)
        {
          StackSpiller.Result result2 = this.RewriteExpression(readOnlyCollection2[max2], stack);
          rewriteAction |= result2.Action;
          if (expressionArray == null && result2.Action != StackSpiller.RewriteAction.None)
            expressionArray = StackSpiller.Clone<Expression>(readOnlyCollection2, max2);
          if (expressionArray != null)
            expressionArray[max2] = result2.Node;
        }
        StackSpiller.Result result3 = this.RewriteExpression(switchCase.Body, stack);
        rewriteAction |= result3.Action;
        if (result3.Action != StackSpiller.RewriteAction.None || expressionArray != null)
        {
          if (expressionArray != null)
            readOnlyCollection2 = new ReadOnlyCollection<Expression>((IList<Expression>) expressionArray);
          switchCase = new SwitchCase(result3.Node, readOnlyCollection2);
          if (switchCaseArray == null)
            switchCaseArray = StackSpiller.Clone<SwitchCase>(readOnlyCollection1, max1);
        }
        if (switchCaseArray != null)
          switchCaseArray[max1] = switchCase;
      }
      StackSpiller.Result result4 = this.RewriteExpression(switchExpression.DefaultBody, stack);
      StackSpiller.RewriteAction action = rewriteAction | result4.Action;
      if (action != StackSpiller.RewriteAction.None)
      {
        if (switchCaseArray != null)
          readOnlyCollection1 = new ReadOnlyCollection<SwitchCase>((IList<SwitchCase>) switchCaseArray);
        expr = (Expression) new SwitchExpression(switchExpression.Type, result1.Node, result4.Node, switchExpression.Comparison, readOnlyCollection1);
      }
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteTryExpression(Expression expr, StackSpiller.Stack stack)
    {
      TryExpression tryExpression = (TryExpression) expr;
      StackSpiller.Result result1 = this.RewriteExpression(tryExpression.Body, StackSpiller.Stack.Empty);
      ReadOnlyCollection<CatchBlock> readOnlyCollection = tryExpression.Handlers;
      CatchBlock[] catchBlockArray = (CatchBlock[]) null;
      StackSpiller.RewriteAction rewriteAction1 = result1.Action;
      if (readOnlyCollection != null)
      {
        for (int max = 0; max < readOnlyCollection.Count; ++max)
        {
          StackSpiller.RewriteAction rewriteAction2 = result1.Action;
          CatchBlock catchBlock = readOnlyCollection[max];
          Expression filter = catchBlock.Filter;
          if (catchBlock.Filter != null)
          {
            StackSpiller.Result result2 = this.RewriteExpression(catchBlock.Filter, StackSpiller.Stack.Empty);
            rewriteAction1 |= result2.Action;
            rewriteAction2 |= result2.Action;
            filter = result2.Node;
          }
          StackSpiller.Result result3 = this.RewriteExpression(catchBlock.Body, StackSpiller.Stack.Empty);
          rewriteAction1 |= result3.Action;
          if ((rewriteAction2 | result3.Action) != StackSpiller.RewriteAction.None)
          {
            catchBlock = Expression.MakeCatchBlock(catchBlock.Test, catchBlock.Variable, result3.Node, filter);
            if (catchBlockArray == null)
              catchBlockArray = StackSpiller.Clone<CatchBlock>(readOnlyCollection, max);
          }
          if (catchBlockArray != null)
            catchBlockArray[max] = catchBlock;
        }
      }
      StackSpiller.Result result4 = this.RewriteExpression(tryExpression.Fault, StackSpiller.Stack.Empty);
      StackSpiller.RewriteAction rewriteAction3 = rewriteAction1 | result4.Action;
      StackSpiller.Result result5 = this.RewriteExpression(tryExpression.Finally, StackSpiller.Stack.Empty);
      StackSpiller.RewriteAction action = rewriteAction3 | result5.Action;
      if (stack != StackSpiller.Stack.Empty)
        action = StackSpiller.RewriteAction.SpillStack;
      if (action != StackSpiller.RewriteAction.None)
      {
        if (catchBlockArray != null)
          readOnlyCollection = new ReadOnlyCollection<CatchBlock>((IList<CatchBlock>) catchBlockArray);
        expr = (Expression) new TryExpression(tryExpression.Type, result1.Node, result5.Node, result4.Node, readOnlyCollection);
      }
      return new StackSpiller.Result(action, expr);
    }

    private StackSpiller.Result RewriteExtensionExpression(Expression expr, StackSpiller.Stack stack)
    {
      StackSpiller.Result result = this.RewriteExpression(expr.ReduceExtensions(), stack);
      return new StackSpiller.Result(result.Action | StackSpiller.RewriteAction.Copy, result.Node);
    }

    private static void RequireNoRefArgs(MethodBase method)
    {
      if (method != (MethodBase) null && Enumerable.Any<ParameterInfo>((IEnumerable<ParameterInfo>) TypeExtensions.GetParametersCached(method), (Func<ParameterInfo, bool>) (p => p.ParameterType.IsByRef)))
        throw System.Linq.Expressions.Error.TryNotSupportedForMethodsWithRefArgs((object) method);
    }

    private static void RequireNotRefInstance(Expression instance)
    {
      if (instance != null && instance.Type.IsValueType && Type.GetTypeCode(instance.Type) == TypeCode.Object)
        throw System.Linq.Expressions.Error.TryNotSupportedForValueTypeInstances((object) instance.Type);
    }

    private StackSpiller.Result RewriteExpression(Expression node, StackSpiller.Stack stack)
    {
      if (node == null)
        return new StackSpiller.Result(StackSpiller.RewriteAction.None, (Expression) null);
      switch (node.NodeType)
      {
        case ExpressionType.Add:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.AddChecked:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.And:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.AndAlso:
          return this.RewriteLogicalBinaryExpression(node, stack);
        case ExpressionType.ArrayLength:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.ArrayIndex:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Call:
          return this.RewriteMethodCallExpression(node, stack);
        case ExpressionType.Coalesce:
          return this.RewriteLogicalBinaryExpression(node, stack);
        case ExpressionType.Conditional:
          return this.RewriteConditionalExpression(node, stack);
        case ExpressionType.Constant:
        case ExpressionType.Parameter:
        case ExpressionType.Quote:
        case ExpressionType.DebugInfo:
        case ExpressionType.Default:
        case ExpressionType.RuntimeVariables:
          return new StackSpiller.Result(StackSpiller.RewriteAction.None, node);
        case ExpressionType.Convert:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.ConvertChecked:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.Divide:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Equal:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.ExclusiveOr:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.GreaterThan:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.GreaterThanOrEqual:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Invoke:
          return this.RewriteInvocationExpression(node, stack);
        case ExpressionType.Lambda:
          return StackSpiller.RewriteLambdaExpression(node, stack);
        case ExpressionType.LeftShift:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.LessThan:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.LessThanOrEqual:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.ListInit:
          return this.RewriteListInitExpression(node, stack);
        case ExpressionType.MemberAccess:
          return this.RewriteMemberExpression(node, stack);
        case ExpressionType.MemberInit:
          return this.RewriteMemberInitExpression(node, stack);
        case ExpressionType.Modulo:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Multiply:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.MultiplyChecked:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Negate:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.UnaryPlus:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.NegateChecked:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.New:
          return this.RewriteNewExpression(node, stack);
        case ExpressionType.NewArrayInit:
          return this.RewriteNewArrayExpression(node, stack);
        case ExpressionType.NewArrayBounds:
          return this.RewriteNewArrayExpression(node, stack);
        case ExpressionType.Not:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.NotEqual:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Or:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.OrElse:
          return this.RewriteLogicalBinaryExpression(node, stack);
        case ExpressionType.Power:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.RightShift:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.Subtract:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.SubtractChecked:
          return this.RewriteBinaryExpression(node, stack);
        case ExpressionType.TypeAs:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.TypeIs:
          return this.RewriteTypeBinaryExpression(node, stack);
        case ExpressionType.Assign:
          return this.RewriteAssignBinaryExpression(node, stack);
        case ExpressionType.Block:
          return this.RewriteBlockExpression(node, stack);
        case ExpressionType.Decrement:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.Dynamic:
          return this.RewriteDynamicExpression(node, stack);
        case ExpressionType.Extension:
          return this.RewriteExtensionExpression(node, stack);
        case ExpressionType.Goto:
          return this.RewriteGotoExpression(node, stack);
        case ExpressionType.Increment:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.Index:
          return this.RewriteIndexExpression(node, stack);
        case ExpressionType.Label:
          return this.RewriteLabelExpression(node, stack);
        case ExpressionType.Loop:
          return this.RewriteLoopExpression(node, stack);
        case ExpressionType.Switch:
          return this.RewriteSwitchExpression(node, stack);
        case ExpressionType.Throw:
          return this.RewriteThrowUnaryExpression(node, stack);
        case ExpressionType.Try:
          return this.RewriteTryExpression(node, stack);
        case ExpressionType.Unbox:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.AddAssign:
        case ExpressionType.AndAssign:
        case ExpressionType.DivideAssign:
        case ExpressionType.ExclusiveOrAssign:
        case ExpressionType.LeftShiftAssign:
        case ExpressionType.ModuloAssign:
        case ExpressionType.MultiplyAssign:
        case ExpressionType.OrAssign:
        case ExpressionType.PowerAssign:
        case ExpressionType.RightShiftAssign:
        case ExpressionType.SubtractAssign:
        case ExpressionType.AddAssignChecked:
        case ExpressionType.MultiplyAssignChecked:
        case ExpressionType.SubtractAssignChecked:
        case ExpressionType.PreIncrementAssign:
        case ExpressionType.PreDecrementAssign:
        case ExpressionType.PostIncrementAssign:
        case ExpressionType.PostDecrementAssign:
          return this.RewriteReducibleExpression(node, stack);
        case ExpressionType.TypeEqual:
          return this.RewriteTypeBinaryExpression(node, stack);
        case ExpressionType.OnesComplement:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.IsTrue:
          return this.RewriteUnaryExpression(node, stack);
        case ExpressionType.IsFalse:
          return this.RewriteUnaryExpression(node, stack);
        default:
          throw ContractUtils.Unreachable;
      }
    }

    private ParameterExpression MakeTemp(Type type)
    {
      return this._tm.Temp(type);
    }

    private int Mark()
    {
      return this._tm.Mark();
    }

    private void Free(int mark)
    {
      this._tm.Free(mark);
    }

    [Conditional("DEBUG")]
    private void VerifyTemps()
    {
    }

    private ParameterExpression ToTemp(Expression expression, out Expression save)
    {
      ParameterExpression parameterExpression = this.MakeTemp(expression.Type);
      save = (Expression) Expression.Assign((Expression) parameterExpression, expression);
      return parameterExpression;
    }

    private static Expression MakeBlock(params Expression[] expressions)
    {
      return StackSpiller.MakeBlock((IList<Expression>) expressions);
    }

    private static Expression MakeBlock(IList<Expression> expressions)
    {
      return (Expression) new SpilledExpressionBlock(expressions);
    }

    private enum Stack
    {
      Empty,
      NonEmpty,
    }

    [Flags]
    private enum RewriteAction
    {
      None = 0,
      Copy = 1,
      SpillStack = 3,
    }

    private struct Result
    {
      internal readonly StackSpiller.RewriteAction Action;
      internal readonly Expression Node;

      internal Result(StackSpiller.RewriteAction action, Expression node)
      {
        this.Action = action;
        this.Node = node;
      }
    }

    private class TempMaker
    {
      private List<ParameterExpression> _temps = new List<ParameterExpression>();
      private int _temp;
      private List<ParameterExpression> _freeTemps;
      private Stack<ParameterExpression> _usedTemps;

      internal List<ParameterExpression> Temps
      {
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
        {
          return this._temps;
        }
      }

      internal ParameterExpression Temp(Type type)
      {
        if (this._freeTemps != null)
        {
          for (int index = this._freeTemps.Count - 1; index >= 0; --index)
          {
            ParameterExpression temp = this._freeTemps[index];
            if (temp.Type == type)
            {
              this._freeTemps.RemoveAt(index);
              return this.UseTemp(temp);
            }
          }
        }
        ParameterExpression temp1 = Expression.Variable(type, "$temp$" + (object) this._temp++);
        this._temps.Add(temp1);
        return this.UseTemp(temp1);
      }

      private ParameterExpression UseTemp(ParameterExpression temp)
      {
        if (this._usedTemps == null)
          this._usedTemps = new Stack<ParameterExpression>();
        this._usedTemps.Push(temp);
        return temp;
      }

      private void FreeTemp(ParameterExpression temp)
      {
        if (this._freeTemps == null)
          this._freeTemps = new List<ParameterExpression>();
        this._freeTemps.Add(temp);
      }

      internal int Mark()
      {
        if (this._usedTemps == null)
          return 0;
        else
          return this._usedTemps.Count;
      }

      internal void Free(int mark)
      {
        if (this._usedTemps == null)
          return;
        while (mark < this._usedTemps.Count)
          this.FreeTemp(this._usedTemps.Pop());
      }

      [Conditional("DEBUG")]
      internal void VerifyTemps()
      {
      }
    }

    private abstract class BindingRewriter
    {
      protected MemberBinding _binding;
      protected StackSpiller.RewriteAction _action;
      protected StackSpiller _spiller;

      internal StackSpiller.RewriteAction Action
      {
        get
        {
          return this._action;
        }
      }

      internal BindingRewriter(MemberBinding binding, StackSpiller spiller)
      {
        this._binding = binding;
        this._spiller = spiller;
      }

      internal abstract MemberBinding AsBinding();

      internal abstract Expression AsExpression(Expression target);

      internal static StackSpiller.BindingRewriter Create(MemberBinding binding, StackSpiller spiller, StackSpiller.Stack stack)
      {
        switch (binding.BindingType)
        {
          case MemberBindingType.Assignment:
            return (StackSpiller.BindingRewriter) new StackSpiller.MemberAssignmentRewriter((MemberAssignment) binding, spiller, stack);
          case MemberBindingType.MemberBinding:
            return (StackSpiller.BindingRewriter) new StackSpiller.MemberMemberBindingRewriter((MemberMemberBinding) binding, spiller, stack);
          case MemberBindingType.ListBinding:
            return (StackSpiller.BindingRewriter) new StackSpiller.ListBindingRewriter((MemberListBinding) binding, spiller, stack);
          default:
            throw System.Linq.Expressions.Error.UnhandledBinding();
        }
      }
    }

    private class MemberMemberBindingRewriter : StackSpiller.BindingRewriter
    {
      private ReadOnlyCollection<MemberBinding> _bindings;
      private StackSpiller.BindingRewriter[] _bindingRewriters;

      internal MemberMemberBindingRewriter(MemberMemberBinding binding, StackSpiller spiller, StackSpiller.Stack stack)
        : base((MemberBinding) binding, spiller)
      {
        this._bindings = binding.Bindings;
        this._bindingRewriters = new StackSpiller.BindingRewriter[this._bindings.Count];
        for (int index = 0; index < this._bindings.Count; ++index)
        {
          StackSpiller.BindingRewriter bindingRewriter = StackSpiller.BindingRewriter.Create(this._bindings[index], spiller, stack);
          StackSpiller.MemberMemberBindingRewriter memberBindingRewriter = this;
          int num = (int) (memberBindingRewriter._action | bindingRewriter.Action);
          memberBindingRewriter._action = (StackSpiller.RewriteAction) num;
          this._bindingRewriters[index] = bindingRewriter;
        }
      }

      internal override MemberBinding AsBinding()
      {
        switch (this._action)
        {
          case StackSpiller.RewriteAction.None:
            return this._binding;
          case StackSpiller.RewriteAction.Copy:
            MemberBinding[] list = new MemberBinding[this._bindings.Count];
            for (int index = 0; index < this._bindings.Count; ++index)
              list[index] = this._bindingRewriters[index].AsBinding();
            return (MemberBinding) Expression.MemberBind(this._binding.Member, (IEnumerable<MemberBinding>) new TrueReadOnlyCollection<MemberBinding>(list));
          default:
            throw ContractUtils.Unreachable;
        }
      }

      internal override Expression AsExpression(Expression target)
      {
        if (target.Type.IsValueType && this._binding.Member is PropertyInfo)
          throw System.Linq.Expressions.Error.CannotAutoInitializeValueTypeMemberThroughProperty((object) this._binding.Member);
        StackSpiller.RequireNotRefInstance(target);
        MemberExpression memberExpression = Expression.MakeMemberAccess(target, this._binding.Member);
        ParameterExpression parameterExpression = this._spiller.MakeTemp(memberExpression.Type);
        Expression[] expressionArray = new Expression[this._bindings.Count + 2];
        expressionArray[0] = (Expression) Expression.Assign((Expression) parameterExpression, (Expression) memberExpression);
        for (int index = 0; index < this._bindings.Count; ++index)
        {
          StackSpiller.BindingRewriter bindingRewriter = this._bindingRewriters[index];
          expressionArray[index + 1] = bindingRewriter.AsExpression((Expression) parameterExpression);
        }
        if (parameterExpression.Type.IsValueType)
          expressionArray[this._bindings.Count + 1] = (Expression) Expression.Block(typeof (void), new Expression[1]
          {
            (Expression) Expression.Assign((Expression) Expression.MakeMemberAccess(target, this._binding.Member), (Expression) parameterExpression)
          });
        else
          expressionArray[this._bindings.Count + 1] = (Expression) Expression.Empty();
        return StackSpiller.MakeBlock(expressionArray);
      }
    }

    private class ListBindingRewriter : StackSpiller.BindingRewriter
    {
      private ReadOnlyCollection<ElementInit> _inits;
      private StackSpiller.ChildRewriter[] _childRewriters;

      internal ListBindingRewriter(MemberListBinding binding, StackSpiller spiller, StackSpiller.Stack stack)
        : base((MemberBinding) binding, spiller)
      {
        this._inits = binding.Initializers;
        this._childRewriters = new StackSpiller.ChildRewriter[this._inits.Count];
        for (int index = 0; index < this._inits.Count; ++index)
        {
          ElementInit elementInit = this._inits[index];
          StackSpiller.ChildRewriter childRewriter = new StackSpiller.ChildRewriter(spiller, stack, elementInit.Arguments.Count);
          childRewriter.Add((IList<Expression>) elementInit.Arguments);
          StackSpiller.ListBindingRewriter listBindingRewriter = this;
          int num = (int) (listBindingRewriter._action | childRewriter.Action);
          listBindingRewriter._action = (StackSpiller.RewriteAction) num;
          this._childRewriters[index] = childRewriter;
        }
      }

      internal override MemberBinding AsBinding()
      {
        switch (this._action)
        {
          case StackSpiller.RewriteAction.None:
            return this._binding;
          case StackSpiller.RewriteAction.Copy:
            ElementInit[] list = new ElementInit[this._inits.Count];
            for (int index = 0; index < this._inits.Count; ++index)
            {
              StackSpiller.ChildRewriter childRewriter = this._childRewriters[index];
              list[index] = childRewriter.Action != StackSpiller.RewriteAction.None ? Expression.ElementInit(this._inits[index].AddMethod, childRewriter[0, -1]) : this._inits[index];
            }
            return (MemberBinding) Expression.ListBind(this._binding.Member, (IEnumerable<ElementInit>) new TrueReadOnlyCollection<ElementInit>(list));
          default:
            throw ContractUtils.Unreachable;
        }
      }

      internal override Expression AsExpression(Expression target)
      {
        if (target.Type.IsValueType && this._binding.Member is PropertyInfo)
          throw System.Linq.Expressions.Error.CannotAutoInitializeValueTypeElementThroughProperty((object) this._binding.Member);
        StackSpiller.RequireNotRefInstance(target);
        MemberExpression memberExpression = Expression.MakeMemberAccess(target, this._binding.Member);
        ParameterExpression parameterExpression = this._spiller.MakeTemp(memberExpression.Type);
        Expression[] expressionArray = new Expression[this._inits.Count + 2];
        expressionArray[0] = (Expression) Expression.Assign((Expression) parameterExpression, (Expression) memberExpression);
        for (int index = 0; index < this._inits.Count; ++index)
        {
          StackSpiller.ChildRewriter childRewriter = this._childRewriters[index];
          StackSpiller.Result result = childRewriter.Finish((Expression) Expression.Call((Expression) parameterExpression, this._inits[index].AddMethod, childRewriter[0, -1]));
          expressionArray[index + 1] = result.Node;
        }
        if (parameterExpression.Type.IsValueType)
          expressionArray[this._inits.Count + 1] = (Expression) Expression.Block(typeof (void), new Expression[1]
          {
            (Expression) Expression.Assign((Expression) Expression.MakeMemberAccess(target, this._binding.Member), (Expression) parameterExpression)
          });
        else
          expressionArray[this._inits.Count + 1] = (Expression) Expression.Empty();
        return StackSpiller.MakeBlock(expressionArray);
      }
    }

    private class MemberAssignmentRewriter : StackSpiller.BindingRewriter
    {
      private Expression _rhs;

      internal MemberAssignmentRewriter(MemberAssignment binding, StackSpiller spiller, StackSpiller.Stack stack)
        : base((MemberBinding) binding, spiller)
      {
        StackSpiller.Result result = spiller.RewriteExpression(binding.Expression, stack);
        this._action = result.Action;
        this._rhs = result.Node;
      }

      internal override MemberBinding AsBinding()
      {
        switch (this._action)
        {
          case StackSpiller.RewriteAction.None:
            return this._binding;
          case StackSpiller.RewriteAction.Copy:
            return (MemberBinding) Expression.Bind(this._binding.Member, this._rhs);
          default:
            throw ContractUtils.Unreachable;
        }
      }

      internal override Expression AsExpression(Expression target)
      {
        StackSpiller.RequireNotRefInstance(target);
        MemberExpression memberExpression = Expression.MakeMemberAccess(target, this._binding.Member);
        ParameterExpression parameterExpression = this._spiller.MakeTemp(memberExpression.Type);
        return StackSpiller.MakeBlock((Expression) Expression.Assign((Expression) parameterExpression, this._rhs), (Expression) Expression.Assign((Expression) memberExpression, (Expression) parameterExpression), (Expression) Expression.Empty());
      }
    }

    private class ChildRewriter
    {
      private readonly StackSpiller _self;
      private readonly Expression[] _expressions;
      private int _expressionsCount;
      private List<Expression> _comma;
      private StackSpiller.RewriteAction _action;
      private StackSpiller.Stack _stack;
      private bool _done;

      internal bool Rewrite
      {
        get
        {
          return this._action != StackSpiller.RewriteAction.None;
        }
      }

      internal StackSpiller.RewriteAction Action
      {
        get
        {
          return this._action;
        }
      }

      internal Expression this[int index]
      {
        get
        {
          this.EnsureDone();
          if (index < 0)
            index += this._expressions.Length;
          return this._expressions[index];
        }
      }

      internal Expression[] this[int first, int last]
      {
        get
        {
          this.EnsureDone();
          if (last < 0)
            last += this._expressions.Length;
          int length = last - first + 1;
          ContractUtils.RequiresArrayRange<Expression>((IList<Expression>) this._expressions, first, length, "first", "last");
          if (length == this._expressions.Length)
            return this._expressions;
          Expression[] expressionArray = new Expression[length];
          Array.Copy((Array) this._expressions, first, (Array) expressionArray, 0, length);
          return expressionArray;
        }
      }

      internal ChildRewriter(StackSpiller self, StackSpiller.Stack stack, int count)
      {
        this._self = self;
        this._stack = stack;
        this._expressions = new Expression[count];
      }

      internal void Add(Expression node)
      {
        if (node == null)
        {
          this._expressions[this._expressionsCount++] = (Expression) null;
        }
        else
        {
          StackSpiller.Result result = this._self.RewriteExpression(node, this._stack);
          this._action |= result.Action;
          this._stack = StackSpiller.Stack.NonEmpty;
          this._expressions[this._expressionsCount++] = result.Node;
        }
      }

      internal void Add(IList<Expression> expressions)
      {
        int index = 0;
        for (int count = expressions.Count; index < count; ++index)
          this.Add(expressions[index]);
      }

      internal void AddArguments(IArgumentProvider expressions)
      {
        int index = 0;
        for (int argumentCount = expressions.ArgumentCount; index < argumentCount; ++index)
          this.Add(expressions.GetArgument(index));
      }

      private void EnsureDone()
      {
        if (this._done)
          return;
        this._done = true;
        if (this._action != StackSpiller.RewriteAction.SpillStack)
          return;
        Expression[] expressionArray = this._expressions;
        int length = expressionArray.Length;
        List<Expression> list = new List<Expression>(length + 1);
        for (int index = 0; index < length; ++index)
        {
          if (expressionArray[index] != null)
          {
            Expression save;
            expressionArray[index] = (Expression) this._self.ToTemp(expressionArray[index], out save);
            list.Add(save);
          }
        }
        list.Capacity = list.Count + 1;
        this._comma = list;
      }

      internal StackSpiller.Result Finish(Expression expr)
      {
        this.EnsureDone();
        if (this._action == StackSpiller.RewriteAction.SpillStack)
        {
          this._comma.Add(expr);
          expr = StackSpiller.MakeBlock((IList<Expression>) this._comma);
        }
        return new StackSpiller.Result(this._action, expr);
      }
    }
  }
}
