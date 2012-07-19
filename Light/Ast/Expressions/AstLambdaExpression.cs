using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Internal;

namespace Light.Ast.Expressions {
    public class AstLambdaExpression : IAstExpression {
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

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.Parameters.Transform(transform).Concat(
                this.Body = transform(this.Body)
            );
        }

        #endregion

        public override string ToString() {
            var needsBrackets = this.Parameters.Count > 1 || !(this.Parameters[0].Type is AstImplicitType);
            var builder = new StringBuilder();
            if (needsBrackets)
                builder.Append("(");

            builder.AppendJoin(", ", this.Parameters);

            if (needsBrackets)
                builder.Append(")");

            builder.Append(" => ");
            builder.Append(this.Body);

            return builder.ToString();
        }
    }
}
