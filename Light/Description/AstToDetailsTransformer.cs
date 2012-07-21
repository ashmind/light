using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Ast.Literals;
using Light.Ast.References;

namespace Light.Description {
    public class AstToDetailsTransformer : AstToStringTransformer {
        protected override void AppendRoot(StringBuilder builder, AstRoot root) {
        }

        protected override void AppendPrimitiveValue(StringBuilder builder, PrimitiveValue value) {
            Append(builder, value.ExpressionType);
        }

        protected override void AppendVariableReference(StringBuilder builder, AstVariableReference variable) {
            builder.Append("(local) ");
            AppendVariableDefinition(builder, variable.Variable);
        }

        protected override void AppendParameterReference(StringBuilder builder, AstParameterReference parameter) {
            builder.Append("(parameter) ");
            AppendParameterDefinition(builder, parameter.Parameter);
        } 
    }
}
