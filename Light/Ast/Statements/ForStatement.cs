using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ForStatement : AstElementBase, IAstStatement {
        private IAstElement source;
        private string variableName;

        public string VariableName {
            get { return this.variableName; }
            set {
                Argument.RequireNotNullAndNotEmpty("value", value);
                this.variableName = value;
            }
        }

        public IAstElement Source {
            get { return this.source; }
            set {
                Argument.RequireNotNull("value", value);
                this.source = value;
            }
        }

        public IList<IAstStatement> Body { get; private set; }

        public ForStatement(string variableName, IAstElement source, IEnumerable<IAstStatement> body) {
            VariableName = variableName;
            Source = source;
            Body = body.ToList();
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Source = transform(this.Source);
            foreach (var element in this.Body.Transform(transform)) {
                yield return element;
            }
        }
    }
}
