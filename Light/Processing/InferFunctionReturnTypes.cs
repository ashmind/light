using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Literals;
using Light.Ast.Statements;
using Light.Ast.Types;

namespace Light.Processing {
    public class InferFunctionReturnTypes : IProcessingStep {
        public void Process(IAstElement root) {
            foreach (var function in root.Descendants<FunctionDefinition>()) {
                if (function.ReturnType != AstImplicitType.Instance)
                    continue;

                function.ReturnType = InferReturnType(function);
            }
        }

        private static IAstTypeReference InferReturnType(FunctionDefinition function) {
            var returns = function.Descendants<ReturnStatement>().ToArray();
            if (returns.Length == 0 || returns.All(r => r.Result == null))
                return AstVoidType.Instance;

            if (returns.Length > 1)
                throw new NotImplementedException("InferFunctionReturnTypes: " + returns.Length + " returns.");

            var primitive = returns[0].Result as PrimitiveValue;
            if (primitive == null)
                throw new NotImplementedException("InferFunctionReturnTypes: return " + returns[0].Result + ".");

            return new AstPrimitiveType(primitive.Type);
        }
    }
}
