// Type: System.Linq.Expressions.ListArgumentProvider
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Dynamic.Utils;

namespace System.Linq.Expressions
{
  internal class ListArgumentProvider : IList<Expression>, ICollection<Expression>, IEnumerable<Expression>, IEnumerable
  {
    private readonly IArgumentProvider _provider;
    private readonly Expression _arg0;

    public Expression this[int index]
    {
      get
      {
        if (index == 0)
          return this._arg0;
        else
          return this._provider.GetArgument(index);
      }
      set
      {
        throw ContractUtils.Unreachable;
      }
    }

    public int Count
    {
      get
      {
        return this._provider.ArgumentCount;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal ListArgumentProvider(IArgumentProvider provider, Expression arg0)
    {
      this._provider = provider;
      this._arg0 = arg0;
    }

    public int IndexOf(Expression item)
    {
      if (this._arg0 == item)
        return 0;
      for (int index = 1; index < this._provider.ArgumentCount; ++index)
      {
        if (this._provider.GetArgument(index) == item)
          return index;
      }
      return -1;
    }

    public void Insert(int index, Expression item)
    {
      throw ContractUtils.Unreachable;
    }

    public void RemoveAt(int index)
    {
      throw ContractUtils.Unreachable;
    }

    public void Add(Expression item)
    {
      throw ContractUtils.Unreachable;
    }

    public void Clear()
    {
      throw ContractUtils.Unreachable;
    }

    public bool Contains(Expression item)
    {
      return this.IndexOf(item) != -1;
    }

    public void CopyTo(Expression[] array, int arrayIndex)
    {
      array[arrayIndex++] = this._arg0;
      for (int index = 1; index < this._provider.ArgumentCount; ++index)
        array[arrayIndex++] = this._provider.GetArgument(index);
    }

    public bool Remove(Expression item)
    {
      throw ContractUtils.Unreachable;
    }

    public IEnumerator<Expression> GetEnumerator()
    {
      yield return this._arg0;
      for (int i = 1; i < this._provider.ArgumentCount; ++i)
        yield return this._provider.GetArgument(i);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      yield return (object) this._arg0;
      for (int i = 1; i < this._provider.ArgumentCount; ++i)
        yield return (object) this._provider.GetArgument(i);
    }
  }
}
