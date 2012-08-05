using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Definitions;

namespace Light.Ast {
    public interface IAstFunctionDefinition : IAstDefinition {
        IList<AstParameterDefinition> Parameters { get; }
    }
}
