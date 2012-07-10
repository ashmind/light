using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class FunctionDefinition : MethodDefinitionBase {
        public string Name { get; private set; }
        public IAstElement ReturnType { get; set; }

        public FunctionDefinition(string name, IEnumerable<IAstElement> parameters, IEnumerable<IStatement> body, IAstElement returnType)
            : base(parameters, body)
        {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
            this.ReturnType = returnType;
        }
    }
}
