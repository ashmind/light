using System.Collections.ObjectModel;
using AshMind.Extensions;

namespace Light.Ast.Definitions {
    public class AnonymousTypeDefinition : IAstElement {
        public ReadOnlyCollection<IAstElement> Members { get; private set; }

        public AnonymousTypeDefinition(params IAstElement[] members) {
            Argument.RequireNotNullAndNotContainsNull("members", members);
            Members = members.AsReadOnly();
        }
    }
}