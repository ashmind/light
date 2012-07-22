using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class MemberExpression : AstElementBase, IAstExpression, IAstAssignable, IAstCallable {
        public IAstElement Target { get; private set; }
        public string Name { get; private set; }

        public MemberExpression(IAstElement target, string name) {
            Argument.RequireNotNull("target", target);
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Target = target;
            this.Name = name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Target != null)
                yield return this.Target = transform(this.Target);
        }

        #region IAstExpression Members

        IAstTypeReference IAstExpression.ExpressionType {
            get { return AstUnknownType.WithNoName; }
        }

        #endregion

        #region IAstCallable Members

        IAstTypeReference IAstCallable.ReturnType {
            get { return AstUnknownType.WithNoName; }
        }

        #endregion
    }
}
