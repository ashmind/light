﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class AstConstructorDefinition : AstMethodDefinitionBase {
        public AstConstructorDefinition() {
        }

        public AstConstructorDefinition(IEnumerable<AstParameterDefinition> parameters, IEnumerable<IAstStatement> body)
            : base(parameters, body) {
        }
    }
}