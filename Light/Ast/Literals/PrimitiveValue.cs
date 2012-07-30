using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Internal;

namespace Light.Ast.Literals {
    public class PrimitiveValue : AstElementBase, IAstExpression {
        public PrimitiveValue(object value) {
            this.Value = value;
            this.ExpressionType = new AstReflectedType(value.GetType(), new Reflector()); // new Reflector() is temporary here
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

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.ExpressionType; }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }
    }
}
