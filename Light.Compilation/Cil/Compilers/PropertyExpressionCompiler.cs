using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Compilation.References;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class PropertyReferenceCompiler : CilCompilerBase<AstPropertyExpression> {
        public override void Compile(ILProcessor processor, AstPropertyExpression expression, CilCompilationContext context) {
            var fieldOrProperty = context.ConvertReference(expression.Reference);
            var field = fieldOrProperty.As<FieldReference>();
            if (field != null) {
                processor.Emit(OpCodes.Ldarg_0); // temporary cheating
                processor.Emit(OpCodes.Ldfld, field);
                return;
            }

            var property = (PropertyReferenceContainer)fieldOrProperty;
            if (expression.Target != null)
                context.Compile(expression.Target);

            processor.Emit(OpCodes.Call, property.GetMethod);
        }
    }
}
