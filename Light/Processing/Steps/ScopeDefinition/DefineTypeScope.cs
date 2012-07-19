using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Processing.Scoping;

namespace Light.Processing.Steps {
    public class DefineTypeScope : ProcessingStepBase<AstTypeDefinition> {
        public DefineTypeScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessBeforeChildren(AstTypeDefinition type, ProcessingContext context) {
            context.ScopeStack.Push(new Scope());
            return type;
        }

        public override IAstElement ProcessAfterChildren(AstTypeDefinition type, ProcessingContext context) {
            context.ScopeStack.Pop();
            return type;
        }
    }
}
