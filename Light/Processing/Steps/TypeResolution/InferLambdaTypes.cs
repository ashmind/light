using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References.Types;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.TypeResolution {
    public class InferLambdaTypes : ProcessingStepBase<AstLambdaExpression> {
        private readonly DelegateTypeBuilder typeBuilder;

        public InferLambdaTypes(DelegateTypeBuilder typeBuilder) : base(ProcessingStage.TypeResolution) {
            this.typeBuilder = typeBuilder;
        }

        public override IAstElement ProcessAfterChildren(AstLambdaExpression lambda, ProcessingContext context) {
            if (!(lambda.ExpressionType is AstUnknownType))
                return lambda;

            lambda.ExpressionType = typeBuilder.BuildType(lambda.Parameters.Select(p => p.Type), ((IAstExpression)lambda.Body).ExpressionType);
            return lambda;
        }
    }
}
