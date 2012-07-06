using System;
using System.Collections.Generic;
using System.Linq;

using Light.Ast;
using Light.Ast.Literals;

namespace Light.Interpretation {
    public class LightInterpreter : AstVisitor<LightInterpreter.Reference> {
        public class Reference {
            public object Value { get; set; }
        }

        public object Evaluate(IEnumerable<IAstElement> elements) {
            var result = new Reference();
            this.Visit(elements, result);
            return result.Value;
        }

        protected override void VisitPrimitiveValue(PrimitiveValue value, Reference context) {
            context.Value = value.Value;
        }
    }
}
