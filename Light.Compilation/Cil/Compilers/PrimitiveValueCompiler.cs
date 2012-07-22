using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Literals;
using Light.Compilation.Internal;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class PrimitiveValueCompiler : CilCompilerBase<PrimitiveValue> {
        private static readonly IDictionary<Type, Action<ILProcessor, PrimitiveValue>> typeBasedCompilers = new Dictionary<Type, Action<ILProcessor, PrimitiveValue>> {
            { typeof(int),    CompileInt32 },
            { typeof(double), CompileDouble },
            { typeof(string), CompileString },
            { typeof(bool),   CompileBoolean },
        };

        public override void Compile(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var compile = typeBasedCompilers.GetValueOrDefault(value.ExpressionType.ActualType);
            if (compile == null)
                throw new NotImplementedException("PrimitiveValueCompiler: cannot compile " + value.ExpressionType + ".");

            compile(processor, value);
        }

        private static void CompileInt32(ILProcessor processor, PrimitiveValue value) {
            processor.EmitLdcI4((int)value.Value);
        }

        private static void CompileDouble(ILProcessor processor, PrimitiveValue value) {
            processor.Emit(OpCodes.Ldc_R8, (double)value.Value);
        }

        private static void CompileString(ILProcessor processor, PrimitiveValue value) {
            processor.Emit(OpCodes.Ldstr, (string)value.Value);
        }

        private static void CompileBoolean(ILProcessor processor, PrimitiveValue value) {
            var opCode = (bool)value.Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0;
            processor.Emit(opCode);
        }
    }
}
