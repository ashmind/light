using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.References.Types {
    public class AstGenericPlaceholderType : AstElementBase, IAstTypeReference {
        public string Name { get; private set; }

        public AstGenericPlaceholderType(string name) {
            Argument.RequireNotNullAndNotEmpty("name", name);
            this.Name = name;
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            throw new NotImplementedException();
        }

        #region IAstTypeReference Members

        IAstMethodReference IAstTypeReference.ResolveMethod(string name, IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("AstGenericPlaceholderType: ResolveMethod");
        }

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
            throw new NotImplementedException("AstGenericPlaceholderType: GetInterfaces");
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

            return Equals(this.Name, placeholder.Name);
        }

        public override int GetHashCode() {
            return string.Intern(this.Name).GetHashCode();
        }
    }
}
