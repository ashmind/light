using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Definitions {
    public class AstParameterDefinition : AstElementBase {
        private IAstTypeReference type;
        public string Name { get; private set; }
        public IAstTypeReference Type
        {
            get { return this.type; }
            set {
                Argument.RequireNotNull("value", value);
                this.type = value;
            }
        }

        public AstParameterDefinition(string name, IAstTypeReference type) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Name = name;
            this.Type = type;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Type = (IAstTypeReference)transform(this.Type);
        }
    }
}
