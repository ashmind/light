using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References;

namespace Light.Processing.Steps {
    public class AddPropertyToScope : ProcessingStepBase<AstPropertyDefinition> {
        public AddPropertyToScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessAfterChildren(AstPropertyDefinition property, ProcessingContext context) {
            context.Scope.Add(property.Name, new AstPropertyReference(property));
            return property;
        }
    }
}
