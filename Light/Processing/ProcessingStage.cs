using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Processing {
    public enum ProcessingStage {
        ScopeDefinition,
        ReferenceResolution,
        TypeInference,
        PreCompilation
    }
}
