using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References.Types {
    public class AstGenericTypeWithArguments : AstElementBase, IAstTypeReference {
        private IAstTypeReference primaryType;
        private readonly Lazy<IAstTypeReference> baseType;
        public IList<IAstTypeReference> TypeArguments { get; private set; }

        public AstGenericTypeWithArguments(IAstTypeReference primaryType, IEnumerable<IAstTypeReference> typeArguments) {
            Argument.RequireNotNull("typeArguments", typeArguments);

            this.PrimaryType = primaryType;
            this.TypeArguments = typeArguments.ToArray();

            this.baseType = new Lazy<IAstTypeReference>(() => AdaptReferencedType(this.PrimaryType.BaseType));
        }

        private IAstTypeReference AdaptReferencedType(IAstTypeReference type) {
            var typeAsGeneric = type as AstGenericTypeWithArguments;
            if (typeAsGeneric == null)
                return type;

            var arguments = typeAsGeneric.TypeArguments.ToArray();
            var parameters = this.PrimaryType.GetTypeParameters().Select((p, index) => new {p.Name, index}).ToDictionary(p => p.Name, p => p.index);

            for (var i = 0; i < arguments.Length; i++) {
                var placeholder = arguments[i] as AstGenericPlaceholderType;
                if (placeholder == null)
                    continue;

                arguments[i] = this.TypeArguments[parameters[placeholder.Name]];
            }

            return new AstGenericTypeWithArguments(typeAsGeneric.PrimaryType, arguments);
        }

        public IAstTypeReference PrimaryType {
            get { return this.primaryType; }
            set {
                Argument.RequireNotNull("value", value);
                if (value is AstGenericTypeWithArguments)
                    throw new ArgumentException("Value can not be AstGenericTypeWithArguments.", "value");

                this.primaryType = value;
            }
        }

        public IAstTypeReference BaseType {
            get { return this.baseType.Value; }
        }

        public IEnumerable<IAstTypeReference> GetInterfaces() {
            return this.PrimaryType.GetInterfaces().Select(AdaptReferencedType);
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.PrimaryType = (IAstTypeReference)transform(this.PrimaryType);
            foreach (var argument in this.TypeArguments.Transform(transform)) {
                yield return argument;
            }
        }

        public IAstMemberReference ResolveMember(string name) {
            return this.PrimaryType.ResolveMember(name);
        }

        #region IAstTypeReference Members

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("AstGenericType: ResolveConstructor");
        }

        string IAstTypeReference.Name {
            get { return this.PrimaryType.Name; }
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
            return this.Equals(obj as AstGenericTypeWithArguments);
        }

        public bool Equals(AstGenericTypeWithArguments type) {
            return type != null
                && Equals(type.PrimaryType, this.PrimaryType)
                && Enumerable.SequenceEqual(type.TypeArguments, this.TypeArguments);
        }

        public override int GetHashCode() {
            return this.PrimaryType.GetHashCode()
                 ^ this.TypeArguments.Aggregate(0, (r1, r2) => r1.GetHashCode() ^ r2.GetHashCode());
        }

        public override string ToString() {
            return "{Generic: " + this.PrimaryType + "<" + string.Join(", ", this.TypeArguments) + ">}";
        }
    }
}
