using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class ParameterReferenceCompiler : CilCompilerBase<AstParameterReference> {
        public override void Compile(ILProcessor processor, AstParameterReference reference, CilCompilationContext context) {
            var parameterIndex = context.MethodAst.Parameters.IndexOf(reference.Parameter);
            processor.Emit(OpCodes.Ldarg, parameterIndex + 1);
        }
    }
}
