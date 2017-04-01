// Type: System.Runtime.CompilerServices.DynamicAttribute
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  [__DynamicallyInvokable]
  public sealed class DynamicAttribute : Attribute
  {
    private readonly bool[] _transformFlags;

    [__DynamicallyInvokable]
    public IList<bool> TransformFlags
    {
      [__DynamicallyInvokable] get
      {
        return (IList<bool>) Array.AsReadOnly<bool>(this._transformFlags);
      }
    }

    [__DynamicallyInvokable]
    public DynamicAttribute()
    {
      this._transformFlags = new bool[1]
      {
        true
      };
    }

    [__DynamicallyInvokable]
    public DynamicAttribute(bool[] transformFlags)
    {
      if (transformFlags == null)
        throw new ArgumentNullException("transformFlags");
      this._transformFlags = transformFlags;
    }
  }
}
