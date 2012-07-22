using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
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

            var delegateType = ((AstReflectedType)reference.ExpressionType).ActualType;
            var delegateConstructor = context.ConvertReference(new AstReflectedConstructor(delegateType.GetConstructors().Single()));

            processor.Emit(OpCodes.Ldftn, context.ConvertReference(reference.Reference));
            processor.Emit(OpCodes.Newobj, delegateConstructor);
        }
    }
}
