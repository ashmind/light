using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Definitions {
    public class PropertyDefinition : IAstElement {
        public string Name { get; private set; }
        public string TypeName { get; private set; }

        public PropertyDefinition(string name, string typeName) {
            Argument.RequireNotNullAndNotEmpty("name", name);

            Name = name;
            TypeName = typeName;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
