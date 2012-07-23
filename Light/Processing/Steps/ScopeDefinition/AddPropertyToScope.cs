using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.References.Properties;

namespace Light.Processing.Steps.ScopeDefinition {
    public class AddPropertyToScope : ProcessingStepBase<AstPropertyDefinition> {
        public AddPropertyToScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessAfterChildren(AstPropertyDefinition property, ProcessingContext context) {
            context.Scope.Add(property.Name, new AstDefinedProperty(property));
            return property;
        }
    }
}
