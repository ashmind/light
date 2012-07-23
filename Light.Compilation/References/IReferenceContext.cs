using Light.Ast.References;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References {
    public interface IReferenceContext {
        TypeReference ConvertReference(IAstTypeReference astType);
        MethodReference ConvertReference(IAstConstructorReference astConstructor);
        MethodReference ConvertReference(IAstMethodReference astMethod);
        Either<FieldReference, PropertyReferenceContainer> ConvertReference(IAstPropertyReference astProperty);
    }
}