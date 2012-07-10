using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class ConstructorDefinition : FunctionDefinitionBase {
        public ConstructorDefinition(IAstElement[] parameters, IAstElement[] body) : base(parameters, body) {
        }
    }
}
