using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Literals;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class PrimitiveValueCompiler : CilCompilerBase<PrimitiveValue> {
        public override void Compile(ILProcessor processor, PrimitiveValue value, Action<IAstElement> recursiveCompile) {
            if (value.Type != typeof(int))
                throw new NotImplementedException();

            processor.Emit(OpCodes.Ldc_I4, (int)value.Value);
        }
    }
}
