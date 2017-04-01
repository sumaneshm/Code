// Type: System.Linq.EnumerableExecutor
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public abstract class EnumerableExecutor
  {
    [__DynamicallyInvokable]
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected EnumerableExecutor()
    {
    }

    internal abstract object ExecuteBoxed();

    internal static EnumerableExecutor Create(Expression expression)
    {
      return (EnumerableExecutor) Activator.CreateInstance(typeof (EnumerableExecutor<>).MakeGenericType(new Type[1]
      {
        expression.Type
      }), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new object[1]
      {
        (object) expression
      }, (CultureInfo) null);
    }
  }
}
