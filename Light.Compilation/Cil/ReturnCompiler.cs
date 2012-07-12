using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class ReturnCompiler : CilCompilerBase<ReturnStatement> {
        public override void Compile(ILProcessor processor, ReturnStatement element, Action<IAstElement> recursiveCompile, ModuleDefinition module) {
            if (element.Result != null)
                recursiveCompile(element.Result);

            processor.Emit(OpCodes.Ret);
        }
    }
}
