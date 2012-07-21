using System.Collections.Generic;
using Light.Ast.Definitions;

namespace Light.Ast.References {
    public class AstPropertyReference : AstElementBase, IAstReference, IAstExpression, IAstAssignable {
        public AstPropertyDefinition Property { get; private set; }

        public AstPropertyReference(AstPropertyDefinition property) {
            Argument.RequireNotNull("property", property);
            this.Property = property;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

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