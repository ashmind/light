using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ForStatement : IAstElement {
        public string VariableName { get; private set; }
        public IAstElement Source { get; private set; }
        public IAstElement[] Body { get; private set; }

        public ForStatement(string variableName, IAstElement source, params IAstElement[] body) {
            VariableName = variableName;
            Source = source;
            Body = body;
        }
    }
}
