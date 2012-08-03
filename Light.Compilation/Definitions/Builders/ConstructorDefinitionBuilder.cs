using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Light.Ast.References.Types;
using Mono.Cecil;

namespace Light.Compilation.Definitions.Builders {
    public class ConstructorDefinitionBuilder : DefinitionBuilderBase<AstConstructorDefinition, TypeDefinition> {
        private readonly MethodDefinitionHelper helper;

        public ConstructorDefinitionBuilder(MethodDefinitionHelper helper) {
            this.helper = helper;
        }

        public override void Build(AstConstructorDefinition ast, TypeDefinition type, DefinitionBuildingContext context) {
            var constructor = new MethodDefinition(
                ".ctor",
                MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.HideBySig,
                type.Module.Import(typeof(void))
            );
            constructor.Attributes |= MethodAttributes.Public;

            this.helper.FinalizeBuild(constructor, ast, type, context);
        }
    }
}
