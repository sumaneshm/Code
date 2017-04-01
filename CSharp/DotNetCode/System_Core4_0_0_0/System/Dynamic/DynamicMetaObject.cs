// Type: System.Dynamic.DynamicMetaObject
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime;
using System.Runtime.Remoting;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public class DynamicMetaObject
  {
    [__DynamicallyInvokable]
    public static readonly DynamicMetaObject[] EmptyMetaObjects = new DynamicMetaObject[0];
    private readonly Expression _expression;
    private readonly BindingRestrictions _restrictions;
    private readonly object _value;
    private readonly bool _hasValue;

    [__DynamicallyInvokable]
    public Expression Expression
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._expression;
      }
    }

    [__DynamicallyInvokable]
    public BindingRestrictions Restrictions
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._restrictions;
      }
    }

    [__DynamicallyInvokable]
    public object Value
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._value;
      }
    }

    [__DynamicallyInvokable]
    public bool HasValue
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._hasValue;
      }
    }

    [__DynamicallyInvokable]
    public Type RuntimeType
    {
      [__DynamicallyInvokable] get
      {
        if (!this._hasValue)
          return (Type) null;
        Type type = this.Expression.Type;
        if (type.IsValueType)
          return type;
        if (this._value != null)
          return this._value.GetType();
        else
          return (Type) null;
      }
    }

    [__DynamicallyInvokable]
    public Type LimitType
    {
      [__DynamicallyInvokable] get
      {
        return this.RuntimeType ?? this.Expression.Type;
      }
    }

    static DynamicMetaObject()
    {
    }

    [__DynamicallyInvokable]
    public DynamicMetaObject(Expression expression, BindingRestrictions restrictions)
    {
      ContractUtils.RequiresNotNull((object) expression, "expression");
      ContractUtils.RequiresNotNull((object) restrictions, "restrictions");
      this._expression = expression;
      this._restrictions = restrictions;
    }

    [__DynamicallyInvokable]
    public DynamicMetaObject(Expression expression, BindingRestrictions restrictions, object value)
      : this(expression, restrictions)
    {
      this._value = value;
      this._hasValue = true;
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindConvert(ConvertBinder binder)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackConvert(this);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindGetMember(GetMemberBinder binder)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackGetMember(this);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackSetMember(this, value);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackDeleteMember(this);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackGetIndex(this, indexes);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackSetIndex(this, indexes, value);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackDeleteIndex(this, indexes);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackInvokeMember(this, args);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackInvoke(this, args);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackCreateInstance(this, args);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackUnaryOperation(this);
    }

    [__DynamicallyInvokable]
    public virtual DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
    {
      ContractUtils.RequiresNotNull((object) binder, "binder");
      return binder.FallbackBinaryOperation(this, arg);
    }

    [__DynamicallyInvokable]
    public virtual IEnumerable<string> GetDynamicMemberNames()
    {
      return (IEnumerable<string>) new string[0];
    }

    [__DynamicallyInvokable]
    public static DynamicMetaObject Create(object value, Expression expression)
    {
      ContractUtils.RequiresNotNull((object) expression, "expression");
      IDynamicMetaObjectProvider metaObjectProvider = value as IDynamicMetaObjectProvider;
      if (metaObjectProvider == null || RemotingServices.IsObjectOutOfAppDomain(value))
        return new DynamicMetaObject(expression, BindingRestrictions.Empty, value);
      DynamicMetaObject metaObject = metaObjectProvider.GetMetaObject(expression);
      if (metaObject == null || !metaObject.HasValue || (metaObject.Value == null || metaObject.Expression != expression))
        throw Error.InvalidMetaObjectCreated((object) metaObjectProvider.GetType());
      else
        return metaObject;
    }

    internal static Expression[] GetExpressions(DynamicMetaObject[] objects)
    {
      ContractUtils.RequiresNotNull((object) objects, "objects");
      Expression[] expressionArray = new Expression[objects.Length];
      for (int index = 0; index < objects.Length; ++index)
      {
        DynamicMetaObject dynamicMetaObject = objects[index];
        ContractUtils.RequiresNotNull((object) dynamicMetaObject, "objects");
        Expression expression = dynamicMetaObject.Expression;
        ContractUtils.RequiresNotNull((object) expression, "objects");
        expressionArray[index] = expression;
      }
      return expressionArray;
    }
  }
}
