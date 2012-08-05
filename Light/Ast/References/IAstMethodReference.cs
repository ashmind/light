using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstMethodReference : IAstMemberReference {
        IEnumerable<IAstTypeReference> ParameterTypes { get; }
        IAstTypeReference ReturnType { get; }
        string Name { get; }

        MethodLocation Location { get; }

        // these are kind of more CLI-specific than Light-specific, probably I'll need to change them later:
        bool IsGeneric { get; }
        IEnumerable<IAstTypeReference> GetGenericParameterTypes();
    }
}
