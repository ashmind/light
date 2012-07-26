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
        private static readonly ReadOnlyCollection<IAstMemberReference> memberReferences = new IAstMemberReference[0].AsReadOnly();
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

        public static ReadOnlyCollection<IAstMemberReference> MemberReferences {
            get { return memberReferences; }
        }

        public static ReadOnlyCollection<AstParameterDefinition> Parameters {
            get { return parameters; }
        }
    }
}
