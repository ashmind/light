using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class AstFunctionReferenceExpression : AstElementBase, IAstExpression, IAstCallable {
        private IAstMethodReference reference;
        public IAstElement Target { get; set; }
        private Lazy<IAstTypeReference> expressionType;

        public AstFunctionReferenceExpression(IAstElement target, IAstMethodReference reference) {
            this.Target = target;
            this.Reference = reference;

            SetExpressionType(() => AstUnknownType.WithNoName);
        }

        public void SetExpressionType(Func<IAstTypeReference> value) {
            this.expressionType = new Lazy<IAstTypeReference>(value);
        }

        public IAstTypeReference ExpressionType {
            get { return this.expressionType.Value; }
        }

        public IAstMethodReference Reference {
            get { return this.reference; }
            set {
                Argument.RequireNotNull("value", value);
                this.reference = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Target != null)
                yield return this.Target = transform(this.Target);

            yield return this.Reference = (IAstMethodReference)transform(this.Reference);
        }

        #region IAstCallable Members

        IAstTypeReference IAstCallable.ReturnType {
            get { return this.Reference.ReturnType; }
        }

        #endregion
    }
}
