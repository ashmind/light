using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class VariableDefinition : IAstElement {
        public string Name { get; private set; }
        public IAstElement Value { get; private set; }

        public VariableDefinition(string name, IAstElement value) {
            Name = @name;
            Value = value;
        }
    }
}
