using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class ConstructorDefinition : MethodDefinitionBase {
        public ConstructorDefinition() {
        }

        public ConstructorDefinition(IEnumerable<IAstElement> parameters, IEnumerable<IAstStatement> body)
            : base(parameters, body) {
        }
    }
}
