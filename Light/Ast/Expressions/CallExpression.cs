using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Expressions {
    public class CallExpression : IAstElement {
        public IAstElement Target { get; private set; }
        public string MethodName { get; private set; }
        public IAstElement[] Arguments { get; private set; }

        public CallExpression(IAstElement target, string methodName, params IAstElement[] arguments) {
            Argument.RequireNotNull("methodName", methodName);
            Argument.RequireAllNotNull("arguments", arguments);

            this.Target = target;
            this.MethodName = methodName;
            this.Arguments = arguments;
        }
    }
}
