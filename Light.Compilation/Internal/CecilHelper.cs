using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Light.Compilation.Internal {
    public static class CecilHelper {
        public static MethodDefinition CreateConstructor(TypeDefinition type) {
            return new MethodDefinition(
                ".ctor",
                MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig,
                GetVoidType(type.Module)
            );
        }

        public static TypeReference GetVoidType(ModuleDefinition module) {
            return module.Import(typeof(void));
        }
    }
}
