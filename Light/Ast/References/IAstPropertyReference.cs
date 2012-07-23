namespace Light.Ast.References {
    public interface IAstPropertyReference : IAstMemberReference {
        IAstTypeReference Type { get; }
    }
}