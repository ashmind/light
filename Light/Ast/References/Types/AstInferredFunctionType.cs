using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References.Types {
    public class AstInferredFunctionType : AstElementBase, IAstFunctionTypeReference {
        private readonly Func<IEnumerable<IAstTypeReference>> getParameterTypes;
        private readonly Func<IAstTypeReference> getReturnType;

        public AstInferredFunctionType(Func<IEnumerable<IAstTypeReference>> getParameterTypes, Func<IAstTypeReference> getReturnType) {
            this.getParameterTypes = getParameterTypes;
            this.getReturnType = getReturnType;
        }

        public IEnumerable<IAstTypeReference> GetParameterTypes() {
            return getParameterTypes();
        }

        public IAstTypeReference ReturnType {
            get { return getReturnType(); }
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

        string IAstTypeReference.Name {
            get { return "<function>"; }
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

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override bool Equals(object obj) {
            return this.Equals(obj as IAstFunctionTypeReference);
        }

        public bool Equals(IAstFunctionTypeReference functionType) {
            if (functionType == null)
                return false;

            return Enumerable.SequenceEqual(this.GetParameterTypes(), functionType.GetParameterTypes())
                && Equals(this.ReturnType, functionType.ReturnType);
        }

        public override int GetHashCode() {
            return this.GetParameterTypes().Aggregate(0, (r1, r2) => r1.GetHashCode() ^ r2.GetHashCode())
                 ^ this.ReturnType.GetHashCode();
        }
    }
}