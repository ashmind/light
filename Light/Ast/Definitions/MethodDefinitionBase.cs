using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public abstract class MethodDefinitionBase : IAstElement {
        protected MethodDefinitionBase() {
            this.Parameters = new List<IAstElement>();
            this.Body = new List<IAstStatement>();
        }

        protected MethodDefinitionBase(IEnumerable<IAstElement> parameters, IEnumerable<IAstStatement> body) {
            var parametersAsList = parameters.ToList();
            var bodyAsList = body.ToList();

            Argument.RequireNotNullAndNotContainsNull("parameters", parametersAsList);
            Argument.RequireNotNullAndNotContainsNull("body", bodyAsList);

            this.Parameters = parametersAsList;
            this.Body = bodyAsList;
        }

        public IList<IAstElement> Parameters { get; private set; }
        public IList<IAstStatement> Body { get; private set; }

        public virtual IEnumerable<IAstElement> Children() {
            return this.Parameters.Concat(this.Body);
        }
    }
}