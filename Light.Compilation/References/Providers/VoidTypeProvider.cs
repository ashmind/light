using System;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class VoidTypeProvider : IReferenceProvider {
        public Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module) {
            if (astReference != AstVoidType.Instance)
                return null;

            return module.Import(typeof(void));
        }
    }
}