using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Expressions {
    public class CallExpression : IStatement {
        public IAstElement Target { get; private set; }
        public string MethodName { get; private set; }
        public IAstElement[] Arguments { get; private set; }

        public CallExpression(IAstElement target, string methodName, params IAstElement[] arguments) {
            Argument.RequireNotNull("methodName", methodName);
            Argument.RequireNotNullAndNotContainsNull("arguments", arguments);

            this.Target = target;
            this.MethodName = methodName;
            this.Arguments = arguments;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield return this.Target;
            foreach (var argument in this.Arguments) {
                yield return argument;
            }
        }

        #endregion
    }
}
