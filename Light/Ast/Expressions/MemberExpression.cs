using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Expressions {
    public class MemberExpression : IAstElement {
        public IAstElement Target { get; private set; }
        public string Name { get; private set; }

        public MemberExpression(IAstElement target, string name) {
            Argument.RequireNotNull("target", target);
            Argument.RequireNotNullAndNotEmpty("name", name);

            this.Target = target;
            this.Name = name;
        }

        public override string ToString() {
            return this.Target + "." + this.Name;
        }
    }
}
