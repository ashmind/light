using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Light.Internal {
    public class NonTerminal<TAstNode> : NonTerminal {
        public NonTerminal(string name) : base(name) { // only for transients
        }
    }
}
