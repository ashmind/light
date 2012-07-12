using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class LambdaExpression : IAstExpression {
        public ReadOnlyCollection<IAstElement> Parameters { get; set; }
        public IAstElement Body { get; set; }

        public LambdaExpression(IAstElement[] parameters, IAstElement body) {
            Argument.RequireNotNullAndNotContainsNull("parameters", parameters);
            Argument.RequireNotNull("body", body);

            Parameters = parameters.AsReadOnly();
            Body = body;
        }

        public IEnumerable<IAstElement> Children() {
            return this.Parameters.Concat(this.Body);
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("LambdaExpression.ExpressionType"); }
        }
    }
}
