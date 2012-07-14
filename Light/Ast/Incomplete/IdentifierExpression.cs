using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class IdentifierExpression : IAstExpression {
        public string Name { get; private set; }

        public IdentifierExpression(string name) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
        }

        public override string ToString() {
            return this.Name;
        }

        IAstTypeReference IAstExpression.ExpressionType {
            get { return null; }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion
    }
}
