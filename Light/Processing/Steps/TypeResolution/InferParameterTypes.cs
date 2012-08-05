using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Ast.Statements;

namespace Light.Processing.Steps.TypeResolution {
    public class InferParameterTypes : ProcessingStepBase<IAstFunctionDefinition> {
        public InferParameterTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(IAstFunctionDefinition function, ProcessingContext context) {
            var genericIndex = 1;
            foreach (var parameter in function.Parameters) {
                if (!parameter.Type.IsImplicit())
                    continue;

                parameter.Type = InferType(parameter, function, ref genericIndex);
            }
            return function;
        }

        private IAstTypeReference InferType(AstParameterDefinition parameter, IAstFunctionDefinition function, ref int genericIndex) {
            var types = new List<IAstTypeReference>();
            CollectTypesFromUsages(function, parameter, types);

            if (types.Count == 1)
                return types[0];

            var placeholder = new AstGenericPlaceholderType("T" + genericIndex, p => types);
            genericIndex += 1;
            return placeholder;
        }

        private void CollectTypesFromUsages(IAstElement parent, AstParameterDefinition parameter, IList<IAstTypeReference> collectedTypes) {
            foreach (var child in parent.Children()) {
                var reference = child as AstParameterReference;
                if (reference != null && reference.Parameter == parameter) {
                    var type = GetTypeFromUsage(parent, reference);
                    if (type != null)
                        collectedTypes.Add(type);

                    continue;
                }

                CollectTypesFromUsages(child, parameter, collectedTypes);
            }
        }

        private IAstTypeReference GetTypeFromUsage(IAstElement parent, AstParameterReference parameter) {
            if (parent is AstReturnStatement || parent is AstLambdaExpression)
                return null;

            var binary = parent as BinaryExpression;
            if (binary != null) {
                var argumentIndex = binary.Left == parameter ? 0 : 1;
                return binary.Operator.ParameterTypes[argumentIndex];
            }

            var call = parent as CallExpression;
            if (call != null) {
                var argumentIndex = call.Arguments.IndexOf(parameter);
                return call.Callee.ParameterTypes.ElementAt(argumentIndex);
            }

            throw new NotImplementedException("InferParameterTypes: Can not collect parameter type from usage in " + parent);
        }
    }
}
