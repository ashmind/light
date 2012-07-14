using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.Definitions;
using Mono.Cecil;

namespace Light.Compilation.Definitions {
    public interface IDefinitionBuilder {
        void Build(IAstDefinition ast, IMemberDefinition parentDefinition, DefinitionBuildingContext context);
        bool CanBuild(IAstDefinition ast, IMemberDefinition parentDefinition);
    }
}
