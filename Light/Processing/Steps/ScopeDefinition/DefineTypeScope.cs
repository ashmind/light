using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Processing.Scoping;

namespace Light.Processing.Steps.ScopeDefinition {
    public class DefineTypeScope : ProcessingStepBase<AstTypeDefinition> {
        public DefineTypeScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessBeforeChildren(AstTypeDefinition type, ProcessingContext context) {
            var scope = new Scope();
            foreach (var member in type.Members) {
                scope.Add(member.Name, member.ToReference());
            }

            context.ScopeStack.Push(scope);
            return type;
        }

        public override IAstElement ProcessAfterChildren(AstTypeDefinition type, ProcessingContext context) {
            context.ScopeStack.Pop();
            return type;
        }
    }
}
