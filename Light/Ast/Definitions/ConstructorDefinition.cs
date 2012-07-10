using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class ConstructorDefinition : FunctionDefinitionBase {
        public ConstructorDefinition() {
        }

        public ConstructorDefinition(IEnumerable<IAstElement> parameters, IEnumerable<IAstElement> body)
            : base(parameters, body) {
        }
    }
}
