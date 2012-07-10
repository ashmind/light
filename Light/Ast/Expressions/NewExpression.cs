using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Expressions {
    public class NewExpression : IAstElement {
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

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Arguments.Concat(this.Initializer);
        }

        #endregion
    }
}