// Type: System.Linq.Expressions.ParameterExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Diagnostics;
using System.Runtime;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.ParameterExpressionProxy))]
  [__DynamicallyInvokable]
  public class ParameterExpression : Expression
  {
    private readonly string _name;

    [__DynamicallyInvokable]
    public override Type Type
    {
      [__DynamicallyInvokable] get
      {
        return typeof (object);
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Parameter;
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
    public bool IsByRef
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this.GetIsByRef();
      }
    }

    internal ParameterExpression(string name)
    {
      this._name = name;
    }

    internal virtual bool GetIsByRef()
    {
      return false;
    }

    [__DynamicallyInvokable]
    protected internal override Expression Accept(ExpressionVisitor visitor)
    {
      return visitor.VisitParameter(this);
    }

    internal static ParameterExpression Make(Type type, string name, bool isByRef)
    {
      if (isByRef)
        return (ParameterExpression) new ByRefParameterExpression(type, name);
      if (!type.IsEnum)
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Object:
            if (type == typeof (object))
              return new ParameterExpression(name);
            if (type == typeof (Exception))
              return (ParameterExpression) new PrimitiveParameterExpression<Exception>(name);
            if (type == typeof (object[]))
              return (ParameterExpression) new PrimitiveParameterExpression<object[]>(name);
            else
              break;
          case TypeCode.DBNull:
            return (ParameterExpression) new PrimitiveParameterExpression<DBNull>(name);
          case TypeCode.Boolean:
            return (ParameterExpression) new PrimitiveParameterExpression<bool>(name);
          case TypeCode.Char:
            return (ParameterExpression) new PrimitiveParameterExpression<char>(name);
          case TypeCode.SByte:
            return (ParameterExpression) new PrimitiveParameterExpression<sbyte>(name);
          case TypeCode.Byte:
            return (ParameterExpression) new PrimitiveParameterExpression<byte>(name);
          case TypeCode.Int16:
            return (ParameterExpression) new PrimitiveParameterExpression<short>(name);
          case TypeCode.UInt16:
            return (ParameterExpression) new PrimitiveParameterExpression<ushort>(name);
          case TypeCode.Int32:
            return (ParameterExpression) new PrimitiveParameterExpression<int>(name);
          case TypeCode.UInt32:
            return (ParameterExpression) new PrimitiveParameterExpression<uint>(name);
          case TypeCode.Int64:
            return (ParameterExpression) new PrimitiveParameterExpression<long>(name);
          case TypeCode.UInt64:
            return (ParameterExpression) new PrimitiveParameterExpression<ulong>(name);
          case TypeCode.Single:
            return (ParameterExpression) new PrimitiveParameterExpression<float>(name);
          case TypeCode.Double:
            return (ParameterExpression) new PrimitiveParameterExpression<double>(name);
          case TypeCode.Decimal:
            return (ParameterExpression) new PrimitiveParameterExpression<Decimal>(name);
          case TypeCode.DateTime:
            return (ParameterExpression) new PrimitiveParameterExpression<DateTime>(name);
          case TypeCode.String:
            return (ParameterExpression) new PrimitiveParameterExpression<string>(name);
        }
      }
      return (ParameterExpression) new TypedParameterExpression(type, name);
    }
  }
}
