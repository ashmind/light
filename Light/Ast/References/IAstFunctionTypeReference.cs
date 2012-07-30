﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstFunctionTypeReference : IAstTypeReference {
        IAstTypeReference ReturnType { get; }
        IEnumerable<IAstTypeReference> GetParameterTypes();
    }
}
