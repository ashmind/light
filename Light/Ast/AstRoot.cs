using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast {
    public class AstRoot : AstElementBase {
        public IList<IAstElement> Elements { get; private set; }

        public AstRoot(params IAstElement[] elements) : this((IEnumerable<IAstElement>)elements) {
        }

        public AstRoot(IEnumerable<IAstElement> elements) {
            var elementList = elements.ToList();
            Argument.RequireNotNullAndNotContainsNull("elements", elementList);
            Elements = elementList;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }
    }
}
