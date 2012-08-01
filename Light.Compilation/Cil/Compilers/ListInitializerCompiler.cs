using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Literals;
using Light.Compilation.Internal;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class ListInitializerCompiler : CilCompilerBase<AstListInitializer> {
        private static readonly IDictionary<MetadataType, OpCode> StelemCodes = new Dictionary<MetadataType, OpCode> {
            { MetadataType.Boolean, OpCodes.Stelem_I1 },
            { MetadataType.Int32,   OpCodes.Stelem_I4 },
            { MetadataType.Double,  OpCodes.Stelem_R8 },
        };

        public override void Compile(ILProcessor processor, AstListInitializer initializer, CilCompilationContext context) {
            var temporaryVariable = context.DefineVariable("temp", initializer.ExpressionType);
            var elementType = context.ConvertReference(initializer.Elements[0].ExpressionType); // temporary cheating

            processor.EmitLdcI4(initializer.Elements.Count);
            processor.Emit(OpCodes.Newarr, elementType);
            processor.Emit(OpCodes.Stloc, temporaryVariable);
            for (var i = 0; i < initializer.Elements.Count; i++) {
                processor.Emit(OpCodes.Ldloc, temporaryVariable);
                processor.EmitLdcI4(i);
                EmitStelem(processor, elementType, initializer.Elements[i], context);
            }

            processor.Emit(OpCodes.Ldloc, temporaryVariable);
        }

        private void EmitStelem(ILProcessor processor, TypeReference elementType, IAstExpression element, CilCompilationContext context) {
            if (elementType.IsPrimitive) {
                if (!StelemCodes.ContainsKey(elementType.MetadataType))
                    throw new NotImplementedException("ListInitializerCompiler.EmitStelem: Element metadata type " + elementType.MetadataType + " is not yet supported.");

                context.Compile(element);
                processor.Emit(StelemCodes[elementType.MetadataType]);
                return;
            }

            if (elementType.IsValueType) {
                processor.Emit(OpCodes.Ldelema, elementType);
                context.Compile(element);
                processor.Emit(OpCodes.Stobj, elementType);
                return;
            }

            context.Compile(element);
            processor.Emit(OpCodes.Stelem_Ref);
        }
    }
}
