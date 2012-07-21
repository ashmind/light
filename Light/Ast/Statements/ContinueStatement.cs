using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ContinueStatement : AstElementBase, IAstStatement {
        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }
    }
}
