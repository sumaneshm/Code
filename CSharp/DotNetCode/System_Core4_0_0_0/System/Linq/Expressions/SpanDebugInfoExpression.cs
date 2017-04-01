// Type: System.Linq.Expressions.SpanDebugInfoExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

namespace System.Linq.Expressions
{
  internal sealed class SpanDebugInfoExpression : DebugInfoExpression
  {
    private readonly int _startLine;
    private readonly int _startColumn;
    private readonly int _endLine;
    private readonly int _endColumn;

    public override int StartLine
    {
      get
      {
        return this._startLine;
      }
    }

    public override int StartColumn
    {
      get
      {
        return this._startColumn;
      }
    }

    public override int EndLine
    {
      get
      {
        return this._endLine;
      }
    }

    public override int EndColumn
    {
      get
      {
        return this._endColumn;
      }
    }

    public override bool IsClear
    {
      get
      {
        return false;
      }
    }

    internal SpanDebugInfoExpression(SymbolDocumentInfo document, int startLine, int startColumn, int endLine, int endColumn)
      : base(document)
    {
      this._startLine = startLine;
      this._startColumn = startColumn;
      this._endLine = endLine;
      this._endColumn = endColumn;
    }

    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitDebugInfo((DebugInfoExpression) this);
    }
  }
}
