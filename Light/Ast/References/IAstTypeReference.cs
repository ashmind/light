using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstTypeReference : IAstReference {
        IAstMethodReference ResolveMethod(string name, IEnumerable<IAstExpression> arguments);
        IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments);
        IAstMemberReference ResolveMember(string name);

        string Name { get; }
    }
}
