// Type: System.Runtime.CompilerServices.SymbolDocumentGenerator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq.Expressions;
using System.Linq.Expressions.Compiler;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Runtime.CompilerServices
{
  internal sealed class SymbolDocumentGenerator : DebugInfoGenerator
  {
    private Dictionary<SymbolDocumentInfo, ISymbolDocumentWriter> _symbolWriters;

    private ISymbolDocumentWriter GetSymbolWriter(MethodBuilder method, SymbolDocumentInfo document)
    {
      if (this._symbolWriters == null)
        this._symbolWriters = new Dictionary<SymbolDocumentInfo, ISymbolDocumentWriter>();
      ISymbolDocumentWriter symbolDocumentWriter;
      if (!this._symbolWriters.TryGetValue(document, out symbolDocumentWriter))
      {
        symbolDocumentWriter = ((ModuleBuilder) method.Module).DefineDocument(document.FileName, document.Language, document.LanguageVendor, SymbolGuids.DocumentType_Text);
        this._symbolWriters.Add(document, symbolDocumentWriter);
      }
      return symbolDocumentWriter;
    }

    internal override void MarkSequencePoint(LambdaExpression method, MethodBase methodBase, ILGenerator ilg, DebugInfoExpression sequencePoint)
    {
      MethodBuilder method1 = methodBase as MethodBuilder;
      if (!((MethodInfo) method1 != (MethodInfo) null))
        return;
      ilg.MarkSequencePoint(this.GetSymbolWriter(method1, sequencePoint.Document), sequencePoint.StartLine, sequencePoint.StartColumn, sequencePoint.EndLine, sequencePoint.EndColumn);
    }

    public override void MarkSequencePoint(LambdaExpression method, int ilOffset, DebugInfoExpression sequencePoint)
    {
      throw Error.PdbGeneratorNeedsExpressionCompiler();
    }

    internal override void SetLocalName(LocalBuilder localBuilder, string name)
    {
      localBuilder.SetLocalSymInfo(name);
    }
  }
}
