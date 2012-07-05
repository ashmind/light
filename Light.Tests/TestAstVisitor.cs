using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Ast.Literals;

namespace Light.Tests {
    public class TestAstVisitor : AstVisitor<StringBuilder> {
        public bool IncludesTypesOfValues { get; set; }

        public string Describe(IAstElement element) {
            var builder = new StringBuilder();
            this.Visit(element, builder);
            return builder.ToString();
        }

        protected override object VisitBinaryExpression(BinaryExpression binary, StringBuilder builder) {
            builder.Append("{");

            this.Visit(binary.Left, builder);
            builder.Append(" ").Append(binary.Operator.Symbol).Append(" ");
            this.Visit(binary.Right, builder);

            builder.Append("}");
            return binary;
        }

        protected override object VisitPrimitiveValue(PrimitiveValue value, StringBuilder builder) {
            if (!this.IncludesTypesOfValues) {
                builder.Append(value.Value);
                return value;
            }

            builder.Append("{")
                   .Append(value.Value)
                   .Append(": ")
                   .Append(value.Type.Name)
                   .Append("}");

            return value;
        }
    }
}
