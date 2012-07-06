﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using AshMind.Extensions;

namespace Light.Ast.Literals {
    public class ListInitializer : IAstElement {
        public ReadOnlyCollection<IAstElement> Elements { get; private set; }

        public ListInitializer(params IAstElement[] elements) {
            Argument.RequireAllNotNull("elements", elements);
            Elements = elements.AsReadOnly();
        }
    }
}