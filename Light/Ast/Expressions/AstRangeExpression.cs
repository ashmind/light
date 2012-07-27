using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class AstRangeExpression : AstElementBase, IAstExpression {
        private IAstExpression @from;
        public IAstExpression to;

        public AstRangeExpression(IAstExpression left, IAstExpression right) {
            this.From = left;
            this.To = right;
        }

        public IAstExpression From {
            get { return this.@from; }
            set {
                Argument.RequireNotNull("value", value);
                this.@from = value;
            }
        }

        public IAstExpression To {
            get { return this.to; }
            set {
                Argument.RequireNotNull("value", value);
                this.to = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.From = (IAstExpression)transform(this.From);
            yield return this.To = (IAstExpression)transform(this.To);
        }

        public IAstTypeReference ExpressionType {
            get { return AstUnknownType.WithNoName; }
        }
    }
}
