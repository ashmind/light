using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Light.Ast.Expressions;

namespace Light.Ast.References {
    public class AstParameterReference : AstElementBase, IAstExpression, IAstReference {
        public AstParameterDefinition Parameter { get; private set; }

        public AstParameterReference(AstParameterDefinition parameter) {
            Argument.RequireNotNull("parameter", parameter);
            Parameter = parameter;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstExpression Members

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.Parameter.Type; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.Parameter; }
        }

        #endregion
    }
}
