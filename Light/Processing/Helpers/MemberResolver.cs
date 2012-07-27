using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Processing.Scoping;

namespace Light.Processing.Helpers {
    public class MemberResolver {
        public IAstMemberReference Resolve(IAstTypeReference declaringType, string name, INameSource scope) {
            var resolved = declaringType.ResolveMember(name);
            if (resolved == null)
                resolved = ResolveMemberFromScope(name, scope);

            if (resolved == null)
                throw new NotImplementedException("MemberResolver: Failed to resolve " + name);

            return resolved;
        }

        private static IAstMemberReference ResolveMemberFromScope(string name, INameSource scope) {
            var extensions = scope.ResolveMember(name);
            if (extensions.Count == 0)
                return null;

            if (extensions.Count == 1)
                return extensions[0];

            if (extensions.All(a => a is IAstMethodReference))
                return new AstMethodGroup(name, extensions.Cast<IAstMethodReference>().ToArray());

            throw new NotImplementedException("MemberResolver: Ambiguous result for " + name);
        }
    }
}
