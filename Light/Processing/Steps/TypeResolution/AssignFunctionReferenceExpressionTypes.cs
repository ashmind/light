using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.TypeResolution {
    public class AssignFunctionReferenceExpressionTypes : ProcessingStepBase<AstFunctionReferenceExpression> {
        public AssignFunctionReferenceExpressionTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstFunctionReferenceExpression expression, ProcessingContext context) {
            expression.ExpressionType = new AstInferredFunctionType(
                () => expression.Function.ParameterTypes,
                () => expression.Function.ReturnType
            );
            return expression;
        }
    }
}
