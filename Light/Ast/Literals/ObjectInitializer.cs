using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Literals {
    public class ObjectInitializer : IAstElement {
        public ReadOnlyCollection<IAstElement> Elements { get; private set; }

        public ObjectInitializer(params IAstElement[] elements) {
            Argument.RequireNotNullAndNotContainsNull("elements", elements);
            Elements = elements.AsReadOnly();
        }
    }
}
