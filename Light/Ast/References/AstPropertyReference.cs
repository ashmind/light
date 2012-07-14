using System.Collections.Generic;
using Light.Ast.Definitions;
using Light.Ast.Expressions;

namespace Light.Ast.References {
    public class AstPropertyReference : IAstReference, IAstExpression, IAstAssignable {
        public AstPropertyDefinition Property { get; private set; }

        public AstPropertyReference(AstPropertyDefinition property) {
            Argument.RequireNotNull("property", property);
            this.Property = property;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstExpression Members

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.Property.Type; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Property; }
        }

        #endregion
    }
}