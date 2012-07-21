using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Names;

namespace Light.Ast.Definitions {
    public class ImportDefinition : AstElementBase {
        public CompositeName Namespace { get; private set; }

        public ImportDefinition(CompositeName @namespace) {
            Namespace = @namespace;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }
    }
}
