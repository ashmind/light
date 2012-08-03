using System;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class GenericMethodWithArgumentsProvider : IReferenceProvider {
        public Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module, IReferenceProvider recursive) {
            var methodAst = astReference as AstGenericMethodWithTypeArguments;
            if (methodAst == null)
                return null;

            var method = recursive.Convert(methodAst.Actual, module, recursive).As<MethodReference>();
            var typeArguments = methodAst.GenericArgumentTypes.Select(a => recursive.Convert(a, module, recursive).As<TypeReference>());
            
            var result = new GenericInstanceMethod(method);
            typeArguments.ForEach(result.GenericArguments.Add);

            return result;
        }
    }
}