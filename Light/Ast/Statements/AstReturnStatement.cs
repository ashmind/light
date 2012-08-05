using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class AstReturnStatement : AstElementBase, IAstStatement {
        public IAstExpression Result { get; private set; }

        public AstReturnStatement() {
        }

        public AstReturnStatement(IAstExpression result) {
            Result = result;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Result != null)
                yield return this.Result = (IAstExpression)transform(this.Result);
        }

        public override string ToString() {
            return "{Return" + (this.Result != null ? " " + this.Result : "") + "}";
        }
    }
}
