using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public abstract class FunctionDefinitionBase : IAstElement {
        protected FunctionDefinitionBase() {
            this.Parameters = new List<IAstElement>();
            this.Body = new List<IAstElement>();
        }

        protected FunctionDefinitionBase(IEnumerable<IAstElement> parameters, IEnumerable<IAstElement> body) {
            var parametersAsList = parameters.ToList();
            var bodyAsList = body.ToList();

            Argument.RequireNotNullAndNotContainsNull("parameters", parametersAsList);
            Argument.RequireNotNullAndNotContainsNull("body", bodyAsList);

            this.Parameters = parametersAsList;
            this.Body = bodyAsList;
        }

        public IList<IAstElement> Parameters { get; private set; }
        public IList<IAstElement> Body { get; private set; }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Parameters.Concat(this.Body);
        }

        #endregion
    }
}