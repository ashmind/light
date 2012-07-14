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

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("LambdaExpression.ExpressionType"); }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.Parameters.Transform(transform).Concat(
                this.Body = transform(this.Body)
            );
        }

        #endregion
    }
}
