using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Framework {
    // temporary explicit conversions until language-level support for this is implemented
    public static class NumberExtensions {
        public static Integer ToInteger(this double value) {
            return new Integer((int)value);
        }

        public static Decimal ToDecimal(this double value) {
            return new Decimal((decimal)value);
        }
    }
}
