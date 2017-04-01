// Type: System.Dynamic.DeleteMemberBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic.Utils;
using System.Runtime;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public abstract class DeleteMemberBinder : DynamicMetaObjectBinder
  {
    private readonly string _name;
    private readonly bool _ignoreCase;

    [__DynamicallyInvokable]
    public string Name
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._name;
      }
    }

    [__DynamicallyInvokable]
    public bool IgnoreCase
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._ignoreCase;
      }
    }

    [__DynamicallyInvokable]
    public override sealed Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        return typeof (void);
      }
    }

    internal override sealed bool IsStandardBinder
    {
      get
      {
        return true;
      }
    }

    [__DynamicallyInvokable]
    protected DeleteMemberBinder(string name, bool ignoreCase)
    {
      ContractUtils.RequiresNotNull((object) name, "name");
      this._name = name;
      this._ignoreCase = ignoreCase;
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DynamicMetaObject FallbackDeleteMember(DynamicMetaObject target)
    {
      return this.FallbackDeleteMember(target, (DynamicMetaObject) null);
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject FallbackDeleteMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion);

    [__DynamicallyInvokable]
    public override sealed DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      ContractUtils.Requires(args == null || args.Length == 0);
      return target.BindDeleteMember(this);
    }
  }
}
