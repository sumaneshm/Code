// Type: System.Dynamic.Utils.TypeUtils
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Dynamic.Utils
{
  internal static class TypeUtils
  {
    private static readonly Assembly _mscorlib = typeof (object).Assembly;
    private static readonly Assembly _systemCore = typeof (Expression).Assembly;
    private const BindingFlags AnyStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    internal const MethodAttributes PublicStatic = MethodAttributes.Public | MethodAttributes.Static;

    static TypeUtils()
    {
    }

    internal static bool AreReferenceAssignable(Type dest, Type src)
    {
      return TypeUtils.AreEquivalent(dest, src) || !dest.IsValueType && !src.IsValueType && dest.IsAssignableFrom(src);
    }

    internal static Type GetNonNullableType(this Type type)
    {
      if (TypeUtils.IsNullableType(type))
        return type.GetGenericArguments()[0];
      else
        return type;
    }

    internal static Type GetNullableType(Type type)
    {
      if (!type.IsValueType || TypeUtils.IsNullableType(type))
        return type;
      return typeof (Nullable<>).MakeGenericType(new Type[1]
      {
        type
      });
    }

    internal static bool IsNullableType(this Type type)
    {
      if (type.IsGenericType)
        return type.GetGenericTypeDefinition() == typeof (Nullable<>);
      else
        return false;
    }

    internal static bool IsBool(Type type)
    {
      return TypeUtils.GetNonNullableType(type) == typeof (bool);
    }

    internal static bool IsNumeric(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      if (!type.IsEnum)
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
            return true;
        }
      }
      return false;
    }

    internal static bool IsInteger(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      if (type.IsEnum)
        return false;
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsArithmetic(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      if (!type.IsEnum)
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Single:
          case TypeCode.Double:
            return true;
        }
      }
      return false;
    }

    internal static bool IsUnsignedInt(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      if (!type.IsEnum)
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.UInt16:
          case TypeCode.UInt32:
          case TypeCode.UInt64:
            return true;
        }
      }
      return false;
    }

    internal static bool IsIntegerOrBool(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      if (!type.IsEnum)
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Boolean:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
            return true;
        }
      }
      return false;
    }

    internal static bool AreEquivalent(Type t1, Type t2)
    {
      if (!(t1 == t2))
        return t1.IsEquivalentTo(t2);
      else
        return true;
    }

    internal static bool IsValidInstanceType(MemberInfo member, Type instanceType)
    {
      Type declaringType = member.DeclaringType;
      if (TypeUtils.AreReferenceAssignable(declaringType, instanceType))
        return true;
      if (instanceType.IsValueType)
      {
        if (TypeUtils.AreReferenceAssignable(declaringType, typeof (object)) || TypeUtils.AreReferenceAssignable(declaringType, typeof (ValueType)) || instanceType.IsEnum && TypeUtils.AreReferenceAssignable(declaringType, typeof (Enum)))
          return true;
        if (declaringType.IsInterface)
        {
          foreach (Type src in instanceType.GetInterfaces())
          {
            if (TypeUtils.AreReferenceAssignable(declaringType, src))
              return true;
          }
        }
      }
      return false;
    }

    internal static bool HasIdentityPrimitiveOrNullableConversion(Type source, Type dest)
    {
      return TypeUtils.AreEquivalent(source, dest) || TypeUtils.IsNullableType(source) && TypeUtils.AreEquivalent(dest, TypeUtils.GetNonNullableType(source)) || TypeUtils.IsNullableType(dest) && TypeUtils.AreEquivalent(source, TypeUtils.GetNonNullableType(dest)) || TypeUtils.IsConvertible(source) && TypeUtils.IsConvertible(dest) && TypeUtils.GetNonNullableType(dest) != typeof (bool);
    }

    internal static bool HasReferenceConversion(Type source, Type dest)
    {
      if (source == typeof (void) || dest == typeof (void))
        return false;
      Type nonNullableType1 = TypeUtils.GetNonNullableType(source);
      Type nonNullableType2 = TypeUtils.GetNonNullableType(dest);
      return nonNullableType1.IsAssignableFrom(nonNullableType2) || nonNullableType2.IsAssignableFrom(nonNullableType1) || (source.IsInterface || dest.IsInterface) || (TypeUtils.IsLegalExplicitVariantDelegateConversion(source, dest) || source == typeof (object) || dest == typeof (object));
    }

    private static bool IsCovariant(Type t)
    {
      return GenericParameterAttributes.None != (t.GenericParameterAttributes & GenericParameterAttributes.Covariant);
    }

    private static bool IsContravariant(Type t)
    {
      return GenericParameterAttributes.None != (t.GenericParameterAttributes & GenericParameterAttributes.Contravariant);
    }

    private static bool IsInvariant(Type t)
    {
      return GenericParameterAttributes.None == (t.GenericParameterAttributes & GenericParameterAttributes.VarianceMask);
    }

    private static bool IsDelegate(Type t)
    {
      return t.IsSubclassOf(typeof (MulticastDelegate));
    }

    internal static bool IsLegalExplicitVariantDelegateConversion(Type source, Type dest)
    {
      if (!TypeUtils.IsDelegate(source) || !TypeUtils.IsDelegate(dest) || (!source.IsGenericType || !dest.IsGenericType))
        return false;
      Type genericTypeDefinition = source.GetGenericTypeDefinition();
      if (dest.GetGenericTypeDefinition() != genericTypeDefinition)
        return false;
      Type[] genericArguments1 = genericTypeDefinition.GetGenericArguments();
      Type[] genericArguments2 = source.GetGenericArguments();
      Type[] genericArguments3 = dest.GetGenericArguments();
      for (int index = 0; index < genericArguments1.Length; ++index)
      {
        Type type1 = genericArguments2[index];
        Type type2 = genericArguments3[index];
        if (!TypeUtils.AreEquivalent(type1, type2))
        {
          Type t = genericArguments1[index];
          if (TypeUtils.IsInvariant(t))
            return false;
          if (TypeUtils.IsCovariant(t))
          {
            if (!TypeUtils.HasReferenceConversion(type1, type2))
              return false;
          }
          else if (TypeUtils.IsContravariant(t) && (type1.IsValueType || type2.IsValueType))
            return false;
        }
      }
      return true;
    }

    internal static bool IsConvertible(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      if (type.IsEnum)
        return true;
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Boolean:
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
          return true;
        default:
          return false;
      }
    }

    internal static bool HasReferenceEquality(Type left, Type right)
    {
      if (left.IsValueType || right.IsValueType)
        return false;
      if (!left.IsInterface && !right.IsInterface && !TypeUtils.AreReferenceAssignable(left, right))
        return TypeUtils.AreReferenceAssignable(right, left);
      else
        return true;
    }

    internal static bool HasBuiltInEqualityOperator(Type left, Type right)
    {
      if (left.IsInterface && !right.IsValueType || right.IsInterface && !left.IsValueType || !left.IsValueType && !right.IsValueType && (TypeUtils.AreReferenceAssignable(left, right) || TypeUtils.AreReferenceAssignable(right, left)))
        return true;
      if (!TypeUtils.AreEquivalent(left, right))
        return false;
      Type nonNullableType = TypeUtils.GetNonNullableType(left);
      return nonNullableType == typeof (bool) || TypeUtils.IsNumeric(nonNullableType) || nonNullableType.IsEnum;
    }

    internal static bool IsImplicitlyConvertible(Type source, Type destination)
    {
      if (!TypeUtils.AreEquivalent(source, destination) && !TypeUtils.IsImplicitNumericConversion(source, destination) && (!TypeUtils.IsImplicitReferenceConversion(source, destination) && !TypeUtils.IsImplicitBoxingConversion(source, destination)))
        return TypeUtils.IsImplicitNullableConversion(source, destination);
      else
        return true;
    }

    internal static MethodInfo GetUserDefinedCoercionMethod(Type convertFrom, Type convertToType, bool implicitOnly)
    {
      Type nonNullableType1 = TypeUtils.GetNonNullableType(convertFrom);
      Type nonNullableType2 = TypeUtils.GetNonNullableType(convertToType);
      MethodInfo[] methods1 = nonNullableType1.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      MethodInfo conversionOperator1 = TypeUtils.FindConversionOperator(methods1, convertFrom, convertToType, implicitOnly);
      if (conversionOperator1 != (MethodInfo) null)
        return conversionOperator1;
      MethodInfo[] methods2 = nonNullableType2.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      MethodInfo conversionOperator2 = TypeUtils.FindConversionOperator(methods2, convertFrom, convertToType, implicitOnly);
      if (conversionOperator2 != (MethodInfo) null)
        return conversionOperator2;
      if (!TypeUtils.AreEquivalent(nonNullableType1, convertFrom) || !TypeUtils.AreEquivalent(nonNullableType2, convertToType))
      {
        MethodInfo conversionOperator3 = TypeUtils.FindConversionOperator(methods1, nonNullableType1, nonNullableType2, implicitOnly);
        if (conversionOperator3 == (MethodInfo) null)
          conversionOperator3 = TypeUtils.FindConversionOperator(methods2, nonNullableType1, nonNullableType2, implicitOnly);
        if (conversionOperator3 != (MethodInfo) null)
          return conversionOperator3;
      }
      return (MethodInfo) null;
    }

    internal static MethodInfo FindConversionOperator(MethodInfo[] methods, Type typeFrom, Type typeTo, bool implicitOnly)
    {
      foreach (MethodInfo methodInfo in methods)
      {
        if ((!(methodInfo.Name != "op_Implicit") || !implicitOnly && !(methodInfo.Name != "op_Explicit")) && (TypeUtils.AreEquivalent(methodInfo.ReturnType, typeTo) && TypeUtils.AreEquivalent(TypeExtensions.GetParametersCached((MethodBase) methodInfo)[0].ParameterType, typeFrom)))
          return methodInfo;
      }
      return (MethodInfo) null;
    }

    private static bool IsImplicitNumericConversion(Type source, Type destination)
    {
      TypeCode typeCode1 = Type.GetTypeCode(source);
      TypeCode typeCode2 = Type.GetTypeCode(destination);
      switch (typeCode1)
      {
        case TypeCode.Char:
          switch (typeCode2)
          {
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.SByte:
          switch (typeCode2)
          {
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.Byte:
          switch (typeCode2)
          {
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.Int16:
          switch (typeCode2)
          {
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.UInt16:
          switch (typeCode2)
          {
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.Int32:
          switch (typeCode2)
          {
            case TypeCode.Int64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.UInt32:
          switch (typeCode2)
          {
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.Int64:
        case TypeCode.UInt64:
          switch (typeCode2)
          {
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              return true;
            default:
              return false;
          }
        case TypeCode.Single:
          return typeCode2 == TypeCode.Double;
        default:
          return false;
      }
    }

    private static bool IsImplicitReferenceConversion(Type source, Type destination)
    {
      return destination.IsAssignableFrom(source);
    }

    private static bool IsImplicitBoxingConversion(Type source, Type destination)
    {
      return source.IsValueType && (destination == typeof (object) || destination == typeof (ValueType)) || source.IsEnum && destination == typeof (Enum);
    }

    private static bool IsImplicitNullableConversion(Type source, Type destination)
    {
      if (TypeUtils.IsNullableType(destination))
        return TypeUtils.IsImplicitlyConvertible(TypeUtils.GetNonNullableType(source), TypeUtils.GetNonNullableType(destination));
      else
        return false;
    }

    internal static bool IsSameOrSubclass(Type type, Type subType)
    {
      if (!TypeUtils.AreEquivalent(type, subType))
        return subType.IsSubclassOf(type);
      else
        return true;
    }

    internal static void ValidateType(Type type)
    {
      if (type.IsGenericTypeDefinition)
        throw Error.TypeIsGeneric((object) type);
      if (type.ContainsGenericParameters)
        throw Error.TypeContainsGenericParameters((object) type);
    }

    internal static Type FindGenericType(Type definition, Type type)
    {
      for (; type != (Type) null && type != typeof (object); type = type.BaseType)
      {
        if (type.IsGenericType && TypeUtils.AreEquivalent(type.GetGenericTypeDefinition(), definition))
          return type;
        if (definition.IsInterface)
        {
          foreach (Type type1 in type.GetInterfaces())
          {
            Type genericType = TypeUtils.FindGenericType(definition, type1);
            if (genericType != (Type) null)
              return genericType;
          }
        }
      }
      return (Type) null;
    }

    internal static bool IsUnsigned(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Char:
        case TypeCode.Byte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsFloatingPoint(Type type)
    {
      type = TypeUtils.GetNonNullableType(type);
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Single:
        case TypeCode.Double:
          return true;
        default:
          return false;
      }
    }

    internal static MethodInfo GetBooleanOperator(Type type, string name)
    {
      do
      {
        MethodInfo methodValidated = TypeExtensions.GetMethodValidated(type, name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[1]
        {
          type
        }, (ParameterModifier[]) null);
        if (methodValidated != (MethodInfo) null && methodValidated.IsSpecialName && !methodValidated.ContainsGenericParameters)
          return methodValidated;
        type = type.BaseType;
      }
      while (type != (Type) null);
      return (MethodInfo) null;
    }

    internal static Type GetNonRefType(this Type type)
    {
      if (!type.IsByRef)
        return type;
      else
        return type.GetElementType();
    }

    internal static bool CanCache(this Type t)
    {
      Assembly assembly = t.Assembly;
      if (assembly != TypeUtils._mscorlib && assembly != TypeUtils._systemCore)
        return false;
      if (t.IsGenericType)
      {
        foreach (Type t1 in t.GetGenericArguments())
        {
          if (!TypeUtils.CanCache(t1))
            return false;
        }
      }
      return true;
    }
  }
}
