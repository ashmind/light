using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Statements {
    public class VariableDefinition : IAstStatement {
        public string Name { get; private set; }
        public IAstElement Value { get; private set; }

        public VariableDefinition(string name, IAstElement value) {
            Name = @name;
            Value = value;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Value == null)
                yield return this.Value = transform(Value);
        }

        #endregion
    }
}
