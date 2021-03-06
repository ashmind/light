﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Light.Internal;

namespace Light.Processing.Scoping {
    public class ExternalNamespaceNameSource : INameSource {
        private readonly Reflector reflector;
        private readonly IDictionary<string, Type> typeCache;
        private readonly Dictionary<string, MethodInfo[]> memberCache;

        public ExternalNamespaceNameSource(string @namespace, IEnumerable<Assembly> assemblies, Reflector reflector) {
            this.reflector = reflector;
            Argument.RequireNotNullAndNotEmpty("namespace", @namespace);
            Argument.RequireNotNull("assemblies", assemblies);

            // this all is horribly inefficient, especially if using multiple namespaces from a single assembly
            // but it can wait for now
            this.typeCache = assemblies.SelectMany(a => a.GetExportedTypes())
                                       .Where(t => t.Namespace == @namespace)
                                       .ToDictionary(t => t.Name);

            if (this.typeCache.Count == 0)
                throw new NotImplementedException("ExternalNamespaceNameSource: No types in " + @namespace);

            this.memberCache = this.typeCache.Values
                                             .SelectMany(t => t.GetMethods())
                                             .Where(m => m.IsStatic)
                                             .Where(m => m.IsDefined<ExtensionAttribute>(false))
                                             .GroupBy(m => m.Name)
                                             .ToDictionary(g => g.Key, g => g.ToArray());
        }

        public IList<IAstReference> ResolveIdentifier(string name) {
            var type = this.typeCache.GetValueOrDefault(name);
            if (type == null)
                return No.References;

            return new[] { new AstReflectedType(type, this.reflector) };
        }

        public IList<IAstMemberReference> ResolveMember(string name) {
            var methods = this.memberCache.GetValueOrDefault(name);
            if (methods == null)
                return No.Members;

            return methods.Select(m => new AstReflectedMethod(m, this.reflector)).ToArray();
        }
    }
}
