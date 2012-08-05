using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Properties;

namespace Light.Ast.Definitions {
    public class AstPropertyDefinition : AstElementBase, IAstMemberDefinition {
        private IAstTypeReference type;
        public string Name { get; private set; }

        public IAstTypeReference Type {
            get { return this.type; }
            set {
                Argument.RequireNotNull("value", value);
                this.type = value;
            }
        }

        public IAstExpression AssignedValue { get; set; }

        public AstPropertyDefinition(string name, IAstTypeReference type, IAstExpression assignedValue) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Name = name;
            this.Type = type;
            this.AssignedValue = assignedValue;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Type = (IAstTypeReference)transform(this.Type);
            if (this.AssignedValue != null)
                yield return this.AssignedValue = (IAstExpression)transform(this.AssignedValue);
        }

        public IAstReference ToReference() {
            return new AstDefinedProperty(this);
        }
    }
}
