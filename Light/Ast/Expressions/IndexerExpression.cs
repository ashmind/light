using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.References;
using Light.Internal;

namespace Light.Ast.Expressions {
    public class IndexerExpression : AstElementBase, IAstExpression {
        private IAstElement target;
        public IList<IAstElement> Arguments { get; private set; }

        public IndexerExpression(IAstElement target, IEnumerable<IAstElement> arguments) {
            var argumentList = arguments.ToList();
            Argument.RequireNotNullAndNotContainsNull("arguments", argumentList);

            this.Target = target;
            this.Arguments = argumentList;
        }

        public IAstElement Target {
            get { return this.target; }
            set {
                Argument.RequireNotNull("value", value);
                this.target = value;
            }
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("IndexerExpression.ExpressionType"); }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Target = transform(this.Target);
            foreach (var argument in this.Arguments.Transform(transform)) {
                yield return argument;
            }
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.Append(this.Target)
                   .Append("[")
                   .AppendJoin(", ", this.Arguments)
                   .Append("]");

            return builder.ToString();
        }
    }
}
