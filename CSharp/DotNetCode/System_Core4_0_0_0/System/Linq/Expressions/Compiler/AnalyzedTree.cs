// Type: System.Linq.Expressions.Compiler.AnalyzedTree
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class AnalyzedTree
  {
    internal readonly Dictionary<object, CompilerScope> Scopes = new Dictionary<object, CompilerScope>();
    internal readonly Dictionary<LambdaExpression, BoundConstants> Constants = new Dictionary<LambdaExpression, BoundConstants>();

    internal DebugInfoGenerator DebugInfoGenerator { get; set; }

    internal AnalyzedTree()
    {
    }
  }
}
