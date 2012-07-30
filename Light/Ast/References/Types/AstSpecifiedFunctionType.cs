using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References.Types {
    public class AstSpecifiedFunctionType : AstElementBase, IAstFunctionTypeReference {
        private IAstTypeReference returnType;
        public IList<IAstTypeReference> ParameterTypes { get; private set; }

        public AstSpecifiedFunctionType(IEnumerable<IAstTypeReference> parameterTypes, IAstTypeReference returnType) {
            Argument.RequireNotNull("parameterTypes", parameterTypes);
            this.ParameterTypes = parameterTypes.ToList();
            this.ReturnType = returnType;
        }

        public IAstTypeReference ReturnType {
            get { return this.returnType; }
            set {
                Argument.RequireNotNull("value", value);
                this.returnType = value;
            }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            foreach (var parameterType in this.ParameterTypes.Transform(transform)) {
                yield return parameterType;
            }
            yield return this.ReturnType = (IAstTypeReference)transform(this.ReturnType);
        }

        #region IAstFunctionTypeReference Members

        IEnumerable<IAstTypeReference> IAstFunctionTypeReference.GetParameterTypes() {
            return this.ParameterTypes;
        }

        #endregion

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

            return Enumerable.SequenceEqual(this.ParameterTypes, functionType.GetParameterTypes())
                && Equals(this.ReturnType, functionType.ReturnType);
        }

        public override int GetHashCode() {
            return this.ParameterTypes.Aggregate(0, (r1, r2) => r1.GetHashCode() ^ r2.GetHashCode())
                 ^ this.ReturnType.GetHashCode();
        }
    }
}