using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class IndexerExpression : IAstExpression {
        public IAstElement Target { get; private set; }
        public ReadOnlyCollection<IAstElement> Arguments { get; private set; }

        public IndexerExpression(IAstElement target, params IAstElement[] arguments) {
            Argument.RequireNotNull("target", target);
            Argument.RequireNotNullAndNotContainsNull("arguments", arguments);

            this.Target = target;
            this.Arguments = arguments.AsReadOnly();
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("IndexerExpression.ExpressionType"); }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Target = transform(this.Target);
            foreach (var argument in this.Arguments.Transform(transform)) {
                yield return argument;
            }
        }

        #endregion
    }
}
