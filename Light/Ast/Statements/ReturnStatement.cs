using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ReturnStatement : IStatement {
        public IAstElement Result { get; private set; }

        public ReturnStatement() {
        }

        public ReturnStatement(IAstElement result) {
            Result = result;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            if (this.Result != null)
                yield return this.Result;
        }

        #endregion
    }
}
