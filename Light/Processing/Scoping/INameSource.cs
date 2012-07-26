using System.Collections.Generic;
using Light.Ast.References;

namespace Light.Processing.Scoping {
    public interface INameSource {
        IList<IAstReference> ResolveIdentifier(string name);
        IList<IAstMemberReference> ResolveMember(string name);
    }
}