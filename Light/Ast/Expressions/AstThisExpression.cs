using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class AstThisExpression : AstElementBase, IAstExpression {
        private IAstTypeReference expressionType = AstUnknownType.WithNoName;

        public IAstTypeReference ExpressionType {
            get { return this.expressionType; }
            set {
                Argument.RequireNotNull("value", value);
                this.expressionType = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }
    }
}
