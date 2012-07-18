using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.Literals;

namespace Light.Tests {
    public class TestAstVisitor : AstVisitor<StringBuilder> {
        public bool IncludesTypesOfValues { get; set; }

        public string Describe(IAstElement element) {
            var builder = new StringBuilder();
            this.Visit(element, builder);
            return builder.ToString();
        }

        protected override void VisitBinaryExpression(BinaryExpression binary, StringBuilder builder) {
            builder.Append("{");

            this.Visit(binary.Left, builder);
            builder.Append(" ").Append(binary.Operator.Name).Append(" ");
            this.Visit(binary.Right, builder);

            builder.Append("}");
        }

        protected override void VisitPrimitiveValue(PrimitiveValue value, StringBuilder builder) {
            if (!this.IncludesTypesOfValues) {
                builder.Append(value);
                return;
            }

            builder.Append("{")
                   .Append(value)
                   .Append(": ")
                   .Append(value.ExpressionType.ActualType.Name)
                   .Append("}");
        }

        protected override void VisitListInitializer(ListInitializer initializer, StringBuilder builder) {
            builder.Append("[");
            AppendCommaSeparatedElements(builder, initializer.Elements);
            builder.Append("]");
        }

        protected override void VisitObjectInitializer(ObjectInitializer initializer, StringBuilder builder) {
            builder.Append("{");
            AppendCommaSeparatedElements(builder, initializer.Elements);
            builder.Append("}");
        }

        protected override void VisitObjectInitializerEntry(ObjectInitializerEntry entry, StringBuilder builder) {
            builder.Append(entry.Name).Append(": ");
            Visit(entry.Value, builder);
        }

        protected override void VisitCallExpression(CallExpression call, StringBuilder builder) {
            if (call.Target != null) {
                Visit(call.Target, builder);
                builder.Append(".");
            }

            builder.Append(call.Method.Name);
            builder.Append("(");
            AppendCommaSeparatedElements(builder, call.Arguments);
            builder.Append(")");
        }

        protected override void VisitIdentifierExpression(IdentifierExpression identifier, StringBuilder builder) {
            builder.Append(identifier.Name);
        }

        protected override void VisitMemberExpression(MemberExpression member, StringBuilder builder) {
            Visit(member.Target, builder);
            builder.Append(".");
            builder.Append(member.Name);
        }

        protected override void VisitIndexerExpression(IndexerExpression indexer, StringBuilder builder) {
            Visit(indexer.Target, builder);
            builder.Append("[");
            AppendCommaSeparatedElements(builder, indexer.Arguments);
            builder.Append("]");
        }

        protected override void VisitNotRecognized(IAstElement element, StringBuilder builder) {
            builder.Append(element);
        }

        private void AppendCommaSeparatedElements(StringBuilder builder, IEnumerable<IAstElement> elements) {
            var first = true;
            foreach (var element in elements) {
                if (!first)
                    builder.Append(", ");

                Visit(element, builder);
                first = false;
            }
        }
    }
}
