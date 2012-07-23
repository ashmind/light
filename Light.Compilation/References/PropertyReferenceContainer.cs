using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Light.Compilation.References {
    public class PropertyReferenceContainer {
        public PropertyReferenceContainer(MethodReference getMethod, MethodReference setMethod) {
            this.GetMethod = getMethod;
            this.SetMethod = setMethod;
        }

        public MethodReference GetMethod { get; private set; }
        public MethodReference SetMethod { get; private set; }
    }
}
