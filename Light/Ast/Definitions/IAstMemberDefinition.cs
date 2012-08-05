using Light.Ast.References;

namespace Light.Ast.Definitions {
    public interface IAstMemberDefinition : IAstDefinition {
        string Name { get; }
        IAstReference ToReference();
    }
}