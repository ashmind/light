using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class ParameterDefinition : IAstElement {
        public string Name { get; private set; }
        public string TypeName { get; private set; }

        public ParameterDefinition(string name, string type) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Name = name;
            this.TypeName = type;
        }
    }
}
