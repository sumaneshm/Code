// Type: System.Dynamic.SetIndexBinder
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
  public abstract class SetIndexBinder : DynamicMetaObjectBinder
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
    protected SetIndexBinder(CallInfo callInfo)
    {
      ContractUtils.RequiresNotNull((object) callInfo, "callInfo");
      this._callInfo = callInfo;
    }

    [__DynamicallyInvokable]
    public override sealed DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      ContractUtils.RequiresNotNull((object) args, "args");
      ContractUtils.Requires(args.Length >= 2, "args");
      DynamicMetaObject dynamicMetaObject = args[args.Length - 1];
      DynamicMetaObject[] indexes = CollectionExtensions.RemoveLast<DynamicMetaObject>(args);
      ContractUtils.RequiresNotNull((object) dynamicMetaObject, "args");
      ContractUtils.RequiresNotNullItems<DynamicMetaObject>((IList<DynamicMetaObject>) indexes, "args");
      return target.BindSetIndex(this, indexes, dynamicMetaObject);
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value)
    {
      return this.FallbackSetIndex(target, indexes, value, (DynamicMetaObject) null);
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion);
  }
}
