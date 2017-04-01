// Type: System.Linq.Expressions.Compiler.ILGen
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Linq.Expressions.Compiler
{
  internal static class ILGen
  {
    internal static void EmitStoreElement(this ILGenerator il, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type.IsEnum)
      {
        il.Emit(OpCodes.Stelem, type);
      }
      else
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Boolean:
          case TypeCode.SByte:
          case TypeCode.Byte:
            il.Emit(OpCodes.Stelem_I1);
            break;
          case TypeCode.Char:
          case TypeCode.Int16:
          case TypeCode.UInt16:
            il.Emit(OpCodes.Stelem_I2);
            break;
          case TypeCode.Int32:
          case TypeCode.UInt32:
            il.Emit(OpCodes.Stelem_I4);
            break;
          case TypeCode.Int64:
          case TypeCode.UInt64:
            il.Emit(OpCodes.Stelem_I8);
            break;
          case TypeCode.Single:
            il.Emit(OpCodes.Stelem_R4);
            break;
          case TypeCode.Double:
            il.Emit(OpCodes.Stelem_R8);
            break;
          default:
            if (type.IsValueType)
            {
              il.Emit(OpCodes.Stelem, type);
              break;
            }
            else
            {
              il.Emit(OpCodes.Stelem_Ref);
              break;
            }
        }
      }
    }

    internal static void EmitInt(this ILGenerator il, int value)
    {
      OpCode opcode;
      switch (value)
      {
        case -1:
          opcode = OpCodes.Ldc_I4_M1;
          break;
        case 0:
          opcode = OpCodes.Ldc_I4_0;
          break;
        case 1:
          opcode = OpCodes.Ldc_I4_1;
          break;
        case 2:
          opcode = OpCodes.Ldc_I4_2;
          break;
        case 3:
          opcode = OpCodes.Ldc_I4_3;
          break;
        case 4:
          opcode = OpCodes.Ldc_I4_4;
          break;
        case 5:
          opcode = OpCodes.Ldc_I4_5;
          break;
        case 6:
          opcode = OpCodes.Ldc_I4_6;
          break;
        case 7:
          opcode = OpCodes.Ldc_I4_7;
          break;
        case 8:
          opcode = OpCodes.Ldc_I4_8;
          break;
        default:
          if (value >= (int) sbyte.MinValue && value <= (int) sbyte.MaxValue)
          {
            il.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
            return;
          }
          else
          {
            il.Emit(OpCodes.Ldc_I4, value);
            return;
          }
      }
      il.Emit(opcode);
    }

    internal static void EmitConstant(this ILGenerator il, object value, Type type)
    {
      if (value == null)
      {
        ILGen.EmitDefault(il, type);
      }
      else
      {
        if (ILGen.TryEmitILConstant(il, value, type))
          return;
        Type type1 = value as Type;
        if (type1 != (Type) null && ILGen.ShouldLdtoken(type1))
        {
          ILGen.EmitType(il, type1);
          if (!(type != typeof (Type)))
            return;
          il.Emit(OpCodes.Castclass, type);
        }
        else
        {
          MethodBase methodBase = value as MethodBase;
          if (!(methodBase != (MethodBase) null) || !ILGen.ShouldLdtoken(methodBase))
            throw ContractUtils.Unreachable;
          ILGen.Emit(il, OpCodes.Ldtoken, methodBase);
          Type declaringType = methodBase.DeclaringType;
          if (declaringType != (Type) null && declaringType.IsGenericType)
          {
            il.Emit(OpCodes.Ldtoken, declaringType);
            il.Emit(OpCodes.Call, typeof (MethodBase).GetMethod("GetMethodFromHandle", new Type[2]
            {
              typeof (RuntimeMethodHandle),
              typeof (RuntimeTypeHandle)
            }));
          }
          else
            il.Emit(OpCodes.Call, typeof (MethodBase).GetMethod("GetMethodFromHandle", new Type[1]
            {
              typeof (RuntimeMethodHandle)
            }));
          if (!(type != typeof (MethodBase)))
            return;
          il.Emit(OpCodes.Castclass, type);
        }
      }
    }

    internal static void EmitArray<T>(this ILGenerator il, IList<T> items)
    {
      ContractUtils.RequiresNotNull((object) items, "items");
      ILGen.EmitInt(il, items.Count);
      il.Emit(OpCodes.Newarr, typeof (T));
      for (int index = 0; index < items.Count; ++index)
      {
        il.Emit(OpCodes.Dup);
        ILGen.EmitInt(il, index);
        ILGen.EmitConstant(il, (object) items[index], typeof (T));
        ILGen.EmitStoreElement(il, typeof (T));
      }
    }

    internal static void Emit(this ILGenerator il, OpCode opcode, MethodBase methodBase)
    {
      if (methodBase.MemberType == MemberTypes.Constructor)
        il.Emit(opcode, (ConstructorInfo) methodBase);
      else
        il.Emit(opcode, (MethodInfo) methodBase);
    }

    internal static void EmitLoadArg(this ILGenerator il, int index)
    {
      switch (index)
      {
        case 0:
          il.Emit(OpCodes.Ldarg_0);
          break;
        case 1:
          il.Emit(OpCodes.Ldarg_1);
          break;
        case 2:
          il.Emit(OpCodes.Ldarg_2);
          break;
        case 3:
          il.Emit(OpCodes.Ldarg_3);
          break;
        default:
          if (index <= (int) byte.MaxValue)
          {
            il.Emit(OpCodes.Ldarg_S, (byte) index);
            break;
          }
          else
          {
            il.Emit(OpCodes.Ldarg, index);
            break;
          }
      }
    }

    internal static void EmitLoadArgAddress(this ILGenerator il, int index)
    {
      if (index <= (int) byte.MaxValue)
        il.Emit(OpCodes.Ldarga_S, (byte) index);
      else
        il.Emit(OpCodes.Ldarga, index);
    }

    internal static void EmitStoreArg(this ILGenerator il, int index)
    {
      if (index <= (int) byte.MaxValue)
        il.Emit(OpCodes.Starg_S, (byte) index);
      else
        il.Emit(OpCodes.Starg, index);
    }

    internal static void EmitLoadValueIndirect(this ILGenerator il, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type.IsValueType)
      {
        if (type == typeof (int))
          il.Emit(OpCodes.Ldind_I4);
        else if (type == typeof (uint))
          il.Emit(OpCodes.Ldind_U4);
        else if (type == typeof (short))
          il.Emit(OpCodes.Ldind_I2);
        else if (type == typeof (ushort))
          il.Emit(OpCodes.Ldind_U2);
        else if (type == typeof (long) || type == typeof (ulong))
          il.Emit(OpCodes.Ldind_I8);
        else if (type == typeof (char))
          il.Emit(OpCodes.Ldind_I2);
        else if (type == typeof (bool))
          il.Emit(OpCodes.Ldind_I1);
        else if (type == typeof (float))
          il.Emit(OpCodes.Ldind_R4);
        else if (type == typeof (double))
          il.Emit(OpCodes.Ldind_R8);
        else
          il.Emit(OpCodes.Ldobj, type);
      }
      else
        il.Emit(OpCodes.Ldind_Ref);
    }

    internal static void EmitStoreValueIndirect(this ILGenerator il, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (type.IsValueType)
      {
        if (type == typeof (int))
          il.Emit(OpCodes.Stind_I4);
        else if (type == typeof (short))
          il.Emit(OpCodes.Stind_I2);
        else if (type == typeof (long) || type == typeof (ulong))
          il.Emit(OpCodes.Stind_I8);
        else if (type == typeof (char))
          il.Emit(OpCodes.Stind_I2);
        else if (type == typeof (bool))
          il.Emit(OpCodes.Stind_I1);
        else if (type == typeof (float))
          il.Emit(OpCodes.Stind_R4);
        else if (type == typeof (double))
          il.Emit(OpCodes.Stind_R8);
        else
          il.Emit(OpCodes.Stobj, type);
      }
      else
        il.Emit(OpCodes.Stind_Ref);
    }

    internal static void EmitLoadElement(this ILGenerator il, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      if (!type.IsValueType)
        il.Emit(OpCodes.Ldelem_Ref);
      else if (type.IsEnum)
      {
        il.Emit(OpCodes.Ldelem, type);
      }
      else
      {
        switch (Type.GetTypeCode(type))
        {
          case TypeCode.Boolean:
          case TypeCode.SByte:
            il.Emit(OpCodes.Ldelem_I1);
            break;
          case TypeCode.Char:
          case TypeCode.UInt16:
            il.Emit(OpCodes.Ldelem_U2);
            break;
          case TypeCode.Byte:
            il.Emit(OpCodes.Ldelem_U1);
            break;
          case TypeCode.Int16:
            il.Emit(OpCodes.Ldelem_I2);
            break;
          case TypeCode.Int32:
            il.Emit(OpCodes.Ldelem_I4);
            break;
          case TypeCode.UInt32:
            il.Emit(OpCodes.Ldelem_U4);
            break;
          case TypeCode.Int64:
          case TypeCode.UInt64:
            il.Emit(OpCodes.Ldelem_I8);
            break;
          case TypeCode.Single:
            il.Emit(OpCodes.Ldelem_R4);
            break;
          case TypeCode.Double:
            il.Emit(OpCodes.Ldelem_R8);
            break;
          default:
            il.Emit(OpCodes.Ldelem, type);
            break;
        }
      }
    }

    internal static void EmitType(this ILGenerator il, Type type)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      il.Emit(OpCodes.Ldtoken, type);
      il.Emit(OpCodes.Call, typeof (Type).GetMethod("GetTypeFromHandle"));
    }

    internal static void EmitFieldAddress(this ILGenerator il, FieldInfo fi)
    {
      ContractUtils.RequiresNotNull((object) fi, "fi");
      if (fi.IsStatic)
        il.Emit(OpCodes.Ldsflda, fi);
      else
        il.Emit(OpCodes.Ldflda, fi);
    }

    internal static void EmitFieldGet(this ILGenerator il, FieldInfo fi)
    {
      ContractUtils.RequiresNotNull((object) fi, "fi");
      if (fi.IsStatic)
        il.Emit(OpCodes.Ldsfld, fi);
      else
        il.Emit(OpCodes.Ldfld, fi);
    }

    internal static void EmitFieldSet(this ILGenerator il, FieldInfo fi)
    {
      ContractUtils.RequiresNotNull((object) fi, "fi");
      if (fi.IsStatic)
        il.Emit(OpCodes.Stsfld, fi);
      else
        il.Emit(OpCodes.Stfld, fi);
    }

    internal static void EmitNew(this ILGenerator il, ConstructorInfo ci)
    {
      ContractUtils.RequiresNotNull((object) ci, "ci");
      if (ci.DeclaringType.ContainsGenericParameters)
        throw Error.IllegalNewGenericParams((object) ci.DeclaringType);
      il.Emit(OpCodes.Newobj, ci);
    }

    internal static void EmitNew(this ILGenerator il, Type type, Type[] paramTypes)
    {
      ContractUtils.RequiresNotNull((object) type, "type");
      ContractUtils.RequiresNotNull((object) paramTypes, "paramTypes");
      ConstructorInfo constructor = type.GetConstructor(paramTypes);
      if (constructor == (ConstructorInfo) null)
        throw Error.TypeDoesNotHaveConstructorForTheSignature();
      ILGen.EmitNew(il, constructor);
    }

    internal static void EmitNull(this ILGenerator il)
    {
      il.Emit(OpCodes.Ldnull);
    }

    internal static void EmitString(this ILGenerator il, string value)
    {
      ContractUtils.RequiresNotNull((object) value, "value");
      il.Emit(OpCodes.Ldstr, value);
    }

    internal static void EmitBoolean(this ILGenerator il, bool value)
    {
      if (value)
        il.Emit(OpCodes.Ldc_I4_1);
      else
        il.Emit(OpCodes.Ldc_I4_0);
    }

    internal static void EmitChar(this ILGenerator il, char value)
    {
      ILGen.EmitInt(il, (int) value);
      il.Emit(OpCodes.Conv_U2);
    }

    internal static void EmitByte(this ILGenerator il, byte value)
    {
      ILGen.EmitInt(il, (int) value);
      il.Emit(OpCodes.Conv_U1);
    }

    internal static void EmitSByte(this ILGenerator il, sbyte value)
    {
      ILGen.EmitInt(il, (int) value);
      il.Emit(OpCodes.Conv_I1);
    }

    internal static void EmitShort(this ILGenerator il, short value)
    {
      ILGen.EmitInt(il, (int) value);
      il.Emit(OpCodes.Conv_I2);
    }

    internal static void EmitUShort(this ILGenerator il, ushort value)
    {
      ILGen.EmitInt(il, (int) value);
      il.Emit(OpCodes.Conv_U2);
    }

    internal static void EmitUInt(this ILGenerator il, uint value)
    {
      ILGen.EmitInt(il, (int) value);
      il.Emit(OpCodes.Conv_U4);
    }

    internal static void EmitLong(this ILGenerator il, long value)
    {
      il.Emit(OpCodes.Ldc_I8, value);
      il.Emit(OpCodes.Conv_I8);
    }

    internal static void EmitULong(this ILGenerator il, ulong value)
    {
      il.Emit(OpCodes.Ldc_I8, (long) value);
      il.Emit(OpCodes.Conv_U8);
    }

    internal static void EmitDouble(this ILGenerator il, double value)
    {
      il.Emit(OpCodes.Ldc_R8, value);
    }

    internal static void EmitSingle(this ILGenerator il, float value)
    {
      il.Emit(OpCodes.Ldc_R4, value);
    }

    internal static bool CanEmitConstant(object value, Type type)
    {
      if (value == null || ILGen.CanEmitILConstant(type))
        return true;
      Type t = value as Type;
      if (t != (Type) null && ILGen.ShouldLdtoken(t))
        return true;
      MethodBase mb = value as MethodBase;
      return mb != (MethodBase) null && ILGen.ShouldLdtoken(mb);
    }

    private static bool CanEmitILConstant(Type type)
    {
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
        case TypeCode.Decimal:
        case TypeCode.String:
          return true;
        default:
          return false;
      }
    }

    internal static void EmitConstant(this ILGenerator il, object value)
    {
      ILGen.EmitConstant(il, value, value.GetType());
    }

    internal static bool ShouldLdtoken(Type t)
    {
      if (!(t is TypeBuilder) && !t.IsGenericParameter)
        return t.IsVisible;
      else
        return true;
    }

    internal static bool ShouldLdtoken(MethodBase mb)
    {
      if (mb is DynamicMethod)
        return false;
      Type declaringType = mb.DeclaringType;
      if (!(declaringType == (Type) null))
        return ILGen.ShouldLdtoken(declaringType);
      else
        return true;
    }

    private static bool TryEmitILConstant(this ILGenerator il, object value, Type type)
    {
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Boolean:
          ILGen.EmitBoolean(il, (bool) value);
          return true;
        case TypeCode.Char:
          ILGen.EmitChar(il, (char) value);
          return true;
        case TypeCode.SByte:
          ILGen.EmitSByte(il, (sbyte) value);
          return true;
        case TypeCode.Byte:
          ILGen.EmitByte(il, (byte) value);
          return true;
        case TypeCode.Int16:
          ILGen.EmitShort(il, (short) value);
          return true;
        case TypeCode.UInt16:
          ILGen.EmitUShort(il, (ushort) value);
          return true;
        case TypeCode.Int32:
          ILGen.EmitInt(il, (int) value);
          return true;
        case TypeCode.UInt32:
          ILGen.EmitUInt(il, (uint) value);
          return true;
        case TypeCode.Int64:
          ILGen.EmitLong(il, (long) value);
          return true;
        case TypeCode.UInt64:
          ILGen.EmitULong(il, (ulong) value);
          return true;
        case TypeCode.Single:
          ILGen.EmitSingle(il, (float) value);
          return true;
        case TypeCode.Double:
          ILGen.EmitDouble(il, (double) value);
          return true;
        case TypeCode.Decimal:
          ILGen.EmitDecimal(il, (Decimal) value);
          return true;
        case TypeCode.String:
          ILGen.EmitString(il, (string) value);
          return true;
        default:
          return false;
      }
    }

    internal static void EmitConvertToType(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      if (TypeUtils.AreEquivalent(typeFrom, typeTo))
        return;
      if (typeFrom == typeof (void) || typeTo == typeof (void))
        throw ContractUtils.Unreachable;
      bool flag1 = TypeUtils.IsNullableType(typeFrom);
      bool flag2 = TypeUtils.IsNullableType(typeTo);
      Type nonNullableType1 = TypeUtils.GetNonNullableType(typeFrom);
      Type nonNullableType2 = TypeUtils.GetNonNullableType(typeTo);
      if (typeFrom.IsInterface || typeTo.IsInterface || (typeFrom == typeof (object) || typeTo == typeof (object)) || (typeFrom == typeof (Enum) || typeFrom == typeof (ValueType) || TypeUtils.IsLegalExplicitVariantDelegateConversion(typeFrom, typeTo)))
        ILGen.EmitCastToType(il, typeFrom, typeTo);
      else if (flag1 || flag2)
        ILGen.EmitNullableConversion(il, typeFrom, typeTo, isChecked);
      else if ((!TypeUtils.IsConvertible(typeFrom) || !TypeUtils.IsConvertible(typeTo)) && (nonNullableType1.IsAssignableFrom(nonNullableType2) || nonNullableType2.IsAssignableFrom(nonNullableType1)))
        ILGen.EmitCastToType(il, typeFrom, typeTo);
      else if (typeFrom.IsArray && typeTo.IsArray)
        ILGen.EmitCastToType(il, typeFrom, typeTo);
      else
        ILGen.EmitNumericConversion(il, typeFrom, typeTo, isChecked);
    }

    private static void EmitCastToType(this ILGenerator il, Type typeFrom, Type typeTo)
    {
      if (!typeFrom.IsValueType && typeTo.IsValueType)
        il.Emit(OpCodes.Unbox_Any, typeTo);
      else if (typeFrom.IsValueType && !typeTo.IsValueType)
      {
        il.Emit(OpCodes.Box, typeFrom);
        if (!(typeTo != typeof (object)))
          return;
        il.Emit(OpCodes.Castclass, typeTo);
      }
      else
      {
        if (typeFrom.IsValueType || typeTo.IsValueType)
          throw Error.InvalidCast((object) typeFrom, (object) typeTo);
        il.Emit(OpCodes.Castclass, typeTo);
      }
    }

    private static void EmitNumericConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      bool flag1 = TypeUtils.IsUnsigned(typeFrom);
      bool flag2 = TypeUtils.IsFloatingPoint(typeFrom);
      if (typeTo == typeof (float))
      {
        if (flag1)
          il.Emit(OpCodes.Conv_R_Un);
        il.Emit(OpCodes.Conv_R4);
      }
      else if (typeTo == typeof (double))
      {
        if (flag1)
          il.Emit(OpCodes.Conv_R_Un);
        il.Emit(OpCodes.Conv_R8);
      }
      else
      {
        TypeCode typeCode = Type.GetTypeCode(typeTo);
        if (isChecked)
        {
          if (flag1)
          {
            switch (typeCode)
            {
              case TypeCode.Char:
              case TypeCode.UInt16:
                il.Emit(OpCodes.Conv_Ovf_U2_Un);
                break;
              case TypeCode.SByte:
                il.Emit(OpCodes.Conv_Ovf_I1_Un);
                break;
              case TypeCode.Byte:
                il.Emit(OpCodes.Conv_Ovf_U1_Un);
                break;
              case TypeCode.Int16:
                il.Emit(OpCodes.Conv_Ovf_I2_Un);
                break;
              case TypeCode.Int32:
                il.Emit(OpCodes.Conv_Ovf_I4_Un);
                break;
              case TypeCode.UInt32:
                il.Emit(OpCodes.Conv_Ovf_U4_Un);
                break;
              case TypeCode.Int64:
                il.Emit(OpCodes.Conv_Ovf_I8_Un);
                break;
              case TypeCode.UInt64:
                il.Emit(OpCodes.Conv_Ovf_U8_Un);
                break;
              default:
                throw Error.UnhandledConvert((object) typeTo);
            }
          }
          else
          {
            switch (typeCode)
            {
              case TypeCode.Char:
              case TypeCode.UInt16:
                il.Emit(OpCodes.Conv_Ovf_U2);
                break;
              case TypeCode.SByte:
                il.Emit(OpCodes.Conv_Ovf_I1);
                break;
              case TypeCode.Byte:
                il.Emit(OpCodes.Conv_Ovf_U1);
                break;
              case TypeCode.Int16:
                il.Emit(OpCodes.Conv_Ovf_I2);
                break;
              case TypeCode.Int32:
                il.Emit(OpCodes.Conv_Ovf_I4);
                break;
              case TypeCode.UInt32:
                il.Emit(OpCodes.Conv_Ovf_U4);
                break;
              case TypeCode.Int64:
                il.Emit(OpCodes.Conv_Ovf_I8);
                break;
              case TypeCode.UInt64:
                il.Emit(OpCodes.Conv_Ovf_U8);
                break;
              default:
                throw Error.UnhandledConvert((object) typeTo);
            }
          }
        }
        else
        {
          switch (typeCode)
          {
            case TypeCode.Char:
            case TypeCode.UInt16:
              il.Emit(OpCodes.Conv_U2);
              break;
            case TypeCode.SByte:
              il.Emit(OpCodes.Conv_I1);
              break;
            case TypeCode.Byte:
              il.Emit(OpCodes.Conv_U1);
              break;
            case TypeCode.Int16:
              il.Emit(OpCodes.Conv_I2);
              break;
            case TypeCode.Int32:
              il.Emit(OpCodes.Conv_I4);
              break;
            case TypeCode.UInt32:
              il.Emit(OpCodes.Conv_U4);
              break;
            case TypeCode.Int64:
              if (flag1)
              {
                il.Emit(OpCodes.Conv_U8);
                break;
              }
              else
              {
                il.Emit(OpCodes.Conv_I8);
                break;
              }
            case TypeCode.UInt64:
              if (flag1 || flag2)
              {
                il.Emit(OpCodes.Conv_U8);
                break;
              }
              else
              {
                il.Emit(OpCodes.Conv_I8);
                break;
              }
            default:
              throw Error.UnhandledConvert((object) typeTo);
          }
        }
      }
    }

    private static void EmitNullableToNullableConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      Label label1 = new Label();
      Label label2 = new Label();
      LocalBuilder local1 = il.DeclareLocal(typeFrom);
      il.Emit(OpCodes.Stloc, local1);
      LocalBuilder local2 = il.DeclareLocal(typeTo);
      il.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitHasValue(il, typeFrom);
      Label label3 = il.DefineLabel();
      il.Emit(OpCodes.Brfalse_S, label3);
      il.Emit(OpCodes.Ldloca, local1);
      ILGen.EmitGetValueOrDefault(il, typeFrom);
      Type nonNullableType1 = TypeUtils.GetNonNullableType(typeFrom);
      Type nonNullableType2 = TypeUtils.GetNonNullableType(typeTo);
      ILGen.EmitConvertToType(il, nonNullableType1, nonNullableType2, isChecked);
      ConstructorInfo constructor = typeTo.GetConstructor(new Type[1]
      {
        nonNullableType2
      });
      il.Emit(OpCodes.Newobj, constructor);
      il.Emit(OpCodes.Stloc, local2);
      Label label4 = il.DefineLabel();
      il.Emit(OpCodes.Br_S, label4);
      il.MarkLabel(label3);
      il.Emit(OpCodes.Ldloca, local2);
      il.Emit(OpCodes.Initobj, typeTo);
      il.MarkLabel(label4);
      il.Emit(OpCodes.Ldloc, local2);
    }

    private static void EmitNonNullableToNullableConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      LocalBuilder local = il.DeclareLocal(typeTo);
      Type nonNullableType = TypeUtils.GetNonNullableType(typeTo);
      ILGen.EmitConvertToType(il, typeFrom, nonNullableType, isChecked);
      ConstructorInfo constructor = typeTo.GetConstructor(new Type[1]
      {
        nonNullableType
      });
      il.Emit(OpCodes.Newobj, constructor);
      il.Emit(OpCodes.Stloc, local);
      il.Emit(OpCodes.Ldloc, local);
    }

    private static void EmitNullableToNonNullableConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      if (typeTo.IsValueType)
        ILGen.EmitNullableToNonNullableStructConversion(il, typeFrom, typeTo, isChecked);
      else
        ILGen.EmitNullableToReferenceConversion(il, typeFrom);
    }

    private static void EmitNullableToNonNullableStructConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      LocalBuilder local = il.DeclareLocal(typeFrom);
      il.Emit(OpCodes.Stloc, local);
      il.Emit(OpCodes.Ldloca, local);
      ILGen.EmitGetValue(il, typeFrom);
      Type nonNullableType = TypeUtils.GetNonNullableType(typeFrom);
      ILGen.EmitConvertToType(il, nonNullableType, typeTo, isChecked);
    }

    private static void EmitNullableToReferenceConversion(this ILGenerator il, Type typeFrom)
    {
      il.Emit(OpCodes.Box, typeFrom);
    }

    private static void EmitNullableConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
    {
      bool flag1 = TypeUtils.IsNullableType(typeFrom);
      bool flag2 = TypeUtils.IsNullableType(typeTo);
      if (flag1 && flag2)
        ILGen.EmitNullableToNullableConversion(il, typeFrom, typeTo, isChecked);
      else if (flag1)
        ILGen.EmitNullableToNonNullableConversion(il, typeFrom, typeTo, isChecked);
      else
        ILGen.EmitNonNullableToNullableConversion(il, typeFrom, typeTo, isChecked);
    }

    internal static void EmitHasValue(this ILGenerator il, Type nullableType)
    {
      MethodInfo method = nullableType.GetMethod("get_HasValue", BindingFlags.Instance | BindingFlags.Public);
      il.Emit(OpCodes.Call, method);
    }

    internal static void EmitGetValue(this ILGenerator il, Type nullableType)
    {
      MethodInfo method = nullableType.GetMethod("get_Value", BindingFlags.Instance | BindingFlags.Public);
      il.Emit(OpCodes.Call, method);
    }

    internal static void EmitGetValueOrDefault(this ILGenerator il, Type nullableType)
    {
      MethodInfo method = nullableType.GetMethod("GetValueOrDefault", Type.EmptyTypes);
      il.Emit(OpCodes.Call, method);
    }

    internal static void EmitArray(this ILGenerator il, Type elementType, int count, Action<int> emit)
    {
      ContractUtils.RequiresNotNull((object) elementType, "elementType");
      ContractUtils.RequiresNotNull((object) emit, "emit");
      if (count < 0)
        throw Error.CountCannotBeNegative();
      ILGen.EmitInt(il, count);
      il.Emit(OpCodes.Newarr, elementType);
      for (int index = 0; index < count; ++index)
      {
        il.Emit(OpCodes.Dup);
        ILGen.EmitInt(il, index);
        emit(index);
        ILGen.EmitStoreElement(il, elementType);
      }
    }

    internal static void EmitArray(this ILGenerator il, Type arrayType)
    {
      ContractUtils.RequiresNotNull((object) arrayType, "arrayType");
      if (!arrayType.IsArray)
        throw Error.ArrayTypeMustBeArray();
      int arrayRank = arrayType.GetArrayRank();
      if (arrayRank == 1)
      {
        il.Emit(OpCodes.Newarr, arrayType.GetElementType());
      }
      else
      {
        Type[] paramTypes = new Type[arrayRank];
        for (int index = 0; index < arrayRank; ++index)
          paramTypes[index] = typeof (int);
        ILGen.EmitNew(il, arrayType, paramTypes);
      }
    }

    internal static void EmitDecimal(this ILGenerator il, Decimal value)
    {
      if (Decimal.Truncate(value) == value)
      {
        if (new Decimal(int.MinValue) <= value && value <= new Decimal(int.MaxValue))
        {
          int num = Decimal.ToInt32(value);
          ILGen.EmitInt(il, num);
          ILGen.EmitNew(il, typeof (Decimal).GetConstructor(new Type[1]
          {
            typeof (int)
          }));
        }
        else if (new Decimal(long.MinValue) <= value && value <= new Decimal(long.MaxValue))
        {
          long num = Decimal.ToInt64(value);
          ILGen.EmitLong(il, num);
          ILGen.EmitNew(il, typeof (Decimal).GetConstructor(new Type[1]
          {
            typeof (long)
          }));
        }
        else
          ILGen.EmitDecimalBits(il, value);
      }
      else
        ILGen.EmitDecimalBits(il, value);
    }

    private static void EmitDecimalBits(this ILGenerator il, Decimal value)
    {
      int[] bits = Decimal.GetBits(value);
      ILGen.EmitInt(il, bits[0]);
      ILGen.EmitInt(il, bits[1]);
      ILGen.EmitInt(il, bits[2]);
      ILGen.EmitBoolean(il, ((long) bits[3] & 2147483648L) != 0L);
      ILGen.EmitByte(il, (byte) (bits[3] >> 16));
      ILGen.EmitNew(il, typeof (Decimal).GetConstructor(new Type[5]
      {
        typeof (int),
        typeof (int),
        typeof (int),
        typeof (bool),
        typeof (byte)
      }));
    }

    internal static void EmitDefault(this ILGenerator il, Type type)
    {
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Empty:
        case TypeCode.DBNull:
        case TypeCode.String:
          il.Emit(OpCodes.Ldnull);
          break;
        case TypeCode.Object:
        case TypeCode.DateTime:
          if (type.IsValueType)
          {
            LocalBuilder local = il.DeclareLocal(type);
            il.Emit(OpCodes.Ldloca, local);
            il.Emit(OpCodes.Initobj, type);
            il.Emit(OpCodes.Ldloc, local);
            break;
          }
          else
          {
            il.Emit(OpCodes.Ldnull);
            break;
          }
        case TypeCode.Boolean:
        case TypeCode.Char:
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
          il.Emit(OpCodes.Ldc_I4_0);
          break;
        case TypeCode.Int64:
        case TypeCode.UInt64:
          il.Emit(OpCodes.Ldc_I4_0);
          il.Emit(OpCodes.Conv_I8);
          break;
        case TypeCode.Single:
          il.Emit(OpCodes.Ldc_R4, 0.0f);
          break;
        case TypeCode.Double:
          il.Emit(OpCodes.Ldc_R8, 0.0);
          break;
        case TypeCode.Decimal:
          il.Emit(OpCodes.Ldc_I4_0);
          il.Emit(OpCodes.Newobj, typeof (Decimal).GetConstructor(new Type[1]
          {
            typeof (int)
          }));
          break;
        default:
          throw ContractUtils.Unreachable;
      }
    }
  }
}
