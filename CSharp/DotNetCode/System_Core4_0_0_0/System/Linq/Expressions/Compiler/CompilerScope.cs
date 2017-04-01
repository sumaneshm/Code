// Type: System.Linq.Expressions.Compiler.CompilerScope
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic.Utils;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions.Compiler
{
  internal sealed class CompilerScope
  {
    internal readonly Dictionary<ParameterExpression, VariableStorageKind> Definitions = new Dictionary<ParameterExpression, VariableStorageKind>();
    private readonly Dictionary<ParameterExpression, CompilerScope.Storage> _locals = new Dictionary<ParameterExpression, CompilerScope.Storage>();
    private CompilerScope _parent;
    internal readonly object Node;
    internal readonly bool IsMethod;
    internal bool NeedsClosure;
    internal Dictionary<ParameterExpression, int> ReferenceCount;
    internal Set<object> MergedScopes;
    private HoistedLocals _hoistedLocals;
    private HoistedLocals _closureHoistedLocals;

    internal HoistedLocals NearestHoistedLocals
    {
      get
      {
        return this._hoistedLocals ?? this._closureHoistedLocals;
      }
    }

    private string CurrentLambdaName
    {
      get
      {
        CompilerScope compilerScope = this;
        LambdaExpression lambdaExpression;
        do
        {
          lambdaExpression = compilerScope.Node as LambdaExpression;
        }
        while (lambdaExpression == null);
        return lambdaExpression.Name;
      }
    }

    internal CompilerScope(object node, bool isMethod)
    {
      this.Node = node;
      this.IsMethod = isMethod;
      IList<ParameterExpression> variables = CompilerScope.GetVariables(node);
      this.Definitions = new Dictionary<ParameterExpression, VariableStorageKind>(variables.Count);
      foreach (ParameterExpression key in (IEnumerable<ParameterExpression>) variables)
        this.Definitions.Add(key, VariableStorageKind.Local);
    }

    internal CompilerScope Enter(LambdaCompiler lc, CompilerScope parent)
    {
      this.SetParent(lc, parent);
      this.AllocateLocals(lc);
      if (this.IsMethod && this._closureHoistedLocals != null)
        this.EmitClosureAccess(lc, this._closureHoistedLocals);
      this.EmitNewHoistedLocals(lc);
      if (this.IsMethod)
        this.EmitCachedVariables();
      return this;
    }

    internal CompilerScope Exit()
    {
      if (!this.IsMethod)
      {
        foreach (CompilerScope.Storage storage in this._locals.Values)
          storage.FreeLocal();
      }
      CompilerScope compilerScope = this._parent;
      this._parent = (CompilerScope) null;
      this._hoistedLocals = (HoistedLocals) null;
      this._closureHoistedLocals = (HoistedLocals) null;
      this._locals.Clear();
      return compilerScope;
    }

    internal void EmitVariableAccess(LambdaCompiler lc, ReadOnlyCollection<ParameterExpression> vars)
    {
      if (this.NearestHoistedLocals != null)
      {
        List<long> list = new List<long>(vars.Count);
        foreach (ParameterExpression parameterExpression in vars)
        {
          ulong num1 = 0UL;
          HoistedLocals hoistedLocals;
          for (hoistedLocals = this.NearestHoistedLocals; !hoistedLocals.Indexes.ContainsKey((Expression) parameterExpression); hoistedLocals = hoistedLocals.Parent)
            ++num1;
          ulong num2 = num1 << 32 | (ulong) (uint) hoistedLocals.Indexes[(Expression) parameterExpression];
          list.Add((long) num2);
        }
        if (list.Count > 0)
        {
          this.EmitGet(this.NearestHoistedLocals.SelfVariable);
          lc.EmitConstantArray<long>(list.ToArray());
          lc.IL.Emit(OpCodes.Call, typeof (RuntimeOps).GetMethod("CreateRuntimeVariables", new Type[2]
          {
            typeof (object[]),
            typeof (long[])
          }));
          return;
        }
      }
      lc.IL.Emit(OpCodes.Call, typeof (RuntimeOps).GetMethod("CreateRuntimeVariables", Type.EmptyTypes));
    }

    internal void AddLocal(LambdaCompiler gen, ParameterExpression variable)
    {
      this._locals.Add(variable, (CompilerScope.Storage) new CompilerScope.LocalStorage(gen, variable));
    }

    internal void EmitGet(ParameterExpression variable)
    {
      this.ResolveVariable(variable).EmitLoad();
    }

    internal void EmitSet(ParameterExpression variable)
    {
      this.ResolveVariable(variable).EmitStore();
    }

    internal void EmitAddressOf(ParameterExpression variable)
    {
      this.ResolveVariable(variable).EmitAddress();
    }

    private CompilerScope.Storage ResolveVariable(ParameterExpression variable)
    {
      return this.ResolveVariable(variable, this.NearestHoistedLocals);
    }

    private CompilerScope.Storage ResolveVariable(ParameterExpression variable, HoistedLocals hoistedLocals)
    {
      for (CompilerScope compilerScope = this; compilerScope != null; compilerScope = compilerScope._parent)
      {
        CompilerScope.Storage storage;
        if (compilerScope._locals.TryGetValue(variable, out storage))
          return storage;
        if (compilerScope.IsMethod)
          break;
      }
      for (HoistedLocals hoistedLocals1 = hoistedLocals; hoistedLocals1 != null; hoistedLocals1 = hoistedLocals1.Parent)
      {
        int index;
        if (hoistedLocals1.Indexes.TryGetValue((Expression) variable, out index))
          return (CompilerScope.Storage) new CompilerScope.ElementBoxStorage(this.ResolveVariable(hoistedLocals1.SelfVariable, hoistedLocals), index, variable);
      }
      throw System.Linq.Expressions.Error.UndefinedVariable((object) variable.Name, (object) variable.Type, (object) this.CurrentLambdaName);
    }

    private void SetParent(LambdaCompiler lc, CompilerScope parent)
    {
      this._parent = parent;
      if (this.NeedsClosure && this._parent != null)
        this._closureHoistedLocals = this._parent.NearestHoistedLocals;
      ReadOnlyCollection<ParameterExpression> vars = CollectionExtensions.ToReadOnly<ParameterExpression>(Enumerable.Where<ParameterExpression>((IEnumerable<ParameterExpression>) this.GetVariables(), (Func<ParameterExpression, bool>) (p => this.Definitions[p] == VariableStorageKind.Hoisted)));
      if (vars.Count <= 0)
        return;
      this._hoistedLocals = new HoistedLocals(this._closureHoistedLocals, vars);
      this.AddLocal(lc, this._hoistedLocals.SelfVariable);
    }

    private void EmitNewHoistedLocals(LambdaCompiler lc)
    {
      if (this._hoistedLocals == null)
        return;
      ILGen.EmitInt(lc.IL, this._hoistedLocals.Variables.Count);
      lc.IL.Emit(OpCodes.Newarr, typeof (object));
      int num = 0;
      foreach (ParameterExpression parameterExpression in this._hoistedLocals.Variables)
      {
        lc.IL.Emit(OpCodes.Dup);
        ILGen.EmitInt(lc.IL, num++);
        Type type = typeof (StrongBox<>).MakeGenericType(new Type[1]
        {
          parameterExpression.Type
        });
        if (this.IsMethod && lc.Parameters.Contains(parameterExpression))
        {
          int index = lc.Parameters.IndexOf(parameterExpression);
          lc.EmitLambdaArgument(index);
          lc.IL.Emit(OpCodes.Newobj, type.GetConstructor(new Type[1]
          {
            parameterExpression.Type
          }));
        }
        else if (parameterExpression == this._hoistedLocals.ParentVariable)
        {
          this.ResolveVariable(parameterExpression, this._closureHoistedLocals).EmitLoad();
          lc.IL.Emit(OpCodes.Newobj, type.GetConstructor(new Type[1]
          {
            parameterExpression.Type
          }));
        }
        else
          lc.IL.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
        if (this.ShouldCache(parameterExpression))
        {
          lc.IL.Emit(OpCodes.Dup);
          this.CacheBoxToLocal(lc, parameterExpression);
        }
        lc.IL.Emit(OpCodes.Stelem_Ref);
      }
      this.EmitSet(this._hoistedLocals.SelfVariable);
    }

    private void EmitCachedVariables()
    {
      if (this.ReferenceCount == null)
        return;
      foreach (KeyValuePair<ParameterExpression, int> keyValuePair in this.ReferenceCount)
      {
        if (this.ShouldCache(keyValuePair.Key, keyValuePair.Value))
        {
          CompilerScope.ElementBoxStorage elementBoxStorage = this.ResolveVariable(keyValuePair.Key) as CompilerScope.ElementBoxStorage;
          if (elementBoxStorage != null)
          {
            elementBoxStorage.EmitLoadBox();
            this.CacheBoxToLocal(elementBoxStorage.Compiler, keyValuePair.Key);
          }
        }
      }
    }

    private bool ShouldCache(ParameterExpression v, int refCount)
    {
      if (refCount > 2)
        return !this._locals.ContainsKey(v);
      else
        return false;
    }

    private bool ShouldCache(ParameterExpression v)
    {
      int refCount;
      if (this.ReferenceCount == null || !this.ReferenceCount.TryGetValue(v, out refCount))
        return false;
      else
        return this.ShouldCache(v, refCount);
    }

    private void CacheBoxToLocal(LambdaCompiler lc, ParameterExpression v)
    {
      CompilerScope.LocalBoxStorage localBoxStorage = new CompilerScope.LocalBoxStorage(lc, v);
      localBoxStorage.EmitStoreBox();
      this._locals.Add(v, (CompilerScope.Storage) localBoxStorage);
    }

    private void EmitClosureAccess(LambdaCompiler lc, HoistedLocals locals)
    {
      if (locals == null)
        return;
      this.EmitClosureToVariable(lc, locals);
      while ((locals = locals.Parent) != null)
      {
        ParameterExpression parameterExpression = locals.SelfVariable;
        CompilerScope.LocalStorage localStorage = new CompilerScope.LocalStorage(lc, parameterExpression);
        localStorage.EmitStore(this.ResolveVariable(parameterExpression));
        this._locals.Add(parameterExpression, (CompilerScope.Storage) localStorage);
      }
    }

    private void EmitClosureToVariable(LambdaCompiler lc, HoistedLocals locals)
    {
      lc.EmitClosureArgument();
      lc.IL.Emit(OpCodes.Ldfld, typeof (Closure).GetField("Locals"));
      this.AddLocal(lc, locals.SelfVariable);
      this.EmitSet(locals.SelfVariable);
    }

    private void AllocateLocals(LambdaCompiler lc)
    {
      foreach (ParameterExpression index in (IEnumerable<ParameterExpression>) this.GetVariables())
      {
        if (this.Definitions[index] == VariableStorageKind.Local)
        {
          CompilerScope.Storage storage = !this.IsMethod || !lc.Parameters.Contains(index) ? (CompilerScope.Storage) new CompilerScope.LocalStorage(lc, index) : (CompilerScope.Storage) new CompilerScope.ArgumentStorage(lc, index);
          this._locals.Add(index, storage);
        }
      }
    }

    private IList<ParameterExpression> GetVariables()
    {
      IList<ParameterExpression> variables = CompilerScope.GetVariables(this.Node);
      if (this.MergedScopes == null)
        return variables;
      List<ParameterExpression> list = new List<ParameterExpression>((IEnumerable<ParameterExpression>) variables);
      foreach (object scope in this.MergedScopes)
        list.AddRange((IEnumerable<ParameterExpression>) CompilerScope.GetVariables(scope));
      return (IList<ParameterExpression>) list;
    }

    private static IList<ParameterExpression> GetVariables(object scope)
    {
      LambdaExpression lambdaExpression = scope as LambdaExpression;
      if (lambdaExpression != null)
        return (IList<ParameterExpression>) lambdaExpression.Parameters;
      BlockExpression blockExpression = scope as BlockExpression;
      if (blockExpression != null)
        return (IList<ParameterExpression>) blockExpression.Variables;
      return (IList<ParameterExpression>) new ParameterExpression[1]
      {
        ((CatchBlock) scope).Variable
      };
    }

    private abstract class Storage
    {
      internal readonly LambdaCompiler Compiler;
      internal readonly ParameterExpression Variable;

      internal Storage(LambdaCompiler compiler, ParameterExpression variable)
      {
        this.Compiler = compiler;
        this.Variable = variable;
      }

      internal abstract void EmitLoad();

      internal abstract void EmitAddress();

      internal abstract void EmitStore();

      internal virtual void EmitStore(CompilerScope.Storage value)
      {
        value.EmitLoad();
        this.EmitStore();
      }

      internal virtual void FreeLocal()
      {
      }
    }

    private sealed class LocalStorage : CompilerScope.Storage
    {
      private readonly LocalBuilder _local;

      internal LocalStorage(LambdaCompiler compiler, ParameterExpression variable)
        : base(compiler, variable)
      {
        this._local = compiler.GetNamedLocal(variable.IsByRef ? variable.Type.MakeByRefType() : variable.Type, variable);
      }

      internal override void EmitLoad()
      {
        this.Compiler.IL.Emit(OpCodes.Ldloc, this._local);
      }

      internal override void EmitStore()
      {
        this.Compiler.IL.Emit(OpCodes.Stloc, this._local);
      }

      internal override void EmitAddress()
      {
        this.Compiler.IL.Emit(OpCodes.Ldloca, this._local);
      }
    }

    private sealed class ArgumentStorage : CompilerScope.Storage
    {
      private readonly int _argument;

      internal ArgumentStorage(LambdaCompiler compiler, ParameterExpression p)
        : base(compiler, p)
      {
        this._argument = compiler.GetLambdaArgument(compiler.Parameters.IndexOf(p));
      }

      internal override void EmitLoad()
      {
        ILGen.EmitLoadArg(this.Compiler.IL, this._argument);
      }

      internal override void EmitStore()
      {
        ILGen.EmitStoreArg(this.Compiler.IL, this._argument);
      }

      internal override void EmitAddress()
      {
        ILGen.EmitLoadArgAddress(this.Compiler.IL, this._argument);
      }
    }

    private sealed class ElementBoxStorage : CompilerScope.Storage
    {
      private readonly int _index;
      private readonly CompilerScope.Storage _array;
      private readonly Type _boxType;
      private readonly FieldInfo _boxValueField;

      internal ElementBoxStorage(CompilerScope.Storage array, int index, ParameterExpression variable)
        : base(array.Compiler, variable)
      {
        this._array = array;
        this._index = index;
        this._boxType = typeof (StrongBox<>).MakeGenericType(new Type[1]
        {
          variable.Type
        });
        this._boxValueField = this._boxType.GetField("Value");
      }

      internal override void EmitLoad()
      {
        this.EmitLoadBox();
        this.Compiler.IL.Emit(OpCodes.Ldfld, this._boxValueField);
      }

      internal override void EmitStore()
      {
        LocalBuilder local = this.Compiler.GetLocal(this.Variable.Type);
        this.Compiler.IL.Emit(OpCodes.Stloc, local);
        this.EmitLoadBox();
        this.Compiler.IL.Emit(OpCodes.Ldloc, local);
        this.Compiler.FreeLocal(local);
        this.Compiler.IL.Emit(OpCodes.Stfld, this._boxValueField);
      }

      internal override void EmitStore(CompilerScope.Storage value)
      {
        this.EmitLoadBox();
        value.EmitLoad();
        this.Compiler.IL.Emit(OpCodes.Stfld, this._boxValueField);
      }

      internal override void EmitAddress()
      {
        this.EmitLoadBox();
        this.Compiler.IL.Emit(OpCodes.Ldflda, this._boxValueField);
      }

      internal void EmitLoadBox()
      {
        this._array.EmitLoad();
        ILGen.EmitInt(this.Compiler.IL, this._index);
        this.Compiler.IL.Emit(OpCodes.Ldelem_Ref);
        this.Compiler.IL.Emit(OpCodes.Castclass, this._boxType);
      }
    }

    private sealed class LocalBoxStorage : CompilerScope.Storage
    {
      private readonly LocalBuilder _boxLocal;
      private readonly Type _boxType;
      private readonly FieldInfo _boxValueField;

      internal LocalBoxStorage(LambdaCompiler compiler, ParameterExpression variable)
        : base(compiler, variable)
      {
        this._boxType = typeof (StrongBox<>).MakeGenericType(new Type[1]
        {
          variable.Type
        });
        this._boxValueField = this._boxType.GetField("Value");
        this._boxLocal = compiler.GetNamedLocal(this._boxType, variable);
      }

      internal override void EmitLoad()
      {
        this.Compiler.IL.Emit(OpCodes.Ldloc, this._boxLocal);
        this.Compiler.IL.Emit(OpCodes.Ldfld, this._boxValueField);
      }

      internal override void EmitAddress()
      {
        this.Compiler.IL.Emit(OpCodes.Ldloc, this._boxLocal);
        this.Compiler.IL.Emit(OpCodes.Ldflda, this._boxValueField);
      }

      internal override void EmitStore()
      {
        LocalBuilder local = this.Compiler.GetLocal(this.Variable.Type);
        this.Compiler.IL.Emit(OpCodes.Stloc, local);
        this.Compiler.IL.Emit(OpCodes.Ldloc, this._boxLocal);
        this.Compiler.IL.Emit(OpCodes.Ldloc, local);
        this.Compiler.FreeLocal(local);
        this.Compiler.IL.Emit(OpCodes.Stfld, this._boxValueField);
      }

      internal override void EmitStore(CompilerScope.Storage value)
      {
        this.Compiler.IL.Emit(OpCodes.Ldloc, this._boxLocal);
        value.EmitLoad();
        this.Compiler.IL.Emit(OpCodes.Stfld, this._boxValueField);
      }

      internal void EmitStoreBox()
      {
        this.Compiler.IL.Emit(OpCodes.Stloc, this._boxLocal);
      }
    }
  }
}
