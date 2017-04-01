// Type: System.Linq.Expressions.ExpressionStringBuilder
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Linq.Expressions
{
  internal sealed class ExpressionStringBuilder : ExpressionVisitor
  {
    private StringBuilder _out;
    private Dictionary<object, int> _ids;

    private ExpressionStringBuilder()
    {
      this._out = new StringBuilder();
    }

    public override string ToString()
    {
      return ((object) this._out).ToString();
    }

    private void AddLabel(LabelTarget label)
    {
      if (this._ids == null)
      {
        this._ids = new Dictionary<object, int>();
        this._ids.Add((object) label, 0);
      }
      else
      {
        if (this._ids.ContainsKey((object) label))
          return;
        this._ids.Add((object) label, this._ids.Count);
      }
    }

    private int GetLabelId(LabelTarget label)
    {
      if (this._ids == null)
      {
        this._ids = new Dictionary<object, int>();
        this.AddLabel(label);
        return 0;
      }
      else
      {
        int count;
        if (!this._ids.TryGetValue((object) label, out count))
        {
          count = this._ids.Count;
          this.AddLabel(label);
        }
        return count;
      }
    }

    private void AddParam(ParameterExpression p)
    {
      if (this._ids == null)
      {
        this._ids = new Dictionary<object, int>();
        this._ids.Add((object) this._ids, 0);
      }
      else
      {
        if (this._ids.ContainsKey((object) p))
          return;
        this._ids.Add((object) p, this._ids.Count);
      }
    }

    private int GetParamId(ParameterExpression p)
    {
      if (this._ids == null)
      {
        this._ids = new Dictionary<object, int>();
        this.AddParam(p);
        return 0;
      }
      else
      {
        int count;
        if (!this._ids.TryGetValue((object) p, out count))
        {
          count = this._ids.Count;
          this.AddParam(p);
        }
        return count;
      }
    }

    private void Out(string s)
    {
      this._out.Append(s);
    }

    private void Out(char c)
    {
      this._out.Append(c);
    }

    internal static string ExpressionToString(Expression node)
    {
      ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
      expressionStringBuilder.Visit(node);
      return expressionStringBuilder.ToString();
    }

    internal static string CatchBlockToString(CatchBlock node)
    {
      ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
      expressionStringBuilder.VisitCatchBlock(node);
      return expressionStringBuilder.ToString();
    }

    internal static string SwitchCaseToString(SwitchCase node)
    {
      ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
      expressionStringBuilder.VisitSwitchCase(node);
      return expressionStringBuilder.ToString();
    }

    internal static string MemberBindingToString(MemberBinding node)
    {
      ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
      expressionStringBuilder.VisitMemberBinding(node);
      return expressionStringBuilder.ToString();
    }

    internal static string ElementInitBindingToString(ElementInit node)
    {
      ExpressionStringBuilder expressionStringBuilder = new ExpressionStringBuilder();
      expressionStringBuilder.VisitElementInit(node);
      return expressionStringBuilder.ToString();
    }

    private static string FormatBinder(CallSiteBinder binder)
    {
      ConvertBinder convertBinder;
      if ((convertBinder = binder as ConvertBinder) != null)
        return "Convert " + (object) convertBinder.Type;
      GetMemberBinder getMemberBinder;
      if ((getMemberBinder = binder as GetMemberBinder) != null)
        return "GetMember " + getMemberBinder.Name;
      SetMemberBinder setMemberBinder;
      if ((setMemberBinder = binder as SetMemberBinder) != null)
        return "SetMember " + setMemberBinder.Name;
      DeleteMemberBinder deleteMemberBinder;
      if ((deleteMemberBinder = binder as DeleteMemberBinder) != null)
        return "DeleteMember " + deleteMemberBinder.Name;
      if (binder is GetIndexBinder)
        return "GetIndex";
      if (binder is SetIndexBinder)
        return "SetIndex";
      if (binder is DeleteIndexBinder)
        return "DeleteIndex";
      InvokeMemberBinder invokeMemberBinder;
      if ((invokeMemberBinder = binder as InvokeMemberBinder) != null)
        return "Call " + invokeMemberBinder.Name;
      if (binder is InvokeBinder)
        return "Invoke";
      if (binder is CreateInstanceBinder)
        return "Create";
      UnaryOperationBinder unaryOperationBinder;
      if ((unaryOperationBinder = binder as UnaryOperationBinder) != null)
        return ((object) unaryOperationBinder.Operation).ToString();
      BinaryOperationBinder binaryOperationBinder;
      if ((binaryOperationBinder = binder as BinaryOperationBinder) != null)
        return ((object) binaryOperationBinder.Operation).ToString();
      else
        return "CallSiteBinder";
    }

    private void VisitExpressions<T>(char open, IList<T> expressions, char close) where T : Expression
    {
      this.VisitExpressions<T>(open, expressions, close, ", ");
    }

    private void VisitExpressions<T>(char open, IList<T> expressions, char close, string seperator) where T : Expression
    {
      this.Out(open);
      if (expressions != null)
      {
        bool flag = true;
        foreach (T obj in (IEnumerable<T>) expressions)
        {
          if (flag)
            flag = false;
          else
            this.Out(seperator);
          this.Visit((Expression) obj);
        }
      }
      this.Out(close);
    }

    protected internal override Expression VisitDynamic(DynamicExpression node)
    {
      this.Out(ExpressionStringBuilder.FormatBinder(node.Binder));
      this.VisitExpressions<Expression>('(', (IList<Expression>) node.Arguments, ')');
      return (Expression) node;
    }

    protected internal override Expression VisitBinary(BinaryExpression node)
    {
      if (node.NodeType == ExpressionType.ArrayIndex)
      {
        this.Visit(node.Left);
        this.Out("[");
        this.Visit(node.Right);
        this.Out("]");
      }
      else
      {
        string s;
        switch (node.NodeType)
        {
          case ExpressionType.Add:
            s = "+";
            break;
          case ExpressionType.AddChecked:
            s = "+";
            break;
          case ExpressionType.And:
            s = node.Type == typeof (bool) || node.Type == typeof (bool?) ? "And" : "&";
            break;
          case ExpressionType.AndAlso:
            s = "AndAlso";
            break;
          case ExpressionType.Coalesce:
            s = "??";
            break;
          case ExpressionType.Divide:
            s = "/";
            break;
          case ExpressionType.Equal:
            s = "==";
            break;
          case ExpressionType.ExclusiveOr:
            s = "^";
            break;
          case ExpressionType.GreaterThan:
            s = ">";
            break;
          case ExpressionType.GreaterThanOrEqual:
            s = ">=";
            break;
          case ExpressionType.LeftShift:
            s = "<<";
            break;
          case ExpressionType.LessThan:
            s = "<";
            break;
          case ExpressionType.LessThanOrEqual:
            s = "<=";
            break;
          case ExpressionType.Modulo:
            s = "%";
            break;
          case ExpressionType.Multiply:
            s = "*";
            break;
          case ExpressionType.MultiplyChecked:
            s = "*";
            break;
          case ExpressionType.NotEqual:
            s = "!=";
            break;
          case ExpressionType.Or:
            s = node.Type == typeof (bool) || node.Type == typeof (bool?) ? "Or" : "|";
            break;
          case ExpressionType.OrElse:
            s = "OrElse";
            break;
          case ExpressionType.Power:
            s = "^";
            break;
          case ExpressionType.RightShift:
            s = ">>";
            break;
          case ExpressionType.Subtract:
            s = "-";
            break;
          case ExpressionType.SubtractChecked:
            s = "-";
            break;
          case ExpressionType.Assign:
            s = "=";
            break;
          case ExpressionType.AddAssign:
            s = "+=";
            break;
          case ExpressionType.AndAssign:
            s = node.Type == typeof (bool) || node.Type == typeof (bool?) ? "&&=" : "&=";
            break;
          case ExpressionType.DivideAssign:
            s = "/=";
            break;
          case ExpressionType.ExclusiveOrAssign:
            s = "^=";
            break;
          case ExpressionType.LeftShiftAssign:
            s = "<<=";
            break;
          case ExpressionType.ModuloAssign:
            s = "%=";
            break;
          case ExpressionType.MultiplyAssign:
            s = "*=";
            break;
          case ExpressionType.OrAssign:
            s = node.Type == typeof (bool) || node.Type == typeof (bool?) ? "||=" : "|=";
            break;
          case ExpressionType.PowerAssign:
            s = "**=";
            break;
          case ExpressionType.RightShiftAssign:
            s = ">>=";
            break;
          case ExpressionType.SubtractAssign:
            s = "-=";
            break;
          case ExpressionType.AddAssignChecked:
            s = "+=";
            break;
          case ExpressionType.MultiplyAssignChecked:
            s = "*=";
            break;
          case ExpressionType.SubtractAssignChecked:
            s = "-=";
            break;
          default:
            throw new InvalidOperationException();
        }
        this.Out("(");
        this.Visit(node.Left);
        this.Out(' ');
        this.Out(s);
        this.Out(' ');
        this.Visit(node.Right);
        this.Out(")");
      }
      return (Expression) node;
    }

    protected internal override Expression VisitParameter(ParameterExpression node)
    {
      if (node.IsByRef)
        this.Out("ref ");
      string name = node.Name;
      if (string.IsNullOrEmpty(name))
        this.Out("Param_" + (object) this.GetParamId(node));
      else
        this.Out(name);
      return (Expression) node;
    }

    protected internal override Expression VisitLambda<T>(Expression<T> node)
    {
      if (node.Parameters.Count == 1)
        this.Visit((Expression) node.Parameters[0]);
      else
        this.VisitExpressions<ParameterExpression>('(', (IList<ParameterExpression>) node.Parameters, ')');
      this.Out(" => ");
      this.Visit(node.Body);
      return (Expression) node;
    }

    protected internal override Expression VisitListInit(ListInitExpression node)
    {
      this.Visit((Expression) node.NewExpression);
      this.Out(" {");
      int index = 0;
      for (int count = node.Initializers.Count; index < count; ++index)
      {
        if (index > 0)
          this.Out(", ");
        this.Out(node.Initializers[index].ToString());
      }
      this.Out("}");
      return (Expression) node;
    }

    protected internal override Expression VisitConditional(ConditionalExpression node)
    {
      this.Out("IIF(");
      this.Visit(node.Test);
      this.Out(", ");
      this.Visit(node.IfTrue);
      this.Out(", ");
      this.Visit(node.IfFalse);
      this.Out(")");
      return (Expression) node;
    }

    protected internal override Expression VisitConstant(ConstantExpression node)
    {
      if (node.Value != null)
      {
        string s = node.Value.ToString();
        if (node.Value is string)
        {
          this.Out("\"");
          this.Out(s);
          this.Out("\"");
        }
        else if (s == node.Value.GetType().ToString())
        {
          this.Out("value(");
          this.Out(s);
          this.Out(")");
        }
        else
          this.Out(s);
      }
      else
        this.Out("null");
      return (Expression) node;
    }

    protected internal override Expression VisitDebugInfo(DebugInfoExpression node)
    {
      this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "<DebugInfo({0}: {1}, {2}, {3}, {4})>", (object) node.Document.FileName, (object) node.StartLine, (object) node.StartColumn, (object) node.EndLine, (object) node.EndColumn));
      return (Expression) node;
    }

    protected internal override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
    {
      this.VisitExpressions<ParameterExpression>('(', (IList<ParameterExpression>) node.Variables, ')');
      return (Expression) node;
    }

    private void OutMember(Expression instance, MemberInfo member)
    {
      if (instance != null)
      {
        this.Visit(instance);
        this.Out("." + member.Name);
      }
      else
        this.Out(member.DeclaringType.Name + "." + member.Name);
    }

    protected internal override Expression VisitMember(MemberExpression node)
    {
      this.OutMember(node.Expression, node.Member);
      return (Expression) node;
    }

    protected internal override Expression VisitMemberInit(MemberInitExpression node)
    {
      if (node.NewExpression.Arguments.Count == 0 && node.NewExpression.Type.Name.Contains("<"))
        this.Out("new");
      else
        this.Visit((Expression) node.NewExpression);
      this.Out(" {");
      int index = 0;
      for (int count = node.Bindings.Count; index < count; ++index)
      {
        MemberBinding node1 = node.Bindings[index];
        if (index > 0)
          this.Out(", ");
        this.VisitMemberBinding(node1);
      }
      this.Out("}");
      return (Expression) node;
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
    {
      this.Out(assignment.Member.Name);
      this.Out(" = ");
      this.Visit(assignment.Expression);
      return assignment;
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
    {
      this.Out(binding.Member.Name);
      this.Out(" = {");
      int index = 0;
      for (int count = binding.Initializers.Count; index < count; ++index)
      {
        if (index > 0)
          this.Out(", ");
        this.VisitElementInit(binding.Initializers[index]);
      }
      this.Out("}");
      return binding;
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      this.Out(binding.Member.Name);
      this.Out(" = {");
      int index = 0;
      for (int count = binding.Bindings.Count; index < count; ++index)
      {
        if (index > 0)
          this.Out(", ");
        this.VisitMemberBinding(binding.Bindings[index]);
      }
      this.Out("}");
      return binding;
    }

    protected override ElementInit VisitElementInit(ElementInit initializer)
    {
      this.Out(initializer.AddMethod.ToString());
      string seperator = ", ";
      this.VisitExpressions<Expression>('(', (IList<Expression>) initializer.Arguments, ')', seperator);
      return initializer;
    }

    protected internal override Expression VisitInvocation(InvocationExpression node)
    {
      this.Out("Invoke(");
      this.Visit(node.Expression);
      string s = ", ";
      int index = 0;
      for (int count = node.Arguments.Count; index < count; ++index)
      {
        this.Out(s);
        this.Visit(node.Arguments[index]);
      }
      this.Out(")");
      return (Expression) node;
    }

    protected internal override Expression VisitMethodCall(MethodCallExpression node)
    {
      int num = 0;
      Expression node1 = node.Object;
      if (Attribute.GetCustomAttribute((MemberInfo) node.Method, typeof (ExtensionAttribute)) != null)
      {
        num = 1;
        node1 = node.Arguments[0];
      }
      if (node1 != null)
      {
        this.Visit(node1);
        this.Out(".");
      }
      this.Out(node.Method.Name);
      this.Out("(");
      int index = num;
      for (int count = node.Arguments.Count; index < count; ++index)
      {
        if (index > num)
          this.Out(", ");
        this.Visit(node.Arguments[index]);
      }
      this.Out(")");
      return (Expression) node;
    }

    protected internal override Expression VisitNewArray(NewArrayExpression node)
    {
      switch (node.NodeType)
      {
        case ExpressionType.NewArrayInit:
          this.Out("new [] ");
          this.VisitExpressions<Expression>('{', (IList<Expression>) node.Expressions, '}');
          break;
        case ExpressionType.NewArrayBounds:
          this.Out("new " + node.Type.ToString());
          this.VisitExpressions<Expression>('(', (IList<Expression>) node.Expressions, ')');
          break;
      }
      return (Expression) node;
    }

    protected internal override Expression VisitNew(NewExpression node)
    {
      this.Out("new " + node.Type.Name);
      this.Out("(");
      ReadOnlyCollection<MemberInfo> members = node.Members;
      for (int index = 0; index < node.Arguments.Count; ++index)
      {
        if (index > 0)
          this.Out(", ");
        if (members != null)
        {
          this.Out(members[index].Name);
          this.Out(" = ");
        }
        this.Visit(node.Arguments[index]);
      }
      this.Out(")");
      return (Expression) node;
    }

    protected internal override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
      this.Out("(");
      this.Visit(node.Expression);
      switch (node.NodeType)
      {
        case ExpressionType.TypeIs:
          this.Out(" Is ");
          break;
        case ExpressionType.TypeEqual:
          this.Out(" TypeEqual ");
          break;
      }
      this.Out(node.TypeOperand.Name);
      this.Out(")");
      return (Expression) node;
    }

    protected internal override Expression VisitUnary(UnaryExpression node)
    {
      switch (node.NodeType)
      {
        case ExpressionType.PreIncrementAssign:
          this.Out("++");
          goto case ExpressionType.Quote;
        case ExpressionType.PreDecrementAssign:
          this.Out("--");
          goto case ExpressionType.Quote;
        case ExpressionType.OnesComplement:
          this.Out("~(");
          goto case ExpressionType.Quote;
        case ExpressionType.Increment:
          this.Out("Increment(");
          goto case ExpressionType.Quote;
        case ExpressionType.Throw:
          this.Out("throw(");
          goto case ExpressionType.Quote;
        case ExpressionType.TypeAs:
          this.Out("(");
          goto case ExpressionType.Quote;
        case ExpressionType.Decrement:
          this.Out("Decrement(");
          goto case ExpressionType.Quote;
        case ExpressionType.Negate:
        case ExpressionType.NegateChecked:
          this.Out("-");
          goto case ExpressionType.Quote;
        case ExpressionType.UnaryPlus:
          this.Out("+");
          goto case ExpressionType.Quote;
        case ExpressionType.Not:
          this.Out("Not(");
          goto case ExpressionType.Quote;
        case ExpressionType.Quote:
          this.Visit(node.Operand);
          switch (node.NodeType)
          {
            case ExpressionType.TypeAs:
              this.Out(" As ");
              this.Out(node.Type.Name);
              this.Out(")");
              goto case ExpressionType.PreIncrementAssign;
            case ExpressionType.PreIncrementAssign:
            case ExpressionType.PreDecrementAssign:
            case ExpressionType.Negate:
            case ExpressionType.UnaryPlus:
            case ExpressionType.NegateChecked:
            case ExpressionType.Quote:
              return (Expression) node;
            case ExpressionType.PostIncrementAssign:
              this.Out("++");
              goto case ExpressionType.PreIncrementAssign;
            case ExpressionType.PostDecrementAssign:
              this.Out("--");
              goto case ExpressionType.PreIncrementAssign;
            default:
              this.Out(")");
              goto case ExpressionType.PreIncrementAssign;
          }
        default:
          this.Out(((object) node.NodeType).ToString());
          this.Out("(");
          goto case ExpressionType.Quote;
      }
    }

    protected internal override Expression VisitBlock(BlockExpression node)
    {
      this.Out("{");
      foreach (ParameterExpression parameterExpression in node.Variables)
      {
        this.Out("var ");
        this.Visit((Expression) parameterExpression);
        this.Out(";");
      }
      this.Out(" ... }");
      return (Expression) node;
    }

    protected internal override Expression VisitDefault(DefaultExpression node)
    {
      this.Out("default(");
      this.Out(node.Type.Name);
      this.Out(")");
      return (Expression) node;
    }

    protected internal override Expression VisitLabel(LabelExpression node)
    {
      this.Out("{ ... } ");
      this.DumpLabel(node.Target);
      this.Out(":");
      return (Expression) node;
    }

    protected internal override Expression VisitGoto(GotoExpression node)
    {
      this.Out(((object) node.Kind).ToString().ToLower(CultureInfo.CurrentCulture));
      this.DumpLabel(node.Target);
      if (node.Value != null)
      {
        this.Out(" (");
        this.Visit(node.Value);
        this.Out(") ");
      }
      return (Expression) node;
    }

    protected internal override Expression VisitLoop(LoopExpression node)
    {
      this.Out("loop { ... }");
      return (Expression) node;
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
      this.Out("case ");
      this.VisitExpressions<Expression>('(', (IList<Expression>) node.TestValues, ')');
      this.Out(": ...");
      return node;
    }

    protected internal override Expression VisitSwitch(SwitchExpression node)
    {
      this.Out("switch ");
      this.Out("(");
      this.Visit(node.SwitchValue);
      this.Out(") { ... }");
      return (Expression) node;
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
      this.Out("catch (" + node.Test.Name);
      if (node.Variable != null)
        this.Out(node.Variable.Name ?? "");
      this.Out(") { ... }");
      return node;
    }

    protected internal override Expression VisitTry(TryExpression node)
    {
      this.Out("try { ... }");
      return (Expression) node;
    }

    protected internal override Expression VisitIndex(IndexExpression node)
    {
      if (node.Object != null)
        this.Visit(node.Object);
      else
        this.Out(node.Indexer.DeclaringType.Name);
      if (node.Indexer != (PropertyInfo) null)
      {
        this.Out(".");
        this.Out(node.Indexer.Name);
      }
      this.VisitExpressions<Expression>('[', (IList<Expression>) node.Arguments, ']');
      return (Expression) node;
    }

    protected internal override Expression VisitExtension(Expression node)
    {
      BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding;
      if (node.GetType().GetMethod("ToString", bindingAttr, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null).DeclaringType != typeof (Expression))
      {
        this.Out(node.ToString());
        return node;
      }
      else
      {
        this.Out("[");
        if (node.NodeType == ExpressionType.Extension)
          this.Out(node.GetType().FullName);
        else
          this.Out(((object) node.NodeType).ToString());
        this.Out("]");
        return node;
      }
    }

    private void DumpLabel(LabelTarget target)
    {
      if (!string.IsNullOrEmpty(target.Name))
        this.Out(target.Name);
      else
        this.Out("UnamedLabel_" + (object) this.GetLabelId(target));
    }
  }
}
