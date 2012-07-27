using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;
using Light.Processing.Helpers;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveMemberReferences : ProcessingStepBase<MemberExpression> {
        private readonly MemberResolver resolver;

        public ResolveMemberReferences(MemberResolver resolver) : base(ProcessingStage.ReferenceResolution) {
            this.resolver = resolver;
        }

        public override IAstElement ProcessAfterChildren(MemberExpression member, ProcessingContext context) {
            var declaringType = member.Target as IAstTypeReference; // static
            if (declaringType == null)
                declaringType = ((IAstExpression)member.Target).ExpressionType;

            var resolved = this.resolver.Resolve(declaringType, member.Name, context);

            var method = resolved as IAstMethodReference;
            if (method != null) {
                return new AstFunctionReferenceExpression(member.Target, method) {
                    SourceSpan = member.SourceSpan
                };
            }

            var property = resolved as IAstPropertyReference;
            if (property != null) {
                return new AstPropertyExpression(member.Target, property) {
                    SourceSpan = member.SourceSpan
                };
            }
            
            throw new NotImplementedException("ResolveMemberReferences: " + resolved.GetType().Name + " is not yet supported.");
        }
    }
}
