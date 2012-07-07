using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Light.Internal {
    public class NonTerminal<TAstNode> : NonTerminal {
        public NonTerminal(string name) : base(name) { // only for transients
        }

        public NonTerminal(string name, Func<ParseTreeNode, TAstNode> nodeCreator) : base(name, (c, n) => n.AstNode = nodeCreator(n)) {
        }
    }
}
