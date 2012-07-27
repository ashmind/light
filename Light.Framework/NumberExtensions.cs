using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Framework {
    public static class NumberExtensions {
        public static Range<int> RangeTo(this int from, int to) {
            return new Range<int>(from, to, Enumerable.Range(from, to - from));
        } 
    }
}
