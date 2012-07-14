using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Ast.Definitions {
    public class FunctionDefinition : MethodDefinitionBase {
        private IAstTypeReference returnType;

        public string Name { get; private set; }
        public IAstTypeReference ReturnType {
            get { return this.returnType; }
            set {
                Argument.RequireNotNull("value", value);
                this.returnType = value;
            }
        }

        public FunctionDefinition(string name, IEnumerable<AstParameterDefinition> parameters, IEnumerable<IAstStatement> body, IAstTypeReference returnType)
            : base(parameters, body)
        {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
            this.ReturnType = returnType;
        }

        public override IEnumerable<IAstElement> Children() {
            return base.Children().Concat(this.ReturnType);
        }
    }
}
