using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.TypeResolution {
    public class ResolveFunctionReferenceExpressionTypes : ProcessingStepBase<AstFunctionReferenceExpression> {
        public ResolveFunctionReferenceExpressionTypes()
            : base(ProcessingStage.TypeResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstFunctionReferenceExpression expression, ProcessingContext context) {
            expression.SetExpressionType(() => MakeExpressionType(expression));
            return expression;
        }

        private static AstReflectedType MakeExpressionType(AstFunctionReferenceExpression expression) {
            var types = new List<IAstTypeReference>(expression.Reference.ParameterTypes);
            var delegateTypeName = "Action";
            if (!(expression.Reference.ReturnType is AstVoidType)) {
                delegateTypeName = "Func";
                types.Add(expression.Reference.ReturnType);
            }
            delegateTypeName += "`" + types.Count;

            var delegateType = Type.GetType("System." + delegateTypeName, true).MakeGenericType(
                types.Cast<AstReflectedType>().Select(t => t.ActualType).ToArray()
            );

            return new AstReflectedType(delegateType);
        }
    }
}
