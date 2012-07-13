using System.Collections.Generic;
using System.Linq;
using Light.Compilation.Instructions;

namespace Light.Ast.Definitions {
    public abstract class MethodDefinitionBase : IAstElement {
        protected MethodDefinitionBase() {
            this.Parameters = new List<ParameterDefinition>();
            this.Body = new List<IAstStatement>();
            this.Compilation = new MethodCompilation();
        }

        protected MethodDefinitionBase(IEnumerable<ParameterDefinition> parameters, IEnumerable<IAstStatement> body) {
            var parametersAsList = parameters.ToList();
            var bodyAsList = body.ToList();

            Argument.RequireNotNullAndNotContainsNull("parameters", parametersAsList);
            Argument.RequireNotNullAndNotContainsNull("body", bodyAsList);

            this.Parameters = parametersAsList;
            this.Body = bodyAsList;

            this.Compilation = new MethodCompilation();
        }

        public IList<ParameterDefinition> Parameters { get; private set; }
        public IList<IAstStatement> Body { get; private set; }
        public MethodCompilation Compilation { get; private set; }

        public virtual IEnumerable<IAstElement> Children() {
            return this.Parameters.Cast<IAstElement>().Concat(this.Body);
        }
    }
}