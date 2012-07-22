using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Statements;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class VariableDefinitionCompiler : CilCompilerBase<AstVariableDefinition> {
        public override void Compile(ILProcessor processor, AstVariableDefinition variable, CilCompilationContext context) {
            var variableDefinition = context.DefineVariable(variable.Name, variable.Type);
            context.MapDefinition(variable, variableDefinition);

            if (variable.AssignedValue == null)
                return;

            context.Compile(variable.AssignedValue);
            processor.Emit(OpCodes.Stloc, variableDefinition);
        }
    }
}
