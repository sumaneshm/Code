// Type: System.Linq.Expressions.Compiler.BoundConstants
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic.Utils;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class BoundConstants
  {
    private readonly List<object> _values = new List<object>();
    private readonly Dictionary<object, int> _indexes = new Dictionary<object, int>((IEqualityComparer<object>) ReferenceEqualityComparer<object>.Instance);
    private readonly Dictionary<BoundConstants.TypedConstant, int> _references = new Dictionary<BoundConstants.TypedConstant, int>();
    private readonly Dictionary<BoundConstants.TypedConstant, LocalBuilder> _cache = new Dictionary<BoundConstants.TypedConstant, LocalBuilder>();

    internal int Count
    {
      get
      {
        return this._values.Count;
      }
    }

    internal object[] ToArray()
    {
      return this._values.ToArray();
    }

    internal void AddReference(object value, Type type)
    {
      if (!this._indexes.ContainsKey(value))
      {
        this._indexes.Add(value, this._values.Count);
        this._values.Add(value);
      }
      Helpers.IncrementCount<BoundConstants.TypedConstant>(new BoundConstants.TypedConstant(value, type), this._references);
    }

    internal void EmitConstant(LambdaCompiler lc, object value, Type type)
    {
      if (!lc.CanEmitBoundConstants)
        throw Error.CannotCompileConstant(value);
      LocalBuilder local;
      if (this._cache.TryGetValue(new BoundConstants.TypedConstant(value, type), out local))
      {
        lc.IL.Emit(OpCodes.Ldloc, local);
      }
      else
      {
        BoundConstants.EmitConstantsArray(lc);
        this.EmitConstantFromArray(lc, value, type);
      }
    }

    internal void EmitCacheConstants(LambdaCompiler lc)
    {
      int num = 0;
      foreach (KeyValuePair<BoundConstants.TypedConstant, int> keyValuePair in this._references)
      {
        if (!lc.CanEmitBoundConstants)
          throw Error.CannotCompileConstant(keyValuePair.Key.Value);
        if (BoundConstants.ShouldCache(keyValuePair.Value))
          ++num;
      }
      if (num == 0)
        return;
      BoundConstants.EmitConstantsArray(lc);
      this._cache.Clear();
      foreach (KeyValuePair<BoundConstants.TypedConstant, int> keyValuePair in this._references)
      {
        if (BoundConstants.ShouldCache(keyValuePair.Value))
        {
          if (--num > 0)
            lc.IL.Emit(OpCodes.Dup);
          LocalBuilder local = lc.IL.DeclareLocal(keyValuePair.Key.Type);
          this.EmitConstantFromArray(lc, keyValuePair.Key.Value, local.LocalType);
          lc.IL.Emit(OpCodes.Stloc, local);
          this._cache.Add(keyValuePair.Key, local);
        }
      }
    }

    private static bool ShouldCache(int refCount)
    {
      return refCount > 2;
    }

    private static void EmitConstantsArray(LambdaCompiler lc)
    {
      lc.EmitClosureArgument();
      lc.IL.Emit(OpCodes.Ldfld, typeof (Closure).GetField("Constants"));
    }

    private void EmitConstantFromArray(LambdaCompiler lc, object value, Type type)
    {
      int count;
      if (!this._indexes.TryGetValue(value, out count))
      {
        this._indexes.Add(value, count = this._values.Count);
        this._values.Add(value);
      }
      ILGen.EmitInt(lc.IL, count);
      lc.IL.Emit(OpCodes.Ldelem_Ref);
      if (type.IsValueType)
      {
        lc.IL.Emit(OpCodes.Unbox_Any, type);
      }
      else
      {
        if (!(type != typeof (object)))
          return;
        lc.IL.Emit(OpCodes.Castclass, type);
      }
    }

    private struct TypedConstant : IEquatable<BoundConstants.TypedConstant>
    {
      internal readonly object Value;
      internal readonly Type Type;

      internal TypedConstant(object value, Type type)
      {
        this.Value = value;
        this.Type = type;
      }

      public override int GetHashCode()
      {
        return RuntimeHelpers.GetHashCode(this.Value) ^ this.Type.GetHashCode();
      }

      public bool Equals(BoundConstants.TypedConstant other)
      {
        if (object.ReferenceEquals(this.Value, other.Value))
          return this.Type.Equals(other.Type);
        else
          return false;
      }

      public override bool Equals(object obj)
      {
        if (obj is BoundConstants.TypedConstant)
          return this.Equals((BoundConstants.TypedConstant) obj);
        else
          return false;
      }
    }
  }
}
