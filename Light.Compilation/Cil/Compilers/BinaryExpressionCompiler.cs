using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class BinaryExpressionCompiler : CilCompilerBase<BinaryExpression> {
        private readonly IDictionary<Tuple<Type, string>, OpCode> BuiltInOpCodes = new Dictionary<Tuple<Type, string>, OpCode> {
            { Tuple.Create(typeof(int), "+"),   OpCodes.Add },
            { Tuple.Create(typeof(int), "-"),   OpCodes.Sub },
            { Tuple.Create(typeof(int), "*"),   OpCodes.Mul },
            { Tuple.Create(typeof(int), "/"),   OpCodes.Div },
            { Tuple.Create(typeof(int), "<"),   OpCodes.Clt },
            { Tuple.Create(typeof(int), ">"),   OpCodes.Cgt },
            { Tuple.Create(typeof(int), "=="),  OpCodes.Ceq },
            { Tuple.Create(typeof(int), "mod"), OpCodes.Rem },
            { Tuple.Create(typeof(bool), "or"), OpCodes.Or  }
        };

        public override void Compile(ILProcessor processor, BinaryExpression binary, CilCompilationContext context) {
            var builtIn = binary.Operator as AstBuiltInOperator;
            if (builtIn != null) {
                context.Compile(binary.Left);
                context.Compile(binary.Right);

                EmitBuiltInOperator(processor, builtIn);
                return;
            }

            CallCompilerHelper.CompileTarget(processor, binary.Left, context);
            context.Compile(binary.Right);

            processor.Emit(OpCodes.Call, context.ConvertReference(binary.Operator));
        }

        private void EmitBuiltInOperator(ILProcessor processor, AstBuiltInOperator @operator) {
            var type = ((AstReflectedType)@operator.OperandType).ActualType;
            var key = Tuple.Create(type, @operator.Name);

            OpCode code;
            var found = BuiltInOpCodes.TryGetValue(key, out code);
            if (!found)
                throw new NotImplementedException("BinaryExpressionCompiler: could not find OpCode for " + key);

            processor.Emit(code);
        }
    }
}
