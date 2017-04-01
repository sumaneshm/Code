// Type: System.Linq.Expressions.ArgumentProviderOps
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;

namespace System.Linq.Expressions
{
  internal static class ArgumentProviderOps
  {
    internal static T[] Map<T>(this IArgumentProvider collection, Func<Expression, T> select)
    {
      T[] objArray = new T[collection.ArgumentCount];
      int num = 0;
      for (int index = 0; index < num; ++index)
        objArray[index] = select(collection.GetArgument(index));
      return objArray;
    }
  }
}
