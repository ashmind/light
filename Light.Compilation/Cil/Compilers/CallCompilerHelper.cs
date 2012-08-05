using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public static class CallCompilerHelper {
        public static void CompileTarget(ILProcessor processor, IAstExpression target, IAstMethodReference function, CilCompilationContext context) {
            context.Compile(target);
            if (function.Location == MethodLocation.Extension)
                return;

            var targetType = context.ConvertReference(target.ExpressionType);
            if (!targetType.IsValueType)
                return;

            var variable = context.DefineVariable("x", targetType);
            processor.Emit(OpCodes.Stloc, variable);
            processor.Emit(OpCodes.Ldloca_S, variable);
        }
    }
}