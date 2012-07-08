using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Definitions {
    public class TypeDefinition : IAstElement {
        public string TypeDefinitionType { get; set; }
        public string Name { get; private set; }
        public ReadOnlyCollection<IAstElement> Members { get; private set; }

        public TypeDefinition(string typeDefinitionType, string name, params IAstElement[] members) {
            Argument.RequireNotNullAndNotEmpty("typeDefinitionType", typeDefinitionType);
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNullAndNotContainsNull("members", members);

            this.TypeDefinitionType = typeDefinitionType;
            this.Name = name;
            this.Members = members.AsReadOnly();
        }
    }
}
