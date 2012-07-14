using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class NewCompiler : CilCompilerBase<NewExpression> {
        public override void Compile(ILProcessor processor, NewExpression @new, CilCompilationContext context) {
            if (@new.Constructor == null)
                throw new NotImplementedException("NewCompiler: Constructor on " + @new + " is null.");

            foreach (var argument in @new.Arguments) {
                context.Compile(argument);
            }
            processor.Emit(OpCodes.Newobj, context.ConvertReference(@new.Constructor));
        }
    }
}
