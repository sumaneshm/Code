// Type: System.Linq.Expressions.SymbolDocumentInfo
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic.Utils;
using System.Linq.Expressions.Compiler;
using System.Runtime;

namespace System.Linq.Expressions
{
  [__DynamicallyInvokable]
  public class SymbolDocumentInfo
  {
    private readonly string _fileName;

    [__DynamicallyInvokable]
    public string FileName
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._fileName;
      }
    }

    [__DynamicallyInvokable]
    public virtual Guid Language
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return Guid.Empty;
      }
    }

    [__DynamicallyInvokable]
    public virtual Guid LanguageVendor
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return Guid.Empty;
      }
    }

    [__DynamicallyInvokable]
    public virtual Guid DocumentType
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return SymbolGuids.DocumentType_Text;
      }
    }

    internal SymbolDocumentInfo(string fileName)
    {
      ContractUtils.RequiresNotNull((object) fileName, "fileName");
      this._fileName = fileName;
    }
  }
}
