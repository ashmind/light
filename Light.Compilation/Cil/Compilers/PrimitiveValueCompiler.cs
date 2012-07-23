using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AshMind.Extensions;
using Light.Ast.Literals;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Light.Compilation.Internal;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class PrimitiveValueCompiler : CilCompilerBase<PrimitiveValue> {
        private static readonly IAstConstructorReference NewBigInteger = new AstReflectedConstructor(typeof(BigInteger).GetConstructor(new[] {typeof(byte[])}));
        private static readonly IAstTypeReference Byte = new AstReflectedType(typeof(byte));
        private static readonly IAstTypeReference ByteArray = new AstReflectedType(typeof(byte[]));

        private static readonly IDictionary<Type, Action<ILProcessor, PrimitiveValue, CilCompilationContext>> typeBasedCompilers = new Dictionary<Type, Action<ILProcessor, PrimitiveValue, CilCompilationContext>> {
            { typeof(int),        CompileInt32 },
            { typeof(double),     CompileDouble },
            { typeof(string),     CompileString },
            { typeof(bool),       CompileBoolean },
            { typeof(BigInteger), CompileBigInteger }
        };

        public override void Compile(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var compile = typeBasedCompilers.GetValueOrDefault(value.ExpressionType.ActualType);
            if (compile == null)
                throw new NotImplementedException("PrimitiveValueCompiler: cannot compile " + value.ExpressionType + ".");

            compile(processor, value, context);
        }

        private static void CompileInt32(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            processor.EmitLdcI4((int)value.Value);
        }

        private static void CompileBigInteger(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var bytes = ((BigInteger)value.Value).ToByteArray();
            var bytesVariable = context.DefineVariable("t", ByteArray); // temporary cheating

            processor.EmitLdcI4(bytes.Length);
            processor.Emit(OpCodes.Newarr, context.ConvertReference(Byte));
            processor.Emit(OpCodes.Stloc, bytesVariable);
            for (var i = 0; i < bytes.Length; i++) {
                processor.Emit(OpCodes.Ldloc, bytesVariable);
                processor.EmitLdcI4(i);
                processor.EmitLdcI4(bytes[i]);
                processor.Emit(OpCodes.Stelem_I1);
            }

            processor.Emit(OpCodes.Ldloc, bytesVariable);
            processor.Emit(OpCodes.Newobj, context.ConvertReference(NewBigInteger));
        }

        private static void CompileDouble(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldc_R8, (double)value.Value);
        }

        private static void CompileString(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldstr, (string)value.Value);
        }

        private static void CompileBoolean(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var opCode = (bool)value.Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0;
            processor.Emit(opCode);
        }
    }
}
