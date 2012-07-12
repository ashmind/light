using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class NewExpression : IAstExpression {
        private string typeName;

        public string TypeName {
            get { return typeName; }
            set {
                Argument.RequireNotNullAndNotEmpty("value", value);
                typeName = value;
            }
        }

        public IList<IAstElement> Arguments { get; private set; }
        public IAstElement Initializer { get; set; }

        public NewExpression(string typeName, IEnumerable<IAstElement> arguments, IAstElement initializer) {
            var argumentList = arguments.ToList();
            Argument.RequireNotNullAndNotContainsNull("arguments", argumentList);

            this.TypeName = typeName;
            this.Arguments = argumentList;
            this.Initializer = initializer;
        }

        public IEnumerable<IAstElement> Children() {
            return this.Arguments.Concat(this.Initializer);
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("NewExpression.ExpressionType"); }
        }
    }
}