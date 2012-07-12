using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Processing.Steps {
    public class ResolveIdentifiers : IProcessingStep {
        public void ProcessBeforeChildren(IAstElement element, ProcessingContext context) {
            var call = element as CallExpression;
            if (call != null && call.Target is IdentifierExpression) {
                ProcessCall(call, context);
                return;
            }

            var member = element as MemberExpression;
            if (member != null && member.Target is IdentifierExpression) {
                ProcessMember(member, context);
                return;
            }

            var identifier = element as IdentifierExpression;
            if (identifier != null)
                ProcessIdentifier(identifier, context);
        }

        private void ProcessCall(CallExpression call, ProcessingContext context) {
            var name = (call.Target as IdentifierExpression).Name;
            var resolved = context.Resolve(name);
            if (resolved.Count == 0)
                throw new NotImplementedException("ResolveIdentifiers: cannot resolve '" + name + "'.");

            if (resolved.Count > 1)
                throw new NotImplementedException("ResolveIdentifiers: ambiguous match for '" + name + "'.");

            var typeReference = resolved[0] as IAstTypeReference;
            if (typeReference == null)
                throw new NotImplementedException("ResolveIdentifiers: " + resolved[0] + " is not yet supported.");

            call.Target = null;
            ((AstUnknownMethod)call.Method).DeclaringType = typeReference;
        }

        private void ProcessMember(MemberExpression member, ProcessingContext context) {
            throw new NotImplementedException("ResolveIdentifiers.ProcessMember");
        }

        private void ProcessIdentifier(IdentifierExpression identifier, ProcessingContext context) {
            throw new NotImplementedException("ResolveIdentifiers.ProcessIdentifier");
        }

        #region IProcessingStep Members

        void IProcessingStep.ProcessAfterChildren(IAstElement element, ProcessingContext context) {
        }

        #endregion
    }
}
