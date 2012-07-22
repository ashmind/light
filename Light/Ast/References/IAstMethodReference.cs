using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstMethodReference : IAstMemberReference {
        ReadOnlyCollection<IAstTypeReference> ParameterTypes { get; }
        IAstTypeReference ReturnType { get; }
        string Name { get; }
    }
}
