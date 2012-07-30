using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References {
    public class ReferenceContext : IReferenceContext, IReferenceProvider {
        private readonly ModuleDefinition module;
        private readonly IReferenceProvider[] providers;

        public ReferenceContext(ModuleDefinition module, params IReferenceProvider[] providers) {
            this.module = module;
            this.providers = providers;
        }

        public virtual TypeReference ConvertReference(IAstTypeReference type, bool returnNullIfFailed = false) {
            Argument.RequireNotNull("type", type);
            return (TypeReference)ConvertReference((IAstReference)type, returnNullIfFailed);
        }

        public virtual MethodReference ConvertReference(IAstConstructorReference constructor, bool returnNullIfFailed = false) {
            Argument.RequireNotNull("constructor", constructor);
            return (MethodReference)ConvertReference((IAstReference)constructor, returnNullIfFailed);
        }

        public virtual MethodReference ConvertReference(IAstMethodReference method, bool returnNullIfFailed = false) {
            Argument.RequireNotNull("method", method);
            return (MethodReference)ConvertReference((IAstReference)method, returnNullIfFailed);
        }

        public virtual Either<FieldReference, PropertyReferenceContainer> ConvertReference(IAstPropertyReference property, bool returnNullIfFailed = false) {
            Argument.RequireNotNull("property", property);
            return ConvertReference((IAstReference)property, returnNullIfFailed).Cast<FieldReference, PropertyReferenceContainer>();
        }

        protected virtual Either<MemberReference, PropertyReferenceContainer> ConvertReference(IAstReference reference, bool returnNullIfFailed = false) {
            var converted = this.providers.Select(r => r.Convert(reference, module, this)).FirstOrDefault(r => r != null);
            if (converted == null) {
                if (returnNullIfFailed)
                    return null;

                throw new NotImplementedException("ReferenceContext: Cannot convert AST reference " + reference + ".");
            }

            return converted;
        }

        #region IReferenceProvider Members

        Either<MemberReference, PropertyReferenceContainer> IReferenceProvider.Convert(IAstReference astReference, ModuleDefinition module, IReferenceProvider recursive) {
            if (module != this.module)
                throw new InvalidOperationException("Expected module to be " + this.module + ", received " + module);

            return this.ConvertReference(astReference);
        }

        #endregion
    }
}
