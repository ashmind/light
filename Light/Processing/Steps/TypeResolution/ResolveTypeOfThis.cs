using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.TypeResolution {
    public class ResolveTypeOfThis : ProcessingStepBase<AstThisExpression> {
        public ResolveTypeOfThis() : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessBeforeChildren(AstThisExpression expression, ProcessingContext context) {
            var type = context.ElementStack.OfType<AstTypeDefinition>().First();
            expression.ExpressionType = new AstDefinedType(type);
            return expression;
        }
    }
}
