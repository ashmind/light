using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.TypeResolution {
    public class AssignLambdaExpressionTypes : ProcessingStepBase<AstLambdaExpression> {
        public AssignLambdaExpressionTypes() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstLambdaExpression lambda, ProcessingContext context) {
            if (!(lambda.ExpressionType is AstUnknownType))
                return lambda;

            lambda.ExpressionType = new AstInferredFunctionType(
                () => lambda.Parameters.Select(p => p.Type),
                () => ((IAstExpression)lambda.Body).ExpressionType
            );
            return lambda;
        }
    }
}
