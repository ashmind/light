using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Ast.Expressions;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Light.Ast.Statements;
using Light.Processing;

namespace Light.Compilation.AstGeneration {
    public class ConvertLambdaExpressionToMethods : ProcessingStepBase<AstLambdaExpression> {
        private const string LastLambdaIndexKey = "ConvertLambdaExpressionToMethods.LastLambdaIndex";

        public ConvertLambdaExpressionToMethods() : base(ProcessingStage.Compilation) {
        }

        public override IAstElement ProcessAfterChildren(AstLambdaExpression lambda, ProcessingContext context) {
            var type = context.ElementStack.OfType<AstTypeDefinition>().First();
            var lastLambdaIndex = (int?)context.FreeData.GetValueOrDefault(LastLambdaIndexKey) ?? 0;

            var method = new AstFunctionDefinition(
                "lambda#" + (lastLambdaIndex + 1),
                lambda.Parameters,
                new[] { new AstReturnStatement((IAstExpression)lambda.Body) },
                ((IAstExpression)lambda.Body).ExpressionType
            ) { Compilation = { Static = true } };
            type.Members.Add(method);

            var rewritten = new AstFunctionReferenceExpression(
                new AstDefinedType(type),
                new AstDefinedMethod(method)
            );
            rewritten.SetExpressionType(() => lambda.ExpressionType);

            return rewritten;
        }
    }
}
