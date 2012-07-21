using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;

namespace Light.Ast.Statements {
    public class AssignmentStatement : AstElementBase, IAstStatement {
        private IAstAssignable target;
        private IAstExpression value;

        public IAstAssignable Target {
            get { return this.target; }
            set {
                Argument.RequireNotNull("value", value);
                this.target = value;
            }
        }

        public IAstExpression Value {
            get { return this.value; }
            set {
                Argument.RequireNotNull("value", value);
                this.value = value;
            }
        }

        public AssignmentStatement(IAstAssignable target, IAstExpression value) {
            this.Target = target;
            this.Value = value;
        }
        
        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Target = (IAstAssignable)transform(this.Target);
            yield return this.Value = (IAstExpression)transform(this.Value);
        }
    }
}
