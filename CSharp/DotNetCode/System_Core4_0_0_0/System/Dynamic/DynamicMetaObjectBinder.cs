// Type: System.Dynamic.DynamicMetaObjectBinder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Linq.Expressions.Compiler;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public abstract class DynamicMetaObjectBinder : CallSiteBinder
  {
    private static readonly Type ComObjectType = typeof (object).Assembly.GetType("System.__ComObject");

    [__DynamicallyInvokable]
    public virtual Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        return typeof (object);
      }
    }

    internal virtual bool IsStandardBinder
    {
      get
      {
        return false;
      }
    }

    static DynamicMetaObjectBinder()
    {
    }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected DynamicMetaObjectBinder()
    {
    }

    [__DynamicallyInvokable]
    public override sealed Expression Bind(object[] args, ReadOnlyCollection<ParameterExpression> parameters, LabelTarget returnLabel)
    {
      ContractUtils.RequiresNotNull((object) args, "args");
      ContractUtils.RequiresNotNull((object) parameters, "parameters");
      ContractUtils.RequiresNotNull((object) returnLabel, "returnLabel");
      if (args.Length == 0)
        throw Error.OutOfRange((object) "args.Length", (object) 1);
      if (parameters.Count == 0)
        throw Error.OutOfRange((object) "parameters.Count", (object) 1);
      if (args.Length != parameters.Count)
        throw new ArgumentOutOfRangeException("args");
      Type type;
      if (this.IsStandardBinder)
      {
        type = this.ReturnType;
        if (returnLabel.Type != typeof (void) && !TypeUtils.AreReferenceAssignable(returnLabel.Type, type))
          throw Error.BinderNotCompatibleWithCallSite((object) type, (object) this, (object) returnLabel.Type);
      }
      else
        type = returnLabel.Type;
      DynamicMetaObject target = DynamicMetaObject.Create(args[0], (Expression) parameters[0]);
      DynamicMetaObject[] argumentMetaObjects = DynamicMetaObjectBinder.CreateArgumentMetaObjects(args, parameters);
      DynamicMetaObject dynamicMetaObject = this.Bind(target, argumentMetaObjects);
      if (dynamicMetaObject == null)
        throw Error.BindingCannotBeNull();
      Expression ifTrue = dynamicMetaObject.Expression;
      BindingRestrictions restrictions = dynamicMetaObject.Restrictions;
      if (type != typeof (void) && !TypeUtils.AreReferenceAssignable(type, ifTrue.Type))
      {
        if (target.Value is IDynamicMetaObjectProvider)
          throw Error.DynamicObjectResultNotAssignable((object) ifTrue.Type, (object) target.Value.GetType(), (object) this, (object) type);
        else
          throw Error.DynamicBinderResultNotAssignable((object) ifTrue.Type, (object) this, (object) type);
      }
      else
      {
        if (this.IsStandardBinder && args[0] is IDynamicMetaObjectProvider && restrictions == BindingRestrictions.Empty)
          throw Error.DynamicBindingNeedsRestrictions((object) target.Value.GetType(), (object) this);
        BindingRestrictions bindingRestrictions = DynamicMetaObjectBinder.AddRemoteObjectRestrictions(restrictions, args, parameters);
        if (ifTrue.NodeType != ExpressionType.Goto)
          ifTrue = (Expression) Expression.Return(returnLabel, ifTrue);
        if (bindingRestrictions != BindingRestrictions.Empty)
          ifTrue = (Expression) Expression.IfThen(bindingRestrictions.ToExpression(), ifTrue);
        return ifTrue;
      }
    }

    [__DynamicallyInvokable]
    public abstract DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args);

    [__DynamicallyInvokable]
    public Expression GetUpdateExpression(Type type)
    {
      return (Expression) Expression.Goto(CallSiteBinder.UpdateLabel, type);
    }

    [__DynamicallyInvokable]
    public DynamicMetaObject Defer(DynamicMetaObject target, params DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) target, "target");
      if (args != null)
        return this.MakeDeferred(target.Restrictions.Merge(BindingRestrictions.Combine((IList<DynamicMetaObject>) args)), CollectionExtensions.AddFirst<DynamicMetaObject>((IList<DynamicMetaObject>) args, target));
      return this.MakeDeferred(target.Restrictions, new DynamicMetaObject[1]
      {
        target
      });
    }

    [__DynamicallyInvokable]
    public DynamicMetaObject Defer(params DynamicMetaObject[] args)
    {
      return this.MakeDeferred(BindingRestrictions.Combine((IList<DynamicMetaObject>) args), args);
    }

    private static DynamicMetaObject[] CreateArgumentMetaObjects(object[] args, ReadOnlyCollection<ParameterExpression> parameters)
    {
      DynamicMetaObject[] dynamicMetaObjectArray;
      if (args.Length != 1)
      {
        dynamicMetaObjectArray = new DynamicMetaObject[args.Length - 1];
        for (int index = 1; index < args.Length; ++index)
          dynamicMetaObjectArray[index - 1] = DynamicMetaObject.Create(args[index], (Expression) parameters[index]);
      }
      else
        dynamicMetaObjectArray = DynamicMetaObject.EmptyMetaObjects;
      return dynamicMetaObjectArray;
    }

    private static BindingRestrictions AddRemoteObjectRestrictions(BindingRestrictions restrictions, object[] args, ReadOnlyCollection<ParameterExpression> parameters)
    {
      for (int index = 0; index < parameters.Count; ++index)
      {
        ParameterExpression parameterExpression = parameters[index];
        MarshalByRefObject marshalByRefObject = args[index] as MarshalByRefObject;
        if (marshalByRefObject != null && !DynamicMetaObjectBinder.IsComObject((object) marshalByRefObject))
        {
          BindingRestrictions restrictions1 = !RemotingServices.IsObjectOutOfAppDomain((object) marshalByRefObject) ? BindingRestrictions.GetExpressionRestriction((Expression) Expression.AndAlso((Expression) Expression.NotEqual((Expression) parameterExpression, (Expression) Expression.Constant((object) null)), (Expression) Expression.Not((Expression) Expression.Call(typeof (RemotingServices).GetMethod("IsObjectOutOfAppDomain"), (Expression) parameterExpression)))) : BindingRestrictions.GetExpressionRestriction((Expression) Expression.AndAlso((Expression) Expression.NotEqual((Expression) parameterExpression, (Expression) Expression.Constant((object) null)), (Expression) Expression.Call(typeof (RemotingServices).GetMethod("IsObjectOutOfAppDomain"), (Expression) parameterExpression)));
          restrictions = restrictions.Merge(restrictions1);
        }
      }
      return restrictions;
    }

    private DynamicMetaObject MakeDeferred(BindingRestrictions rs, params DynamicMetaObject[] args)
    {
      Expression[] expressions = DynamicMetaObject.GetExpressions(args);
      return new DynamicMetaObject((Expression) DynamicExpression.Make(this.ReturnType, DelegateHelpers.MakeDeferredSiteDelegate(args, this.ReturnType), (CallSiteBinder) this, (ReadOnlyCollection<Expression>) new TrueReadOnlyCollection<Expression>(expressions)), rs);
    }

    private static bool IsComObject(object obj)
    {
      if (obj != null)
        return DynamicMetaObjectBinder.ComObjectType.IsAssignableFrom(obj.GetType());
      else
        return false;
    }
  }
}
