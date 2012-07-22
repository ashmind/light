using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Ast.Statements;

namespace Light.Processing.Steps.TypeResolution {
    public class InferFunctionReturnTypes : ProcessingStepBase<AstFunctionDefinition> {
        public InferFunctionReturnTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstFunctionDefinition function, ProcessingContext context) {
            if (function.ReturnType != AstImplicitType.Instance)
                return function;

            function.ReturnType = InferReturnType(function);
            return function;
        }

        private static IAstTypeReference InferReturnType(AstFunctionDefinition function) {
            var returns = function.Descendants<AstReturnStatement>().ToArray();
            if (returns.Length == 0 || returns.All(r => r.Result == null))
                return AstVoidType.Instance;

            if (returns.Length > 1)
                throw new NotImplementedException("InferFunctionReturnTypes: " + returns.Length + " returns.");

            return returns[0].Result.ExpressionType;
        }
    }
}
