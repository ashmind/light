using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Incomplete;
using Light.Ast.Statements;

namespace Light.Processing.Steps.TypeResolution {
    public class InferVariableTypes : ProcessingStepBase<AstVariableDefinition> {
        public InferVariableTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstVariableDefinition variable, ProcessingContext context) {
            if (variable.Type != AstImplicitType.Instance)
                return variable;

            if (variable.AssignedValue == null)
                throw new NotImplementedException("InferVariableTypes: Variable not assigned.");

            variable.Type = variable.AssignedValue.ExpressionType;
            return variable;
        }
    }
}
