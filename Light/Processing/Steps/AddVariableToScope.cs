using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Ast.Statements;

namespace Light.Processing.Steps {
    public class AddVariableToScope : ProcessingStepBase<AstVariableDefinition> {
        public AddVariableToScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessAfterChildren(AstVariableDefinition variable, ProcessingContext context) {
            context.Scope.Add(variable.Name, new AstVariableReference(variable));
            return variable;
        }
    }
}
