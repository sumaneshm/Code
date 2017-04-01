// Type: System.Linq.Expressions.LambdaExpression
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic.Utils;
using System.Linq.Expressions.Compiler;
using System.Reflection.Emit;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  [DebuggerTypeProxy(typeof (Expression.LambdaExpressionProxy))]
  [__DynamicallyInvokable]
  public abstract class LambdaExpression : Expression
  {
    private readonly string _name;
    private readonly Expression _body;
    private readonly ReadOnlyCollection<ParameterExpression> _parameters;
    private readonly Type _delegateType;
    private readonly bool _tailCall;

    [__DynamicallyInvokable]
    public override sealed Type Type
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._delegateType;
      }
    }

    [__DynamicallyInvokable]
    public override sealed ExpressionType NodeType
    {
      [__DynamicallyInvokable] get
      {
        return ExpressionType.Lambda;
      }
    }

    [__DynamicallyInvokable]
    public ReadOnlyCollection<ParameterExpression> Parameters
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._parameters;
      }
    }

    [__DynamicallyInvokable]
    public string Name
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._name;
      }
    }

    [__DynamicallyInvokable]
    public Expression Body
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._body;
      }
    }

    [__DynamicallyInvokable]
    public Type ReturnType
    {
      [__DynamicallyInvokable] get
      {
        return this.Type.GetMethod("Invoke").ReturnType;
      }
    }

    [__DynamicallyInvokable]
    public bool TailCall
    {
      [__DynamicallyInvokable, TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get
      {
        return this._tailCall;
      }
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal LambdaExpression(Type delegateType, string name, Expression body, bool tailCall, ReadOnlyCollection<ParameterExpression> parameters)
    {
      this._name = name;
      this._body = body;
      this._parameters = parameters;
      this._delegateType = delegateType;
      this._tailCall = tailCall;
    }

    [__DynamicallyInvokable]
    public Delegate Compile()
    {
      return LambdaCompiler.Compile(this, (DebugInfoGenerator) null);
    }

    public Delegate Compile(DebugInfoGenerator debugInfoGenerator)
    {
      ContractUtils.RequiresNotNull((object) debugInfoGenerator, "debugInfoGenerator");
      return LambdaCompiler.Compile(this, debugInfoGenerator);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void CompileToMethod(MethodBuilder method)
    {
      this.CompileToMethodInternal(method, (DebugInfoGenerator) null);
    }

    public void CompileToMethod(MethodBuilder method, DebugInfoGenerator debugInfoGenerator)
    {
      ContractUtils.RequiresNotNull((object) debugInfoGenerator, "debugInfoGenerator");
      this.CompileToMethodInternal(method, debugInfoGenerator);
    }

    private void CompileToMethodInternal(MethodBuilder method, DebugInfoGenerator debugInfoGenerator)
    {
      ContractUtils.RequiresNotNull((object) method, "method");
      ContractUtils.Requires(method.IsStatic, "method");
      if ((Type) (method.DeclaringType as TypeBuilder) == (Type) null)
        throw Error.MethodBuilderDoesNotHaveTypeBuilder();
      LambdaCompiler.Compile(this, method, debugInfoGenerator);
    }

    internal abstract LambdaExpression Accept(StackSpiller spiller);
  }
}
