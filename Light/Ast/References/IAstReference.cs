﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.References {
    public interface IAstReference : IAstElement {
        object Target { get; }
    }
}
