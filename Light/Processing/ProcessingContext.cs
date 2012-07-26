using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.References;
using Light.Processing.Scoping;

namespace Light.Processing {
    public class ProcessingContext : INameSource {
        public ProcessingContext() {
            this.ElementStack = new Stack<IAstElement>();

            this.ScopeStack = new Stack<Scope>();
            this.ScopeStack.Push(new Scope());
        }

        public Stack<IAstElement> ElementStack { get; private set; }

        public Stack<Scope> ScopeStack { get; private set; }
        public Scope Scope {
            get { return this.ScopeStack.Peek(); }
        }

        public IList<IAstReference> ResolveIdentifier(string name) {
            return this.ScopeStack.Select(s => s.ResolveIdentifier(name)).FirstOrDefault(r => r.Count > 0)
                ?? No.References;
        }

        public IList<IAstMemberReference> ResolveMember(string name) {
            return this.ScopeStack.Select(s => s.ResolveMember(name)).FirstOrDefault(r => r.Count > 0)
                ?? No.MemberReferences;
        }
    }
}
