using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class PropertyReferenceCompiler : CilCompilerBase<AstPropertyReference> {
        public override void Compile(ILProcessor processor, AstPropertyReference reference, CilCompilationContext context) {
            var field = context.ConvertFieldReference(reference);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldfld, field);
        }
    }
}
