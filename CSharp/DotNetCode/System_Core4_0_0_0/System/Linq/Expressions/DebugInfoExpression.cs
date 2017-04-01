// Type: System.Linq.Expressions.DebugInfoExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.DebugInfoExpressionProxy))]
  [__DynamicallyInvokable]
  public class DebugInfoExpression : Expression
  {
    private readonly SymbolDocumentInfo _document;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable] get
      {
        return typeof (void);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.DebugInfo;
      }
    }

    [__DynamicallyInvokable]
    public virtual int StartLine
    {
      [__DynamicallyInvokable] get
      {
        throw ContractUtils.Unreachable;
      }
    }

    [__DynamicallyInvokable]
    public virtual int StartColumn
    {
      [__DynamicallyInvokable] get
      {
        throw ContractUtils.Unreachable;
      }
    }

    [__DynamicallyInvokable]
    public virtual int EndLine
    {
      [__DynamicallyInvokable] get
      {
        throw ContractUtils.Unreachable;
      }
    }

    [__DynamicallyInvokable]
    public virtual int EndColumn
    {
      [__DynamicallyInvokable] get
      {
        throw ContractUtils.Unreachable;
      }
    }

    [__DynamicallyInvokable]
    public SymbolDocumentInfo Document
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._document;
      }
    }

    [__DynamicallyInvokable]
    public virtual bool IsClear
    {
      [__DynamicallyInvokable] get
      {
        throw ContractUtils.Unreachable;
      }
    }

    internal DebugInfoExpression(SymbolDocumentInfo document)
    {
      this._document = document;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitDebugInfo(this);
    }
  }
}
