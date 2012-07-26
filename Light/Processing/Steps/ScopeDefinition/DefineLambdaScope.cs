using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Processing.Scoping;

namespace Light.Processing.Steps.ScopeDefinition {
    public class DefineLambdaScope : ProcessingStepBase<AstLambdaExpression> {
        public DefineLambdaScope() : base(ProcessingStage.ScopeDefinition) {
        }

        public override IAstElement ProcessBeforeChildren(AstLambdaExpression lambda, ProcessingContext context) {
            var scope = new Scope();
            foreach (var parameter in lambda.Parameters) {
                scope.Add(parameter.Name, new AstParameterReference(parameter));
            }
            context.ScopeStack.Push(scope);
            return lambda;
        }

        public override IAstElement ProcessAfterChildren(AstLambdaExpression lambda, ProcessingContext context) {
            context.ScopeStack.Pop();
            return lambda;
        }
    }
}
