using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Definitions {
    public class TypeDefinition : IAstElement {
        private string definitionType;
        private string name;

        public string DefinitionType {
            get { return definitionType; }
            set {
                Argument.RequireNotNullAndNotEmpty("definitionType", value);
                this.definitionType = value;
            }
        }

        public string Name {
            get { return name; }
            set {
                Argument.RequireNotNullAndNotEmpty("name", value);
                this.name = value;
            }
        }

        public IList<IAstElement> Members { get; private set; }

        public TypeDefinition(string definitionType, string name, params IAstElement[] members) {
            Argument.RequireNotNullAndNotContainsNull("members", members);

            this.DefinitionType = definitionType;
            this.Name = name;
            this.Members = members.ToList();
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Members;
        }

        #endregion
    }
}
