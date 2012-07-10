using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Definitions {
    public class TypeDefinition : IAstElement {
        public string DefinitionType                   { get; private set; }
        public string Name                             { get; private set; }
        public ReadOnlyCollection<IAstElement> Members { get; private set; }

        public TypeDefinition(string definitionType, string name, params IAstElement[] members) {
            Argument.RequireNotNullAndNotEmpty("definitionType", definitionType);
            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNullAndNotContainsNull("members", members);

            this.DefinitionType = definitionType;
            this.Name = name;
            this.Members = members.AsReadOnly();
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Members;
        }

        #endregion
    }
}
