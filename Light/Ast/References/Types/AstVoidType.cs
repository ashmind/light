using System;
using System.Collections.Generic;
using System.Linq;

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

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            return null;
        }

        IAstMemberReference IAstTypeReference.ResolveMember(string name) {
            return null;
        }

        IAstTypeReference IAstTypeReference.BaseType {
            get { return AstAnyType.Instance; }
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetInterfaces() {
            return No.Types;
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetTypeParameters() {
            return No.Types;
        }

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
