using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class VariableReferenceCompiler : CilCompilerBase<AstVariableReference> {
        public override void Compile(ILProcessor processor, AstVariableReference variable, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldloc, context.ConvertReference(variable));
        }
    }
}
