using System;
using System.Collections.Generic;
using System.Linq;

using Light.Ast;
using Light.Ast.Literals;

namespace Light.Interpretation {
    public class LightInterpreter : AstVisitor<object> {
        public object Evaluate(IAstElement element) {
            return this.Visit(element, null);
        }

        protected override object VisitBinaryExpression(BinaryExpression binary, object context) {
            return base.VisitBinaryExpression(binary, context);
        }

        protected override object VisitPrimitiveValue(PrimitiveValue value, object context) {
            return value.Value;
        }
    }
}
