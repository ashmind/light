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
using Light.Framework;
using Light.Internal;
using Mono.Cecil.Cil;
using Decimal = Light.Framework.Decimal;

namespace Light.Compilation.Cil.Compilers {
    public class PrimitiveValueCompiler : CilCompilerBase<PrimitiveValue> {
        private static readonly Reflector Reflector = new Reflector();

        private static readonly IAstConstructorReference NewIntegerFromInt32 = new AstReflectedConstructor(typeof(Integer).GetConstructor(new[] { typeof(int) }), Reflector);
        private static readonly IAstConstructorReference NewIntegerFromBytes = new AstReflectedConstructor(typeof(Integer).GetConstructor(new[] { typeof(byte[]) }), Reflector);
        private static readonly IAstConstructorReference NewDecimalFromBits  = new AstReflectedConstructor(typeof(Decimal).GetConstructor(new[] { typeof(int[]) }), Reflector);
        
        private static readonly IAstTypeReference Byte = Reflector.Reflect(typeof(byte));
        private static readonly IAstTypeReference ByteArray = Reflector.Reflect(typeof(byte[]));
        private static readonly IAstTypeReference Int32 = Reflector.Reflect(typeof(int));
        private static readonly IAstTypeReference Int32Array = Reflector.Reflect(typeof(int[]));

        private static readonly IDictionary<Type, Action<ILProcessor, PrimitiveValue, CilCompilationContext>> typeBasedCompilers = new Dictionary<Type, Action<ILProcessor, PrimitiveValue, CilCompilationContext>> {
            { typeof(Integer),    CompileInteger },
            { typeof(Decimal),    CompileDecimal },
            { typeof(string),     CompileString },
            { typeof(bool),       CompileBoolean }
        };

        public override void Compile(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var compile = typeBasedCompilers.GetValueOrDefault(((AstReflectedType)value.ExpressionType).ActualType);
            if (compile == null)
                throw new NotImplementedException("PrimitiveValueCompiler: cannot compile " + value.ExpressionType + ".");

            compile(processor, value, context);
        }

        private static void CompileInteger(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var integer = (Integer)value.Value;
            if (integer.Kind == IntegerKind.Int32) {
                processor.EmitLdcI4(integer.Int32Value);
                processor.Emit(OpCodes.Newobj, context.ConvertReference(NewIntegerFromInt32));
            }
            else {
                EmitBigIntegerBytes(processor, integer.BigIntegerValue, context);
                processor.Emit(OpCodes.Newobj, context.ConvertReference(NewIntegerFromBytes));
            }
        }

        private static void EmitBigIntegerBytes(ILProcessor processor, BigInteger value, CilCompilationContext context) {
            var bytes = value.ToByteArray();
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
        }

        private static void CompileDecimal(ILProcessor processor, PrimitiveValue value, CilCompilationContext context) {
            var @decimal = (Decimal)value.Value;
            var bits = @decimal.GetBits();

            var bitsVariable = context.DefineVariable("t", Int32Array); // temporary cheating

            processor.EmitLdcI4(bits.Length);
            processor.Emit(OpCodes.Newarr, context.ConvertReference(Int32));
            processor.Emit(OpCodes.Stloc, bitsVariable);
            for (var i = 0; i < bits.Length; i++) {
                processor.Emit(OpCodes.Ldloc, bitsVariable);
                processor.EmitLdcI4(i);
                processor.EmitLdcI4(bits[i]);
                processor.Emit(OpCodes.Stelem_I4);
            }
            processor.Emit(OpCodes.Ldloc, bitsVariable);
            processor.Emit(OpCodes.Newobj, context.ConvertReference(NewDecimalFromBits));
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
