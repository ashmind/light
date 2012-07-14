using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Light.Ast.Expressions;

namespace Light.Ast.References {
    public class AstParameterReference : IAstExpression, IAstReference {
        public AstParameterDefinition Parameter { get; private set; }

        public AstParameterReference(AstParameterDefinition parameter) {
            Parameter = parameter;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstExpression Members

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.Parameter.Type; }
        }

        #endregion
    }
}
