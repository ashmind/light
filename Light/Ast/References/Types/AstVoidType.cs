using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Errors;
using Light.Ast.Expressions;

namespace Light.Ast.References.Types {
    public class AstVoidType : AstElementBase, IAstTypeReference {
        public static AstVoidType Instance { get; private set; }

        static AstVoidType() {
            Instance = new AstVoidType();
        }

        private AstVoidType() {
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            return new AstMissingMethod(name);
        }

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            return null;
        }

        IAstMemberReference IAstTypeReference.ResolveMember(string name) {
            return null;
        }

        #endregion

        #region IAstTypeReference Members

        string IAstTypeReference.Name {
            get { return "void"; }
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion
    }
}
