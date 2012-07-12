using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Literals;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class PrimitiveValueCompiler : CilCompilerBase<PrimitiveValue> {
        private static readonly IDictionary<Type, Action<ILProcessor, PrimitiveValue>> typeBasedCompilers = new Dictionary<Type, Action<ILProcessor, PrimitiveValue>> {
            { typeof(int),    CompileInt32 },
            { typeof(string), CompileString }
        };

        public override void Compile(ILProcessor processor, PrimitiveValue value, Action<IAstElement> recursiveCompile, ModuleDefinition module) {
            var compile = typeBasedCompilers.GetValueOrDefault(value.ExpressionType.ActualType);
            if (compile == null)
                throw new NotImplementedException("PrimitiveValueCompiler: cannot compile " + value.ExpressionType + ".");

            compile(processor, value);
        }

        private static void CompileInt32(ILProcessor processor, PrimitiveValue value) {
            processor.Emit(OpCodes.Ldc_I4, (int)value.Value);
        }

        private static void CompileString(ILProcessor processor, PrimitiveValue value) {
            processor.Emit(OpCodes.Ldstr, (string)value.Value);
        }
    }
}
