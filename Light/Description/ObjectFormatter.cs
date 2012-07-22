using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Light.Description {
    public class ObjectFormatter {
        public string Format(object value) {
            var function = value as Delegate;
            if (function != null)
                return "<function@" + function.Method.MethodHandle.Value + ">";

            var list = value as IList;
            if (list != null)
                return "[" + string.Join(", ", list.Cast<object>()) + "]";

            var @string = value as string;
            if (@string != null)
                return "\"" + @string + "\"";

            return value.ToString();
        }
    }
}