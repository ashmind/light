using Light.Ast.References;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References {
    public interface IReferenceContext {
        TypeReference ConvertReference(IAstTypeReference astType, bool returnNullIfFailed = false);
        MethodReference ConvertReference(IAstConstructorReference astConstructor, bool returnNullIfFailed = false);
        MethodReference ConvertReference(IAstMethodReference astMethod, bool returnNullIfFailed = false);
        Either<FieldReference, PropertyReferenceContainer> ConvertReference(IAstPropertyReference astProperty, bool returnNullIfFailed = false);
    }
}