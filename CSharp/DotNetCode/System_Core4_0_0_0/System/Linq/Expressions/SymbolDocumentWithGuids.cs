// Type: System.Linq.Expressions.SymbolDocumentWithGuids
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Linq.Expressions.Compiler;

namespace System.Linq.Expressions
{
  internal sealed class SymbolDocumentWithGuids : SymbolDocumentInfo
  {
    private readonly Guid _language;
    private readonly Guid _vendor;
    private readonly Guid _documentType;

    public override Guid Language
    {
      get
      {
        return this._language;
      }
    }

    public override Guid LanguageVendor
    {
      get
      {
        return this._vendor;
      }
    }

    public override Guid DocumentType
    {
      get
      {
        return this._documentType;
      }
    }

    internal SymbolDocumentWithGuids(string fileName, ref Guid language)
      : base(fileName)
    {
      this._language = language;
      this._documentType = SymbolGuids.DocumentType_Text;
    }

    internal SymbolDocumentWithGuids(string fileName, ref Guid language, ref Guid vendor)
      : base(fileName)
    {
      this._language = language;
      this._vendor = vendor;
      this._documentType = SymbolGuids.DocumentType_Text;
    }

    internal SymbolDocumentWithGuids(string fileName, ref Guid language, ref Guid vendor, ref Guid documentType)
      : base(fileName)
    {
      this._language = language;
      this._vendor = vendor;
      this._documentType = documentType;
    }
  }
}
