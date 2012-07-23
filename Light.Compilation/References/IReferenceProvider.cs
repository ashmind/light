using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References {
    public interface IReferenceProvider {
        Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module);
    }
}
