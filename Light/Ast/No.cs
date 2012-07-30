using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.References;

namespace Light.Ast {
    public static class No {
        private static readonly ReadOnlyCollection<IAstElement> elements = new IAstElement[0].AsReadOnly();
        private static readonly ReadOnlyCollection<IAstExpression> expressions = new IAstExpression[0].AsReadOnly();
        private static readonly ReadOnlyCollection<IAstReference> references = new IAstReference[0].AsReadOnly();
        private static readonly ReadOnlyCollection<IAstTypeReference> types = new IAstTypeReference[0].AsReadOnly();
        private static readonly ReadOnlyCollection<IAstMemberReference> members = new IAstMemberReference[0].AsReadOnly();
        private static readonly ReadOnlyCollection<AstParameterDefinition> parameters = new AstParameterDefinition[0].AsReadOnly();

        public static ReadOnlyCollection<IAstElement> Elements {
            get { return elements; }
        }

        public static ReadOnlyCollection<IAstExpression> Expressions {
            get { return expressions; }
        }

        public static ReadOnlyCollection<IAstReference> References {
            get { return references; }
        }

        public static ReadOnlyCollection<IAstTypeReference> Types {
            get { return types; }
        }

        public static ReadOnlyCollection<IAstMemberReference> Members {
            get { return members; }
        }

        public static ReadOnlyCollection<AstParameterDefinition> Parameters {
            get { return parameters; }
        }
    }
}
