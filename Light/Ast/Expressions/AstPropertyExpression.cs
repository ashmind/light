using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class AstPropertyExpression : AstElementBase, IAstExpression, IAstAssignable {
        public IAstElement Target { get; set; }
        private IAstPropertyReference reference;

        public AstPropertyExpression(IAstElement target, IAstPropertyReference reference) {
            this.Target = target;
            this.Reference = reference;
        }

        public IAstPropertyReference Reference {
            get { return this.reference; }
            set {
                Argument.RequireNotNull("value", value);
                this.reference = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Target != null)
                yield return this.Target = transform(this.Target);

            yield return this.Reference = (IAstPropertyReference)transform(this.Reference);
        }

        #region IAstExpression Members

        public IAstTypeReference ExpressionType {
            get { return reference.Type; }
        }

        #endregion
    }
}
