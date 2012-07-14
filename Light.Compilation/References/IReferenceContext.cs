using Light.Ast.References;
using Mono.Cecil;

namespace Light.Compilation.References {
    public interface IReferenceContext {
        TypeReference ConvertReference(IAstTypeReference astType);
        MethodReference ConvertReference(IAstConstructorReference astConstructor);
        MethodReference ConvertReference(IAstMethodReference astMethod);
        FieldReference ConvertReference(AstPropertyReference astProperty);
    }
}