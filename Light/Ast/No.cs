using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References;

namespace Light.Ast {
    public static class No {
        private static readonly ReadOnlyCollection<IAstElement> elements = new IAstElement[0].AsReadOnly();
        private static readonly ReadOnlyCollection<IAstReference> references = new IAstReference[0].AsReadOnly();
        private static readonly ReadOnlyCollection<ParameterDefinition> parameters = new ParameterDefinition[0].AsReadOnly();

        public static ReadOnlyCollection<IAstElement> Elements {
            get { return elements; }
        }

        public static ReadOnlyCollection<IAstReference> References {
            get { return references; }
        }

        public static ReadOnlyCollection<ParameterDefinition> Parameters {
            get { return parameters; }
        }
    }
}
