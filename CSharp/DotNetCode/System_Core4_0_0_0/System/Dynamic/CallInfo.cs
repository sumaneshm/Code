// Type: System.Dynamic.CallInfo
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Runtime;

namespace System.Dynamic
{
  [__DynamicallyInvokable]
  public sealed class CallInfo
  {
    private readonly int _argCount;
    private readonly ReadOnlyCollection<string> _argNames;

    [__DynamicallyInvokable]
    public int ArgumentCount
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._argCount;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<string> ArgumentNames
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._argNames;
      }
    }

    [__DynamicallyInvokable]
    public CallInfo(int argCount, params string[] argNames)
      : this(argCount, (IEnumerable<string>) argNames)
    {
    }

    [__DynamicallyInvokable]
    public CallInfo(int argCount, IEnumerable<string> argNames)
    {
      ContractUtils.RequiresNotNull((object) argNames, "argNames");
      ReadOnlyCollection<string> readOnlyCollection = CollectionExtensions.ToReadOnly<string>(argNames);
      if (argCount < readOnlyCollection.Count)
        throw Error.ArgCntMustBeGreaterThanNameCnt();
      ContractUtils.RequiresNotNullItems<string>((IList<string>) readOnlyCollection, "argNames");
      this._argCount = argCount;
      this._argNames = readOnlyCollection;
    }

    [__DynamicallyInvokable]
    public override int GetHashCode()
    {
      return this._argCount ^ CollectionExtensions.ListHashCode<string>((IEnumerable<string>) this._argNames);
    }

    [__DynamicallyInvokable]
    public override bool Equals(object obj)
    {
      CallInfo callInfo = obj as CallInfo;
      if (this._argCount == callInfo._argCount)
        return CollectionExtensions.ListEquals<string>((ICollection<string>) this._argNames, (ICollection<string>) callInfo._argNames);
      else
        return false;
    }
  }
}
