using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Names;

namespace Light.Ast.Definitions {
    public class ImportDefinition : IAstElement {
        public CompositeName Namespace { get; private set; }

        public ImportDefinition(CompositeName @namespace) {
            Namespace = @namespace;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion
    }
}
