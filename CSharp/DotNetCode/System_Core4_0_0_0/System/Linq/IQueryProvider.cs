// Type: System.Linq.IQueryProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Linq.Expressions;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public interface IQueryProvider
  {
    [__DynamicallyInvokable]
    IQueryable CreateQuery(Expression expression);

    [__DynamicallyInvokable]
    IQueryable<TElement> CreateQuery<TElement>(Expression expression);

    [__DynamicallyInvokable]
    object Execute(Expression expression);

    [__DynamicallyInvokable]
    TResult Execute<TResult>(Expression expression);
  }
}
