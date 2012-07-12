using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References.Methods;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class CallCompiler : CilCompilerBase<CallExpression> {
        public override void Compile(ILProcessor processor, CallExpression call, Action<IAstElement> recursiveCompile, ModuleDefinition module) {
            if (call.Target != null)
                recursiveCompile(call.Target);

            foreach (var argument in call.Arguments) {
                recursiveCompile(argument);
            }

            var reflectedMethod = call.Method as AstReflectedMethod;
            if (reflectedMethod == null)
                throw new NotImplementedException("CallCompiler: Cannot compile call to " + call.Method + ".");

            processor.Emit(OpCodes.Call, module.Import(reflectedMethod.Method));
        }
    }
}
