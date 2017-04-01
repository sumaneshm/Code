// Type: System.Dynamic.SetMemberBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic.Utils;
using System.Runtime;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public abstract class SetMemberBinder : DynamicMetaObjectBinder
  {
    private readonly string _name;
    private readonly bool _ignoreCase;

    [__DynamicallyInvokable]
    public override sealed Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        return typeof (object);
      }
    }

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

    internal override sealed bool IsStandardBinder
    {
      get
      {
        return true;
      }
    }

    [__DynamicallyInvokable]
    protected SetMemberBinder(string name, bool ignoreCase)
    {
      ContractUtils.RequiresNotNull((object) name, "name");
      this._name = name;
      this._ignoreCase = ignoreCase;
    }

    [__DynamicallyInvokable]
    public override sealed DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      ContractUtils.RequiresNotNull((object) args, "args");
      ContractUtils.Requires(args.Length == 1, "args");
      DynamicMetaObject dynamicMetaObject = args[0];
      ContractUtils.RequiresNotNull((object) dynamicMetaObject, "args");
      return target.BindSetMember(this, dynamicMetaObject);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value)
    {
      return this.FallbackSetMember(target, value, (DynamicMetaObject) null);
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion);
  }
}
