using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Statements {
    public class AstVariableDefinition : AstElementBase, IAstStatement {
        private IAstTypeReference type;

        public string Name { get; private set; }
        public IAstExpression AssignedValue { get; set; }

        public IAstTypeReference Type {
            get { return this.type; }
            set {
                Argument.RequireNotNull("value", value);
                this.type = value;
            }
        }

        public AstVariableDefinition(string name, IAstTypeReference type, IAstExpression value) {
            this.Name = @name;
            this.Type = type;
            this.AssignedValue = value;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            if (this.AssignedValue != null)
                yield return this.AssignedValue = (IAstExpression)transform(this.AssignedValue);
            yield return this.Type = (IAstTypeReference)transform(this.Type);
        }
    }
}
