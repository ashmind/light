using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References.Methods;
using Light.Processing.Complex;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveMethodGroupsInCalls : ProcessingStepBase<CallExpression> {
        private readonly OverloadResolver overloadResolver;

        public ResolveMethodGroupsInCalls(OverloadResolver overloadResolver) : base(ProcessingStage.ReferenceResolution) {
            this.overloadResolver = overloadResolver;
        }

        public override IAstElement ProcessAfterChildren(CallExpression call, ProcessingContext context) {
            var function = call.Callee as AstFunctionReferenceExpression;
            if (function == null)
                return call;

            var group = function.Reference as AstMethodGroup;
            if (group == null)
                return call;

            function.Reference = this.overloadResolver.ResolveMethodGroup(group, function.Target, call.Arguments);
            function.Reference.SourceSpan = group.SourceSpan;

            return call;
        }
    }
}