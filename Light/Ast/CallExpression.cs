using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Names;

namespace Light.Ast {
    public class CallExpression : IAstElement {
        public CompositeName Callee { get; private set; }
        public IAstElement[] Arguments { get; private set; }

        public CallExpression(CompositeName callee, params IAstElement[] arguments) {
            Argument.RequireNotNull("callee", callee);
            Argument.RequireAllNotNull("arguments", arguments);

            this.Callee = callee;
            this.Arguments = arguments;
        }
    }
}
