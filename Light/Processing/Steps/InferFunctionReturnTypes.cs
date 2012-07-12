using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Incomplete;
using Light.Ast.Literals;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Ast.Statements;
using Light.Ast.Types;

namespace Light.Processing.Steps {
    public class InferFunctionReturnTypes : ProcessingStepBase<FunctionDefinition> {
        public override void ProcessAfterChildren(FunctionDefinition function, ProcessingContext context) {
            if (function.ReturnType != AstImplicitType.Instance)
                return;

            function.ReturnType = InferReturnType(function);
        }

        private static IAstTypeReference InferReturnType(FunctionDefinition function) {
            var returns = function.Descendants<ReturnStatement>().ToArray();
            if (returns.Length == 0 || returns.All(r => r.Result == null))
                return AstVoidType.Instance;

            if (returns.Length > 1)
                throw new NotImplementedException("InferFunctionReturnTypes: " + returns.Length + " returns.");

            return returns[0].Result.ExpressionType;
        }
    }
}
