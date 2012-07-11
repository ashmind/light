using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Types {
    public static class AstTypeExtensions {
        public static bool IsVoid(this IAstTypeReference type) {
            return type == AstVoidType.Instance;
        }

        public static bool IsImplicit(this IAstTypeReference type) {
            return type == AstImplicitType.Instance;
        }
    }
}
