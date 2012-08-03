using System;
using Light.Ast.Definitions;
using Mono.Cecil;

namespace Light.Compilation.Definitions.Builders {
    public class MethodDefinitionHelper {
        public virtual void FinalizeBuild(MethodDefinition method, AstMethodDefinitionBase ast, TypeDefinition type, DefinitionBuildingContext context) {
            BuildParameters(method, ast, context);
            type.Methods.Add(method);
            context.MapDefinition(ast, method);
        }

        protected virtual void BuildParameters(MethodDefinition method, AstMethodDefinitionBase ast, DefinitionBuildingContext context) {
            foreach (var parameter in ast.Parameters) {
                method.Parameters.Add(new ParameterDefinition(parameter.Name, ParameterAttributes.None, context.ConvertReference(parameter.Type)));
            }
        }
    }
}