using System.Collections.Generic;
using Light.Parsing;

namespace Light.Ast {
    public abstract class AstElementBase : IAstElement {
        protected abstract IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform);
        public SourceSpan SourceSpan { get; set; }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.VisitOrTransformChildren(AstElementTransform transform) {
            return this.VisitOrTransformChildren(transform);
        }

        #endregion
    }
}