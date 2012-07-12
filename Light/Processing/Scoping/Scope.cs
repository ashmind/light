using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;

namespace Light.Processing.Scoping {
    public class Scope : INameSource {
        private readonly IDictionary<string, IAstReference> items;

        public Scope() {
            this.Sources = new List<INameSource>();
            this.items = new Dictionary<string, IAstReference>();
        }

        public void Add(string name, IAstReference @object) {
            this.items.Add(name, @object);
        }

        public IList<IAstReference> Resolve(string name) {
            var inScope = this.items.GetValueOrDefault(name);
            if (inScope != null)
                return new[] { inScope };

            return this.Sources.SelectMany(s => s.Resolve(name)).ToArray();
        }

        public IList<INameSource> Sources { get; private set; }
    }
}
