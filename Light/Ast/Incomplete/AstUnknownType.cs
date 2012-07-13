using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstUnknownType : IAstTypeReference {
        public string Name { get; private set; }

        public AstUnknownType(string name) {
            this.Name = name;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return No.Elements;
        }

        #endregion

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<Expressions.IAstExpression> arguments) {
            throw new NotImplementedException("Unknown type can not resolve methods.");
        }

        #endregion
    }
}
