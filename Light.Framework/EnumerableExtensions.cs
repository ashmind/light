using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Framework {
    public static class EnumerableExtensions {
        public static Integer Sum(this IEnumerable<Integer> values) {
            return values.Aggregate((a, b) => a.Plus(b));
        }
    }
}
