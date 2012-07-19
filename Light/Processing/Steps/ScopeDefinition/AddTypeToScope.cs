using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.ScopeDefinition {
    public class AddTypeToScope : ProcessingStepBase<AstTypeDefinition> {
        public AddTypeToScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessAfterChildren(AstTypeDefinition type, ProcessingContext context) {
            context.Scope.Add(type.Name, new AstDefinedType(type));
            return type;
        }
    }
}
