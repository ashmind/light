using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.Ast;

namespace Light.Description {
    public abstract partial class AstToStringTransformer {
        public string Transform(IAstElement element) {
            var builder = new StringBuilder();
            this.Append(builder, element);
            return builder.ToString();
        }

        protected void AppendAll(StringBuilder builder, string delimiter, IEnumerable<IAstElement> elements) {
            var first = true;
            foreach (var element in elements) {
                if (!first)
                    builder.Append(delimiter);

                Append(builder, element);
                first = false;
            }
        }
    }
}
