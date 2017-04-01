// Type: System.Linq.Expressions.ClearDebugInfoExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

namespace System.Linq.Expressions
{
  internal sealed class ClearDebugInfoExpression : DebugInfoExpression
  {
    public override bool IsClear
    {
      get
      {
        return true;
      }
    }

    public override int StartLine
    {
      get
      {
        return 16707566;
      }
    }

    public override int StartColumn
    {
      get
      {
        return 0;
      }
    }

    public override int EndLine
    {
      get
      {
        return 16707566;
      }
    }

    public override int EndColumn
    {
      get
      {
        return 0;
      }
    }

    internal ClearDebugInfoExpression(SymbolDocumentInfo document)
      : base(document)
    {
    }
  }
}
