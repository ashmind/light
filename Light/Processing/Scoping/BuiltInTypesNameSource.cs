using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Types;

namespace Light.Processing.Scoping {
    public class BuiltInTypesNameSource : INameSource {
        public IDictionary<string, AstReflectedType> Types { get; private set; }

        public BuiltInTypesNameSource() {
            Types = new Dictionary<string, AstReflectedType> {
                {"string", new AstReflectedType(typeof(string)) },
                {"integer", new AstReflectedType(typeof(int)) }
            };
        }

        public IList<IAstReference> Resolve(string name) {
            var type = this.Types.GetValueOrDefault(name);
            if (type == null)
                return No.References;

            return new[] { type };
        }
    }
}
