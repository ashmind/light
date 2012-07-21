using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Internal;

namespace Light.Description {
    public class AstToCodeTransformer : AstToStringTransformer {
        protected override void AppendRoot(StringBuilder builder, AstRoot root) {
            AppendAll(builder, Environment.NewLine, root.Elements);
        }

        protected override void AppendCallExpression(StringBuilder builder, CallExpression call) {
            if (call.Target != null) {
                Append(builder, call.Target);
                builder.Append(".");
            }

            Append(builder, call.Method);
            builder.Append("(");
            AppendAll(builder, ", ", call.Arguments);
            builder.Append(")");
        }
    }
}
