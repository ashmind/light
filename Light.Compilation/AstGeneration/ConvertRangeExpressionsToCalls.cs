using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Processing;

namespace Light.Compilation.AstGeneration {
    public class ConvertRangeExpressionsToCalls : ProcessingStepBase<AstRangeExpression> {
        public ConvertRangeExpressionsToCalls() : base(ProcessingStage.Compilation) {
        }

        public override IAstElement ProcessAfterChildren(AstRangeExpression element, ProcessingContext context) {
            return new CallExpression(
                new AstFunctionReferenceExpression(element.From, element.Method),
                element.To
            );
        }

        //private bool IsStatic(IAstMethodReference method) {
        //    // temporary cheating
        //    var methodInfo = method.Target as MethodInfo;
        //    if (methodInfo != null)
        //        return methodInfo.IsStatic;

        //    var declared = method.Target as AstFunctionDefinition;
        //    if (declared != null)
        //        return declared.Compilation.Static;

        //    throw new NotImplementedException("ConvertRangeExpressionsToCalls: Can not identify whether " + method + " is static.");
        //}
    }
}
