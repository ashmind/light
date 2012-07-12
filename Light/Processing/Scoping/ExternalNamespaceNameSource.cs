using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Ast.Types;

namespace Light.Processing.Scoping {
    public class ExternalNamespaceNameSource : INameSource {
        private readonly IDictionary<string, Type> typeCache;

        public ExternalNamespaceNameSource(Assembly assembly, string @namespace) {
            Argument.RequireNotNull("assembly", assembly);
            Argument.RequireNotNullAndNotEmpty("namespace", @namespace);

            this.typeCache = assembly.GetExportedTypes()
                                     .Where(t => t.Namespace == @namespace)
                                     .ToDictionary(t => t.Name);
        }

        public IList<IAstReference> Resolve(string name) {
            var type = this.typeCache.GetValueOrDefault(name);
            if (type == null)
                return No.References;

            return new[] { new AstReflectedType(type) };
        }
    }
}
