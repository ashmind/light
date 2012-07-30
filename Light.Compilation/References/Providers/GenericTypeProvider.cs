using System;
using System.Linq;
using AshMind.Extensions;
using Light.Ast.References;
using Light.Ast.References.Types;
using Light.Compilation.Internal;
using Mono.Cecil;

namespace Light.Compilation.References.Providers {
    public class GenericTypeProvider : IReferenceProvider {
        public Either<MemberReference, PropertyReferenceContainer> Convert(IAstReference astReference, ModuleDefinition module, IReferenceProvider recursive) {
            var generic = astReference as AstGenericTypeWithArguments;
            if (generic == null)
                return null;

            var primary = recursive.Convert(generic.PrimaryType, module, recursive).As<TypeReference>();
            var arguments = generic.TypeArguments.Select(a => recursive.Convert(a, module, recursive).As<TypeReference>());
            
            var result = new GenericInstanceType(primary);
            arguments.ForEach(result.GenericArguments.Add);

            return result;
        }
    }
}