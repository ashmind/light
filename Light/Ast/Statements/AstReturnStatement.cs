﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;

namespace Light.Ast.Statements {
    public class AstReturnStatement : IAstStatement {
        public IAstExpression Result { get; private set; }

        public AstReturnStatement() {
        }

        public AstReturnStatement(IAstExpression result) {
            Result = result;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Result != null)
                yield return this.Result = (IAstExpression)transform(this.Result);
        }

        #endregion
    }
}