using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.Types;
using Mono.Cecil;

namespace Light.Compilation.Types {
    public interface ITypeResolver {
        TypeReference Resolve(IAstTypeReference astReference, ModuleDefinition module);
    }
}
