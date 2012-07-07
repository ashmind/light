using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Statements {
    public class IfStatement : IAstElement {
        public IAstElement Condition { get; private set; }
        public ReadOnlyCollection<IAstElement> Body { get; private set; }

        public IfStatement(IAstElement condition, params IAstElement[] body) {
            Argument.RequireNotNull("condition", condition);
            Argument.RequireNotNullNotEmptyAndNotContainsNull("body", body);

            Condition = condition;
            Body = body.AsReadOnly();
        }
    }
}
