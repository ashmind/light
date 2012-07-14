using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Processing.Steps {
    public class ResolveIdentifiers : ProcessingStepBase<IAstElement> {
        public ResolveIdentifiers() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessBeforeChildren(IAstElement element, ProcessingContext context) {
            var call = element as CallExpression;
            if (call != null && call.Target is IdentifierExpression)
                return ProcessCall(call, context);

            var member = element as MemberExpression;
            if (member != null && member.Target is IdentifierExpression)
                return ProcessMember(member, context);

            var identifier = element as IdentifierExpression;
            if (identifier != null)
                return ProcessIdentifier(identifier, context);

            return element;
        }

        private IAstElement ProcessCall(CallExpression call, ProcessingContext context) {
            var name = (call.Target as IdentifierExpression).Name;
            var resolved = context.Resolve(name);
            RequireExactlyOne(resolved, name);

            var typeReference = resolved[0] as IAstTypeReference;
            if (typeReference == null)
                throw new NotImplementedException("ResolveIdentifiers: " + resolved[0] + " is not yet supported.");

            call.Target = null;
            ((AstUnknownMethod)call.Method).DeclaringType = typeReference;
            return call;
        }

        private IAstElement ProcessMember(MemberExpression member, ProcessingContext context) {
            throw new NotImplementedException("ResolveIdentifiers.ProcessMember");
        }

        private IAstElement ProcessIdentifier(IdentifierExpression identifier, ProcessingContext context) {
            var resolved = context.Resolve(identifier.Name);
            RequireExactlyOne(resolved, identifier.Name);

            return resolved[0];
        }

        private static void RequireExactlyOne(IList<IAstReference> resolved, string name) {
            if (resolved.Count == 0)
                throw new NotImplementedException("ResolveIdentifiers: cannot resolve '" + name + "'.");

            if (resolved.Count > 1)
                throw new NotImplementedException("ResolveIdentifiers: ambiguous match for '" + name + "'.");
        }
    }
}
