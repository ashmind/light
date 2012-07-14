using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Mono.Cecil;

namespace Light.Compilation.References {
    public interface IReferenceProvider {
        MemberReference Convert(IAstReference astReference, ModuleDefinition module);
    }
}
