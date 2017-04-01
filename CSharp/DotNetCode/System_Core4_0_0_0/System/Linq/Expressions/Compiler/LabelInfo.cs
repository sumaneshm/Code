// Type: System.Linq.Expressions.Compiler.LabelInfo
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class LabelInfo
  {
    private readonly Set<LabelScopeInfo> _definitions = new Set<LabelScopeInfo>();
    private readonly List<LabelScopeInfo> _references = new List<LabelScopeInfo>();
    private OpCode _opCode = OpCodes.Leave;
    private readonly LabelTarget _node;
    private Label _label;
    private bool _labelDefined;
    private LocalBuilder _value;
    private readonly bool _canReturn;
    private bool _acrossBlockJump;
    private readonly ILGenerator _ilg;

    internal Label Label
    {
      get
      {
        this.EnsureLabelAndValue();
        return this._label;
      }
    }

    internal bool CanReturn
    {
      get
      {
        return this._canReturn;
      }
    }

    internal bool CanBranch
    {
      get
      {
        return this._opCode != OpCodes.Leave;
      }
    }

    internal LabelInfo(ILGenerator il, LabelTarget node, bool canReturn)
    {
      this._ilg = il;
      this._node = node;
      this._canReturn = canReturn;
    }

    internal void Reference(LabelScopeInfo block)
    {
      this._references.Add(block);
      if (this._definitions.Count <= 0)
        return;
      this.ValidateJump(block);
    }

    internal void Define(LabelScopeInfo block)
    {
      for (LabelScopeInfo labelScopeInfo = block; labelScopeInfo != null; labelScopeInfo = labelScopeInfo.Parent)
      {
        if (labelScopeInfo.ContainsTarget(this._node))
          throw System.Linq.Expressions.Error.LabelTargetAlreadyDefined((object) this._node.Name);
      }
      this._definitions.Add(block);
      block.AddLabelInfo(this._node, this);
      if (this._definitions.Count == 1)
      {
        foreach (LabelScopeInfo reference in this._references)
          this.ValidateJump(reference);
      }
      else
      {
        if (this._acrossBlockJump)
          throw System.Linq.Expressions.Error.AmbiguousJump((object) this._node.Name);
        this._labelDefined = false;
      }
    }

    private void ValidateJump(LabelScopeInfo reference)
    {
      this._opCode = this._canReturn ? OpCodes.Ret : OpCodes.Br;
      for (LabelScopeInfo labelScopeInfo = reference; labelScopeInfo != null; labelScopeInfo = labelScopeInfo.Parent)
      {
        if (this._definitions.Contains(labelScopeInfo))
          return;
        if (labelScopeInfo.Kind != LabelScopeKind.Finally && labelScopeInfo.Kind != LabelScopeKind.Filter)
        {
          if (labelScopeInfo.Kind == LabelScopeKind.Try || labelScopeInfo.Kind == LabelScopeKind.Catch)
            this._opCode = OpCodes.Leave;
        }
        else
          break;
      }
      this._acrossBlockJump = true;
      if (this._node != null && this._node.Type != typeof (void))
        throw System.Linq.Expressions.Error.NonLocalJumpWithValue((object) this._node.Name);
      if (this._definitions.Count > 1)
        throw System.Linq.Expressions.Error.AmbiguousJump((object) this._node.Name);
      LabelScopeInfo first = Enumerable.First<LabelScopeInfo>((IEnumerable<LabelScopeInfo>) this._definitions);
      LabelScopeInfo labelScopeInfo1 = Helpers.CommonNode<LabelScopeInfo>(first, reference, (Func<LabelScopeInfo, LabelScopeInfo>) (b => b.Parent));
      this._opCode = this._canReturn ? OpCodes.Ret : OpCodes.Br;
      for (LabelScopeInfo labelScopeInfo2 = reference; labelScopeInfo2 != labelScopeInfo1; labelScopeInfo2 = labelScopeInfo2.Parent)
      {
        if (labelScopeInfo2.Kind == LabelScopeKind.Finally)
          throw System.Linq.Expressions.Error.ControlCannotLeaveFinally();
        if (labelScopeInfo2.Kind == LabelScopeKind.Filter)
          throw System.Linq.Expressions.Error.ControlCannotLeaveFilterTest();
        if (labelScopeInfo2.Kind == LabelScopeKind.Try || labelScopeInfo2.Kind == LabelScopeKind.Catch)
          this._opCode = OpCodes.Leave;
      }
      for (LabelScopeInfo labelScopeInfo2 = first; labelScopeInfo2 != labelScopeInfo1; labelScopeInfo2 = labelScopeInfo2.Parent)
      {
        if (!labelScopeInfo2.CanJumpInto)
        {
          if (labelScopeInfo2.Kind == LabelScopeKind.Expression)
            throw System.Linq.Expressions.Error.ControlCannotEnterExpression();
          else
            throw System.Linq.Expressions.Error.ControlCannotEnterTry();
        }
      }
    }

    internal void ValidateFinish()
    {
      if (this._references.Count > 0 && this._definitions.Count == 0)
        throw System.Linq.Expressions.Error.LabelTargetUndefined((object) this._node.Name);
    }

    internal void EmitJump()
    {
      if (this._opCode == OpCodes.Ret)
      {
        this._ilg.Emit(OpCodes.Ret);
      }
      else
      {
        this.StoreValue();
        this._ilg.Emit(this._opCode, this.Label);
      }
    }

    private void StoreValue()
    {
      this.EnsureLabelAndValue();
      if (this._value == null)
        return;
      this._ilg.Emit(OpCodes.Stloc, this._value);
    }

    internal void Mark()
    {
      if (this._canReturn)
      {
        if (!this._labelDefined)
          return;
        this._ilg.Emit(OpCodes.Ret);
      }
      else
        this.StoreValue();
      this.MarkWithEmptyStack();
    }

    internal void MarkWithEmptyStack()
    {
      this._ilg.MarkLabel(this.Label);
      if (this._value == null)
        return;
      this._ilg.Emit(OpCodes.Ldloc, this._value);
    }

    private void EnsureLabelAndValue()
    {
      if (this._labelDefined)
        return;
      this._labelDefined = true;
      this._label = this._ilg.DefineLabel();
      if (this._node == null || !(this._node.Type != typeof (void)))
        return;
      this._value = this._ilg.DeclareLocal(this._node.Type);
    }
  }
}
