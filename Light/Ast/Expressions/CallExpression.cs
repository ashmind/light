﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast.References;
using Light.Internal;

namespace Light.Ast.Expressions {
    public class CallExpression : IAstExpression, IAstStatement {
        private IAstMethodReference method;
        public IAstElement Target { get; set; }
        public IList<IAstExpression> Arguments { get; private set; }

        public CallExpression(IAstElement target, IAstMethodReference method, IList<IAstExpression> arguments) {
            Argument.RequireNotNullAndNotContainsNull("arguments", arguments);

            this.Target = target;
            this.Method = method;
            this.Arguments = arguments.ToList();
        }

        public IAstMethodReference Method {
            get { return this.method; }
            set {
                Argument.RequireNotNull("value", value);
                this.method = value;
            }
        }

        public IAstTypeReference ExpressionType {
            get { return this.Method.ReturnType; }
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            if (this.Target != null)
                yield return this.Target = transform(this.Target);

            foreach (var argument in this.Arguments.Transform(transform)) {
                yield return argument;
            }
        }

        #endregion

        public override string ToString() {
            var builder = new StringBuilder();
            if (this.Target != null)
                builder.Append(this.Target).Append(".");

            builder.Append(this.Method)
                   .Append("(")
                   .AppendJoin(", ", this.Arguments)
                   .Append(")");

            return builder.ToString();
        }
    }
}
