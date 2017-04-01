// Type: System.Linq.TypeHelper
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;

namespace System.Linq
{
  internal static class TypeHelper
  {
    internal static Type FindGenericType(Type definition, Type type)
    {
      for (; type != (Type) null && type != typeof (object); type = type.BaseType)
      {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
          return type;
        if (definition.IsInterface)
        {
          foreach (Type type1 in type.GetInterfaces())
          {
            Type genericType = TypeHelper.FindGenericType(definition, type1);
            if (genericType != (Type) null)
              return genericType;
          }
        }
      }
      return (Type) null;
    }

    internal static bool IsEnumerableType(Type enumerableType)
    {
      return TypeHelper.FindGenericType(typeof (IEnumerable<>), enumerableType) != (Type) null;
    }

    internal static bool IsKindOfGeneric(Type type, Type definition)
    {
      return TypeHelper.FindGenericType(definition, type) != (Type) null;
    }

    internal static Type GetElementType(Type enumerableType)
    {
      Type genericType = TypeHelper.FindGenericType(typeof (IEnumerable<>), enumerableType);
      if (genericType != (Type) null)
        return genericType.GetGenericArguments()[0];
      else
        return enumerableType;
    }

    internal static bool IsNullableType(Type type)
    {
      if (type != (Type) null && type.IsGenericType)
        return type.GetGenericTypeDefinition() == typeof (Nullable<>);
      else
        return false;
    }

    internal static Type GetNonNullableType(Type type)
    {
      if (TypeHelper.IsNullableType(type))
        return type.GetGenericArguments()[0];
      else
        return type;
    }
  }
}
