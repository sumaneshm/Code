// Type: System.Linq.Expressions.DebugViewWriter
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Dynamic.Utils;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions
{
  internal sealed class DebugViewWriter : ExpressionVisitor
  {
    private Stack<int> _stack = new Stack<int>();
    private const int Tab = 4;
    private const int MaxColumn = 120;
    private TextWriter _out;
    private int _column;
    private int _delta;
    private DebugViewWriter.Flow _flow;
    private Queue<LambdaExpression> _lambdas;
    private Dictionary<LambdaExpression, int> _lambdaIds;
    private Dictionary<ParameterExpression, int> _paramIds;
    private Dictionary<LabelTarget, int> _labelIds;

    private int Base
    {
      get
      {
        if (this._stack.Count <= 0)
          return 0;
        else
          return this._stack.Peek();
      }
    }

    private int Delta
    {
      get
      {
        return this._delta;
      }
    }

    private int Depth
    {
      get
      {
        return this.Base + this.Delta;
      }
    }

    private DebugViewWriter(TextWriter file)
    {
      this._out = file;
    }

    private void Indent()
    {
      this._delta += 4;
    }

    private void Dedent()
    {
      this._delta -= 4;
    }

    private void NewLine()
    {
      this._flow = DebugViewWriter.Flow.NewLine;
    }

    private static int GetId<T>(T e, ref Dictionary<T, int> ids)
    {
      if (ids == null)
      {
        ids = new Dictionary<T, int>();
        ids.Add(e, 1);
        return 1;
      }
      else
      {
        int num;
        if (!ids.TryGetValue(e, out num))
        {
          num = ids.Count + 1;
          ids.Add(e, num);
        }
        return num;
      }
    }

    private int GetLambdaId(LambdaExpression le)
    {
      return DebugViewWriter.GetId<LambdaExpression>(le, ref this._lambdaIds);
    }

    private int GetParamId(ParameterExpression p)
    {
      return DebugViewWriter.GetId<ParameterExpression>(p, ref this._paramIds);
    }

    private int GetLabelTargetId(LabelTarget target)
    {
      return DebugViewWriter.GetId<LabelTarget>(target, ref this._labelIds);
    }

    internal static void WriteTo(Expression node, TextWriter writer)
    {
      new DebugViewWriter(writer).WriteTo(node);
    }

    private void WriteTo(Expression node)
    {
      LambdaExpression lambda = node as LambdaExpression;
      if (lambda != null)
        this.WriteLambda(lambda);
      else
        this.Visit(node);
      while (this._lambdas != null && this._lambdas.Count > 0)
      {
        this.WriteLine();
        this.WriteLine();
        this.WriteLambda(this._lambdas.Dequeue());
      }
    }

    private void Out(string s)
    {
      this.Out(DebugViewWriter.Flow.None, s, DebugViewWriter.Flow.None);
    }

    private void Out(DebugViewWriter.Flow before, string s)
    {
      this.Out(before, s, DebugViewWriter.Flow.None);
    }

    private void Out(string s, DebugViewWriter.Flow after)
    {
      this.Out(DebugViewWriter.Flow.None, s, after);
    }

    private void Out(DebugViewWriter.Flow before, string s, DebugViewWriter.Flow after)
    {
      switch (this.GetFlow(before))
      {
        case DebugViewWriter.Flow.Space:
          this.Write(" ");
          break;
        case DebugViewWriter.Flow.NewLine:
          this.WriteLine();
          this.Write(new string(' ', this.Depth));
          break;
      }
      this.Write(s);
      this._flow = after;
    }

    private void WriteLine()
    {
      this._out.WriteLine();
      this._column = 0;
    }

    private void Write(string s)
    {
      this._out.Write(s);
      this._column += s.Length;
    }

    private DebugViewWriter.Flow GetFlow(DebugViewWriter.Flow flow)
    {
      DebugViewWriter.Flow flow1 = this.CheckBreak(this._flow);
      flow = this.CheckBreak(flow);
      return (DebugViewWriter.Flow) Math.Max((int) flow1, (int) flow);
    }

    private DebugViewWriter.Flow CheckBreak(DebugViewWriter.Flow flow)
    {
      if ((flow & DebugViewWriter.Flow.Break) != DebugViewWriter.Flow.None)
      {
        if (this._column > 120 + this.Depth)
          flow = DebugViewWriter.Flow.NewLine;
        else
          flow &= ~DebugViewWriter.Flow.Break;
      }
      return flow;
    }

    private static string FormatBinder(CallSiteBinder binder)
    {
      ConvertBinder convertBinder;
      if ((convertBinder = binder as ConvertBinder) != null)
        return "Convert " + convertBinder.Type.ToString();
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
        return "UnaryOperation " + (object) unaryOperationBinder.Operation;
      BinaryOperationBinder binaryOperationBinder;
      if ((binaryOperationBinder = binder as BinaryOperationBinder) != null)
        return "BinaryOperation " + (object) binaryOperationBinder.Operation;
      else
        return binder.ToString();
    }

    private void VisitExpressions<T>(char open, IList<T> expressions) where T : Expression
    {
      this.VisitExpressions<T>(open, ',', expressions);
    }

    private void VisitExpressions<T>(char open, char separator, IList<T> expressions) where T : Expression
    {
      this.VisitExpressions<T>(open, separator, expressions, (Action<T>) (e => this.Visit((Expression) e)));
    }

    private void VisitDeclarations(IList<ParameterExpression> expressions)
    {
      this.VisitExpressions<ParameterExpression>('(', ',', expressions, (Action<ParameterExpression>) (variable =>
      {
        this.Out(variable.Type.ToString());
        if (variable.IsByRef)
          this.Out("&");
        this.Out(" ");
        this.VisitParameter(variable);
      }));
    }

    private void VisitExpressions<T>(char open, char separator, IList<T> expressions, Action<T> visit)
    {
      this.Out(open.ToString());
      if (expressions != null)
      {
        this.Indent();
        bool flag = true;
        foreach (T obj in (IEnumerable<T>) expressions)
        {
          if (flag)
          {
            if ((int) open == 123 || expressions.Count > 1)
              this.NewLine();
            flag = false;
          }
          else
            this.Out(separator.ToString(), DebugViewWriter.Flow.NewLine);
          visit(obj);
        }
        this.Dedent();
      }
      char ch;
      switch (open)
      {
        case '[':
          ch = ']';
          break;
        case '{':
          ch = '}';
          break;
        case '(':
          ch = ')';
          break;
        case '<':
          ch = '>';
          break;
        default:
          throw ContractUtils.Unreachable;
      }
      if ((int) open == 123)
        this.NewLine();
      this.Out(ch.ToString(), DebugViewWriter.Flow.Break);
    }

    protected internal override Expression VisitDynamic(DynamicExpression node)
    {
      this.Out(".Dynamic", DebugViewWriter.Flow.Space);
      this.Out(DebugViewWriter.FormatBinder(node.Binder));
      this.VisitExpressions<Expression>('(', (IList<Expression>) node.Arguments);
      return (Expression) node;
    }

    protected internal override Expression VisitBinary(BinaryExpression node)
    {
      if (node.NodeType == ExpressionType.ArrayIndex)
      {
        this.ParenthesizedVisit((Expression) node, node.Left);
        this.Out("[");
        this.Visit(node.Right);
        this.Out("]");
      }
      else
      {
        bool flag1 = DebugViewWriter.NeedsParentheses((Expression) node, node.Left);
        bool flag2 = DebugViewWriter.NeedsParentheses((Expression) node, node.Right);
        bool flag3 = false;
        DebugViewWriter.Flow before = DebugViewWriter.Flow.Space;
        string s;
        switch (node.NodeType)
        {
          case ExpressionType.Add:
            s = "+";
            break;
          case ExpressionType.AddChecked:
            s = "+";
            flag3 = true;
            break;
          case ExpressionType.And:
            s = "&";
            break;
          case ExpressionType.AndAlso:
            s = "&&";
            before = DebugViewWriter.Flow.Space | DebugViewWriter.Flow.Break;
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
            flag3 = true;
            break;
          case ExpressionType.NotEqual:
            s = "!=";
            break;
          case ExpressionType.Or:
            s = "|";
            break;
          case ExpressionType.OrElse:
            s = "||";
            before = DebugViewWriter.Flow.Space | DebugViewWriter.Flow.Break;
            break;
          case ExpressionType.Power:
            s = "**";
            break;
          case ExpressionType.RightShift:
            s = ">>";
            break;
          case ExpressionType.Subtract:
            s = "-";
            break;
          case ExpressionType.SubtractChecked:
            s = "-";
            flag3 = true;
            break;
          case ExpressionType.Assign:
            s = "=";
            break;
          case ExpressionType.AddAssign:
            s = "+=";
            break;
          case ExpressionType.AndAssign:
            s = "&=";
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
            s = "|=";
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
            flag3 = true;
            break;
          case ExpressionType.MultiplyAssignChecked:
            s = "*=";
            flag3 = true;
            break;
          case ExpressionType.SubtractAssignChecked:
            s = "-=";
            flag3 = true;
            break;
          default:
            throw new InvalidOperationException();
        }
        if (flag1)
          this.Out("(", DebugViewWriter.Flow.None);
        this.Visit(node.Left);
        if (flag1)
          this.Out(DebugViewWriter.Flow.None, ")", DebugViewWriter.Flow.Break);
        if (flag3)
          s = string.Format((IFormatProvider) CultureInfo.CurrentCulture, "#{0}", new object[1]
          {
            (object) s
          });
        this.Out(before, s, DebugViewWriter.Flow.Space | DebugViewWriter.Flow.Break);
        if (flag2)
          this.Out("(", DebugViewWriter.Flow.None);
        this.Visit(node.Right);
        if (flag2)
          this.Out(DebugViewWriter.Flow.None, ")", DebugViewWriter.Flow.Break);
      }
      return (Expression) node;
    }

    protected internal override Expression VisitParameter(ParameterExpression node)
    {
      this.Out("$");
      if (string.IsNullOrEmpty(node.Name))
        this.Out("var" + (object) this.GetParamId(node));
      else
        this.Out(DebugViewWriter.GetDisplayName(node.Name));
      return (Expression) node;
    }

    protected internal override Expression VisitLambda<T>(Expression<T> node)
    {
      this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{0} {1}<{2}>", (object) ".Lambda", (object) this.GetLambdaName((LambdaExpression) node), (object) node.Type.ToString()));
      if (this._lambdas == null)
        this._lambdas = new Queue<LambdaExpression>();
      if (!this._lambdas.Contains((LambdaExpression) node))
        this._lambdas.Enqueue((LambdaExpression) node);
      return (Expression) node;
    }

    private static bool IsSimpleExpression(Expression node)
    {
      BinaryExpression binaryExpression = node as BinaryExpression;
      if (binaryExpression != null && !(binaryExpression.Left is BinaryExpression))
        return !(binaryExpression.Right is BinaryExpression);
      else
        return false;
    }

    protected internal override Expression VisitConditional(ConditionalExpression node)
    {
      if (DebugViewWriter.IsSimpleExpression(node.Test))
      {
        this.Out(".If (");
        this.Visit(node.Test);
        this.Out(") {", DebugViewWriter.Flow.NewLine);
      }
      else
      {
        this.Out(".If (", DebugViewWriter.Flow.NewLine);
        this.Indent();
        this.Visit(node.Test);
        this.Dedent();
        this.Out(DebugViewWriter.Flow.NewLine, ") {", DebugViewWriter.Flow.NewLine);
      }
      this.Indent();
      this.Visit(node.IfTrue);
      this.Dedent();
      this.Out(DebugViewWriter.Flow.NewLine, "} .Else {", DebugViewWriter.Flow.NewLine);
      this.Indent();
      this.Visit(node.IfFalse);
      this.Dedent();
      this.Out(DebugViewWriter.Flow.NewLine, "}");
      return (Expression) node;
    }

    protected internal override Expression VisitConstant(ConstantExpression node)
    {
      object obj = node.Value;
      if (obj == null)
        this.Out("null");
      else if (obj is string && node.Type == typeof (string))
        this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "\"{0}\"", new object[1]
        {
          obj
        }));
      else if (obj is char && node.Type == typeof (char))
        this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}'", new object[1]
        {
          obj
        }));
      else if (obj is int && node.Type == typeof (int) || obj is bool && node.Type == typeof (bool))
      {
        this.Out(obj.ToString());
      }
      else
      {
        string constantValueSuffix = DebugViewWriter.GetConstantValueSuffix(node.Type);
        if (constantValueSuffix != null)
        {
          this.Out(obj.ToString());
          this.Out(constantValueSuffix);
        }
        else
          this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ".Constant<{0}>({1})", new object[2]
          {
            (object) node.Type.ToString(),
            obj
          }));
      }
      return (Expression) node;
    }

    private static string GetConstantValueSuffix(Type type)
    {
      if (type == typeof (uint))
        return "U";
      if (type == typeof (long))
        return "L";
      if (type == typeof (ulong))
        return "UL";
      if (type == typeof (double))
        return "D";
      if (type == typeof (float))
        return "F";
      if (type == typeof (Decimal))
        return "M";
      else
        return (string) null;
    }

    protected internal override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
    {
      this.Out(".RuntimeVariables");
      this.VisitExpressions<ParameterExpression>('(', (IList<ParameterExpression>) node.Variables);
      return (Expression) node;
    }

    private void OutMember(Expression node, Expression instance, MemberInfo member)
    {
      if (instance != null)
      {
        this.ParenthesizedVisit(node, instance);
        this.Out("." + member.Name);
      }
      else
        this.Out(member.DeclaringType.ToString() + "." + member.Name);
    }

    protected internal override Expression VisitMember(MemberExpression node)
    {
      this.OutMember((Expression) node, node.Expression, node.Member);
      return (Expression) node;
    }

    protected internal override Expression VisitInvocation(InvocationExpression node)
    {
      this.Out(".Invoke ");
      this.ParenthesizedVisit((Expression) node, node.Expression);
      this.VisitExpressions<Expression>('(', (IList<Expression>) node.Arguments);
      return (Expression) node;
    }

    private static bool NeedsParentheses(Expression parent, Expression child)
    {
      if (child == null)
        return false;
      switch (parent.NodeType)
      {
        case ExpressionType.Unbox:
        case ExpressionType.IsTrue:
        case ExpressionType.IsFalse:
        case ExpressionType.Decrement:
        case ExpressionType.Increment:
          return true;
        default:
          int operatorPrecedence1 = DebugViewWriter.GetOperatorPrecedence(child);
          int operatorPrecedence2 = DebugViewWriter.GetOperatorPrecedence(parent);
          if (operatorPrecedence1 == operatorPrecedence2)
          {
            switch (parent.NodeType)
            {
              case ExpressionType.Modulo:
              case ExpressionType.Subtract:
              case ExpressionType.SubtractChecked:
              case ExpressionType.Divide:
                BinaryExpression binaryExpression = parent as BinaryExpression;
                return child == binaryExpression.Right;
              case ExpressionType.Multiply:
              case ExpressionType.MultiplyChecked:
              case ExpressionType.Add:
              case ExpressionType.AddChecked:
                return false;
              case ExpressionType.Or:
              case ExpressionType.OrElse:
              case ExpressionType.And:
              case ExpressionType.AndAlso:
              case ExpressionType.ExclusiveOr:
                return false;
              default:
                return true;
            }
          }
          else if (child != null && child.NodeType == ExpressionType.Constant && (parent.NodeType == ExpressionType.Negate || parent.NodeType == ExpressionType.NegateChecked))
            return true;
          else
            return operatorPrecedence1 < operatorPrecedence2;
      }
    }

    private static int GetOperatorPrecedence(Expression node)
    {
      switch (node.NodeType)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return 10;
        case ExpressionType.And:
          return 6;
        case ExpressionType.AndAlso:
          return 3;
        case ExpressionType.Coalesce:
        case ExpressionType.Assign:
        case ExpressionType.AddAssign:
        case ExpressionType.AndAssign:
        case ExpressionType.DivideAssign:
        case ExpressionType.ExclusiveOrAssign:
        case ExpressionType.LeftShiftAssign:
        case ExpressionType.ModuloAssign:
        case ExpressionType.MultiplyAssign:
        case ExpressionType.OrAssign:
        case ExpressionType.PowerAssign:
        case ExpressionType.RightShiftAssign:
        case ExpressionType.SubtractAssign:
        case ExpressionType.AddAssignChecked:
        case ExpressionType.MultiplyAssignChecked:
        case ExpressionType.SubtractAssignChecked:
          return 1;
        case ExpressionType.Constant:
        case ExpressionType.Parameter:
          return 15;
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
        case ExpressionType.Decrement:
        case ExpressionType.Increment:
        case ExpressionType.Throw:
        case ExpressionType.Unbox:
        case ExpressionType.PreIncrementAssign:
        case ExpressionType.PreDecrementAssign:
        case ExpressionType.OnesComplement:
        case ExpressionType.IsTrue:
        case ExpressionType.IsFalse:
          return 12;
        case ExpressionType.Divide:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
          return 11;
        case ExpressionType.Equal:
        case ExpressionType.NotEqual:
          return 7;
        case ExpressionType.ExclusiveOr:
          return 5;
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.TypeAs:
        case ExpressionType.TypeIs:
        case ExpressionType.TypeEqual:
          return 8;
        case ExpressionType.LeftShift:
        case ExpressionType.RightShift:
          return 9;
        case ExpressionType.Or:
          return 4;
        case ExpressionType.OrElse:
          return 2;
        case ExpressionType.Power:
          return 13;
        default:
          return 14;
      }
    }

    private void ParenthesizedVisit(Expression parent, Expression nodeToVisit)
    {
      if (DebugViewWriter.NeedsParentheses(parent, nodeToVisit))
      {
        this.Out("(");
        this.Visit(nodeToVisit);
        this.Out(")");
      }
      else
        this.Visit(nodeToVisit);
    }

    protected internal override Expression VisitMethodCall(MethodCallExpression node)
    {
      this.Out(".Call ");
      if (node.Object != null)
        this.ParenthesizedVisit((Expression) node, node.Object);
      else if (node.Method.DeclaringType != (Type) null)
        this.Out(node.Method.DeclaringType.ToString());
      else
        this.Out("<UnknownType>");
      this.Out(".");
      this.Out(node.Method.Name);
      this.VisitExpressions<Expression>('(', (IList<Expression>) node.Arguments);
      return (Expression) node;
    }

    protected internal override Expression VisitNewArray(NewArrayExpression node)
    {
      if (node.NodeType == ExpressionType.NewArrayBounds)
      {
        this.Out(".NewArray " + node.Type.GetElementType().ToString());
        this.VisitExpressions<Expression>('[', (IList<Expression>) node.Expressions);
      }
      else
      {
        this.Out(".NewArray " + node.Type.ToString(), DebugViewWriter.Flow.Space);
        this.VisitExpressions<Expression>('{', (IList<Expression>) node.Expressions);
      }
      return (Expression) node;
    }

    protected internal override Expression VisitNew(NewExpression node)
    {
      this.Out(".New " + node.Type.ToString());
      this.VisitExpressions<Expression>('(', (IList<Expression>) node.Arguments);
      return (Expression) node;
    }

    protected override ElementInit VisitElementInit(ElementInit node)
    {
      if (node.Arguments.Count == 1)
        this.Visit(node.Arguments[0]);
      else
        this.VisitExpressions<Expression>('{', (IList<Expression>) node.Arguments);
      return node;
    }

    protected internal override Expression VisitListInit(ListInitExpression node)
    {
      this.Visit((Expression) node.NewExpression);
      this.VisitExpressions<ElementInit>('{', ',', (IList<ElementInit>) node.Initializers, (Action<ElementInit>) (e => this.VisitElementInit(e)));
      return (Expression) node;
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
    {
      this.Out(assignment.Member.Name);
      this.Out(DebugViewWriter.Flow.Space, "=", DebugViewWriter.Flow.Space);
      this.Visit(assignment.Expression);
      return assignment;
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
    {
      this.Out(binding.Member.Name);
      this.Out(DebugViewWriter.Flow.Space, "=", DebugViewWriter.Flow.Space);
      this.VisitExpressions<ElementInit>('{', ',', (IList<ElementInit>) binding.Initializers, (Action<ElementInit>) (e => this.VisitElementInit(e)));
      return binding;
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      this.Out(binding.Member.Name);
      this.Out(DebugViewWriter.Flow.Space, "=", DebugViewWriter.Flow.Space);
      this.VisitExpressions<MemberBinding>('{', ',', (IList<MemberBinding>) binding.Bindings, (Action<MemberBinding>) (e => this.VisitMemberBinding(e)));
      return binding;
    }

    protected internal override Expression VisitMemberInit(MemberInitExpression node)
    {
      this.Visit((Expression) node.NewExpression);
      this.VisitExpressions<MemberBinding>('{', ',', (IList<MemberBinding>) node.Bindings, (Action<MemberBinding>) (e => this.VisitMemberBinding(e)));
      return (Expression) node;
    }

    protected internal override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
      this.ParenthesizedVisit((Expression) node, node.Expression);
      switch (node.NodeType)
      {
        case ExpressionType.TypeIs:
          this.Out(DebugViewWriter.Flow.Space, ".Is", DebugViewWriter.Flow.Space);
          break;
        case ExpressionType.TypeEqual:
          this.Out(DebugViewWriter.Flow.Space, ".TypeEqual", DebugViewWriter.Flow.Space);
          break;
      }
      this.Out(node.TypeOperand.ToString());
      return (Expression) node;
    }

    protected internal override Expression VisitUnary(UnaryExpression node)
    {
      switch (node.NodeType)
      {
        case ExpressionType.Increment:
          this.Out(".Increment");
          break;
        case ExpressionType.Throw:
          if (node.Operand == null)
          {
            this.Out(".Rethrow");
            break;
          }
          else
          {
            this.Out(".Throw", DebugViewWriter.Flow.Space);
            break;
          }
        case ExpressionType.Unbox:
          this.Out(".Unbox");
          break;
        case ExpressionType.PreIncrementAssign:
          this.Out("++");
          break;
        case ExpressionType.PreDecrementAssign:
          this.Out("--");
          break;
        case ExpressionType.OnesComplement:
          this.Out("~");
          break;
        case ExpressionType.IsTrue:
          this.Out(".IsTrue");
          break;
        case ExpressionType.IsFalse:
          this.Out(".IsFalse");
          break;
        case ExpressionType.Decrement:
          this.Out(".Decrement");
          break;
        case ExpressionType.Negate:
          this.Out("-");
          break;
        case ExpressionType.UnaryPlus:
          this.Out("+");
          break;
        case ExpressionType.NegateChecked:
          this.Out("#-");
          break;
        case ExpressionType.Not:
          this.Out(node.Type == typeof (bool) ? "!" : "~");
          break;
        case ExpressionType.Quote:
          this.Out("'");
          break;
        case ExpressionType.Convert:
          this.Out("(" + node.Type.ToString() + ")");
          break;
        case ExpressionType.ConvertChecked:
          this.Out("#(" + node.Type.ToString() + ")");
          break;
      }
      this.ParenthesizedVisit((Expression) node, node.Operand);
      switch (node.NodeType)
      {
        case ExpressionType.ArrayLength:
          this.Out(".Length");
          break;
        case ExpressionType.TypeAs:
          this.Out(DebugViewWriter.Flow.Space, ".As", DebugViewWriter.Flow.Space | DebugViewWriter.Flow.Break);
          this.Out(node.Type.ToString());
          break;
        case ExpressionType.PostIncrementAssign:
          this.Out("++");
          break;
        case ExpressionType.PostDecrementAssign:
          this.Out("--");
          break;
      }
      return (Expression) node;
    }

    protected internal override Expression VisitBlock(BlockExpression node)
    {
      this.Out(".Block");
      if (node.Type != node.GetExpression(node.ExpressionCount - 1).Type)
        this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "<{0}>", new object[1]
        {
          (object) node.Type.ToString()
        }));
      this.VisitDeclarations((IList<ParameterExpression>) node.Variables);
      this.Out(" ");
      this.VisitExpressions<Expression>('{', ';', (IList<Expression>) node.Expressions);
      return (Expression) node;
    }

    protected internal override Expression VisitDefault(DefaultExpression node)
    {
      this.Out(".Default(" + node.Type.ToString() + ")");
      return (Expression) node;
    }

    protected internal override Expression VisitLabel(LabelExpression node)
    {
      this.Out(".Label", DebugViewWriter.Flow.NewLine);
      this.Indent();
      this.Visit(node.DefaultValue);
      this.Dedent();
      this.NewLine();
      this.DumpLabel(node.Target);
      return (Expression) node;
    }

    protected internal override Expression VisitGoto(GotoExpression node)
    {
      this.Out("." + ((object) node.Kind).ToString(), DebugViewWriter.Flow.Space);
      this.Out(this.GetLabelTargetName(node.Target), DebugViewWriter.Flow.Space);
      this.Out("{", DebugViewWriter.Flow.Space);
      this.Visit(node.Value);
      this.Out(DebugViewWriter.Flow.Space, "}");
      return (Expression) node;
    }

    protected internal override Expression VisitLoop(LoopExpression node)
    {
      this.Out(".Loop", DebugViewWriter.Flow.Space);
      if (node.ContinueLabel != null)
        this.DumpLabel(node.ContinueLabel);
      this.Out(" {", DebugViewWriter.Flow.NewLine);
      this.Indent();
      this.Visit(node.Body);
      this.Dedent();
      this.Out(DebugViewWriter.Flow.NewLine, "}");
      if (node.BreakLabel != null)
      {
        this.Out("", DebugViewWriter.Flow.NewLine);
        this.DumpLabel(node.BreakLabel);
      }
      return (Expression) node;
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
      foreach (Expression node1 in node.TestValues)
      {
        this.Out(".Case (");
        this.Visit(node1);
        this.Out("):", DebugViewWriter.Flow.NewLine);
      }
      this.Indent();
      this.Indent();
      this.Visit(node.Body);
      this.Dedent();
      this.Dedent();
      this.NewLine();
      return node;
    }

    protected internal override Expression VisitSwitch(SwitchExpression node)
    {
      this.Out(".Switch ");
      this.Out("(");
      this.Visit(node.SwitchValue);
      this.Out(") {", DebugViewWriter.Flow.NewLine);
      ExpressionVisitor.Visit<SwitchCase>(node.Cases, new Func<SwitchCase, SwitchCase>(((ExpressionVisitor) this).VisitSwitchCase));
      if (node.DefaultBody != null)
      {
        this.Out(".Default:", DebugViewWriter.Flow.NewLine);
        this.Indent();
        this.Indent();
        this.Visit(node.DefaultBody);
        this.Dedent();
        this.Dedent();
        this.NewLine();
      }
      this.Out("}");
      return (Expression) node;
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
      this.Out(DebugViewWriter.Flow.NewLine, "} .Catch (" + node.Test.ToString());
      if (node.Variable != null)
      {
        this.Out(DebugViewWriter.Flow.Space, "");
        this.VisitParameter(node.Variable);
      }
      if (node.Filter != null)
      {
        this.Out(") .If (", DebugViewWriter.Flow.Break);
        this.Visit(node.Filter);
      }
      this.Out(") {", DebugViewWriter.Flow.NewLine);
      this.Indent();
      this.Visit(node.Body);
      this.Dedent();
      return node;
    }

    protected internal override Expression VisitTry(TryExpression node)
    {
      this.Out(".Try {", DebugViewWriter.Flow.NewLine);
      this.Indent();
      this.Visit(node.Body);
      this.Dedent();
      ExpressionVisitor.Visit<CatchBlock>(node.Handlers, new Func<CatchBlock, CatchBlock>(((ExpressionVisitor) this).VisitCatchBlock));
      if (node.Finally != null)
      {
        this.Out(DebugViewWriter.Flow.NewLine, "} .Finally {", DebugViewWriter.Flow.NewLine);
        this.Indent();
        this.Visit(node.Finally);
        this.Dedent();
      }
      else if (node.Fault != null)
      {
        this.Out(DebugViewWriter.Flow.NewLine, "} .Fault {", DebugViewWriter.Flow.NewLine);
        this.Indent();
        this.Visit(node.Fault);
        this.Dedent();
      }
      this.Out(DebugViewWriter.Flow.NewLine, "}");
      return (Expression) node;
    }

    protected internal override Expression VisitIndex(IndexExpression node)
    {
      if (node.Indexer != (PropertyInfo) null)
        this.OutMember((Expression) node, node.Object, (MemberInfo) node.Indexer);
      else
        this.ParenthesizedVisit((Expression) node, node.Object);
      this.VisitExpressions<Expression>('[', (IList<Expression>) node.Arguments);
      return (Expression) node;
    }

    protected internal override Expression VisitExtension(Expression node)
    {
      this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ".Extension<{0}>", new object[1]
      {
        (object) node.GetType().ToString()
      }));
      if (node.CanReduce)
      {
        this.Out(DebugViewWriter.Flow.Space, "{", DebugViewWriter.Flow.NewLine);
        this.Indent();
        this.Visit(node.Reduce());
        this.Dedent();
        this.Out(DebugViewWriter.Flow.NewLine, "}");
      }
      return node;
    }

    protected internal override Expression VisitDebugInfo(DebugInfoExpression node)
    {
      this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ".DebugInfo({0}: {1}, {2} - {3}, {4})", (object) node.Document.FileName, (object) node.StartLine, (object) node.StartColumn, (object) node.EndLine, (object) node.EndColumn));
      return (Expression) node;
    }

    private void DumpLabel(LabelTarget target)
    {
      this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ".LabelTarget {0}:", new object[1]
      {
        (object) this.GetLabelTargetName(target)
      }));
    }

    private string GetLabelTargetName(LabelTarget target)
    {
      if (!string.IsNullOrEmpty(target.Name))
        return DebugViewWriter.GetDisplayName(target.Name);
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "#Label{0}", new object[1]
      {
        (object) this.GetLabelTargetId(target)
      });
    }

    private void WriteLambda(LambdaExpression lambda)
    {
      this.Out(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ".Lambda {0}<{1}>", new object[2]
      {
        (object) this.GetLambdaName(lambda),
        (object) lambda.Type.ToString()
      }));
      this.VisitDeclarations((IList<ParameterExpression>) lambda.Parameters);
      this.Out(DebugViewWriter.Flow.Space, "{", DebugViewWriter.Flow.NewLine);
      this.Indent();
      this.Visit(lambda.Body);
      this.Dedent();
      this.Out(DebugViewWriter.Flow.NewLine, "}");
    }

    private string GetLambdaName(LambdaExpression lambda)
    {
      if (string.IsNullOrEmpty(lambda.Name))
        return "#Lambda" + (object) this.GetLambdaId(lambda);
      else
        return DebugViewWriter.GetDisplayName(lambda.Name);
    }

    private static bool ContainsWhiteSpace(string name)
    {
      foreach (char c in name)
      {
        if (char.IsWhiteSpace(c))
          return true;
      }
      return false;
    }

    private static string QuoteName(string name)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}'", new object[1]
      {
        (object) name
      });
    }

    private static string GetDisplayName(string name)
    {
      if (DebugViewWriter.ContainsWhiteSpace(name))
        return DebugViewWriter.QuoteName(name);
      else
        return name;
    }

    [Flags]
    private enum Flow
    {
      None = 0,
      Space = 1,
      NewLine = 2,
      Break = 32768,
    }
  }
}
