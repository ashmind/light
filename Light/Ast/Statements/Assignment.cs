using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class Assignment : IAstStatement {
        public IAstElement Target { get; private set; }
        public IAstElement Value { get; private set; }

        public Assignment(IAstElement target, IAstElement value) {
            Argument.RequireNotNull("target", target);
            Argument.RequireNotNull("value", value);

            this.Target = target;
            this.Value = value;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Target = transform(this.Target);
            yield return this.Value = transform(this.Value);
        }

        #endregion
    }
}
