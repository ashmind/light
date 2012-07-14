using System;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class ReflectedReferenceProvider : IReferenceProvider {
        public MemberReference Convert(IAstReference astReference, ModuleDefinition module) {
            var reflectedType = astReference as AstReflectedType;
            if (reflectedType != null)
                return module.Import(reflectedType.ActualType);

            var reflectedMethod = astReference as AstReflectedMethod;
            if (reflectedMethod != null)
                return module.Import(reflectedMethod.Method);

            var reflectedConstructor = astReference as AstReflectedConstructor;
            if (reflectedConstructor != null)
                return module.Import(reflectedConstructor.Constructor);

            return null;
        }
    }
}