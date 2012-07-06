using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Names;

namespace Light.Ast {
    public class ParameterDefinition : IAstElement {
        public string Name { get; private set; }
        public CompositeName Type { get; private set; }

        public ParameterDefinition(string name, CompositeName type) {
            Argument.RequireNotNullOrEmpty("name", name);

            this.Name = name;
            this.Type = type;
        }
    }
}
