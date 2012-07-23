using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Compilation.Internal;
using Light.Compilation.References;
using Mono.Cecil;

namespace Light.Compilation.Definitions {
    public class DefinitionBuildingContext : ReferenceContext {
        private readonly IDictionary<IAstDefinition, MemberReference> references = new Dictionary<IAstDefinition, MemberReference>();
        private readonly IList<Action> debt = new List<Action>(); 

        public DefinitionBuildingContext(ModuleDefinition module, params IReferenceProvider[] referenceProviders)
            : base(module, referenceProviders) {
        }

        public void AddDebt(Action action) {
            this.debt.Add(action);
        }

        public void ClearDebt() {
            this.debt.ForEach(a => a());
            this.debt.Clear();
        }

        public AstMethodDefinitionBase GetAst(MethodDefinition method) {
            return (AstMethodDefinitionBase)references.Single(r => r.Value == method).Key;
        }

        public void MapDefinition(AstTypeDefinition typeAst, TypeDefinition type) {
            references.Add(typeAst, type);
        }

        public void MapDefinition(AstMethodDefinitionBase methodAst, MethodDefinition method) {
            references.Add(methodAst, method);
        }

        public void MapDefinition(AstPropertyDefinition propertyAst, FieldDefinition field) {
            references.Add(propertyAst, field);
        }

        protected override Either<MemberReference, PropertyReferenceContainer> ConvertReference(IAstReference reference, bool returnNullIfFailed = false) {
            var definition = reference.Target as IAstDefinition;
            if (definition != null) {
                var result = this.references.GetValueOrDefault(definition);
                if (result != null)
                    return result;
            }

            return base.ConvertReference(reference, returnNullIfFailed);
        }
    }
}
