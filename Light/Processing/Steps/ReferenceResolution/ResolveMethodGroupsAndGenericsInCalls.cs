﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveMethodGroupsAndGenericsInCalls : ProcessingStepBase<CallExpression> {
        private readonly MethodCallResolver callResolver;

        public ResolveMethodGroupsAndGenericsInCalls(MethodCallResolver callResolver) : base(ProcessingStage.ReferenceResolution) {
            this.callResolver = callResolver;
        }

        public override IAstElement ProcessAfterChildren(CallExpression call, ProcessingContext context) {
            var function = call.Callee as AstFunctionReferenceExpression;
            if (function == null)
                return call;

            var sourceSpan = function.Function.SourceSpan;
            function.Function = this.callResolver.Resolve(function.Function, function.Target, call.Arguments);
            function.Function.SourceSpan = sourceSpan;

            return call;
        }
    }
}