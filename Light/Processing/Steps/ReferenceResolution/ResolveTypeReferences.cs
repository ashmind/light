using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Incomplete;
using Light.Ast.References;

namespace Light.Processing.Steps.ReferenceResolution {
    public class ResolveTypeReferences : ProcessingStepBase<AstUnknownType> {
        public ResolveTypeReferences() : base(ProcessingStage.ReferenceResolution) {
        }

        public override IAstElement ProcessAfterChildren(AstUnknownType type, ProcessingContext context) {
            var resolved = ResolveSingleType(context, type.Name);
            resolved.SourceSpan = type.SourceSpan;
            return resolved;
        }

        private static IAstReference ResolveSingleType(ProcessingContext context, string name) {
            var resolved = context.Resolve(name);
            if (resolved.Count == 0)
                throw new NotImplementedException("ResolveTypeReferences: cannot resolve '" + name + "'.");

            if (resolved.Count > 1)
                throw new NotImplementedException("ResolveTypeReferences: ambiguous match for '" + name + "'.");

            return resolved.Single();
        }
    }
}
