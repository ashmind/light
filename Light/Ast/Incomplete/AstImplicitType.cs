using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.Types;

namespace Light.Ast.Incomplete {
    public class AstImplicitType : IAstTypeReference {
        public static AstImplicitType Instance { get; private set; }

        static AstImplicitType() {
            Instance = new AstImplicitType();
        }

        private AstImplicitType() {
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<Expressions.IAstExpression> arguments) {
            throw new NotImplementedException("Implicit type can not resolve methods.");
        }

        #endregion
    }
}
