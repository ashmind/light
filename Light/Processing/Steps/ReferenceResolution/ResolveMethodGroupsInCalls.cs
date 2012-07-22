using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveMethodGroupsInCalls : ProcessingStepBase<CallExpression> {
        public ResolveMethodGroupsInCalls() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(CallExpression call, ProcessingContext context) {
            var function = call.Callee as AstFunctionReferenceExpression;
            if (function == null)
                return call;

            var group = function.Reference as AstMethodGroup;
            if (group == null)
                return call;

            // little bit of cheating for now
            var type = ((AstReflectedMethod)group.Methods[0]).Method.DeclaringType;
            var method = type.GetMethod(group.Name, call.Arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType).ToArray());
            function.Reference = new AstReflectedMethod(method) { SourceSpan = group.SourceSpan };

            return call;
        }
    }
}
