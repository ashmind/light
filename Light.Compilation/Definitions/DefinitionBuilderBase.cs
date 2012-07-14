using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;
using Mono.Cecil;

namespace Light.Compilation.Definitions {
    public abstract class DefinitionBuilderBase<TAstDefinition, TParentDefinition> : IDefinitionBuilder
        where TAstDefinition : IAstDefinition
        where TParentDefinition : IMemberDefinition
    {
        public abstract void Build(TAstDefinition ast, TParentDefinition parentDefinition, DefinitionBuildingContext context);

        void IDefinitionBuilder.Build(IAstDefinition ast, IMemberDefinition parentDefinition, DefinitionBuildingContext context) {
            this.Build((TAstDefinition)ast, (TParentDefinition)parentDefinition, context);
        }

        bool IDefinitionBuilder.CanBuild(IAstDefinition ast, IMemberDefinition parentDefinition) {
            return ast is TAstDefinition
                && parentDefinition is TParentDefinition;
        }
    }
}
