using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Expressions {
    public class LambdaExpression : IAstElement {
        public ReadOnlyCollection<IAstElement> Parameters { get; set; }
        public IAstElement Body { get; set; }

        public LambdaExpression(IAstElement[] parameters, IAstElement body) {
            Argument.RequireNotNull("parameters", parameters);
            Argument.RequireAllNotNull("parameters", parameters);
            Argument.RequireNotNull("body", body);

            Parameters = parameters.AsReadOnly();
            Body = body;
        }
    }
}
