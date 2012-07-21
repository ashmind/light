using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.References;

namespace Light.Ast.Statements {
    public class AstBaseConstructorCall : AstElementBase, IAstStatement {
        public IAstConstructorReference Constructor { get; set; }

        public AstBaseConstructorCall(IAstConstructorReference constructor) {
            Constructor = constructor;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }
    }
}
