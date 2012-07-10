using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.Statements;

namespace Light.Processing {
    public class GenerateReturns : IProcessingStep {
        public void Process(IAstElement root) {
            foreach (var function in root.Descendants<MethodDefinitionBase>()) {
                if (function.Body.Any())
                    continue;

                // TODO: consider return types
                function.Body.Add(new ReturnStatement());
            }
        }
    }
}
