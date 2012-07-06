using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast {
    public class NewExpression : IAstElement {
        public string TypeName { get; private set; }
        public IAstElement[] Arguments { get; private set; }

        public NewExpression(string typeName, params IAstElement[] arguments) {
            Argument.RequireNotNullOrEmpty("typeName", typeName);
            Argument.RequireAllNotNull("arguments", arguments);
            
            this.TypeName = typeName;
            this.Arguments = arguments;
        }
    }
}
