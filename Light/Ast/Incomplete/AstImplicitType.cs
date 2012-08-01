using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Incomplete {
    public class AstImplicitType : AstElementBase, IAstTypeReference {
        public static AstImplicitType Instance { get; private set; }

        static AstImplicitType() {
            Instance = new AstImplicitType();
        }

        private AstImplicitType() {
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstTypeReference Members
        
        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("Implicit type can not resolve constructors.");
        }

        IAstMemberReference IAstTypeReference.ResolveMember(string name) {
            throw new NotImplementedException("Implicit type can not resolve members.");
        }

        string IAstTypeReference.Name {
            get { return ""; }
        }

        IAstTypeReference IAstTypeReference.BaseType {
            get { throw new NotImplementedException("Implicit type has no BaseType."); }
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetInterfaces() {
            throw new NotImplementedException("Implicit type can not get interfaces.");
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetTypeParameters() {
            throw new NotImplementedException("Implicit type can not get type parameters.");
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override string ToString() {
            return "{ImplicitType}";
        }
    }
}
