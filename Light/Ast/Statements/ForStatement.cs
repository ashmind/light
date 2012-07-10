using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class ForStatement : IStatement {
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

        public IList<IStatement> Body { get; private set; }

        public ForStatement(string variableName, IAstElement source, IEnumerable<IStatement> body) {
            VariableName = variableName;
            Source = source;
            Body = body.ToList();
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield return this.Source;
            foreach (var element in this.Body) {
                yield return element;
            }
        }

        #endregion
    }
}
