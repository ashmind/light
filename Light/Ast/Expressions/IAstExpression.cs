using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;

namespace Light.Ast.Expressions {
    public interface IAstExpression : IAstElement {
        IAstTypeReference ExpressionType { get; }
    }
}
