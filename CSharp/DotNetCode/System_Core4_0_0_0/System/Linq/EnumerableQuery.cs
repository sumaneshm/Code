// Type: System.Linq.EnumerableQuery
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public abstract class EnumerableQuery
  {
    internal abstract Expression Expression { get; }

    internal abstract IEnumerable Enumerable { get; }

    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EnumerableQuery()
    {
    }

    internal static IQueryable Create(Type elementType, Expression expression)
    {
      return (IQueryable) Activator.CreateInstance(typeof (EnumerableQuery<>).MakeGenericType(new Type[1]
      {
        elementType
      }), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new object[1]
      {
        (object) expression
      }, (CultureInfo) null);
    }

    internal static IQueryable Create(Type elementType, IEnumerable sequence)
    {
      return (IQueryable) Activator.CreateInstance(typeof (EnumerableQuery<>).MakeGenericType(new Type[1]
      {
        elementType
      }), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new object[1]
      {
        (object) sequence
      }, (CultureInfo) null);
    }
  }
}
