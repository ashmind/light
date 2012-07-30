using System;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Properties;
using Light.Ast.References.Types;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class ReflectedReferenceProvider : IReferenceProvider {
        public Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module, IReferenceProvider recursive) {
            var reflectedType = astReference as AstReflectedType;
            if (reflectedType != null)
                return module.Import(reflectedType.ActualType);

            var reflectedMethod = astReference as AstReflectedMethod;
            if (reflectedMethod != null)
                return module.Import(reflectedMethod.Method);

            var reflectedConstructor = astReference as AstReflectedConstructor;
            if (reflectedConstructor != null)
                return module.Import(reflectedConstructor.Constructor);

            var reflectedProperty = astReference as AstReflectedProperty;
            if (reflectedProperty != null) {
                var getMethod = reflectedProperty.Property.GetGetMethod();
                var setMethod = reflectedProperty.Property.GetSetMethod();
                return new PropertyReferenceContainer(
                    getMethod != null ? module.Import(getMethod) : null,
                    setMethod != null ? module.Import(setMethod) : null
                );
            }

            return null;
        }
    }
}