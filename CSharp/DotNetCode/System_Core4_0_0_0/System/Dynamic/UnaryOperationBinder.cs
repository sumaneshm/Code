// Type: System.Dynamic.UnaryOperationBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public abstract class UnaryOperationBinder : DynamicMetaObjectBinder
  {
    private ExpressionType _operation;

    [__DynamicallyInvokable]
    public override sealed Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        switch (this._operation)
        {
          case ExpressionType.IsTrue:
          case ExpressionType.IsFalse:
            return typeof (bool);
          default:
            return typeof (object);
        }
      }
    }

    [__DynamicallyInvokable]
    public ExpressionType Operation
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._operation;
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
    protected UnaryOperationBinder(ExpressionType operation)
    {
      ContractUtils.Requires(UnaryOperationBinder.OperationIsValid(operation), "operation");
      this._operation = operation;
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target)
    {
      return this.FallbackUnaryOperation(target, (DynamicMetaObject) null);
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion);

    [__DynamicallyInvokable]
    public override sealed DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      ContractUtils.Requires(args == null || args.Length == 0, "args");
      return target.BindUnaryOperation(this);
    }

    internal static bool OperationIsValid(ExpressionType operation)
    {
      switch (operation)
      {
        case ExpressionType.Decrement:
        case ExpressionType.Extension:
        case ExpressionType.Increment:
        case ExpressionType.OnesComplement:
        case ExpressionType.IsTrue:
        case ExpressionType.IsFalse:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.Not:
          return true;
        default:
          return false;
      }
    }
  }
}
