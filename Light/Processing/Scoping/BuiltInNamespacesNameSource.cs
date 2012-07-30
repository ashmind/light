using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Framework;
using Light.Internal;

namespace Light.Processing.Scoping {
    public class BuiltInNamespacesNameSource : INameSource {
        private readonly ExternalNamespaceNameSource[] namespaces;

        public BuiltInNamespacesNameSource(Reflector reflector) {
            this.namespaces = new[] {
                new ExternalNamespaceNameSource("Light.Framework", new[] { typeof(Range<>).Assembly }, reflector), 
            };
        }

        public IList<IAstReference> ResolveIdentifier(string name) {
            return this.namespaces.SelectMany(n => n.ResolveIdentifier(name)).ToArray();
        }

        public IList<IAstMemberReference> ResolveMember(string name) {
            return this.namespaces.SelectMany(n => n.ResolveMember(name)).ToArray();
        }
    }
}
