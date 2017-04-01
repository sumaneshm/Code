// Type: System.Linq.Expressions.Compiler.LabelScopeInfo
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class LabelScopeInfo
  {
    private Dictionary<LabelTarget, LabelInfo> Labels;
    internal readonly LabelScopeKind Kind;
    internal readonly LabelScopeInfo Parent;

    internal bool CanJumpInto
    {
      get
      {
        switch (this.Kind)
        {
          case LabelScopeKind.Statement:
          case LabelScopeKind.Block:
          case LabelScopeKind.Switch:
          case LabelScopeKind.Lambda:
            return true;
          default:
            return false;
        }
      }
    }

    internal LabelScopeInfo(LabelScopeInfo parent, LabelScopeKind kind)
    {
      this.Parent = parent;
      this.Kind = kind;
    }

    internal bool ContainsTarget(LabelTarget target)
    {
      if (this.Labels == null)
        return false;
      else
        return this.Labels.ContainsKey(target);
    }

    internal bool TryGetLabelInfo(LabelTarget target, out LabelInfo info)
    {
      if (this.Labels != null)
        return this.Labels.TryGetValue(target, out info);
      info = (LabelInfo) null;
      return false;
    }

    internal void AddLabelInfo(LabelTarget target, LabelInfo info)
    {
      if (this.Labels == null)
        this.Labels = new Dictionary<LabelTarget, LabelInfo>();
      this.Labels.Add(target, info);
    }
  }
}
