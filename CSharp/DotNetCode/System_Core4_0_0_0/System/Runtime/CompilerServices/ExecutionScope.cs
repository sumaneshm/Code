// Type: System.Runtime.CompilerServices.ExecutionScope
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Linq.Expressions;

namespace System.Runtime.CompilerServices
{
  [Obsolete("do not use this type", true)]
  public class ExecutionScope
  {
    public ExecutionScope Parent;
    public object[] Globals;
    public object[] Locals;

    internal ExecutionScope()
    {
      this.Parent = (ExecutionScope) null;
      this.Globals = (object[]) null;
      this.Locals = (object[]) null;
    }

    public object[] CreateHoistedLocals()
    {
      throw new NotSupportedException();
    }

    public Delegate CreateDelegate(int indexLambda, object[] locals)
    {
      throw new NotSupportedException();
    }

    public Expression IsolateExpression(Expression expression, object[] locals)
    {
      throw new NotSupportedException();
    }
  }
}
