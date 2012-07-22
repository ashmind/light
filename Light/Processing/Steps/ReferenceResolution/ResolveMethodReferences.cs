using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveMethodReferences : ProcessingStepBase<CallExpression> {
        public ResolveMethodReferences() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(CallExpression call, ProcessingContext context) {
            if (!(call.Method is AstUnknownMethod))
                return call;

            var declaringType = call.Target as IAstTypeReference; // static call
            if (declaringType == null)
                declaringType = ((IAstExpression)call.Target).ExpressionType;

            var sourceSpan = call.Method.SourceSpan;
            call.Method = declaringType.ResolveMethod(call.Method.Name, call.Arguments);
            call.Method.SourceSpan = sourceSpan;
            return call;
        }
    }
}
