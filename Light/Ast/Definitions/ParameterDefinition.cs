using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Definitions {
    public class ParameterDefinition : IAstElement {
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

        public ParameterDefinition(string name, IAstTypeReference type) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Name = name;
            this.Type = type;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return No.Elements;
        }

        #endregion
    }
}
