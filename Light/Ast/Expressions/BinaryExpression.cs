using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Expressions {
    public class BinaryExpression : IAstElement {
        public BinaryExpression(IAstElement left, BinaryOperator @operator, IAstElement right) {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public IAstElement Left { get; private set; }
        public BinaryOperator Operator { get; private set; }
        public IAstElement Right { get; private set; }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield return Left;
            yield return Right;
        }

        #endregion
    }
}
