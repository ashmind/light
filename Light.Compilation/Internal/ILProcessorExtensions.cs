using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil.Cil;

namespace Light.Compilation.Internal {
    public static class ILProcessorExtensions {
        private static readonly IDictionary<int, OpCode> OptimizedLdcI4 = new Dictionary<int, OpCode> {
            { 0, OpCodes.Ldc_I4_0 },
            { 1, OpCodes.Ldc_I4_1 },
            { 2, OpCodes.Ldc_I4_2 },
        };

        public static void EmitLdcI4(this ILProcessor processor, int value) {
            OpCode optimizedCode;
            var optimized = OptimizedLdcI4.TryGetValue(value, out optimizedCode);
            if (optimized) {
                processor.Emit(optimizedCode);
                return;
            }

            processor.Emit(OpCodes.Ldc_I4, value);
        }
    }
}
