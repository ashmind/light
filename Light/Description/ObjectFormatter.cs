using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Description {
    public class ObjectFormatter {
        public bool AllowPotentialSideEffects { get; set; }

        public string Format(object value) {
            if (value is IEnumerable && value.GetType() != typeof(string)) {
                var builder = new StringBuilder();
                Append(builder, value);
                return builder.ToString();
            }

            return FormatSimple(value);
        }

        private static string FormatSimple(object value) {
            if (value is bool)
                return ((bool)value) ? "true" : "false";

            var function = value as Delegate;
            if (function != null)
                return "<function:" + function.Method.ReflectedType.Name + "." + function.Method.Name + ">";

            var @string = value as string;
            if (@string != null)
                return "\"" + @string + "\"";

            return value.ToString();
        }

        private void Append(StringBuilder builder, object value) {
            var list = value as IList;
            if (list != null) {
                builder.Append("[");
                AppendAll(builder, ", ", list.Cast<object>());
                builder.Append("]");
                return;
            }

            var enumerable = value as IEnumerable;
            if (enumerable != null && this.AllowPotentialSideEffects) {
                builder.Append("(");
                AppendAll(builder, ", ", enumerable.Cast<object>().Take(5));
                builder.Append(", …)");
                return;
            }

            builder.Append(FormatSimple(value));
        }

        // duplicates AppendAll in AstToStringTransformer. TODO: abstract
        protected void AppendAll(StringBuilder builder, string delimiter, IEnumerable<object> values) {
            var first = true;
            foreach (var value in values) {
                if (!first)
                    builder.Append(delimiter);

                Append(builder, value);
                first = false;
            }
        }
    }
}