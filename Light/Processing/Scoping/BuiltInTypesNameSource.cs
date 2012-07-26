using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.BuiltIn;

namespace Light.Processing.Scoping {
    public class BuiltInTypesNameSource : INameSource {
        private readonly BuiltInTypeMap map;

        public BuiltInTypesNameSource(BuiltInTypeMap map) {
            this.map = map;
        }

        public IList<IAstReference> ResolveIdentifier(string name) {
            var type = this.map.GetTypeByAlias(name);
            if (type == null)
                return No.References;

            return new[] { new AstReflectedType(type) };
        }

        public IList<IAstMemberReference> ResolveMember(string name) {
            return No.MemberReferences;
        }
    }
}
