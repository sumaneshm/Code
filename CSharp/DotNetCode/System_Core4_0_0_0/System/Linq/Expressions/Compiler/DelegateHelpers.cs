// Type: System.Linq.Expressions.Compiler.DelegateHelpers
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Dynamic.Utils;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Compiler
{
  internal static class DelegateHelpers
  {
    private static readonly Type[] _DelegateCtorSignature = new Type[2]
    {
      typeof (object),
      typeof (IntPtr)
    };
    private static DelegateHelpers.TypeInfo _DelegateCache = new DelegateHelpers.TypeInfo();
    private const MethodAttributes CtorAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName;
    private const MethodImplAttributes ImplAttributes = MethodImplAttributes.CodeTypeMask;
    private const MethodAttributes InvokeAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask;
    private const int MaximumArity = 17;

    static DelegateHelpers()
    {
    }

    internal static Type GetFuncType(Type[] types)
    {
      switch (types.Length)
      {
        case 1:
          return typeof (Func<>).MakeGenericType(types);
        case 2:
          return typeof (Func<,>).MakeGenericType(types);
        case 3:
          return typeof (Func<,,>).MakeGenericType(types);
        case 4:
          return typeof (Func<,,,>).MakeGenericType(types);
        case 5:
          return typeof (Func<,,,,>).MakeGenericType(types);
        case 6:
          return typeof (Func<,,,,,>).MakeGenericType(types);
        case 7:
          return typeof (Func<,,,,,,>).MakeGenericType(types);
        case 8:
          return typeof (Func<,,,,,,,>).MakeGenericType(types);
        case 9:
          return typeof (Func<,,,,,,,,>).MakeGenericType(types);
        case 10:
          return typeof (Func<,,,,,,,,,>).MakeGenericType(types);
        case 11:
          return typeof (Func<,,,,,,,,,,>).MakeGenericType(types);
        case 12:
          return typeof (Func<,,,,,,,,,,,>).MakeGenericType(types);
        case 13:
          return typeof (Func<,,,,,,,,,,,,>).MakeGenericType(types);
        case 14:
          return typeof (Func<,,,,,,,,,,,,,>).MakeGenericType(types);
        case 15:
          return typeof (Func<,,,,,,,,,,,,,,>).MakeGenericType(types);
        case 16:
          return typeof (Func<,,,,,,,,,,,,,,,>).MakeGenericType(types);
        case 17:
          return typeof (Func<,,,,,,,,,,,,,,,,>).MakeGenericType(types);
        default:
          return (Type) null;
      }
    }

    internal static Type GetActionType(Type[] types)
    {
      switch (types.Length)
      {
        case 0:
          return typeof (Action);
        case 1:
          return typeof (Action<>).MakeGenericType(types);
        case 2:
          return typeof (Action<,>).MakeGenericType(types);
        case 3:
          return typeof (Action<,,>).MakeGenericType(types);
        case 4:
          return typeof (Action<,,,>).MakeGenericType(types);
        case 5:
          return typeof (Action<,,,,>).MakeGenericType(types);
        case 6:
          return typeof (Action<,,,,,>).MakeGenericType(types);
        case 7:
          return typeof (Action<,,,,,,>).MakeGenericType(types);
        case 8:
          return typeof (Action<,,,,,,,>).MakeGenericType(types);
        case 9:
          return typeof (Action<,,,,,,,,>).MakeGenericType(types);
        case 10:
          return typeof (Action<,,,,,,,,,>).MakeGenericType(types);
        case 11:
          return typeof (Action<,,,,,,,,,,>).MakeGenericType(types);
        case 12:
          return typeof (Action<,,,,,,,,,,,>).MakeGenericType(types);
        case 13:
          return typeof (Action<,,,,,,,,,,,,>).MakeGenericType(types);
        case 14:
          return typeof (Action<,,,,,,,,,,,,,>).MakeGenericType(types);
        case 15:
          return typeof (Action<,,,,,,,,,,,,,,>).MakeGenericType(types);
        case 16:
          return typeof (Action<,,,,,,,,,,,,,,,>).MakeGenericType(types);
        default:
          return (Type) null;
      }
    }

    private static Type MakeNewCustomDelegate(Type[] types)
    {
      Type returnType = types[types.Length - 1];
      Type[] parameterTypes = CollectionExtensions.RemoveLast<Type>(types);
      TypeBuilder typeBuilder = AssemblyGen.DefineDelegateType("Delegate" + (object) types.Length);
      typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName, CallingConventions.Standard, DelegateHelpers._DelegateCtorSignature).SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
      typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask, returnType, parameterTypes).SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
      return typeBuilder.CreateType();
    }

    internal static Type MakeDelegateType(Type[] types)
    {
      lock (DelegateHelpers._DelegateCache)
      {
        DelegateHelpers.TypeInfo local_0 = DelegateHelpers._DelegateCache;
        for (int local_1 = 0; local_1 < types.Length; ++local_1)
          local_0 = DelegateHelpers.NextTypeInfo(types[local_1], local_0);
        if (local_0.DelegateType == (Type) null)
          local_0.DelegateType = DelegateHelpers.MakeNewDelegate((Type[]) types.Clone());
        return local_0.DelegateType;
      }
    }

    internal static Type MakeCallSiteDelegate(ReadOnlyCollection<Expression> types, Type returnType)
    {
      lock (DelegateHelpers._DelegateCache)
      {
        DelegateHelpers.TypeInfo local_0_1 = DelegateHelpers.NextTypeInfo(typeof (CallSite), DelegateHelpers._DelegateCache);
        for (int local_1 = 0; local_1 < types.Count; ++local_1)
          local_0_1 = DelegateHelpers.NextTypeInfo(types[local_1].Type, local_0_1);
        DelegateHelpers.TypeInfo local_0_2 = DelegateHelpers.NextTypeInfo(returnType, local_0_1);
        if (local_0_2.DelegateType == (Type) null)
          local_0_2.MakeDelegateType(returnType, (IList<Expression>) types);
        return local_0_2.DelegateType;
      }
    }

    internal static Type MakeDeferredSiteDelegate(DynamicMetaObject[] args, Type returnType)
    {
      lock (DelegateHelpers._DelegateCache)
      {
        DelegateHelpers.TypeInfo local_0_1 = DelegateHelpers.NextTypeInfo(typeof (CallSite), DelegateHelpers._DelegateCache);
        for (int local_1 = 0; local_1 < args.Length; ++local_1)
        {
          DynamicMetaObject local_2 = args[local_1];
          Type local_3 = local_2.Expression.Type;
          if (DelegateHelpers.IsByRef(local_2))
            local_3 = local_3.MakeByRefType();
          local_0_1 = DelegateHelpers.NextTypeInfo(local_3, local_0_1);
        }
        DelegateHelpers.TypeInfo local_0_2 = DelegateHelpers.NextTypeInfo(returnType, local_0_1);
        if (local_0_2.DelegateType == (Type) null)
        {
          Type[] local_4 = new Type[args.Length + 2];
          local_4[0] = typeof (CallSite);
          local_4[local_4.Length - 1] = returnType;
          for (int local_5 = 0; local_5 < args.Length; ++local_5)
          {
            DynamicMetaObject local_6 = args[local_5];
            Type local_7 = local_6.Expression.Type;
            if (DelegateHelpers.IsByRef(local_6))
              local_7 = local_7.MakeByRefType();
            local_4[local_5 + 1] = local_7;
          }
          local_0_2.DelegateType = DelegateHelpers.MakeNewDelegate(local_4);
        }
        return local_0_2.DelegateType;
      }
    }

    private static bool IsByRef(DynamicMetaObject mo)
    {
      ParameterExpression parameterExpression = mo.Expression as ParameterExpression;
      if (parameterExpression != null)
        return parameterExpression.IsByRef;
      else
        return false;
    }

    internal static DelegateHelpers.TypeInfo NextTypeInfo(Type initialArg)
    {
      lock (DelegateHelpers._DelegateCache)
        return DelegateHelpers.NextTypeInfo(initialArg, DelegateHelpers._DelegateCache);
    }

    internal static DelegateHelpers.TypeInfo GetNextTypeInfo(Type initialArg, DelegateHelpers.TypeInfo curTypeInfo)
    {
      lock (DelegateHelpers._DelegateCache)
        return DelegateHelpers.NextTypeInfo(initialArg, curTypeInfo);
    }

    private static DelegateHelpers.TypeInfo NextTypeInfo(Type initialArg, DelegateHelpers.TypeInfo curTypeInfo)
    {
      Type index = initialArg;
      if (curTypeInfo.TypeChain == null)
        curTypeInfo.TypeChain = new Dictionary<Type, DelegateHelpers.TypeInfo>();
      DelegateHelpers.TypeInfo typeInfo;
      if (!curTypeInfo.TypeChain.TryGetValue(index, out typeInfo))
      {
        typeInfo = new DelegateHelpers.TypeInfo();
        if (TypeUtils.CanCache(index))
          curTypeInfo.TypeChain[index] = typeInfo;
      }
      return typeInfo;
    }

    private static Type MakeNewDelegate(Type[] types)
    {
      if (types.Length > 17 || Enumerable.Any<Type>((IEnumerable<Type>) types, (Func<Type, bool>) (t => t.IsByRef)))
        return DelegateHelpers.MakeNewCustomDelegate(types);
      else
        return !(types[types.Length - 1] == typeof (void)) ? DelegateHelpers.GetFuncType(types) : DelegateHelpers.GetActionType(CollectionExtensions.RemoveLast<Type>(types));
    }

    internal class TypeInfo
    {
      public Type DelegateType;
      public Dictionary<Type, DelegateHelpers.TypeInfo> TypeChain;

      public Type MakeDelegateType(Type retType, params Expression[] args)
      {
        return this.MakeDelegateType(retType, (IList<Expression>) args);
      }

      public Type MakeDelegateType(Type retType, IList<Expression> args)
      {
        Type[] types = new Type[args.Count + 2];
        types[0] = typeof (CallSite);
        types[types.Length - 1] = retType;
        for (int index = 0; index < args.Count; ++index)
          types[index + 1] = args[index].Type;
        return this.DelegateType = DelegateHelpers.MakeNewDelegate(types);
      }
    }
  }
}
