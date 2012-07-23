using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AshMind.Extensions;

namespace Light.BuiltIn {
    public class BuiltInTypeMap {
        protected IDictionary<string, Type[]> Types { get; private set; }

        public BuiltInTypeMap() {
            Types = new Dictionary<string, Type[]> {
                {"string",  new[] { typeof(string) } },
                {"integer", new[] { typeof(int), typeof(BigInteger) } },
                {"boolean", new[] { typeof(bool) } }
            };
        }

        public Type GetTypeByAlias(string alias) {
            var types = Types.GetValueOrDefault(alias);
            if (types == null)
                return null;

            return types[0]; // TODO: heuristics
        }

        public string GetAliasByType(Type type) {
            return Types.Where(t => t.Value.Contains(type))
                        .Select(t => t.Key)
                        .FirstOrDefault();
        }
    }
}
