using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveMemberReferences : ProcessingStepBase<MemberExpression> {
        public ResolveMemberReferences() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(MemberExpression member, ProcessingContext context) {
            var declaringType = member.Target as IAstTypeReference; // static
            if (declaringType == null)
                declaringType = ((IAstExpression)member.Target).ExpressionType;

            var resolved = declaringType.ResolveMember(member.Name);
            if (resolved == null)
                throw new NotImplementedException("ResolveMemberReferences: Failed to resolve " + member.Name);

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
