// Type: System.Linq.Expressions.Compiler.VariableBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class VariableBinder : ExpressionVisitor
  {
    private readonly AnalyzedTree _tree = new AnalyzedTree();
    private readonly Stack<CompilerScope> _scopes = new Stack<CompilerScope>();
    private readonly Stack<BoundConstants> _constants = new Stack<BoundConstants>();
    private bool _inQuote;

    private string CurrentLambdaName
    {
      get
      {
        foreach (CompilerScope compilerScope in this._scopes)
        {
          LambdaExpression lambdaExpression = compilerScope.Node as LambdaExpression;
          if (lambdaExpression != null)
            return lambdaExpression.Name;
        }
        throw ContractUtils.Unreachable;
      }
    }

    private VariableBinder()
    {
    }

    internal static AnalyzedTree Bind(LambdaExpression lambda)
    {
      VariableBinder variableBinder = new VariableBinder();
      variableBinder.Visit((Expression) lambda);
      return variableBinder._tree;
    }

    protected internal override Expression VisitConstant(ConstantExpression node)
    {
      if (this._inQuote)
        return (Expression) node;
      if (ILGen.CanEmitConstant(node.Value, node.Type))
        return (Expression) node;
      this._constants.Peek().AddReference(node.Value, node.Type);
      return (Expression) node;
    }

    protected internal override Expression VisitUnary(UnaryExpression node)
    {
      if (node.NodeType == ExpressionType.Quote)
      {
        bool flag = this._inQuote;
        this._inQuote = true;
        this.Visit(node.Operand);
        this._inQuote = flag;
      }
      else
        this.Visit(node.Operand);
      return (Expression) node;
    }

    protected internal override Expression VisitLambda<T>(Expression<T> node)
    {
      this._scopes.Push(this._tree.Scopes[(object) node] = new CompilerScope((object) node, true));
      this._constants.Push(this._tree.Constants[(LambdaExpression) node] = new BoundConstants());
      this.Visit(this.MergeScopes((Expression) node));
      this._constants.Pop();
      this._scopes.Pop();
      return (Expression) node;
    }

    protected internal override Expression VisitInvocation(InvocationExpression node)
    {
      LambdaExpression lambdaOperand = node.LambdaOperand;
      if (lambdaOperand == null)
        return base.VisitInvocation(node);
      this._scopes.Push(this._tree.Scopes[(object) lambdaOperand] = new CompilerScope((object) lambdaOperand, false));
      this.Visit(this.MergeScopes((Expression) lambdaOperand));
      this._scopes.Pop();
      this.Visit(node.Arguments);
      return (Expression) node;
    }

    protected internal override Expression VisitBlock(BlockExpression node)
    {
      if (node.Variables.Count == 0)
      {
        this.Visit(node.Expressions);
        return (Expression) node;
      }
      else
      {
        this._scopes.Push(this._tree.Scopes[(object) node] = new CompilerScope((object) node, false));
        this.Visit(this.MergeScopes((Expression) node));
        this._scopes.Pop();
        return (Expression) node;
      }
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
      if (node.Variable == null)
      {
        this.Visit(node.Body);
        return node;
      }
      else
      {
        this._scopes.Push(this._tree.Scopes[(object) node] = new CompilerScope((object) node, false));
        this.Visit(node.Body);
        this._scopes.Pop();
        return node;
      }
    }

    private ReadOnlyCollection<Expression> MergeScopes(Expression node)
    {
      LambdaExpression lambdaExpression = node as LambdaExpression;
      ReadOnlyCollection<Expression> readOnlyCollection;
      if (lambdaExpression != null)
        readOnlyCollection = new ReadOnlyCollection<Expression>((IList<Expression>) new Expression[1]
        {
          lambdaExpression.Body
        });
      else
        readOnlyCollection = ((BlockExpression) node).Expressions;
      CompilerScope compilerScope = this._scopes.Peek();
      for (; readOnlyCollection.Count == 1 && readOnlyCollection[0].NodeType == ExpressionType.Block; {
        BlockExpression blockExpression;
        readOnlyCollection = blockExpression.Expressions;
      }
      )
      {
        blockExpression = (BlockExpression) readOnlyCollection[0];
        if (blockExpression.Variables.Count > 0)
        {
          foreach (ParameterExpression key in blockExpression.Variables)
          {
            if (compilerScope.Definitions.ContainsKey(key))
              return readOnlyCollection;
          }
          if (compilerScope.MergedScopes == null)
            compilerScope.MergedScopes = new Set<object>((IEqualityComparer<object>) ReferenceEqualityComparer<object>.Instance);
          compilerScope.MergedScopes.Add((object) blockExpression);
          foreach (ParameterExpression key in blockExpression.Variables)
            compilerScope.Definitions.Add(key, VariableStorageKind.Local);
        }
        node = (Expression) blockExpression;
      }
      return readOnlyCollection;
    }

    protected internal override Expression VisitParameter(ParameterExpression node)
    {
      this.Reference(node, VariableStorageKind.Local);
      CompilerScope compilerScope1 = (CompilerScope) null;
      foreach (CompilerScope compilerScope2 in this._scopes)
      {
        if (compilerScope2.IsMethod || compilerScope2.Definitions.ContainsKey(node))
        {
          compilerScope1 = compilerScope2;
          break;
        }
      }
      if (compilerScope1.ReferenceCount == null)
        compilerScope1.ReferenceCount = new Dictionary<ParameterExpression, int>();
      Helpers.IncrementCount<ParameterExpression>(node, compilerScope1.ReferenceCount);
      return (Expression) node;
    }

    protected internal override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
    {
      foreach (ParameterExpression node1 in node.Variables)
        this.Reference(node1, VariableStorageKind.Hoisted);
      return (Expression) node;
    }

    private void Reference(ParameterExpression node, VariableStorageKind storage)
    {
      CompilerScope compilerScope1 = (CompilerScope) null;
      foreach (CompilerScope compilerScope2 in this._scopes)
      {
        if (compilerScope2.Definitions.ContainsKey(node))
        {
          compilerScope1 = compilerScope2;
          break;
        }
        else
        {
          compilerScope2.NeedsClosure = true;
          if (compilerScope2.IsMethod)
            storage = VariableStorageKind.Hoisted;
        }
      }
      if (compilerScope1 == null)
        throw Error.UndefinedVariable((object) node.Name, (object) node.Type, (object) this.CurrentLambdaName);
      if (storage != VariableStorageKind.Hoisted)
        return;
      if (node.IsByRef)
        throw Error.CannotCloseOverByRef((object) node.Name, (object) this.CurrentLambdaName);
      compilerScope1.Definitions[node] = VariableStorageKind.Hoisted;
    }
  }
}
