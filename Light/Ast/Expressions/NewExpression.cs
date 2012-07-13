using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
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

        public IList<IAstElement> Arguments { get; private set; }
        public IAstElement Initializer { get; set; }

        public NewExpression(IAstTypeReference type, IEnumerable<IAstElement> arguments, IAstElement initializer) {
            var argumentList = arguments.ToList();
            Argument.RequireNotNullAndNotContainsNull("arguments", argumentList);

            this.Type = type;
            this.Arguments = argumentList;
            this.Initializer = initializer;
        }

        public IEnumerable<IAstElement> Children() {
            return this.Arguments.Concat(this.Initializer);
        }

        IAstTypeReference IAstExpression.ExpressionType {
            get { return this.Type; }
        }
    }
}