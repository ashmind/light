using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;

namespace Light.Ast.Literals {
    public class ListInitializer : IAstExpression {
        public IList<IAstElement> Elements { get; private set; }

        public ListInitializer(params IAstElement[] elements) {
            Argument.RequireNotNullAndNotContainsNull("elements", elements);
            Elements = elements.ToList();
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("ListInitializer.ExpressionType"); }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }

        #endregion
    }
}
