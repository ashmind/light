using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References;
using Light.Compilation.References;
using Mono.Cecil;

namespace Light.Compilation.Definitions {
    public class DefinitionBuildingContext : ReferenceContext {
        private readonly IDictionary<IAstDefinition, MemberReference> references = new Dictionary<IAstDefinition, MemberReference>();

        public DefinitionBuildingContext(ModuleDefinition module, params IReferenceProvider[] referenceProviders)
            : base(module, referenceProviders) {
        }

        public void Add(AstTypeDefinition typeAst, TypeDefinition type) {
            references.Add(typeAst, type);
        }

        public void Add(MethodDefinitionBase methodAst, MethodDefinition method) {
            references.Add(methodAst, method);
        }

        public void Add(AstPropertyDefinition propertyAst, FieldDefinition field) {
            references.Add(propertyAst, field);
        }

        protected override MemberReference ConvertReference(IAstReference reference) {
            var definition = reference.Target as IAstDefinition;
            if (definition != null) {
                var result = this.references.GetValueOrDefault(definition);
                if (result != null)
                    return result;
            }

            return base.ConvertReference(reference);
        }
    }
}
