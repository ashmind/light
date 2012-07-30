using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.BuiltIn;
using Light.Internal;

namespace Light.Processing.Scoping {
    public class BuiltInTypesNameSource : INameSource {
        private readonly BuiltInTypeMap map;
        private readonly Reflector reflector;

        public BuiltInTypesNameSource(BuiltInTypeMap map, Reflector reflector) {
            this.map = map;
            this.reflector = reflector;
        }

        public IList<IAstReference> ResolveIdentifier(string name) {
            var type = this.map.GetTypeByAlias(name);
            if (type == null)
                return No.References;

            return new[] { new AstReflectedType(type, this.reflector) };
        }

        public IList<IAstMemberReference> ResolveMember(string name) {
            return No.Members;
        }
    }
}
