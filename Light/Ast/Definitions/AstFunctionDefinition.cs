using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Methods;

namespace Light.Ast.Definitions {
    public class AstFunctionDefinition : AstMethodDefinitionBase, IAstMemberDefinition {
        private IAstTypeReference fixedReturnType;
        private Func<IAstTypeReference> dependentReturnType;

        public string Name { get; private set; }
        public IAstTypeReference ReturnType {
            get { return this.fixedReturnType ?? this.dependentReturnType(); }
            set {
                Argument.RequireNotNull("value", value);
                this.fixedReturnType = value;
            }
        }

        public AstFunctionDefinition(string name, IEnumerable<AstParameterDefinition> parameters, IEnumerable<IAstStatement> body, IAstTypeReference returnType)
            : base(parameters, body)
        {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
            this.fixedReturnType = returnType;
        }

        public void SetDependentReturnType(Func<IAstTypeReference> dependentReturnType) {
            Argument.RequireNotNull("dependentReturnType", dependentReturnType);
            this.fixedReturnType = null;
            this.dependentReturnType = dependentReturnType;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            foreach (var child in base.VisitOrTransformChildren(transform)) {
                yield return child;
            }
            if (this.fixedReturnType != null)
                yield return this.fixedReturnType = (IAstTypeReference)transform(this.fixedReturnType);
        }

        public IAstReference ToReference() {
            return new AstDefinedMethod(this);
        }

        public override string ToString() {
            return string.Format("{{Defined: {0} {1}({2})}}", this.ReturnType, this.Name, string.Join(", ", this.Parameters));
        }
    }
}