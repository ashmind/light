using System;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class AnyTypeProvider : IReferenceProvider {
        public Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module, IReferenceProvider recursive) {
            if (astReference != AstAnyType.Instance)
                return null;

            return module.Import(typeof(object));
        }
    }
}