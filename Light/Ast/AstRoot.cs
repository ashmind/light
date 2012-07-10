using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Ast {
    public class AstRoot : IAstElement {
        public IList<IAstElement> Elements { get; private set; }

        public AstRoot(IEnumerable<IAstElement> elements) {
            var elmentList = elements.ToList();
            Argument.RequireNotNullAndNotContainsNull("elements", elmentList);
            Elements = elmentList;
        }

        #region IAstElement Members

        IEnumerable<IAstElement> IAstElement.Children() {
            return this.Elements;
        }

        #endregion
    }
}
