using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ContinueStatement : IAstStatement {
        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion
    }
}
