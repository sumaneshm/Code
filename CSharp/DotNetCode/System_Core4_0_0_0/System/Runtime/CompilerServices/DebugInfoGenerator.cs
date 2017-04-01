// Type: System.Runtime.CompilerServices.DebugInfoGenerator
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime;

namespace System.Runtime.CompilerServices
{
  public abstract class DebugInfoGenerator
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected DebugInfoGenerator()
    {
    }

    public static DebugInfoGenerator CreatePdbGenerator()
    {
      return (DebugInfoGenerator) new SymbolDocumentGenerator();
    }

    public abstract void MarkSequencePoint(LambdaExpression method, int ilOffset, DebugInfoExpression sequencePoint);

    internal virtual void MarkSequencePoint(LambdaExpression method, MethodBase methodBase, ILGenerator ilg, DebugInfoExpression sequencePoint)
    {
      this.MarkSequencePoint(method, ilg.ILOffset, sequencePoint);
    }

    internal virtual void SetLocalName(LocalBuilder localBuilder, string name)
    {
    }
  }
}
