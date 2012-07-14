using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Statements;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class ReturnCompiler : CilCompilerBase<ReturnStatement> {
        public override void Compile(ILProcessor processor, ReturnStatement element, CilCompilationContext context) {
            if (element.Result != null)
                context.Compile(element.Result);

            processor.Emit(OpCodes.Ret);
        }
    }
}
