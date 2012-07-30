using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References.Types {
    public class AstAnyType : AstElementBase, IAstTypeReference {
        public static AstAnyType Instance { get; private set; }

        static AstAnyType() {
            Instance = new AstAnyType();
        }

        private AstAnyType() {
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            return null;
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
            get { return "any"; }
        }

        IAstTypeReference IAstTypeReference.BaseType {
            get { return null; }
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetInterfaces() {
            return No.Types;
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetTypeParameters() {
            return No.Types;
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override bool Equals(object obj) {
            return obj == Instance;
        }

        public override int GetHashCode() {
            return typeof(AstAnyType).GetHashCode();
        }
    }
}
