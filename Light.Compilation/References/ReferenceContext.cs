using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Mono.Cecil;

namespace Light.Compilation.References {
    public class ReferenceContext : IReferenceContext {
        private readonly ModuleDefinition module;
        private readonly IReferenceProvider[] providers;

        public ReferenceContext(ModuleDefinition module, params IReferenceProvider[] providers) {
            this.module = module;
            this.providers = providers;
        }

        public virtual TypeReference ConvertTypeReference(IAstTypeReference type) {
            return (TypeReference)this.ConvertReference(type);
        }

        public virtual MethodReference ConvertMethodReference(IAstMethodReference method) {
            return (MethodReference)this.ConvertReference(method);
        }

        public virtual FieldReference ConvertFieldReference(AstPropertyReference property) {
            return (FieldReference)this.ConvertReference(property);
        }

        protected virtual MemberReference ConvertReference(IAstReference reference) {
            var converted = this.providers.Select(r => r.Convert(reference, module)).FirstOrDefault(r => r != null);
            if (converted == null)
                throw new NotImplementedException("ResolutionFacade: Cannot convert AST reference " + reference + ".");

            return converted;
        }
    }
}
