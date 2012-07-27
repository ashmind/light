using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class CallExpression : AstElementBase, IAstExpression, IAstStatement {
        public IAstCallable callee;
        public IList<IAstExpression> Arguments { get; private set; }

        public CallExpression(IAstCallable callee, params IAstExpression[] arguments) 
            : this(callee, (IEnumerable<IAstExpression>)arguments)
        {
        }

        public CallExpression(IAstCallable callee, IEnumerable<IAstExpression> arguments) {
            Argument.RequireNotNull("arguments", arguments);
            var argumentList = arguments.ToList();
            Argument.RequireNotContainsNull("arguments", argumentList);

            this.Callee = callee;
            this.Arguments = argumentList;
        }

        public IAstCallable Callee {
            get { return this.callee; }
            set {
                Argument.RequireNotNull("value", value);
                this.callee = value;
            }
        }

        public IAstTypeReference ExpressionType {
            get { return callee.ReturnType; }
        }

        protected override IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform) {
            yield return this.Callee = (IAstCallable)transform(this.Callee);
            foreach (var argument in this.Arguments.Transform(transform)) {
                yield return argument;
            }
        }
    }
}
