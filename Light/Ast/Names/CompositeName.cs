using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AshMind.Extensions;

namespace Light.Ast.Names {
    public class CompositeName {
        public ReadOnlyCollection<string> Parts { get; private set; }

        public CompositeName(params string[] parts) {
            Argument.RequireNotNullOrEmpty("parts", parts);
            Argument.RequireAllNotNull("parts", parts);

            Parts = parts.AsReadOnly();
        }
    }
}
