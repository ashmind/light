using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Literals {
    public class ListInitializer : IAstElement {
        public ReadOnlyCollection<IAstElement> Elements { get; private set; }

        public ListInitializer(params IAstElement[] elements) {
            if (elements.Contains(null))
                throw new ArgumentException("List must not contain null elements.", "elements");

            Elements = elements.AsReadOnly();
        }
    }
}
