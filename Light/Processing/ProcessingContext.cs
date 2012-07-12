using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast.References;
using Light.Processing.Scoping;

namespace Light.Processing {
    public class ProcessingContext : INameSource {
        public ProcessingContext() {
            this.ScopeStack = new Stack<Scope>();
            this.ScopeStack.Push(new Scope());
        }

        public Stack<Scope> ScopeStack { get; private set; }
        public Scope Scope {
            get { return this.ScopeStack.Peek(); }
        }

        public IList<IAstReference> Resolve(string name) {
            return this.ScopeStack.Select(s => s.Resolve(name)).FirstOrDefault(r => r.Count > 0)
                ?? new IAstReference[0];
        }
    }
}
