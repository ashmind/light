using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstImplicitType : IAstTypeReference {
        public static AstImplicitType Instance { get; private set; }

        static AstImplicitType() {
            Instance = new AstImplicitType();
        }

        private AstImplicitType() {
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<Expressions.IAstExpression> arguments) {
            throw new NotImplementedException("Implicit type can not resolve methods.");
        }

        #endregion

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstReference Members

        string IAstReference.Name {
            get { return ""; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion
    }
}
