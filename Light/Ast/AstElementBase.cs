using System.Collections.Generic;
using Light.Description;
using Light.Parsing;

namespace Light.Ast {
    public abstract class AstElementBase : IAstElement {
        protected abstract IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform);
        public SourceSpan SourceSpan { get; set; }

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.VisitOrTransformChildren(transform);
        }
    }
}