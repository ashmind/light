using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.Literals;

namespace Light.Description {
    public class AstToCodeTransformer : AstToStringTransformer {
        protected override void AppendRoot(StringBuilder builder, AstRoot root) {
            AppendAll(builder, Environment.NewLine, root.Elements);
        }

        protected override void AppendTypeDefinition(StringBuilder builder, AstTypeDefinition type) {
            builder.AppendLine("public class " + type.Name);
            AppendAll(builder, Environment.NewLine, type.Members);
            builder.AppendLine();
            builder.AppendLine("end");
        }

        protected override void AppendBinaryExpression(StringBuilder builder, BinaryExpression binaryExpression) {
            var useSpacesAroundOperator  = binaryExpression.Operator.Name != "..";
            if (this.UseParenthesesInAllExpressions)
                builder.Append("(");

            Append(builder, binaryExpression.Left);
            if (useSpacesAroundOperator)
                builder.Append(" ");

            Append(builder, binaryExpression.Operator);

            if (useSpacesAroundOperator)
                builder.Append(" ");
            Append(builder, binaryExpression.Right);

            if (this.UseParenthesesInAllExpressions)
                builder.Append(")");
        }

        protected override void AppendThisExpression(StringBuilder builder, AstThisExpression thisExpression) {
            builder.Append("this");
        }

        protected override void AppendMemberExpression(StringBuilder builder, MemberExpression member) {
            Append(builder, member.Target);
            builder.Append(".").Append(member.Name);
        }

        protected override void AppendFunctionReferenceExpression(StringBuilder builder, AstFunctionReferenceExpression expression) {
            if (expression.Target != null) {
                Append(builder, expression.Target);
                builder.Append(".");
            }
            builder.Append(expression.Function.Name);
        }

        protected override void AppendCallExpression(StringBuilder builder, CallExpression call) {
            Append(builder, call.Callee);
            builder.Append("(");
            AppendAll(builder, ", ", call.Arguments);
            builder.Append(")");
        }

        protected override void AppendIndexerExpression(StringBuilder builder, IndexerExpression indexer) {
            Append(builder, indexer.Target);
            builder.Append("[");
            AppendAll(builder, ", ", indexer.Arguments);
            builder.Append("]");
        }

        protected override void AppendUnknownType(StringBuilder builder, AstUnknownType unknownType) {
            builder.Append(unknownType.Name.IsNullOrEmpty() ? "?" : unknownType.Name);
        }

        protected override void AppendImplicitType(StringBuilder builder, AstImplicitType implicitType) {
        }

        protected override void AppendLambdaExpression(StringBuilder builder, AstLambdaExpression lambda) {
            var needsBrackets = lambda.Parameters.Count > 1 || !(lambda.Parameters[0].Type is AstImplicitType);
            if (needsBrackets)
                builder.Append("(");

            AppendAll(builder, ", ", lambda.Parameters);

            if (needsBrackets)
                builder.Append(")");

            builder.Append(" => ");
            Append(builder, lambda.Body);
        }

        protected override void AppendParameterDefinition(StringBuilder builder, AstParameterDefinition parameter) {
            var length = builder.Length;
            Append(builder, parameter.Type);
            if (builder.Length > length)
                builder.Append(" ");

            builder.Append(parameter.Name);
        }

        protected override void AppendListInitializer(StringBuilder builder, AstListInitializer initializer) {
            builder.Append("[");
            AppendAll(builder, ", ", initializer.Elements);
            builder.Append("]");
        }

        protected override void AppendObjectInitializer(StringBuilder builder, ObjectInitializer initializer) {
            builder.Append("{");
            AppendAll(builder, ", ", initializer.Elements);
            builder.Append("}");
        }

        protected override void AppendObjectInitializerEntry(StringBuilder builder, ObjectInitializerEntry entry) {
            builder.Append(entry.Name).Append(": ");
            Append(builder, entry.Value);
        }

        public bool UseParenthesesInAllExpressions { get; set; }
    }
}
