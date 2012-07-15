using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Statements;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil.Compilers {
    public class VariableDefinitionCompiler : CilCompilerBase<AstVariableDefinition> {
        public override void Compile(ILProcessor processor, AstVariableDefinition variable, CilCompilationContext context) {
            var variableDefinition = new VariableDefinition(variable.Name, context.ConvertReference(variable.Type));
            
            context.Method.Body.InitLocals = true;
            context.Method.Body.Variables.Add(variableDefinition);

            context.MapDefinition(variable, variableDefinition);

            if (variable.AssignedValue == null)
                return;

            context.Compile(variable.AssignedValue);
            processor.Emit(OpCodes.Stloc, variableDefinition);
        }
    }
}
