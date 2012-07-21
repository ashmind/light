using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.References;
using Light.Internal;

namespace Light.Ast.Literals {
    public class ListInitializer : AstElementBase, IAstExpression {
        public IList<IAstElement> Elements { get; private set; }

        public ListInitializer(IEnumerable<IAstElement> elements) {
            var elementList = elements.ToList();
            Argument.RequireNotNullAndNotContainsNull("elements", elementList);
            Elements = elementList;
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("ListInitializer.ExpressionType"); }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Elements.Transform(transform);
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.Append("[")
                   .AppendJoin(", ", this.Elements)
                   .Append("]");

            return builder.ToString();
        }
    }
}
