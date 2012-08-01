using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstTypeReference : IAstReference {
        IAstConstructorReference ResolveConstructor(IEnumerable<IAstExpression> arguments);
        IAstMemberReference ResolveMember(string name);

        string Name { get; }

        // too CLI-specific, might improve that in the future:
        IAstTypeReference BaseType { get; }
        IEnumerable<IAstTypeReference> GetInterfaces();
        IEnumerable<IAstTypeReference> GetTypeParameters();
    }
}
