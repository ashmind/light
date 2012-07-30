using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Internal;
using Light.Processing.Scoping;

namespace Light.Processing.Steps.ScopeDefinition {
    public class AddImportToScope : ProcessingStepBase<ImportDefinition> {
        private readonly Reflector reflector;

        private static readonly HashSet<Assembly> CurrentlySupportedAssemblies = new HashSet<Assembly> {
            typeof(object).Assembly,
            typeof(Enumerable).Assembly
        };

        public AddImportToScope(Reflector reflector) : base(ProcessingStage.ScopeDefinition) {
            this.reflector = reflector;
        }

        public override IAstElement ProcessAfterChildren(ImportDefinition import, ProcessingContext context) {
            context.Scope.Add(new ExternalNamespaceNameSource(import.Namespace.ToString(), CurrentlySupportedAssemblies, this.reflector));
            return import;
        }
    }
}
