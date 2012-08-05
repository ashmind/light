using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Decimal = Light.Framework.Decimal;

namespace Light.Tests.Helpers {
    public static class TestArgumentConverter {
        public static dynamic Convert(object value) {
            if (value is int)
                return new Integer((int)value);

            if (value is double)
                return new Decimal((decimal)(double)value);

            var intArray = value as int[];
            if (intArray != null)
                return intArray.Select(i => new Integer(i)).ToArray();

            var doubleArray = value as double[];
            if (doubleArray != null)
                return doubleArray.Select(d => new Decimal((decimal)d)).ToArray();

            var objectArray = value as object[];
            if (objectArray != null)
                return Convert(objectArray);

            return value;
        }

        public static dynamic[] Convert(object[] values) {
            for (var i = 0; i < values.Length; i++) {
                values[i] = Convert(values[i]);
            }
            return values;
        }
    }
}
