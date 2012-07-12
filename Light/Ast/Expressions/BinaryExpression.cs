using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Expressions {
    public class BinaryExpression : IAstExpression {
        public BinaryExpression(IAstElement left, BinaryOperator @operator, IAstElement right) {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public IAstElement Left { get; private set; }
        public BinaryOperator Operator { get; private set; }
        public IAstElement Right { get; private set; }

        public IEnumerable<IAstElement> Children() {
            yield return Left;
            yield return Right;
        }

        public References.IAstTypeReference ExpressionType {
            get { return Operator.ResultType; }
        }
    }
}
