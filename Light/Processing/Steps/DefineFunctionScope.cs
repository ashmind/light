using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Processing.Scoping;

namespace Light.Processing.Steps {
    public class DefineFunctionScope : ProcessingStepBase<FunctionDefinition> {
        public override IAstElement ProcessBeforeChildren(FunctionDefinition function, ProcessingContext context) {
            var scope = new Scope();
            foreach (var parameter in function.Parameters) {
                scope.Add(parameter.Name, new AstParameterReference(parameter));
            }

            context.ScopeStack.Push(scope);
            return function;
        }

        public override IAstElement ProcessAfterChildren(FunctionDefinition function, ProcessingContext context) {
            context.ScopeStack.Pop();
            return function;
        }
    }
}
