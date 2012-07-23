using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.References.Properties;
using Light.Ast.Statements;
using Light.Processing;

namespace Light.Compilation.AstGeneration {
    public class MovePropertyInitializationToConstructors : ProcessingStepBase<AstTypeDefinition> {
        public MovePropertyInitializationToConstructors() : base(ProcessingStage.Compilation) {
        }

        public override IAstElement ProcessAfterChildren(AstTypeDefinition type, ProcessingContext context) {
            foreach (var constructor in type.GetConstructors()) {
                foreach (var property in type.GetProperties().Reverse()) {
                    if (property.AssignedValue == null)
                        continue;

                    constructor.Body.Insert(0, new AssignmentStatement(
                        new AstPropertyExpression(new AstThisExpression(), new AstDefinedProperty(property)),
                        property.AssignedValue
                    ));
                    property.AssignedValue = null;
                }
            }

            return type;
        }
    }
}
