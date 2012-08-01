using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Statements;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class BaseConstructorCallCompiler : CilCompilerBase<AstBaseConstructorCall> {
        public override void Compile(ILProcessor processor, AstBaseConstructorCall call, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Call, context.ConvertReference(call.Constructor));
        }
    }
}
