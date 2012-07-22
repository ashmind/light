using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Ast.Definitions {
    public class AstFunctionDefinition : AstMethodDefinitionBase, IAstMemberDefinition {
        private IAstTypeReference returnType;

        public string Name { get; private set; }
        public IAstTypeReference ReturnType {
            get { return this.returnType; }
            set {
                Argument.RequireNotNull("value", value);
                this.returnType = value;
            }
        }

        public AstFunctionDefinition(string name, IEnumerable<AstParameterDefinition> parameters, IEnumerable<IAstStatement> body, IAstTypeReference returnType)
            : base(parameters, body)
        {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
            this.ReturnType = returnType;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            foreach (var child in base.VisitOrTransformChildren(transform)) {
                yield return child;
            }
            yield return this.ReturnType = (IAstTypeReference)transform(this.ReturnType);
        }
    }
}
