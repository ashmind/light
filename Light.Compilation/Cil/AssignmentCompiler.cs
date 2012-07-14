using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.Statements;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class AssignmentCompiler : CilCompilerBase<AssignmentStatement> {
        public override void Compile(ILProcessor processor, AssignmentStatement assignment, CilCompilationContext context) {
            var property = assignment.Target as AstPropertyReference;
            if (property != null) {
                CompileFieldOrPropertyAssignment(processor, property, assignment.Value, context);
                return;
            }

            throw new NotImplementedException("AssignmentCompiler: Cannot compile assignment to " + assignment.Target + ".");
        }

        private void CompileFieldOrPropertyAssignment(ILProcessor processor, AstPropertyReference property, IAstExpression value, CilCompilationContext context) {
            processor.Emit(OpCodes.Ldarg_0);
            context.Compile(value);
            var field = context.ConvertFieldReference(property);
            processor.Emit(OpCodes.Stfld, field);
        }
    }
}
