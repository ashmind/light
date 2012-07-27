using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveRangeMethodReferences : ProcessingStepBase<AstRangeExpression> {
        private readonly MemberResolver resolver;

        public ResolveRangeMethodReferences(MemberResolver resolver)
            : base(ProcessingStage.ReferenceResolution) {
            this.resolver = resolver;
        }

        public override IAstElement ProcessAfterChildren(AstRangeExpression range, ProcessingContext context) {
            if (!(range.Method is AstUnknownMethod))
                return range;

            range.Method = (IAstMethodReference)this.resolver.Resolve(range.From.ExpressionType, range.Method.Name, context);
            return range;
        }
    }
}