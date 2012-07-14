using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast {
    public interface IAstElement {
        IEnumerable<IAstElement> VisitOrTransformChildren(AstElementTransform transform);
    }
}