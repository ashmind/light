using System.Collections.Generic;
using Light.Ast.References;

namespace Light.Processing.Scoping {
    public interface INameSource {
        IList<IAstReference> Resolve(string name);
    }
}