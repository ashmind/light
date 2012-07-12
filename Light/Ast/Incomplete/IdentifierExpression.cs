using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class IdentifierExpression : IAstExpression {
        public string Name { get; private set; }

        public IdentifierExpression(string name) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
        }

        public override string ToString() {
            return this.Name;
        }

        public IEnumerable<IAstElement> Children() {
            return No.Elements;
        }

        IAstTypeReference IAstExpression.ExpressionType {
            get { return null; }
        }
    }
}
