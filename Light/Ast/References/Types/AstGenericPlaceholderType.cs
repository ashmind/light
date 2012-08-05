using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References.Methods;

namespace Light.Ast.References.Types {
    public class AstGenericPlaceholderType : AstElementBase, IAstTypeReference, IAstDefinition {
        private readonly object target;
        public string Name { get; private set; }
        public IList<IAstTypeReference> Constraints { get; private set; } 

        public AstGenericPlaceholderType(
            string name,
            Func<AstGenericPlaceholderType, IEnumerable<IAstTypeReference>> getConstraints,
            bool canAddConstraints = false,
            object target = null
        ) {
            var constraints = getConstraints(this);

            Argument.RequireNotNullAndNotEmpty("name", name);
            Argument.RequireNotNull("constraints", constraints);

            this.Name = name;
            this.Constraints = constraints.ToList();
            this.target = target;
            if (!canAddConstraints)
                this.Constraints = this.Constraints.AsReadOnly();
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return No.Elements;
        }

        public IAstMemberReference ResolveMember(string name) {
            const bool isMethod = true;
            var resolved = this.Constraints.Select(c => c.ResolveMember(name))
                .Where(m => m != null)
                .ToLookup(m => m is IAstMethodReference);

            var nonMethod = resolved[!isMethod].SingleOrDefault();
            var methods = resolved[isMethod].Cast<IAstMethodReference>().ToArray();

            if (nonMethod != null) {
                if (methods.Length > 0)
                    throw new NotImplementedException("AstGenericPlaceholderType: Ambiguous member " + name);

                return nonMethod;
            }

            if (methods.Length == 1)
                return methods[0];

            if (methods.Length > 0)
                return new AstMethodGroup(name, methods);

            return null;
        }

        #region IAstTypeReference Members

        IAstConstructorReference IAstTypeReference.ResolveConstructor(IEnumerable<IAstExpression> arguments) {
            throw new NotImplementedException("AstGenericPlaceholderType: ResolveConstructor");
        }

        IAstTypeReference IAstTypeReference.BaseType {
            get { return this.Constraints.SingleOrDefault(c => !c.Name.StartsWith("I") /* HACK level: over 9000 */) ?? AstAnyType.Instance; }
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetInterfaces() {
            return this.Constraints.Where(c => c.Name.StartsWith("I")); // HACK level: over 9000
        }

        IEnumerable<IAstTypeReference> IAstTypeReference.GetTypeParameters() {
            throw new NotImplementedException("AstGenericPlaceholderType: GetTypeParameters");
        }

        #endregion

        #region IAstReference Members

        object IAstReference.Target {
            get { return this.target; }
        }

        #endregion

        public override bool Equals(object obj) {
            return this.Equals(obj as AstGenericPlaceholderType);
        }

        public bool Equals(AstGenericPlaceholderType placeholder) {
            if (placeholder == null)
                return false;

            return this.target != null ? this.target == placeholder.target : this == placeholder;
        }

        public override int GetHashCode() {
            return this.target != null ? this.target.GetHashCode() : base.GetHashCode();
        }
    }
}
