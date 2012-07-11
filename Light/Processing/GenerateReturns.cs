using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Statements;
using Light.Ast.Types;

namespace Light.Processing {
    public class GenerateReturns : IProcessingStep {
        public void Process(IAstElement root) {
            foreach (var method in root.Descendants<MethodDefinitionBase>()) {
                if (method.Body.OfType<ReturnStatement>().Any())
                    continue;

                var function = method as FunctionDefinition;
                if (function != null && !function.ReturnType.IsVoid())
                    throw new NotSupportedException("GenerateReturns: function return type is " + function.ReturnType);

                method.Body.Add(new ReturnStatement());
            }
        }
    }
}
