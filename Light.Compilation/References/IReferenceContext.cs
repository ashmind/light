using Light.Ast.References;
using Mono.Cecil;

namespace Light.Compilation.References {
    public interface IReferenceContext {
        TypeReference ConvertTypeReference(IAstTypeReference astType);
        MethodReference ConvertMethodReference(IAstMethodReference astMethod);
        FieldReference ConvertFieldReference(AstPropertyReference astProperty);
    }
}