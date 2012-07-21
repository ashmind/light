using System.Collections.Generic;
using Light.Ast.Statements;

namespace Light.Ast.References {
    public class AstVariableReference : AstElementBase, IAstReference, IAstExpression {
        public AstVariableDefinition Variable { get; private set; }

        public AstVariableReference(AstVariableDefinition variable) {
            this.Variable = variable;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Variable; }
        }

        #endregion

        #region IAstExpression Members

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.Variable.Type; }
        }

        #endregion
    }
}