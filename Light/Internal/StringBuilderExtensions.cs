using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Internal {
    public static class StringBuilderExtensions {
        public static StringBuilder AppendJoin<T>(this StringBuilder builder, string delimiter, IEnumerable<T> items) {
            var first = true;
            foreach (var item in items) {
                if (!first)
                    builder.Append(delimiter);

                builder.Append(item);
                first = false;
            }

            return builder;
        }
    }
}
