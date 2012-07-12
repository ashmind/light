using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class CallExpression : IAstExpression, IAstStatement {
        private IAstMethodReference method;
        public IAstElement Target { get; set; }
        public IList<IAstExpression> Arguments { get; private set; }

        public CallExpression(IAstElement target, IAstMethodReference method, IList<IAstExpression> arguments) {
            Argument.RequireNotNullAndNotContainsNull("arguments", arguments);

            this.Target = target;
            this.Method = method;
            this.Arguments = arguments.ToList();
        }

        public IAstMethodReference Method {
            get { return this.method; }
            set {
                Argument.RequireNotNull("value", value);
                this.method = value;
            }
        }

        public IEnumerable<IAstElement> Children() {
            if (this.Target != null)
                yield return this.Target;

            foreach (var argument in this.Arguments) {
                yield return argument;
            }
        }

        public IAstTypeReference ExpressionType {
            get { return this.Method.ReturnType; }
        }
    }
}
