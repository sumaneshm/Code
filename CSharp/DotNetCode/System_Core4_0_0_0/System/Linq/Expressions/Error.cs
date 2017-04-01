// Type: System.Linq.Expressions.Error
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: FF46A1FC-D9B9-4455-A224-A9DA86AA1C2B
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Core.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Expressions
{
  internal static class Error
  {
    internal static Exception TypeMustBeDerivedFromSystemDelegate()
    {
      return (Exception) new ArgumentException(Strings.TypeMustBeDerivedFromSystemDelegate);
    }

    internal static Exception TypeParameterIsNotDelegate(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.TypeParameterIsNotDelegate(p0));
    }

    internal static Exception NoOrInvalidRuleProduced()
    {
      return (Exception) new InvalidOperationException(Strings.NoOrInvalidRuleProduced);
    }

    internal static Exception FirstArgumentMustBeCallSite()
    {
      return (Exception) new ArgumentException(Strings.FirstArgumentMustBeCallSite);
    }

    internal static Exception QueueEmpty()
    {
      return (Exception) new InvalidOperationException(Strings.QueueEmpty);
    }

    internal static Exception MustRewriteToSameNode(object p0, object p1, object p2)
    {
      return (Exception) new InvalidOperationException(Strings.MustRewriteToSameNode(p0, p1, p2));
    }

    internal static Exception CollectionModifiedWhileEnumerating()
    {
      return (Exception) new InvalidOperationException(Strings.CollectionModifiedWhileEnumerating);
    }

    internal static Exception EnumerationIsDone()
    {
      return (Exception) new InvalidOperationException(Strings.EnumerationIsDone);
    }

    internal static Exception HomogenousAppDomainRequired()
    {
      return (Exception) new InvalidOperationException(Strings.HomogenousAppDomainRequired);
    }

    internal static Exception ArgCntMustBeGreaterThanNameCnt()
    {
      return (Exception) new ArgumentException(Strings.ArgCntMustBeGreaterThanNameCnt);
    }

    internal static Exception ReducibleMustOverrideReduce()
    {
      return (Exception) new ArgumentException(Strings.ReducibleMustOverrideReduce);
    }

    internal static Exception MustReduceToDifferent()
    {
      return (Exception) new ArgumentException(Strings.MustReduceToDifferent);
    }

    internal static Exception ReducedNotCompatible()
    {
      return (Exception) new ArgumentException(Strings.ReducedNotCompatible);
    }

    internal static Exception SetterHasNoParams()
    {
      return (Exception) new ArgumentException(Strings.SetterHasNoParams);
    }

    internal static Exception PropertyCannotHaveRefType()
    {
      return (Exception) new ArgumentException(Strings.PropertyCannotHaveRefType);
    }

    internal static Exception IndexesOfSetGetMustMatch()
    {
      return (Exception) new ArgumentException(Strings.IndexesOfSetGetMustMatch);
    }

    internal static Exception AccessorsCannotHaveVarArgs()
    {
      return (Exception) new ArgumentException(Strings.AccessorsCannotHaveVarArgs);
    }

    internal static Exception AccessorsCannotHaveByRefArgs()
    {
      return (Exception) new ArgumentException(Strings.AccessorsCannotHaveByRefArgs);
    }

    internal static Exception BoundsCannotBeLessThanOne()
    {
      return (Exception) new ArgumentException(Strings.BoundsCannotBeLessThanOne);
    }

    internal static Exception TypeMustNotBeByRef()
    {
      return (Exception) new ArgumentException(Strings.TypeMustNotBeByRef);
    }

    internal static Exception TypeDoesNotHaveConstructorForTheSignature()
    {
      return (Exception) new ArgumentException(Strings.TypeDoesNotHaveConstructorForTheSignature);
    }

    internal static Exception CountCannotBeNegative()
    {
      return (Exception) new ArgumentException(Strings.CountCannotBeNegative);
    }

    internal static Exception ArrayTypeMustBeArray()
    {
      return (Exception) new ArgumentException(Strings.ArrayTypeMustBeArray);
    }

    internal static Exception SetterMustBeVoid()
    {
      return (Exception) new ArgumentException(Strings.SetterMustBeVoid);
    }

    internal static Exception PropertyTyepMustMatchSetter()
    {
      return (Exception) new ArgumentException(Strings.PropertyTyepMustMatchSetter);
    }

    internal static Exception BothAccessorsMustBeStatic()
    {
      return (Exception) new ArgumentException(Strings.BothAccessorsMustBeStatic);
    }

    internal static Exception OnlyStaticMethodsHaveNullInstance()
    {
      return (Exception) new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance);
    }

    internal static Exception PropertyTypeCannotBeVoid()
    {
      return (Exception) new ArgumentException(Strings.PropertyTypeCannotBeVoid);
    }

    internal static Exception InvalidUnboxType()
    {
      return (Exception) new ArgumentException(Strings.InvalidUnboxType);
    }

    internal static Exception ArgumentMustNotHaveValueType()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustNotHaveValueType);
    }

    internal static Exception MustBeReducible()
    {
      return (Exception) new ArgumentException(Strings.MustBeReducible);
    }

    internal static Exception DefaultBodyMustBeSupplied()
    {
      return (Exception) new ArgumentException(Strings.DefaultBodyMustBeSupplied);
    }

    internal static Exception MethodBuilderDoesNotHaveTypeBuilder()
    {
      return (Exception) new ArgumentException(Strings.MethodBuilderDoesNotHaveTypeBuilder);
    }

    internal static Exception ArgumentTypeCannotBeVoid()
    {
      return (Exception) new ArgumentException(Strings.ArgumentTypeCannotBeVoid);
    }

    internal static Exception LabelMustBeVoidOrHaveExpression()
    {
      return (Exception) new ArgumentException(Strings.LabelMustBeVoidOrHaveExpression);
    }

    internal static Exception LabelTypeMustBeVoid()
    {
      return (Exception) new ArgumentException(Strings.LabelTypeMustBeVoid);
    }

    internal static Exception QuotedExpressionMustBeLambda()
    {
      return (Exception) new ArgumentException(Strings.QuotedExpressionMustBeLambda);
    }

    internal static Exception VariableMustNotBeByRef(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.VariableMustNotBeByRef(p0, p1));
    }

    internal static Exception DuplicateVariable(object p0)
    {
      return (Exception) new ArgumentException(Strings.DuplicateVariable(p0));
    }

    internal static Exception StartEndMustBeOrdered()
    {
      return (Exception) new ArgumentException(Strings.StartEndMustBeOrdered);
    }

    internal static Exception FaultCannotHaveCatchOrFinally()
    {
      return (Exception) new ArgumentException(Strings.FaultCannotHaveCatchOrFinally);
    }

    internal static Exception TryMustHaveCatchFinallyOrFault()
    {
      return (Exception) new ArgumentException(Strings.TryMustHaveCatchFinallyOrFault);
    }

    internal static Exception BodyOfCatchMustHaveSameTypeAsBodyOfTry()
    {
      return (Exception) new ArgumentException(Strings.BodyOfCatchMustHaveSameTypeAsBodyOfTry);
    }

    internal static Exception ExtensionNodeMustOverrideProperty(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.ExtensionNodeMustOverrideProperty(p0));
    }

    internal static Exception UserDefinedOperatorMustBeStatic(object p0)
    {
      return (Exception) new ArgumentException(Strings.UserDefinedOperatorMustBeStatic(p0));
    }

    internal static Exception UserDefinedOperatorMustNotBeVoid(object p0)
    {
      return (Exception) new ArgumentException(Strings.UserDefinedOperatorMustNotBeVoid(p0));
    }

    internal static Exception CoercionOperatorNotDefined(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.CoercionOperatorNotDefined(p0, p1));
    }

    internal static Exception DynamicBinderResultNotAssignable(object p0, object p1, object p2)
    {
      return (Exception) new InvalidCastException(Strings.DynamicBinderResultNotAssignable(p0, p1, p2));
    }

    internal static Exception DynamicObjectResultNotAssignable(object p0, object p1, object p2, object p3)
    {
      return (Exception) new InvalidCastException(Strings.DynamicObjectResultNotAssignable(p0, p1, p2, p3));
    }

    internal static Exception DynamicBindingNeedsRestrictions(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.DynamicBindingNeedsRestrictions(p0, p1));
    }

    internal static Exception BinderNotCompatibleWithCallSite(object p0, object p1, object p2)
    {
      return (Exception) new InvalidOperationException(Strings.BinderNotCompatibleWithCallSite(p0, p1, p2));
    }

    internal static Exception UnaryOperatorNotDefined(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.UnaryOperatorNotDefined(p0, p1));
    }

    internal static Exception BinaryOperatorNotDefined(object p0, object p1, object p2)
    {
      return (Exception) new InvalidOperationException(Strings.BinaryOperatorNotDefined(p0, p1, p2));
    }

    internal static Exception ReferenceEqualityNotDefined(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.ReferenceEqualityNotDefined(p0, p1));
    }

    internal static Exception OperandTypesDoNotMatchParameters(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.OperandTypesDoNotMatchParameters(p0, p1));
    }

    internal static Exception OverloadOperatorTypeDoesNotMatchConversionType(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.OverloadOperatorTypeDoesNotMatchConversionType(p0, p1));
    }

    internal static Exception ConversionIsNotSupportedForArithmeticTypes()
    {
      return (Exception) new InvalidOperationException(Strings.ConversionIsNotSupportedForArithmeticTypes);
    }

    internal static Exception ArgumentMustBeArray()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeArray);
    }

    internal static Exception ArgumentMustBeBoolean()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeBoolean);
    }

    internal static Exception EqualityMustReturnBoolean(object p0)
    {
      return (Exception) new ArgumentException(Strings.EqualityMustReturnBoolean(p0));
    }

    internal static Exception ArgumentMustBeFieldInfoOrPropertInfo()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeFieldInfoOrPropertInfo);
    }

    internal static Exception ArgumentMustBeFieldInfoOrPropertInfoOrMethod()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeFieldInfoOrPropertInfoOrMethod);
    }

    internal static Exception ArgumentMustBeInstanceMember()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeInstanceMember);
    }

    internal static Exception ArgumentMustBeInteger()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeInteger);
    }

    internal static Exception ArgumentMustBeArrayIndexType()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeArrayIndexType);
    }

    internal static Exception ArgumentMustBeSingleDimensionalArrayType()
    {
      return (Exception) new ArgumentException(Strings.ArgumentMustBeSingleDimensionalArrayType);
    }

    internal static Exception ArgumentTypesMustMatch()
    {
      return (Exception) new ArgumentException(Strings.ArgumentTypesMustMatch);
    }

    internal static Exception CannotAutoInitializeValueTypeElementThroughProperty(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.CannotAutoInitializeValueTypeElementThroughProperty(p0));
    }

    internal static Exception CannotAutoInitializeValueTypeMemberThroughProperty(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.CannotAutoInitializeValueTypeMemberThroughProperty(p0));
    }

    internal static Exception IncorrectTypeForTypeAs(object p0)
    {
      return (Exception) new ArgumentException(Strings.IncorrectTypeForTypeAs(p0));
    }

    internal static Exception CoalesceUsedOnNonNullType()
    {
      return (Exception) new InvalidOperationException(Strings.CoalesceUsedOnNonNullType);
    }

    internal static Exception ExpressionTypeCannotInitializeArrayType(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.ExpressionTypeCannotInitializeArrayType(p0, p1));
    }

    internal static Exception ExpressionTypeDoesNotMatchConstructorParameter(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeDoesNotMatchConstructorParameter(p0, p1));
    }

    internal static Exception ArgumentTypeDoesNotMatchMember(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ArgumentTypeDoesNotMatchMember(p0, p1));
    }

    internal static Exception ArgumentMemberNotDeclOnType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ArgumentMemberNotDeclOnType(p0, p1));
    }

    internal static Exception ExpressionTypeDoesNotMatchMethodParameter(object p0, object p1, object p2)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeDoesNotMatchMethodParameter(p0, p1, p2));
    }

    internal static Exception ExpressionTypeDoesNotMatchParameter(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeDoesNotMatchParameter(p0, p1));
    }

    internal static Exception ExpressionTypeDoesNotMatchReturn(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeDoesNotMatchReturn(p0, p1));
    }

    internal static Exception ExpressionTypeDoesNotMatchAssignment(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeDoesNotMatchAssignment(p0, p1));
    }

    internal static Exception ExpressionTypeDoesNotMatchLabel(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeDoesNotMatchLabel(p0, p1));
    }

    internal static Exception ExpressionTypeNotInvocable(object p0)
    {
      return (Exception) new ArgumentException(Strings.ExpressionTypeNotInvocable(p0));
    }

    internal static Exception FieldNotDefinedForType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.FieldNotDefinedForType(p0, p1));
    }

    internal static Exception InstanceFieldNotDefinedForType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.InstanceFieldNotDefinedForType(p0, p1));
    }

    internal static Exception FieldInfoNotDefinedForType(object p0, object p1, object p2)
    {
      return (Exception) new ArgumentException(Strings.FieldInfoNotDefinedForType(p0, p1, p2));
    }

    internal static Exception IncorrectNumberOfIndexes()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfIndexes);
    }

    internal static Exception IncorrectNumberOfLambdaArguments()
    {
      return (Exception) new InvalidOperationException(Strings.IncorrectNumberOfLambdaArguments);
    }

    internal static Exception IncorrectNumberOfLambdaDeclarationParameters()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfLambdaDeclarationParameters);
    }

    internal static Exception IncorrectNumberOfMethodCallArguments(object p0)
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfMethodCallArguments(p0));
    }

    internal static Exception IncorrectNumberOfConstructorArguments()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfConstructorArguments);
    }

    internal static Exception IncorrectNumberOfMembersForGivenConstructor()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfMembersForGivenConstructor);
    }

    internal static Exception IncorrectNumberOfArgumentsForMembers()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfArgumentsForMembers);
    }

    internal static Exception LambdaTypeMustBeDerivedFromSystemDelegate()
    {
      return (Exception) new ArgumentException(Strings.LambdaTypeMustBeDerivedFromSystemDelegate);
    }

    internal static Exception MemberNotFieldOrProperty(object p0)
    {
      return (Exception) new ArgumentException(Strings.MemberNotFieldOrProperty(p0));
    }

    internal static Exception MethodContainsGenericParameters(object p0)
    {
      return (Exception) new ArgumentException(Strings.MethodContainsGenericParameters(p0));
    }

    internal static Exception MethodIsGeneric(object p0)
    {
      return (Exception) new ArgumentException(Strings.MethodIsGeneric(p0));
    }

    internal static Exception MethodNotPropertyAccessor(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.MethodNotPropertyAccessor(p0, p1));
    }

    internal static Exception PropertyDoesNotHaveGetter(object p0)
    {
      return (Exception) new ArgumentException(Strings.PropertyDoesNotHaveGetter(p0));
    }

    internal static Exception PropertyDoesNotHaveSetter(object p0)
    {
      return (Exception) new ArgumentException(Strings.PropertyDoesNotHaveSetter(p0));
    }

    internal static Exception PropertyDoesNotHaveAccessor(object p0)
    {
      return (Exception) new ArgumentException(Strings.PropertyDoesNotHaveAccessor(p0));
    }

    internal static Exception NotAMemberOfType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.NotAMemberOfType(p0, p1));
    }

    internal static Exception OperatorNotImplementedForType(object p0, object p1)
    {
      return (Exception) new NotImplementedException(Strings.OperatorNotImplementedForType(p0, p1));
    }

    internal static Exception ParameterExpressionNotValidAsDelegate(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ParameterExpressionNotValidAsDelegate(p0, p1));
    }

    internal static Exception PropertyNotDefinedForType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.PropertyNotDefinedForType(p0, p1));
    }

    internal static Exception InstancePropertyNotDefinedForType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.InstancePropertyNotDefinedForType(p0, p1));
    }

    internal static Exception InstancePropertyWithoutParameterNotDefinedForType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.InstancePropertyWithoutParameterNotDefinedForType(p0, p1));
    }

    internal static Exception InstancePropertyWithSpecifiedParametersNotDefinedForType(object p0, object p1, object p2)
    {
      return (Exception) new ArgumentException(Strings.InstancePropertyWithSpecifiedParametersNotDefinedForType(p0, p1, p2));
    }

    internal static Exception InstanceAndMethodTypeMismatch(object p0, object p1, object p2)
    {
      return (Exception) new ArgumentException(Strings.InstanceAndMethodTypeMismatch(p0, p1, p2));
    }

    internal static Exception TypeContainsGenericParameters(object p0)
    {
      return (Exception) new ArgumentException(Strings.TypeContainsGenericParameters(p0));
    }

    internal static Exception TypeIsGeneric(object p0)
    {
      return (Exception) new ArgumentException(Strings.TypeIsGeneric(p0));
    }

    internal static Exception TypeMissingDefaultConstructor(object p0)
    {
      return (Exception) new ArgumentException(Strings.TypeMissingDefaultConstructor(p0));
    }

    internal static Exception ListInitializerWithZeroMembers()
    {
      return (Exception) new ArgumentException(Strings.ListInitializerWithZeroMembers);
    }

    internal static Exception ElementInitializerMethodNotAdd()
    {
      return (Exception) new ArgumentException(Strings.ElementInitializerMethodNotAdd);
    }

    internal static Exception ElementInitializerMethodNoRefOutParam(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.ElementInitializerMethodNoRefOutParam(p0, p1));
    }

    internal static Exception ElementInitializerMethodWithZeroArgs()
    {
      return (Exception) new ArgumentException(Strings.ElementInitializerMethodWithZeroArgs);
    }

    internal static Exception ElementInitializerMethodStatic()
    {
      return (Exception) new ArgumentException(Strings.ElementInitializerMethodStatic);
    }

    internal static Exception TypeNotIEnumerable(object p0)
    {
      return (Exception) new ArgumentException(Strings.TypeNotIEnumerable(p0));
    }

    internal static Exception UnexpectedCoalesceOperator()
    {
      return (Exception) new InvalidOperationException(Strings.UnexpectedCoalesceOperator);
    }

    internal static Exception InvalidCast(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.InvalidCast(p0, p1));
    }

    internal static Exception UnhandledBinary(object p0)
    {
      return (Exception) new ArgumentException(Strings.UnhandledBinary(p0));
    }

    internal static Exception UnhandledBinding()
    {
      return (Exception) new ArgumentException(Strings.UnhandledBinding);
    }

    internal static Exception UnhandledBindingType(object p0)
    {
      return (Exception) new ArgumentException(Strings.UnhandledBindingType(p0));
    }

    internal static Exception UnhandledConvert(object p0)
    {
      return (Exception) new ArgumentException(Strings.UnhandledConvert(p0));
    }

    internal static Exception UnhandledExpressionType(object p0)
    {
      return (Exception) new ArgumentException(Strings.UnhandledExpressionType(p0));
    }

    internal static Exception UnhandledUnary(object p0)
    {
      return (Exception) new ArgumentException(Strings.UnhandledUnary(p0));
    }

    internal static Exception UnknownBindingType()
    {
      return (Exception) new ArgumentException(Strings.UnknownBindingType);
    }

    internal static Exception UserDefinedOpMustHaveConsistentTypes(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.UserDefinedOpMustHaveConsistentTypes(p0, p1));
    }

    internal static Exception UserDefinedOpMustHaveValidReturnType(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.UserDefinedOpMustHaveValidReturnType(p0, p1));
    }

    internal static Exception LogicalOperatorMustHaveBooleanOperators(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.LogicalOperatorMustHaveBooleanOperators(p0, p1));
    }

    internal static Exception MethodDoesNotExistOnType(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.MethodDoesNotExistOnType(p0, p1));
    }

    internal static Exception MethodWithArgsDoesNotExistOnType(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.MethodWithArgsDoesNotExistOnType(p0, p1));
    }

    internal static Exception GenericMethodWithArgsDoesNotExistOnType(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.GenericMethodWithArgsDoesNotExistOnType(p0, p1));
    }

    internal static Exception MethodWithMoreThanOneMatch(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.MethodWithMoreThanOneMatch(p0, p1));
    }

    internal static Exception PropertyWithMoreThanOneMatch(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.PropertyWithMoreThanOneMatch(p0, p1));
    }

    internal static Exception IncorrectNumberOfTypeArgsForFunc()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfTypeArgsForFunc);
    }

    internal static Exception IncorrectNumberOfTypeArgsForAction()
    {
      return (Exception) new ArgumentException(Strings.IncorrectNumberOfTypeArgsForAction);
    }

    internal static Exception ArgumentCannotBeOfTypeVoid()
    {
      return (Exception) new ArgumentException(Strings.ArgumentCannotBeOfTypeVoid);
    }

    internal static Exception AmbiguousMatchInExpandoObject(object p0)
    {
      return (Exception) new AmbiguousMatchException(Strings.AmbiguousMatchInExpandoObject(p0));
    }

    internal static Exception SameKeyExistsInExpando(object p0)
    {
      return (Exception) new ArgumentException(Strings.SameKeyExistsInExpando(p0));
    }

    internal static Exception KeyDoesNotExistInExpando(object p0)
    {
      return (Exception) new KeyNotFoundException(Strings.KeyDoesNotExistInExpando(p0));
    }

    internal static Exception BindingCannotBeNull()
    {
      return (Exception) new InvalidOperationException(Strings.BindingCannotBeNull);
    }

    internal static Exception InvalidOperation(object p0)
    {
      return (Exception) new ArgumentException(Strings.InvalidOperation(p0));
    }

    internal static Exception OutOfRange(object p0, object p1)
    {
      return (Exception) new ArgumentOutOfRangeException(Strings.OutOfRange(p0, p1));
    }

    internal static Exception LabelTargetAlreadyDefined(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.LabelTargetAlreadyDefined(p0));
    }

    internal static Exception LabelTargetUndefined(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.LabelTargetUndefined(p0));
    }

    internal static Exception ControlCannotLeaveFinally()
    {
      return (Exception) new InvalidOperationException(Strings.ControlCannotLeaveFinally);
    }

    internal static Exception ControlCannotLeaveFilterTest()
    {
      return (Exception) new InvalidOperationException(Strings.ControlCannotLeaveFilterTest);
    }

    internal static Exception AmbiguousJump(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.AmbiguousJump(p0));
    }

    internal static Exception ControlCannotEnterTry()
    {
      return (Exception) new InvalidOperationException(Strings.ControlCannotEnterTry);
    }

    internal static Exception ControlCannotEnterExpression()
    {
      return (Exception) new InvalidOperationException(Strings.ControlCannotEnterExpression);
    }

    internal static Exception NonLocalJumpWithValue(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.NonLocalJumpWithValue(p0));
    }

    internal static Exception ExtensionNotReduced()
    {
      return (Exception) new InvalidOperationException(Strings.ExtensionNotReduced);
    }

    internal static Exception CannotCompileConstant(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.CannotCompileConstant(p0));
    }

    internal static Exception CannotCompileDynamic()
    {
      return (Exception) new NotSupportedException(Strings.CannotCompileDynamic);
    }

    internal static Exception InvalidLvalue(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.InvalidLvalue(p0));
    }

    internal static Exception InvalidMemberType(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.InvalidMemberType(p0));
    }

    internal static Exception UnknownLiftType(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.UnknownLiftType(p0));
    }

    internal static Exception InvalidOutputDir()
    {
      return (Exception) new ArgumentException(Strings.InvalidOutputDir);
    }

    internal static Exception InvalidAsmNameOrExtension()
    {
      return (Exception) new ArgumentException(Strings.InvalidAsmNameOrExtension);
    }

    internal static Exception CollectionReadOnly()
    {
      return (Exception) new NotSupportedException(Strings.CollectionReadOnly);
    }

    internal static Exception IllegalNewGenericParams(object p0)
    {
      return (Exception) new ArgumentException(Strings.IllegalNewGenericParams(p0));
    }

    internal static Exception UndefinedVariable(object p0, object p1, object p2)
    {
      return (Exception) new InvalidOperationException(Strings.UndefinedVariable(p0, p1, p2));
    }

    internal static Exception CannotCloseOverByRef(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.CannotCloseOverByRef(p0, p1));
    }

    internal static Exception UnexpectedVarArgsCall(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.UnexpectedVarArgsCall(p0));
    }

    internal static Exception RethrowRequiresCatch()
    {
      return (Exception) new InvalidOperationException(Strings.RethrowRequiresCatch);
    }

    internal static Exception TryNotAllowedInFilter()
    {
      return (Exception) new InvalidOperationException(Strings.TryNotAllowedInFilter);
    }

    internal static Exception MustRewriteChildToSameType(object p0, object p1, object p2)
    {
      return (Exception) new InvalidOperationException(Strings.MustRewriteChildToSameType(p0, p1, p2));
    }

    internal static Exception MustRewriteWithoutMethod(object p0, object p1)
    {
      return (Exception) new InvalidOperationException(Strings.MustRewriteWithoutMethod(p0, p1));
    }

    internal static Exception TryNotSupportedForMethodsWithRefArgs(object p0)
    {
      return (Exception) new NotSupportedException(Strings.TryNotSupportedForMethodsWithRefArgs(p0));
    }

    internal static Exception TryNotSupportedForValueTypeInstances(object p0)
    {
      return (Exception) new NotSupportedException(Strings.TryNotSupportedForValueTypeInstances(p0));
    }

    internal static Exception TestValueTypeDoesNotMatchComparisonMethodParameter(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.TestValueTypeDoesNotMatchComparisonMethodParameter(p0, p1));
    }

    internal static Exception SwitchValueTypeDoesNotMatchComparisonMethodParameter(object p0, object p1)
    {
      return (Exception) new ArgumentException(Strings.SwitchValueTypeDoesNotMatchComparisonMethodParameter(p0, p1));
    }

    internal static Exception InvalidMetaObjectCreated(object p0)
    {
      return (Exception) new InvalidOperationException(Strings.InvalidMetaObjectCreated(p0));
    }

    internal static Exception PdbGeneratorNeedsExpressionCompiler()
    {
      return (Exception) new NotSupportedException(Strings.PdbGeneratorNeedsExpressionCompiler);
    }

    internal static Exception ArgumentNull(string paramName)
    {
      return (Exception) new ArgumentNullException(paramName);
    }

    internal static Exception ArgumentOutOfRange(string paramName)
    {
      return (Exception) new ArgumentOutOfRangeException(paramName);
    }

    internal static Exception NotImplemented()
    {
      return (Exception) new NotImplementedException();
    }

    internal static Exception NotSupported()
    {
      return (Exception) new NotSupportedException();
    }
  }
}
