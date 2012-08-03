using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;

namespace Light.Ast.References.Types {
    public class AstGenericPlaceholderType : AstElementBase, IAstTypeReference, IAstDefinition {
        private readonly object identity;
        public string Name { get; private set; }

        public AstGenericPlaceholderType(string name, object identity = null) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.identity = identity;
            this.Name = name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        #region IAstTypeReference Members

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("AstGenericPlaceholderType: ResolveConstructor");
        }

        IAstMemberReference IAstTypeReference.ResolveMember(string name) {
            throw new NotImplementedException("AstGenericPlaceholderType: ResolveMember");
        }

        IAstTypeReference IAstTypeReference.BaseType {
            get { throw new NotImplementedException("AstGenericPlaceholderType: GetAncestors"); }
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetInterfaces() {
            throw new NotImplementedException("AstGenericPlaceholderType: GetInterfaces");
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetTypeParameters() {
            throw new NotImplementedException("AstGenericPlaceholderType: GetTypeParameters");
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return null; }
        }

        #endregion

        public override bool Equals(object obj) {
            return this.Equals(obj as AstGenericPlaceholderType);
        }

        public bool Equals(AstGenericPlaceholderType placeholder) {
            if (placeholder == null)
                return false;

            return this.identity == placeholder.identity
                || this == placeholder;
        }

        public override int GetHashCode() {
            return this.identity != null ? this.identity.GetHashCode() : base.GetHashCode();
        }
    }
}
