using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class AssignmentCompiler : CilCompilerBase<AssignmentStatement> {
        public override void Compile(ILProcessor processor, AssignmentStatement assignment, CilCompilationContext context) {
            var property = assignment.Target as AstPropertyExpression;
            if (property != null) {
                CompileFieldOrPropertyAssignment(processor, property, assignment.Value, context);
                return;
            }

            throw new NotImplementedException("AssignmentCompiler: Assignment to " + assignment.Target + " is not yet supported.");
        }

        private void CompileFieldOrPropertyAssignment(ILProcessor processor, AstPropertyExpression property, IAstExpression value, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldarg_0);
            context.Compile(value);
            var fieldOrProperty = context.ConvertReference(property.Reference);
            var field = fieldOrProperty.As<FieldReference>();
            if (field == null)
                throw new NotImplementedException("AssignmentCompiler: Assignment to " + fieldOrProperty + " is not yet supported.");

            processor.Emit(OpCodes.Stfld, field);
        }
    }
}