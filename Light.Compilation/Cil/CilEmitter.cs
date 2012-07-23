using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Light.Compilation.Cil {
    public class CilEmitter {
        public ILProcessor LowLevel { get; private set; }

        public CilEmitter(ILProcessor lowLevel) {
            this.LowLevel = lowLevel;
        }
    }
}
