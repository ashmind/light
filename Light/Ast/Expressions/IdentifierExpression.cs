using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.Names;

namespace Light.Ast {
    public class IdentifierExpression : IAstElement {
        public CompositeName Name { get; private set; }

        public IdentifierExpression(CompositeName name) {
            Argument.RequireNotNull("name", name);
            this.Name = name;
        }
    }
}
