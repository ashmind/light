using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AshMind.Extensions;
using Light.Framework;

namespace Light.BuiltIn {
    public class BuiltInTypeMap {
        protected IDictionary<string, Type> Types { get; private set; }

        public BuiltInTypeMap() {
            Types = new Dictionary<string, Type> {
                {"string",  typeof(string) },
                {"integer", typeof(Integer) },
                {"boolean", typeof(bool) }
            };
        }

        public Type GetTypeByAlias(string alias) {
            return Types.GetValueOrDefault(alias);
        }

        public string GetAliasByType(Type type) {
            return Types.Where(t => t.Value == type)
                        .Select(t => t.Key)
                        .FirstOrDefault();
        }
    }
}
