

using System.Text;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.Literals;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Properties;
using Light.Ast.References.Types;
using Light.Ast.Statements;

namespace Light.Description {
    partial class AstToStringTransformer {
        protected void Append(StringBuilder builder, IAstElement element) {
            var assignmentStatement = element as AssignmentStatement;
            if (assignmentStatement != null) {
                AppendAssignmentStatement(builder, assignmentStatement);
                return;
            }
			
            var anyType = element as AstAnyType;
            if (anyType != null) {
                AppendAnyType(builder, anyType);
                return;
            }
			
            var baseConstructorCall = element as AstBaseConstructorCall;
            if (baseConstructorCall != null) {
                AppendBaseConstructorCall(builder, baseConstructorCall);
                return;
            }
			
            var builtInOperator = element as AstBuiltInOperator;
            if (builtInOperator != null) {
                AppendBuiltInOperator(builder, builtInOperator);
                return;
            }
			
            var constructorDefinition = element as AstConstructorDefinition;
            if (constructorDefinition != null) {
                AppendConstructorDefinition(builder, constructorDefinition);
                return;
            }
			
            var definedConstructor = element as AstDefinedConstructor;
            if (definedConstructor != null) {
                AppendDefinedConstructor(builder, definedConstructor);
                return;
            }
			
            var definedMethod = element as AstDefinedMethod;
            if (definedMethod != null) {
                AppendDefinedMethod(builder, definedMethod);
                return;
            }
			
            var definedProperty = element as AstDefinedProperty;
            if (definedProperty != null) {
                AppendDefinedProperty(builder, definedProperty);
                return;
            }
			
            var definedType = element as AstDefinedType;
            if (definedType != null) {
                AppendDefinedType(builder, definedType);
                return;
            }
			
            var functionDefinition = element as AstFunctionDefinition;
            if (functionDefinition != null) {
                AppendFunctionDefinition(builder, functionDefinition);
                return;
            }
			
            var functionReferenceExpression = element as AstFunctionReferenceExpression;
            if (functionReferenceExpression != null) {
                AppendFunctionReferenceExpression(builder, functionReferenceExpression);
                return;
            }
			
            var genericMethodWithTypeArguments = element as AstGenericMethodWithTypeArguments;
            if (genericMethodWithTypeArguments != null) {
                AppendGenericMethodWithTypeArguments(builder, genericMethodWithTypeArguments);
                return;
            }
			
            var genericPlaceholderType = element as AstGenericPlaceholderType;
            if (genericPlaceholderType != null) {
                AppendGenericPlaceholderType(builder, genericPlaceholderType);
                return;
            }
			
            var genericTypeWithArguments = element as AstGenericTypeWithArguments;
            if (genericTypeWithArguments != null) {
                AppendGenericTypeWithArguments(builder, genericTypeWithArguments);
                return;
            }
			
            var implicitType = element as AstImplicitType;
            if (implicitType != null) {
                AppendImplicitType(builder, implicitType);
                return;
            }
			
            var inferredFunctionType = element as AstInferredFunctionType;
            if (inferredFunctionType != null) {
                AppendInferredFunctionType(builder, inferredFunctionType);
                return;
            }
			
            var lambdaExpression = element as AstLambdaExpression;
            if (lambdaExpression != null) {
                AppendLambdaExpression(builder, lambdaExpression);
                return;
            }
			
            var listInitializer = element as AstListInitializer;
            if (listInitializer != null) {
                AppendListInitializer(builder, listInitializer);
                return;
            }
			
            var methodGroup = element as AstMethodGroup;
            if (methodGroup != null) {
                AppendMethodGroup(builder, methodGroup);
                return;
            }
			
            var newExpression = element as AstNewExpression;
            if (newExpression != null) {
                AppendNewExpression(builder, newExpression);
                return;
            }
			
            var parameterDefinition = element as AstParameterDefinition;
            if (parameterDefinition != null) {
                AppendParameterDefinition(builder, parameterDefinition);
                return;
            }
			
            var parameterReference = element as AstParameterReference;
            if (parameterReference != null) {
                AppendParameterReference(builder, parameterReference);
                return;
            }
			
            var propertyDefinition = element as AstPropertyDefinition;
            if (propertyDefinition != null) {
                AppendPropertyDefinition(builder, propertyDefinition);
                return;
            }
			
            var propertyExpression = element as AstPropertyExpression;
            if (propertyExpression != null) {
                AppendPropertyExpression(builder, propertyExpression);
                return;
            }
			
            var reflectedConstructor = element as AstReflectedConstructor;
            if (reflectedConstructor != null) {
                AppendReflectedConstructor(builder, reflectedConstructor);
                return;
            }
			
            var reflectedMethod = element as AstReflectedMethod;
            if (reflectedMethod != null) {
                AppendReflectedMethod(builder, reflectedMethod);
                return;
            }
			
            var reflectedProperty = element as AstReflectedProperty;
            if (reflectedProperty != null) {
                AppendReflectedProperty(builder, reflectedProperty);
                return;
            }
			
            var reflectedType = element as AstReflectedType;
            if (reflectedType != null) {
                AppendReflectedType(builder, reflectedType);
                return;
            }
			
            var returnStatement = element as AstReturnStatement;
            if (returnStatement != null) {
                AppendReturnStatement(builder, returnStatement);
                return;
            }
			
            var root = element as AstRoot;
            if (root != null) {
                AppendRoot(builder, root);
                return;
            }
			
            var specifiedFunctionType = element as AstSpecifiedFunctionType;
            if (specifiedFunctionType != null) {
                AppendSpecifiedFunctionType(builder, specifiedFunctionType);
                return;
            }
			
            var thisExpression = element as AstThisExpression;
            if (thisExpression != null) {
                AppendThisExpression(builder, thisExpression);
                return;
            }
			
            var typeDefinition = element as AstTypeDefinition;
            if (typeDefinition != null) {
                AppendTypeDefinition(builder, typeDefinition);
                return;
            }
			
            var unknownMethod = element as AstUnknownMethod;
            if (unknownMethod != null) {
                AppendUnknownMethod(builder, unknownMethod);
                return;
            }
			
            var unknownType = element as AstUnknownType;
            if (unknownType != null) {
                AppendUnknownType(builder, unknownType);
                return;
            }
			
            var variableDefinition = element as AstVariableDefinition;
            if (variableDefinition != null) {
                AppendVariableDefinition(builder, variableDefinition);
                return;
            }
			
            var variableReference = element as AstVariableReference;
            if (variableReference != null) {
                AppendVariableReference(builder, variableReference);
                return;
            }
			
            var voidType = element as AstVoidType;
            if (voidType != null) {
                AppendVoidType(builder, voidType);
                return;
            }
			
            var binaryExpression = element as BinaryExpression;
            if (binaryExpression != null) {
                AppendBinaryExpression(builder, binaryExpression);
                return;
            }
			
            var callExpression = element as CallExpression;
            if (callExpression != null) {
                AppendCallExpression(builder, callExpression);
                return;
            }
			
            var continueStatement = element as ContinueStatement;
            if (continueStatement != null) {
                AppendContinueStatement(builder, continueStatement);
                return;
            }
			
            var forStatement = element as ForStatement;
            if (forStatement != null) {
                AppendForStatement(builder, forStatement);
                return;
            }
			
            var identifierExpression = element as IdentifierExpression;
            if (identifierExpression != null) {
                AppendIdentifierExpression(builder, identifierExpression);
                return;
            }
			
            var ifOrUnlessStatement = element as IfOrUnlessStatement;
            if (ifOrUnlessStatement != null) {
                AppendIfOrUnlessStatement(builder, ifOrUnlessStatement);
                return;
            }
			
            var importDefinition = element as ImportDefinition;
            if (importDefinition != null) {
                AppendImportDefinition(builder, importDefinition);
                return;
            }
			
            var indexerExpression = element as IndexerExpression;
            if (indexerExpression != null) {
                AppendIndexerExpression(builder, indexerExpression);
                return;
            }
			
            var memberExpression = element as MemberExpression;
            if (memberExpression != null) {
                AppendMemberExpression(builder, memberExpression);
                return;
            }
			
            var objectInitializer = element as ObjectInitializer;
            if (objectInitializer != null) {
                AppendObjectInitializer(builder, objectInitializer);
                return;
            }
			
            var objectInitializerEntry = element as ObjectInitializerEntry;
            if (objectInitializerEntry != null) {
                AppendObjectInitializerEntry(builder, objectInitializerEntry);
                return;
            }
			
            var primitiveValue = element as PrimitiveValue;
            if (primitiveValue != null) {
                AppendPrimitiveValue(builder, primitiveValue);
                return;
            }
			
            var stringWithInterpolation = element as StringWithInterpolation;
            if (stringWithInterpolation != null) {
                AppendStringWithInterpolation(builder, stringWithInterpolation);
                return;
            }
			
            builder.Append(element);
        }

        protected virtual void AppendAssignmentStatement(StringBuilder builder, AssignmentStatement assignmentStatement) {
            builder.Append(assignmentStatement);
        }

        protected virtual void AppendAnyType(StringBuilder builder, AstAnyType anyType) {
            builder.Append(anyType);
        }

        protected virtual void AppendBaseConstructorCall(StringBuilder builder, AstBaseConstructorCall baseConstructorCall) {
            builder.Append(baseConstructorCall);
        }

        protected virtual void AppendBuiltInOperator(StringBuilder builder, AstBuiltInOperator builtInOperator) {
            builder.Append(builtInOperator);
        }

        protected virtual void AppendConstructorDefinition(StringBuilder builder, AstConstructorDefinition constructorDefinition) {
            builder.Append(constructorDefinition);
        }

        protected virtual void AppendDefinedConstructor(StringBuilder builder, AstDefinedConstructor definedConstructor) {
            builder.Append(definedConstructor);
        }

        protected virtual void AppendDefinedMethod(StringBuilder builder, AstDefinedMethod definedMethod) {
            builder.Append(definedMethod);
        }

        protected virtual void AppendDefinedProperty(StringBuilder builder, AstDefinedProperty definedProperty) {
            builder.Append(definedProperty);
        }

        protected virtual void AppendDefinedType(StringBuilder builder, AstDefinedType definedType) {
            builder.Append(definedType);
        }

        protected virtual void AppendFunctionDefinition(StringBuilder builder, AstFunctionDefinition functionDefinition) {
            builder.Append(functionDefinition);
        }

        protected virtual void AppendFunctionReferenceExpression(StringBuilder builder, AstFunctionReferenceExpression functionReferenceExpression) {
            builder.Append(functionReferenceExpression);
        }

        protected virtual void AppendGenericMethodWithTypeArguments(StringBuilder builder, AstGenericMethodWithTypeArguments genericMethodWithTypeArguments) {
            builder.Append(genericMethodWithTypeArguments);
        }

        protected virtual void AppendGenericPlaceholderType(StringBuilder builder, AstGenericPlaceholderType genericPlaceholderType) {
            builder.Append(genericPlaceholderType);
        }

        protected virtual void AppendGenericTypeWithArguments(StringBuilder builder, AstGenericTypeWithArguments genericTypeWithArguments) {
            builder.Append(genericTypeWithArguments);
        }

        protected virtual void AppendImplicitType(StringBuilder builder, AstImplicitType implicitType) {
            builder.Append(implicitType);
        }

        protected virtual void AppendInferredFunctionType(StringBuilder builder, AstInferredFunctionType inferredFunctionType) {
            builder.Append(inferredFunctionType);
        }

        protected virtual void AppendLambdaExpression(StringBuilder builder, AstLambdaExpression lambdaExpression) {
            builder.Append(lambdaExpression);
        }

        protected virtual void AppendListInitializer(StringBuilder builder, AstListInitializer listInitializer) {
            builder.Append(listInitializer);
        }

        protected virtual void AppendMethodGroup(StringBuilder builder, AstMethodGroup methodGroup) {
            builder.Append(methodGroup);
        }

        protected virtual void AppendNewExpression(StringBuilder builder, AstNewExpression newExpression) {
            builder.Append(newExpression);
        }

        protected virtual void AppendParameterDefinition(StringBuilder builder, AstParameterDefinition parameterDefinition) {
            builder.Append(parameterDefinition);
        }

        protected virtual void AppendParameterReference(StringBuilder builder, AstParameterReference parameterReference) {
            builder.Append(parameterReference);
        }

        protected virtual void AppendPropertyDefinition(StringBuilder builder, AstPropertyDefinition propertyDefinition) {
            builder.Append(propertyDefinition);
        }

        protected virtual void AppendPropertyExpression(StringBuilder builder, AstPropertyExpression propertyExpression) {
            builder.Append(propertyExpression);
        }

        protected virtual void AppendReflectedConstructor(StringBuilder builder, AstReflectedConstructor reflectedConstructor) {
            builder.Append(reflectedConstructor);
        }

        protected virtual void AppendReflectedMethod(StringBuilder builder, AstReflectedMethod reflectedMethod) {
            builder.Append(reflectedMethod);
        }

        protected virtual void AppendReflectedProperty(StringBuilder builder, AstReflectedProperty reflectedProperty) {
            builder.Append(reflectedProperty);
        }

        protected virtual void AppendReflectedType(StringBuilder builder, AstReflectedType reflectedType) {
            builder.Append(reflectedType);
        }

        protected virtual void AppendReturnStatement(StringBuilder builder, AstReturnStatement returnStatement) {
            builder.Append(returnStatement);
        }

        protected virtual void AppendRoot(StringBuilder builder, AstRoot root) {
            builder.Append(root);
        }

        protected virtual void AppendSpecifiedFunctionType(StringBuilder builder, AstSpecifiedFunctionType specifiedFunctionType) {
            builder.Append(specifiedFunctionType);
        }

        protected virtual void AppendThisExpression(StringBuilder builder, AstThisExpression thisExpression) {
            builder.Append(thisExpression);
        }

        protected virtual void AppendTypeDefinition(StringBuilder builder, AstTypeDefinition typeDefinition) {
            builder.Append(typeDefinition);
        }

        protected virtual void AppendUnknownMethod(StringBuilder builder, AstUnknownMethod unknownMethod) {
            builder.Append(unknownMethod);
        }

        protected virtual void AppendUnknownType(StringBuilder builder, AstUnknownType unknownType) {
            builder.Append(unknownType);
        }

        protected virtual void AppendVariableDefinition(StringBuilder builder, AstVariableDefinition variableDefinition) {
            builder.Append(variableDefinition);
        }

        protected virtual void AppendVariableReference(StringBuilder builder, AstVariableReference variableReference) {
            builder.Append(variableReference);
        }

        protected virtual void AppendVoidType(StringBuilder builder, AstVoidType voidType) {
            builder.Append(voidType);
        }

        protected virtual void AppendBinaryExpression(StringBuilder builder, BinaryExpression binaryExpression) {
            builder.Append(binaryExpression);
        }

        protected virtual void AppendCallExpression(StringBuilder builder, CallExpression callExpression) {
            builder.Append(callExpression);
        }

        protected virtual void AppendContinueStatement(StringBuilder builder, ContinueStatement continueStatement) {
            builder.Append(continueStatement);
        }

        protected virtual void AppendForStatement(StringBuilder builder, ForStatement forStatement) {
            builder.Append(forStatement);
        }

        protected virtual void AppendIdentifierExpression(StringBuilder builder, IdentifierExpression identifierExpression) {
            builder.Append(identifierExpression);
        }

        protected virtual void AppendIfOrUnlessStatement(StringBuilder builder, IfOrUnlessStatement ifOrUnlessStatement) {
            builder.Append(ifOrUnlessStatement);
        }

        protected virtual void AppendImportDefinition(StringBuilder builder, ImportDefinition importDefinition) {
            builder.Append(importDefinition);
        }

        protected virtual void AppendIndexerExpression(StringBuilder builder, IndexerExpression indexerExpression) {
            builder.Append(indexerExpression);
        }

        protected virtual void AppendMemberExpression(StringBuilder builder, MemberExpression memberExpression) {
            builder.Append(memberExpression);
        }

        protected virtual void AppendObjectInitializer(StringBuilder builder, ObjectInitializer objectInitializer) {
            builder.Append(objectInitializer);
        }

        protected virtual void AppendObjectInitializerEntry(StringBuilder builder, ObjectInitializerEntry objectInitializerEntry) {
            builder.Append(objectInitializerEntry);
        }

        protected virtual void AppendPrimitiveValue(StringBuilder builder, PrimitiveValue primitiveValue) {
            builder.Append(primitiveValue);
        }

        protected virtual void AppendStringWithInterpolation(StringBuilder builder, StringWithInterpolation stringWithInterpolation) {
            builder.Append(stringWithInterpolation);
        }

        }
}