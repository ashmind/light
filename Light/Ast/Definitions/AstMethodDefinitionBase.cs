using System.Collections.Generic;
using System.Linq;
using Light.Compilation.Instructions;

namespace Light.Ast.Definitions {
    public abstract class AstMethodDefinitionBase : IAstDefinition {
        protected AstMethodDefinitionBase() {
            this.Parameters = new List<AstParameterDefinition>();
            this.Body = new List<IAstStatement>();
            this.Compilation = new MethodCompilation();
        }

        protected AstMethodDefinitionBase(IEnumerable<AstParameterDefinition> parameters, IEnumerable<IAstStatement> body) {
            var parametersAsList = parameters.ToList();
            var bodyAsList = body.ToList();

            Argument.RequireNotNullAndNotContainsNull("parameters", parametersAsList);
            Argument.RequireNotNullAndNotContainsNull("body", bodyAsList);

            this.Parameters = parametersAsList;
            this.Body = bodyAsList;

            this.Compilation = new MethodCompilation();
        }

        public IList<AstParameterDefinition> Parameters { get; private set; }
        public IList<IAstStatement> Body { get; private set; }
        public MethodCompilation Compilation { get; private set; }

        public virtual IEnumerable<IAstElement> Children() {
            return this.Parameters.Cast<IAstElement>().Concat(this.Body);
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return Enumerable.Concat(
                Parameters.Transform(transform).Cast<IAstElement>(),
                Body.Transform(transform)
            );
        }

        #endregion
    }
}