using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Incomplete;
using Light.Ast.References.Types;

namespace Light.Ast.References {
    public static class AstTypeExtensions {
        public static bool IsVoid(this IAstTypeReference type) {
            return type == AstVoidType.Instance;
        }

        public static bool IsImplicit(this IAstTypeReference type) {
            return type == AstImplicitType.Instance;
        }

        public static bool IsUnknown(this IAstTypeReference type) {
            return type is AstUnknownType;
        }

        public static IEnumerable<IAstTypeReference> GetAncestors(this IAstTypeReference type) {
            var @base = type.BaseType;
            while (@base != null) {
                yield return @base;
                @base = @base.BaseType;
            }
        }
    }
}
