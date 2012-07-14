using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References.Methods;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class CallCompiler : CilCompilerBase<CallExpression> {
        public override void Compile(ILProcessor processor, CallExpression call, CilCompilationContext context) {
            if (call.Target != null)
                context.Compile(call.Target);

            foreach (var argument in call.Arguments) {
                context.Compile(argument);
            }

            var reflectedMethod = call.Method as AstReflectedMethod;
            if (reflectedMethod == null)
                throw new NotImplementedException("CallCompiler: Cannot compile call to " + call.Method + ".");

            processor.Emit(OpCodes.Call, context.Module.Import(reflectedMethod.Method));
        }
    }
}
