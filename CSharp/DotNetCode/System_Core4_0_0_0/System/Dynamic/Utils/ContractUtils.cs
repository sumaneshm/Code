// Type: System.Dynamic.Utils.ContractUtils
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace System.Dynamic.Utils
{
  internal static class ContractUtils
  {
    internal static Exception Unreachable
    {
      get
      {
        return (Exception) new InvalidOperationException("Code supposed to be unreachable");
      }
    }

    internal static void Requires(bool precondition, string paramName)
    {
      if (!precondition)
        throw new ArgumentException(Strings.InvalidArgumentValue, paramName);
    }

    internal static void RequiresNotNull(object value, string paramName)
    {
      if (value == null)
        throw new ArgumentNullException(paramName);
    }

    internal static void RequiresNotEmpty<T>(ICollection<T> collection, string paramName)
    {
      ContractUtils.RequiresNotNull((object) collection, paramName);
      if (collection.Count == 0)
        throw new ArgumentException(Strings.NonEmptyCollectionRequired, paramName);
    }

    internal static void RequiresArrayRange<T>(IList<T> array, int offset, int count, string offsetName, string countName)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException(countName);
      if (offset < 0 || array.Count - offset < count)
        throw new ArgumentOutOfRangeException(offsetName);
    }

    internal static void RequiresNotNullItems<T>(IList<T> array, string arrayName)
    {
      ContractUtils.RequiresNotNull((object) array, arrayName);
      for (int index = 0; index < array.Count; ++index)
      {
        if ((object) array[index] == null)
          throw new ArgumentNullException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0}[{1}]", new object[2]
          {
            (object) arrayName,
            (object) index
          }));
      }
    }

    internal static void Requires(bool precondition)
    {
      if (!precondition)
        throw new ArgumentException(Strings.MethodPreconditionViolated);
    }
  }
}
