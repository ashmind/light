using System;
using System.Collections.Generic;
using System.Linq;
using Light.Parsing;

namespace Light.Ast {
    public interface IAstElement {
        IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform);
        SourceSpan SourceSpan { get; set; }
    }
}