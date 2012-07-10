using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast.Literals {
    public class StringWithInterpolation : IAstElement {
        public string Text { get; private set; }

        public StringWithInterpolation(string text) {
            Text = text;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            yield break;
        }

        #endregion
    }
}
