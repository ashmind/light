using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References {
    public class ReferenceContext : IReferenceContext {
        private readonly ModuleDefinition module;
        private readonly IReferenceProvider[] providers;

        public ReferenceContext(ModuleDefinition module, params IReferenceProvider[] providers) {
            this.module = module;
            this.providers = providers;
        }

        public virtual TypeReference ConvertReference(IAstTypeReference type) {
            Argument.RequireNotNull("type", type);
            return (TypeReference)ConvertReference((IAstReference)type);
        }

        public virtual MethodReference ConvertReference(IAstConstructorReference constructor) {
            Argument.RequireNotNull("constructor", constructor);
            return (MethodReference)ConvertReference((IAstReference)constructor);
        }

        public virtual MethodReference ConvertReference(IAstMethodReference method) {
            Argument.RequireNotNull("method", method);
            return (MethodReference)ConvertReference((IAstReference)method);
        }

        public virtual Either<FieldReference, PropertyReferenceContainer> ConvertReference(IAstPropertyReference property) {
            Argument.RequireNotNull("property", property);
            return ConvertReference((IAstReference)property).Cast<FieldReference, PropertyReferenceContainer>();
        }

        protected virtual Either<MemberReference, PropertyReferenceContainer> ConvertReference(IAstReference reference) {
            var converted = this.providers.Select(r => r.Convert(reference, module)).FirstOrDefault(r => r != null);
            if (converted == null)
                throw new NotImplementedException("ResolutionFacade: Cannot convert AST reference " + reference + ".");

            return converted;
        }
    }
}
