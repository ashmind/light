using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.TypeResolution {
    public class ResolveFunctionReferenceExpressionTypes : ProcessingStepBase<AstFunctionReferenceExpression> {
        private readonly DelegateTypeBuilder typeBuilder;   

        public ResolveFunctionReferenceExpressionTypes(DelegateTypeBuilder typeBuilder)
            : base(ProcessingStage.TypeResolution) {
            this.typeBuilder = typeBuilder;
        }

        public override IAstElement ProcessAfterChildren(AstFunctionReferenceExpression expression, ProcessingContext context) {
            expression.SetExpressionType(() => typeBuilder.BuildType(expression.Reference.ParameterTypes, expression.Reference.ReturnType));
            return expression;
        }
    }
}
