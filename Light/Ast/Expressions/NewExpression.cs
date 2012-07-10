using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast {
    public class NewExpression : IAstElement {
        public string TypeName { get; private set; }
        public IAstElement[] Arguments { get; private set; }
        public IAstElement Initializer { get; private set; }

        public NewExpression(string typeName, params IAstElement[] arguments) : this(typeName, arguments, null) {
        }

        public NewExpression(string typeName, IAstElement[] arguments, IAstElement initializer) {
            Argument.RequireNotNullAndNotEmpty("typeName", typeName);
            Argument.RequireNotNullAndNotContainsNull("arguments", arguments);

            this.TypeName = typeName;
            this.Arguments = arguments;
            this.Initializer = initializer;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Arguments.Concat(this.Initializer);
        }

        #endregion
    }
}
