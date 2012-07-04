using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Light.Ast;

namespace Light.Interpretation {
    public class LightInterpreter : AstVisitor {
        public object Evaluate(Expression expression) {
            return this.Visit(expression);
        }

        protected override object VisitBinary(BinaryExpression binary) {
            return base.VisitBinary(binary);
        }

        protected override object VisitConstant(ConstantExpression constant) {
            return constant.Value;
        }
    }
}
