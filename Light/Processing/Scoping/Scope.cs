using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Processing.Scoping {
    public class Scope : INameSource {
        private readonly IDictionary<string, IAstReference> items;
        private readonly IList<INameSource> sources;

        public Scope() {
            this.sources = new List<INameSource>();
            this.items = new Dictionary<string, IAstReference>();
        }

        public void Add(string name, IAstReference @object) {
            this.items.Add(name, @object);
        }

        public void Add(INameSource source) {
            this.sources.Add(source);
        }

        public IList<IAstReference> ResolveIdentifier(string name) {
            return Resolve(name, s => s.ResolveIdentifier(name));
        }

        public IList<IAstMemberReference> ResolveMember(string name) {
            return Resolve(name, s => s.ResolveMember(name));
        }

        private IList<T> Resolve<T>(string name, Func<INameSource, IEnumerable<T>> resolve) 
            where T : IAstReference
        {
            var inScope = this.items.GetValueOrDefault(name);
            if (inScope != null)
                return new[] { (T)inScope };

            return this.sources.SelectMany(resolve).ToArray();
        }
    }
}
