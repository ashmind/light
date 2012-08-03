using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.Definitions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Ast.Expressions {
    public class AstLambdaExpression : AstElementBase, IAstExpression {
        private IAstElement body;
        public IList<AstParameterDefinition> Parameters { get; private set; }
        public IAstTypeReference ExpressionType { get; private set; }

        public AstLambdaExpression(IEnumerable<AstParameterDefinition> parameters, IAstElement body) {
            var parametersList = parameters.ToList();
            Argument.RequireNotNullAndNotContainsNull("parameters", parametersList);

            Parameters = parametersList;
            Body = body;
            ExpressionType = new AstInferredFunctionType(
                () => this.Parameters.Select(p => p.Type),
                () => this.ReturnType
            );
        }

        public IAstElement Body {
            get { return this.body; }
            set {
                Argument.RequireNotNull("value", value);
                this.body = value;
            }
        }

        public IAstTypeReference ReturnType {
            get { return ((IAstExpression)this.Body).ExpressionType; }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            foreach (var parameter in this.Parameters.Transform(transform)) {
                yield return parameter;
            }
            yield return this.Body = transform(this.Body);
        }
    }
}
