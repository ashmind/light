using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Expressions {
    public class IdentifierExpression : IAstElement {
        public string Name { get; private set; }

        public IdentifierExpression(string name) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
        }

        public override string ToString() {
            return this.Name;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
