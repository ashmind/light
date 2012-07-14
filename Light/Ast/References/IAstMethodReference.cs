using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstMethodReference : IAstReference {
        IAstTypeReference ReturnType { get; }
        IAstTypeReference DeclaringType { get; }
        string Name { get; }
    }
}
