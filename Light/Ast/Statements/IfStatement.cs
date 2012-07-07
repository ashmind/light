using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class IfStatement : IAstElement {
        public IAstElement Condition { get; private set; }
        public IAstElement[] Body { get; private set; }

        public IfStatement(IAstElement condition, params IAstElement[] body) {
            Condition = condition;
            Body = body;
        }
    }
}
