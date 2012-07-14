using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Ast.Literals {
    public class StringWithInterpolation : IAstExpression {
        private static readonly IAstTypeReference StringType = new AstReflectedType(typeof(string));

        public string Text { get; private set; }

        public StringWithInterpolation(string text) {
            Text = text;
        }

        public IAstTypeReference ExpressionType {
            get { return StringType; }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion
    }
}
