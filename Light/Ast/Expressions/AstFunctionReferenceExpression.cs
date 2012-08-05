using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Ast.Expressions {
    public class AstFunctionReferenceExpression : AstElementBase, IAstExpression, IAstCallable {
        private IAstMethodReference function;
        public IAstElement Target { get; set; }
        public IAstTypeReference ExpressionType { get; private set; }

        public AstFunctionReferenceExpression(IAstElement target, IAstMethodReference reference) {
            this.Target = target;
            this.Function = reference;
            this.ExpressionType = new AstInferredFunctionType(
                () => this.Function.ParameterTypes,
                () => this.Function.ReturnType
            );
        }

        public IAstMethodReference Function {
            get { return this.function; }
            set {
                Argument.RequireNotNull("value", value);
                this.function = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Target != null)
                yield return this.Target = transform(this.Target);

            yield return this.Function = (IAstMethodReference)transform(this.Function);
        }

        #region IAstCallable Members

        IAstTypeReference IAstCallable.ReturnType {
            get { return this.Function.ReturnType; }
        }

        IEnumerable<IAstTypeReference> IAstCallable.ParameterTypes {
            get { return this.Function.ParameterTypes; }
        }

        #endregion

        public override string ToString() {
            return this.Function.ToString();
        }
    }
}
