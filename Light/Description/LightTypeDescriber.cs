using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Description {
    public class LightTypeDescriber {
        public string Describe(IAstTypeReference type) {
            var reflected = type as AstReflectedType;
            if (reflected != null)
                return reflected.ActualType.Name;

            return null;
        }
    }
}
