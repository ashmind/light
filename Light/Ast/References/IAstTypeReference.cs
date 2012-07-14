using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.Expressions;

namespace Light.Ast.References {
    public interface IAstTypeReference : IAstReference {
        IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments);
        IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments);
        string Name { get; }
    }
}
