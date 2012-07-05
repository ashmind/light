using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Names;

namespace Light.Ast.Statements {
    public class ImportStatement : IAstElement {
        public CompositeName Namespace { get; private set; }

        public ImportStatement(CompositeName @namespace) {
            Namespace = @namespace;
        }
    }
}
