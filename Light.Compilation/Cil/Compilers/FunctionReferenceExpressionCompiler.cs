using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class FunctionReferenceExpressionCompiler : CilCompilerBase<AstFunctionReferenceExpression> {
        public override void Compile(ILProcessor processor, AstFunctionReferenceExpression reference, CilCompilationContext context) {
            if (reference.Target != null && !(reference.Target is IAstTypeReference)) {
                context.Compile(reference.Target);
            }
            else {
                processor.Emit(OpCodes.Ldnull);
            }

            var delegateType = context.ConvertReference(reference.ExpressionType);
            var delegateConstructor = context.Method.Module.Import(delegateType.Resolve().Methods.Single(m => m.Name == ".ctor"));
            delegateConstructor.DeclaringType = delegateType; // fixes issue with delegateType.Resolve() eliminating generic arguments

            processor.Emit(OpCodes.Ldftn, context.ConvertReference(reference.Reference));
            processor.Emit(OpCodes.Newobj, delegateConstructor);
        }
    }
}
