using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Expressions {
    public class IndexerExpression : IAstElement {
        public IAstElement Target { get; private set; }
        public ReadOnlyCollection<IAstElement> Arguments { get; private set; }

        public IndexerExpression(IAstElement target, params IAstElement[] arguments) {
            Argument.RequireNotNull("target", target);
            Argument.RequireNotNullAndNotContainsNull("arguments", arguments);

            this.Target = target;
            this.Arguments = arguments.AsReadOnly();
        }
    }
}
