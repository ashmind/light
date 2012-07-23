using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Expressions;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveIdentifiers : ProcessingStepBase<IdentifierExpression> {
        public ResolveIdentifiers() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessBeforeChildren(IdentifierExpression identifier, ProcessingContext context) {
            var resolved = context.Resolve(identifier.Name);
            RequireExactlyOne(resolved, identifier.Name);

            var result = (IAstElement)resolved[0];

            var property = result as IAstPropertyReference;
            if (property != null)
                result = new AstPropertyExpression(null, property);

            result.SourceSpan = identifier.SourceSpan;
            return result;
        }

        private static void RequireExactlyOne(IList<IAstReference> resolved, string name) {
            if (resolved.Count == 0)
                throw new NotImplementedException("ResolveIdentifiers: cannot resolve '" + name + "'.");

            if (resolved.Count > 1)
                throw new NotImplementedException("ResolveIdentifiers: ambiguous match for '" + name + "'.");
        }
    }
}
