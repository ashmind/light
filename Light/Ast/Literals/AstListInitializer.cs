using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Ast.Literals {
    public class AstListInitializer : AstElementBase, IAstExpression {
        public IList<IAstExpression> Elements { get; private set; }

        public AstListInitializer(IEnumerable<IAstExpression> elements) {
            Argument.RequireNotNull("elements", elements);
            var elementList = elements.ToList();
            Argument.RequireNotContainsNull("elements", elementList);

            Elements = elementList;
            ExpressionType = AstImplicitType.Instance;
        }

        public IAstTypeReference ExpressionType { get; set; }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }
    }
}
