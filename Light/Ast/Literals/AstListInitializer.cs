using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Ast.Literals {
    public class AstListInitializer : AstElementBase, IAstExpression {
        public IList<IAstExpression> Elements { get; private set; }

        public AstListInitializer(IEnumerable<IAstExpression> elements) {
            Argument.RequireNotNull("elements", elements);
            var elementList = elements.ToList();
            Argument.RequireNotContainsNull("elements", elementList);
            Elements = elementList;
        }

        public IAstTypeReference ExpressionType {
            get {
                var elementType = this.Elements[0].ExpressionType; // temporary cheating
                return new AstReflectedType(((AstReflectedType)elementType).ActualType.MakeArrayType());
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }
    }
}
