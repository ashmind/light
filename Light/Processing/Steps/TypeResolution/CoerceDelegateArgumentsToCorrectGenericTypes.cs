using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;

namespace Light.Processing.Steps.TypeResolution {
    public class CoerceDelegateArgumentsToCorrectGenericTypes : ProcessingStepBase<CallExpression> {
        public CoerceDelegateArgumentsToCorrectGenericTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(CallExpression call, ProcessingContext context) {
            for (var i = 0; i < call.Arguments.Count; i++) {
                var functionArgument = call.Arguments[i] as AstFunctionReferenceExpression;
                if (functionArgument == null || !functionArgument.Function.IsGeneric)
                    continue;

                var callee = (AstFunctionReferenceExpression)call.Callee;

                var parameterType = (IAstFunctionTypeReference)callee.Function.ParameterTypes[i];

                functionArgument.Function = CoerceToType(functionArgument.Function, parameterType);
            }
            return call;
        }

        private IAstMethodReference CoerceToType(IAstMethodReference function, IAstFunctionTypeReference functionType) {
            var genericArgumentTypes = new IAstTypeReference[function.GenericParameterTypes.Count];
            var functionTypeParameterTypes = functionType.GetParameterTypes().ToArray();
            for (var i = 0; i < genericArgumentTypes.Length; i++) {
                var parameterIndex = function.ParameterTypes.IndexOf(function.GenericParameterTypes[i]);
                genericArgumentTypes[i] = functionTypeParameterTypes[parameterIndex];
            }

            return function.WithGenericArguments(genericArgumentTypes);
        }
    }
}
