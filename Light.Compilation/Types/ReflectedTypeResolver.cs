using System;
using Light.Ast.References.Types;
using Light.Ast.Types;
using Mono.Cecil;

namespace Light.Compilation.Types {
    public class ReflectedTypeResolver : TypeResolverBase<AstReflectedType> {
        public override TypeReference Resolve(AstReflectedType astReference, ModuleDefinition module) {
            return module.Import(astReference.ActualType);
        }
    }
}