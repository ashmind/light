using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Ast.Literals;

namespace Light.Tests {
    public class TestAstVisitor : AstVisitor<StringBuilder> {
        public bool IncludesTypesOfValues { get; set; }

        public string Describe(IEnumerable<IAstElement> elements) {
            var builder = new StringBuilder();
            foreach (var element in elements) {
                this.Visit(element, builder);
            }
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
                builder.Append(value);
                return value;
            }

            builder.Append("{")
                   .Append(value)
                   .Append(": ")
                   .Append(value.Type.Name)
                   .Append("}");

            return value;
        }

        protected override object VisitListInitializer(ListInitializer initializer, StringBuilder builder) {
            builder.Append("[");
            var first = true;
            foreach (var element in initializer.Elements) {
                if (!first)
                    builder.Append(", ");

                Visit(element, builder);
                first = false;
            }
            builder.Append("]");

            return initializer;
        }
    }
}
