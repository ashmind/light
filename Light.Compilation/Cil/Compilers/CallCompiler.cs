using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class CallCompiler : CilCompilerBase<CallExpression> {
        public override void Compile(ILProcessor processor, CallExpression call, CilCompilationContext context) {
            var function = call.Callee as AstFunctionReferenceExpression;
            if (function == null)
                throw new NotImplementedException("CallCompiler: " + call.Callee.GetType().Name + " is not yet supported as call.Callee.");

            if (function.Target != null && !(function.Target is IAstTypeReference))
                CallCompilerHelper.CompileTarget(processor, (IAstExpression)function.Target, context);

            foreach (var argument in call.Arguments) {
                context.Compile(argument);
            }

            processor.Emit(OpCodes.Call, context.ConvertReference(function.Reference));
        }
    }
}
