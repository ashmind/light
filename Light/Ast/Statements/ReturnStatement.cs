using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ReturnStatement : IAstElement {
        public IAstElement Result { get; private set; }

        public ReturnStatement(IAstElement result) {
            Result = result;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield return this.Result;
        }

        #endregion
    }
}
