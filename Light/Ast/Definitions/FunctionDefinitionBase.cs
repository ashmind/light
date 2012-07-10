using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Definitions {
    public abstract class FunctionDefinitionBase : IAstElement {
        protected FunctionDefinitionBase(IAstElement[] parameters, IAstElement[] body) {
            Argument.RequireNotNullAndNotContainsNull("parameters", parameters);
            Argument.RequireNotNullAndNotContainsNull("body", body);
            
            this.Parameters = parameters.AsReadOnly();
            this.Body = body.AsReadOnly();
        }

        public ReadOnlyCollection<IAstElement> Parameters { get; private set; }
        public ReadOnlyCollection<IAstElement> Body { get; private set; }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Parameters.Concat(this.Body);
        }

        #endregion
    }
}