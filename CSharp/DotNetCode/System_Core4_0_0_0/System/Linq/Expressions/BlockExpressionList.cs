// Type: System.Linq.Expressions.BlockExpressionList
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections;
using System.Collections.Generic;
using System.Dynamic.Utils;

namespace System.Linq.Expressions
{
  internal class BlockExpressionList : IList<Expression>, ICollection<Expression>, IEnumerable<Expression>, IEnumerable
  {
    private readonly BlockExpression _block;
    private readonly Expression _arg0;

    public Expression this[int index]
    {
      get
      {
        if (index == 0)
          return this._arg0;
        else
          return this._block.GetExpression(index);
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
        return this._block.ExpressionCount;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    internal BlockExpressionList(BlockExpression provider, Expression arg0)
    {
      this._block = provider;
      this._arg0 = arg0;
    }

    public int IndexOf(Expression item)
    {
      if (this._arg0 == item)
        return 0;
      for (int index = 1; index < this._block.ExpressionCount; ++index)
      {
        if (this._block.GetExpression(index) == item)
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
      for (int index = 1; index < this._block.ExpressionCount; ++index)
        array[arrayIndex++] = this._block.GetExpression(index);
    }

    public bool Remove(Expression item)
    {
      throw ContractUtils.Unreachable;
    }

    public IEnumerator<Expression> GetEnumerator()
    {
      yield return this._arg0;
      for (int i = 1; i < this._block.ExpressionCount; ++i)
        yield return this._block.GetExpression(i);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      yield return (object) this._arg0;
      for (int i = 1; i < this._block.ExpressionCount; ++i)
        yield return (object) this._block.GetExpression(i);
    }
  }
}
