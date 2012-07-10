using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.Ast.Literals {
    public class PrimitiveValue : IAstElement {
        public PrimitiveValue(object value) {
            this.Value = value;
        }

        public Type Type {
            get { return Value.GetType(); }
        }

        public object Value { get; private set; }

        public override string ToString() {
            if (Value is string)
                return "'" + Value + "'";

            return Value.ToString();
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
