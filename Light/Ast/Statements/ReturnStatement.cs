using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ReturnStatement : IAstElement {
        public IAstElement Result { get; private set; }

        public ReturnStatement(IAstElement result) {
            Result = result;
        }
    }
}
