using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Ast.Literals {
    public class PrimitiveValue : IAstExpression {
        public PrimitiveValue(object value) {
            this.Value = value;
            this.ExpressionType = new AstReflectedType(value.GetType());
        }

        public object Value { get; private set; }

        public override string ToString() {
            if (Value is string)
                return "'" + Value + "'";

            if (Value is bool)
                return Value.ToString().ToLowerInvariant();

            return Value.ToString();
        }

        public AstReflectedType ExpressionType { get; private set; }

        public IEnumerable<IAstElement> Children() {
            return No.Elements;
        }

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.ExpressionType; }
        }
    }
}
