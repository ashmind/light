using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Definitions {
    public class AstConstructorDefinition : AstMethodDefinitionBase, IAstMemberDefinition {
        public AstConstructorDefinition() {
        }

        public AstConstructorDefinition(IEnumerable<AstParameterDefinition> parameters, IEnumerable<IAstStatement> body)
            : base(parameters, body) {
        }

        #region IAstMemberDefinition Members

        string IAstMemberDefinition.Name {
            get { return "new"; }
        }

        #endregion
    }
}
