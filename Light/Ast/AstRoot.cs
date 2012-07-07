using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast {
    public class AstRoot : IAstElement {
        public ReadOnlyCollection<IAstElement> Elements { get; private set; }

        public AstRoot(params IAstElement[] elements) {
            Argument.RequireNotNullAndNotContainsNull("elements", elements);
            Elements = elements.AsReadOnly();
        }
    }
}
