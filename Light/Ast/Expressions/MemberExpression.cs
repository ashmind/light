﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public class MemberExpression : IAstExpression {
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

        public IEnumerable<IAstElement> Children() {
            if (this.Target != null)
                yield return this.Target;
        }

        public IAstTypeReference ExpressionType {
            get { throw new NotImplementedException("MemberExpression.ExpressionType"); }
        }
    }
}
