using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class AstLambdaExpression : AstElementBase, IAstExpression {
        private IAstElement body;
        public IList<AstParameterDefinition> Parameters { get; private set; }

        public AstLambdaExpression(IEnumerable<AstParameterDefinition> parameters, IAstElement body) {
            var parametersList = parameters.ToList();
            Argument.RequireNotNullAndNotContainsNull("parameters", parametersList);

            Parameters = parametersList;
            Body = body;
        }

        public IAstElement Body {
            get { return this.body; }
            set {
                Argument.RequireNotNull("value", value);
                this.body = value;
            }
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("LambdaExpression.ExpressionType"); }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            return this.Parameters.Transform(transform).Concat(
                this.Body = transform(this.Body)
            );
        }
    }
}
