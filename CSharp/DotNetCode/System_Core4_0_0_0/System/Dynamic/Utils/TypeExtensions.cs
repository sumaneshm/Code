// Type: System.Dynamic.Utils.TypeExtensions
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Dynamic.Utils
{
  internal static class TypeExtensions
  {
    private static readonly CacheDict<MethodBase, ParameterInfo[]> _ParamInfoCache = new CacheDict<MethodBase, ParameterInfo[]>(75);

    static TypeExtensions()
    {
    }

    internal static Type GetReturnType(this MethodBase mi)
    {
      if (!mi.IsConstructor)
        return ((MethodInfo) mi).ReturnType;
      else
        return mi.DeclaringType;
    }

    internal static ParameterInfo[] GetParametersCached(this MethodBase method)
    {
      CacheDict<MethodBase, ParameterInfo[]> cacheDict = TypeExtensions._ParamInfoCache;
      ParameterInfo[] parameters;
      if (!cacheDict.TryGetValue(method, out parameters))
      {
        parameters = method.GetParameters();
        Type declaringType = method.DeclaringType;
        if (declaringType != (Type) null && TypeUtils.CanCache(declaringType))
          cacheDict[method] = parameters;
      }
      return parameters;
    }

    internal static bool IsByRefParameter(this ParameterInfo pi)
    {
      if (pi.ParameterType.IsByRef)
        return true;
      else
        return (pi.Attributes & ParameterAttributes.Out) == ParameterAttributes.Out;
    }

    internal static Delegate CreateDelegate(this MethodInfo methodInfo, Type delegateType, object target)
    {
      DynamicMethod dynamicMethod = methodInfo as DynamicMethod;
      if ((MethodInfo) dynamicMethod != (MethodInfo) null)
        return dynamicMethod.CreateDelegate(delegateType, target);
      else
        return Delegate.CreateDelegate(delegateType, target, methodInfo);
    }

    internal static MethodInfo GetMethodValidated(this Type type, string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
    {
      MethodInfo method = type.GetMethod(name, bindingAttr, binder, types, modifiers);
      if (!TypeExtensions.MatchesArgumentTypes(method, types))
        return (MethodInfo) null;
      else
        return method;
    }

    private static bool MatchesArgumentTypes(this MethodInfo mi, Type[] argTypes)
    {
      if (mi == (MethodInfo) null || argTypes == null)
        return false;
      ParameterInfo[] parameters = mi.GetParameters();
      if (parameters.Length != argTypes.Length)
        return false;
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (!TypeUtils.AreReferenceAssignable(parameters[index].ParameterType, argTypes[index]))
          return false;
      }
      return true;
    }
  }
}
