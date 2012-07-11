using System;
using Light.Ast.Types;
using Mono.Cecil;

namespace Light.Compilation.Types {
    public class PrimitiveTypeResolver : TypeResolverBase<AstPrimitiveType> {
        public override TypeReference Resolve(AstPrimitiveType astReference, ModuleDefinition module) {
            return module.Import(astReference.Type);
        }
    }
}