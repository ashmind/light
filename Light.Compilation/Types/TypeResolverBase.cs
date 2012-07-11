using System;
using Light.Ast.Types;
using Mono.Cecil;

namespace Light.Compilation.Types {
    public abstract class TypeResolverBase<TAstTypeReference> : ITypeResolver
        where TAstTypeReference : class, IAstTypeReference
    {
        public abstract TypeReference Resolve(TAstTypeReference astReference, ModuleDefinition module);

        TypeReference ITypeResolver.Resolve(IAstTypeReference astReference, ModuleDefinition module) {
            var typed = astReference as TAstTypeReference;
            if (typed == null)
                return null;

            return Resolve(typed, module);
        }
    }
}