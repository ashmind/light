using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class NewExpression : IAstExpression {
        private IAstTypeReference type;
        public IAstTypeReference Type {
            get { return type; }
            set {
                Argument.RequireNotNull("value", value);
                type = value;
            }
        }

        public IAstConstructorReference Constructor { get; set; }
        public IList<IAstExpression> Arguments { get; private set; }
        public IAstElement Initializer { get; set; }

        public NewExpression(IAstTypeReference type, IEnumerable<IAstExpression> arguments, IAstElement initializer) {
            var argumentList = arguments.ToList();
            Argument.RequireNotNullAndNotContainsNull("arguments", argumentList);

            this.Type = type;
            this.Arguments = argumentList;
            this.Initializer = initializer;
        }

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.Type; }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Type = (IAstTypeReference)transform(this.Type);
            foreach (var argument in this.Arguments.Transform(transform)) {
                yield return argument;
            }
            if (this.Initializer == null)
                yield break;
            
            yield return this.Initializer = transform(this.Initializer);
        }

        #endregion
    }
}