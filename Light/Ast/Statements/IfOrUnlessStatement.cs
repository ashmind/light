using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class IfOrUnlessStatement : AstElementBase, IAstStatement {
        private IAstElement condition;
        public IfOrUnlessKind Kind { get; set; }
        public IList<IAstElement> Body { get; private set; }

        public IfOrUnlessStatement(IfOrUnlessKind kind, IAstElement condition, IEnumerable<IAstElement> body) {
            var bodyList = body.ToList();
            Argument.RequireNotNullNotEmptyAndNotContainsNull("body", bodyList);

            this.Kind = kind;
            this.Condition = condition;
            this.Body = bodyList;
        }

        public IAstElement Condition {
            get { return this.condition; }
            set {
                Argument.RequireNotNull("value", value);
                this.condition = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Condition = transform(this.Condition);
            foreach (var element in this.Body.Transform(transform)) {
                yield return element;
            }
        }
    }
}
