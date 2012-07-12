using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;

namespace Light.Ast.Statements {
    public class ReturnStatement : IAstStatement {
        public IAstExpression Result { get; private set; }

        public ReturnStatement() {
        }

        public ReturnStatement(IAstExpression result) {
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
