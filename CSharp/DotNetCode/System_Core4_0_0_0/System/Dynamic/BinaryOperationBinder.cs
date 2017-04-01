// Type: System.Dynamic.BinaryOperationBinder
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
  public abstract class BinaryOperationBinder : DynamicMetaObjectBinder
  {
    private ExpressionType _operation;

    [__DynamicallyInvokable]
    public override sealed Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        return typeof (object);
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
    protected BinaryOperationBinder(ExpressionType operation)
    {
      ContractUtils.Requires(BinaryOperationBinder.OperationIsValid(operation), "operation");
      this._operation = operation;
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg)
    {
      return this.FallbackBinaryOperation(target, arg, (DynamicMetaObject) null);
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject FallbackBinaryOperation(DynamicMetaObject target, DynamicMetaObject arg, DynamicMetaObject errorSuggestion);

    [__DynamicallyInvokable]
    public override sealed DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      ContractUtils.RequiresNotNull((object) args, "args");
      ContractUtils.Requires(args.Length == 1, "args");
      DynamicMetaObject dynamicMetaObject = args[0];
      ContractUtils.RequiresNotNull((object) dynamicMetaObject, "args");
      return target.BindBinaryOperation(this, dynamicMetaObject);
    }

    internal static bool OperationIsValid(ExpressionType operation)
    {
      switch (operation)
      {
        case ExpressionType.NotEqual:
        case ExpressionType.Or:
        case ExpressionType.Power:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.Extension:
        case ExpressionType.AddAssign:
        case ExpressionType.AndAssign:
        case ExpressionType.DivideAssign:
        case ExpressionType.ExclusiveOrAssign:
        case ExpressionType.LeftShiftAssign:
        case ExpressionType.ModuloAssign:
        case ExpressionType.MultiplyAssign:
        case ExpressionType.OrAssign:
        case ExpressionType.PowerAssign:
        case ExpressionType.RightShiftAssign:
        case ExpressionType.SubtractAssign:
        case ExpressionType.Add:
        case ExpressionType.And:
        case ExpressionType.Divide:
        case ExpressionType.Equal:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LeftShift:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
          return true;
        default:
          return false;
      }
    }
  }
}
