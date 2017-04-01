// Type: System.Dynamic.CreateInstanceBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Runtime;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public abstract class CreateInstanceBinder : DynamicMetaObjectBinder
  {
    private readonly CallInfo _callInfo;

    [__DynamicallyInvokable]
    public override sealed Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        return typeof (object);
      }
    }

    [__DynamicallyInvokable]
    public CallInfo CallInfo
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._callInfo;
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
    protected CreateInstanceBinder(CallInfo callInfo)
    {
      ContractUtils.RequiresNotNull((object) callInfo, "callInfo");
      this._callInfo = callInfo;
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      return this.FallbackCreateInstance(target, args, (DynamicMetaObject) null);
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion);

    [__DynamicallyInvokable]
    public override sealed DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      ContractUtils.RequiresNotNullItems<DynamicMetaObject>((IList<DynamicMetaObject>) args, "args");
      return target.BindCreateInstance(this, args);
    }
  }
}
