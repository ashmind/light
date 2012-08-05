using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.TypeResolution {
    public class CoerceDelegateArgumentsToCorrectGenericTypes : ProcessingStepBase<CallExpression> {
        private readonly GenericTypeHelper genericHelper;

        public CoerceDelegateArgumentsToCorrectGenericTypes(GenericTypeHelper genericHelper) : base(ProcessingStage.TypeResolution) {
            this.genericHelper = genericHelper;
        }

        public override IAstElement ProcessAfterChildren(CallExpression call, ProcessingContext context) {
            var callee = (AstFunctionReferenceExpression)call.Callee;
            var caleeParameters = (callee.Function.ParameterTypes as IList<IAstTypeReference>) ?? callee.Function.ParameterTypes.ToArray();

            for (var i = 0; i < call.Arguments.Count; i++) {
                var functionArgument = call.Arguments[i] as AstFunctionReferenceExpression;
                if (functionArgument == null || !functionArgument.Function.IsGeneric)
                    continue;

                var parameterIndex = callee.Function.Location == MethodLocation.Target ? i : i + 1;
                var parameterType = (IAstFunctionTypeReference)caleeParameters[parameterIndex];

                functionArgument.Function = CoerceToType(functionArgument.Function, parameterType);
            }
            return call;
        }

        private IAstMethodReference CoerceToType(IAstMethodReference function, IAstFunctionTypeReference functionType) {
            var genericParameterTypes = function.GetGenericParameterTypes().ToArray();
            var genericArgumentTypes = new IAstTypeReference[genericParameterTypes.Length];
            var functionTypeParameterTypes = functionType.GetParameterTypes().ToArray();
            var functionParameterTypes = (function.ParameterTypes as IList<IAstTypeReference>) ?? function.ParameterTypes.ToArray();

            for (var i = 0; i < genericArgumentTypes.Length; i++) {
                var parameterIndex = functionParameterTypes.IndexOf(genericParameterTypes[i]);
                genericArgumentTypes[i] = functionTypeParameterTypes[parameterIndex];
            }

            return new AstGenericMethodWithTypeArguments(function, genericArgumentTypes, this.genericHelper);
        }
    }
}
