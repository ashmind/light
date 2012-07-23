using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class ThisCompiler : CilCompilerBase<AstThisExpression> {
        public override void Compile(ILProcessor processor, AstThisExpression @this, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldarg_0);
        }
    }
}
