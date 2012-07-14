using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Errors;
using Light.Ast.Expressions;

namespace Light.Ast.References.Types {
    public class AstVoidType : IAstTypeReference {
        public static AstVoidType Instance { get; private set; }

        static AstVoidType() {
            Instance = new AstVoidType();
        }

        private AstVoidType() {
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            return new AstMissingMethod(name, this);
        }

        #endregion

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #endregion

        #region IAstReference Members

        string IAstReference.Name {
            get { return "void"; }
        }

        #endregion
    }
}
