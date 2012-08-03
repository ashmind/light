using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References.Types;
using Mono.Cecil;

namespace Light.Compilation.Definitions.Builders {
    public class FunctionDefinitionBuilder : DefinitionBuilderBase<AstFunctionDefinition, TypeDefinition> {
        private readonly MethodDefinitionHelper helper;

        public FunctionDefinitionBuilder(MethodDefinitionHelper helper) {
            this.helper = helper;
        }

        public override void Build(AstFunctionDefinition ast, TypeDefinition type, DefinitionBuildingContext context) {
            var attributes = MethodAttributes.Public;
            if (ast.Compilation.Static)
                attributes |= MethodAttributes.Static;

            var method = new MethodDefinition(ast.Name, attributes, type.Module.Import(typeof(void)));

            var astGenericParameterTypes = ast.Parameters.Select(p => p.Type).OfType<AstGenericPlaceholderType>();
            foreach (var astPlaceholder in astGenericParameterTypes) {
                var genericParameter = new GenericParameter(astPlaceholder.Name, method);
                method.GenericParameters.Add(genericParameter);
                context.MapDefinition(astPlaceholder, genericParameter);
            }

            var returnType = context.ConvertReference(ast.ReturnType, returnNullIfFailed: true);
            if (returnType != null) {
                method.ReturnType = returnType;
            }
            else {
                context.AddDebt(() => method.ReturnType = context.ConvertReference(ast.ReturnType));
            }

            if (ast.Compilation.EntryPoint)
                type.Module.EntryPoint = method;

            this.helper.FinalizeBuild(method, ast, type, context);
        }
    }
}
