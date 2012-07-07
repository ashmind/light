using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Definitions {
    public class FunctionDefinition : IAstElement {
        public string Name { get; private set; }
        public ReadOnlyCollection<IAstElement> Parameters { get; private set; }
        public ReadOnlyCollection<IAstElement> Body { get; private set; }

        public FunctionDefinition(string name, IAstElement[] parameters, IAstElement[] body) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNullAndNotContainsNull("parameters", parameters);
            Argument.RequireNotNullAndNotContainsNull("body", body);

            this.Name = name;
            this.Parameters = parameters.AsReadOnly();
            this.Body = body.AsReadOnly();
        }
    }
}
