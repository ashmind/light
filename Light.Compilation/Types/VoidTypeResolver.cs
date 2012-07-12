using System;
using Light.Ast.References.Types;
using Mono.Cecil;

namespace Light.Compilation.Types {
    public class VoidTypeResolver : TypeResolverBase<AstVoidType> {
        public override TypeReference Resolve(AstVoidType astReference, ModuleDefinition module) {
            return module.Import(typeof(void));
        }
    }
}