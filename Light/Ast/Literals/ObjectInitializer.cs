using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Literals {
    public class ObjectInitializer : AstElementBase, IAstExpression {
        public IList<IAstElement> Elements { get; private set; }

        public ObjectInitializer(IEnumerable<IAstElement> elements) {
            var elementList = elements.ToList();
            Argument.RequireNotNullAndNotContainsNull("elements", elementList);
            this.Elements = elementList;
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("ObjectInitializer.ExpressionType"); }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }

        public override string ToString() {
            return "{" + string.Join(", ", this.Elements) + "}";
        }
    }
}
